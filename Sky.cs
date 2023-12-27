using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValleyClone
{
	public class Drop : LayerGeneric
	{
		private SpriteGroup _allSprites;
		private int _lifeTime;
		Stopwatch stopwatch = new Stopwatch();
		private float _startTime;
		private Texture2D _image;
		private bool _moving;
		private Vector2 _pos = new();
		private Vector2 _direction = new();
		private int _speed;

		public Drop(GraphicsDevice graphicsDevice, Texture2D surf, Rectangle rect, SpriteGroup groups, Settings settings, int z, bool moving) : base(graphicsDevice, surf, rect, groups, settings, z)
		{
			_allSprites = groups;
			_allSprites.Add(this);

			Random rnd = new Random();
			_lifeTime = rnd.Next(400, 500);
			stopwatch.Start();
			_startTime = stopwatch.ElapsedMilliseconds;

			_image = surf;

			X = rect.X;
			Y = rect.Y;

			//moving
			_moving = moving;
			if (_moving)
			{
				_pos.X = X;
				_pos.Y = Y;
				_direction = new(-2, 4);
				_speed = rnd.Next(150, 200);
			}
		}

		public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(_image, offsetTmp, Color.White);
			spriteBatch.End();
		}

		public override void Update(float dt)
		{
			if (_moving)
			{
				_pos += _direction * _speed * dt;
				X = (int)Math.Round(_pos.X, 0);
				Y = (int)Math.Round(_pos.Y, 0);
			}

			if (stopwatch.ElapsedMilliseconds - _startTime >= _lifeTime)
			{
				_allSprites.Remove(this);
			}
		}
	}

	public class Sky
	{
		private GraphicsDevice _graphicsDevice;
		private Settings _settings;

		private SpriteGroup _allSprites;
		private List<Texture2D> _rainDrops = new();
		private List<Texture2D> _rainFloor = new();

		private int _floorW;
		private int _floorH;

		Random rnd = new Random();

		public Sky(SpriteGroup allSprites, GraphicsDevice graphicsDevice, Settings settings)
		{
			_graphicsDevice = graphicsDevice;
			_settings = settings;

			_allSprites = allSprites;

			_rainDrops = Support.ImportFolder($"{settings.Path}graphics/rain/drops", graphicsDevice);
			_rainFloor = Support.ImportFolder($"{settings.Path}graphics/rain/floor", graphicsDevice);

			_floorW = Texture2D.FromFile(graphicsDevice, $"{settings.Path}graphics/world/ground.png").Width;
			_floorH = Texture2D.FromFile(graphicsDevice, $"{settings.Path}graphics/world/ground.png").Height;
		}

		public void CreateFloor()
		{
			for (int i = 0; i < 5; i++)
			{
				new Drop(_graphicsDevice, _rainFloor[rnd.Next(0, 3)], new(rnd.Next(0, _floorW), rnd.Next(0, _floorH), 0, 0), _allSprites, _settings, _settings.LAYERS["rain floor"], false);
			}
		}

		public void CreateDrops()
		{
			for (int i = 0; i < 5; i++)
			{
				new Drop(_graphicsDevice, _rainDrops[rnd.Next(0, 3)], new(rnd.Next(0, _floorW), rnd.Next(0, _floorH), 0, 0), _allSprites, _settings, _settings.LAYERS["rain drops"], false);
			}
		}

		public void Update()
		{
			CreateFloor();
			CreateDrops();
		}

	}
}

