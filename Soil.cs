using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;
using Microsoft.Xna.Framework.Audio;

namespace StardewValleyClone
{
	public class SoilTile : Sprite
	{
		private GraphicsDevice _graphicsDevice;
		private Texture2D _image;
		private Vector2 _pos;

		public SoilTile(GraphicsDevice graphicsDevice, Vector2 pos, Texture2D image, SpriteGroup groups, SpriteGroup soilSprites, Settings settings) : base(graphicsDevice, pos)
		{
			groups.Add(this);
			soilSprites.Add(this);
			_graphicsDevice = graphicsDevice;

			_image = image;
			_pos = pos;
			X = (int)_pos.X;
			Y = (int)_pos.Y;
			Z = settings.LAYERS["soil"];
		}

		public Rectangle Rect { get => new Rectangle((int)_pos.X, (int)_pos.Y, _image.Width, _image.Height); }

		public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(_image, offsetTmp, Color.White);
			spriteBatch.End();
		}
	}

	public class WaterTile : Sprite
	{
		private Texture2D _image;
		private Vector2 _pos;
		private Rectangle _rect;

		public WaterTile(GraphicsDevice graphicsDevice, Vector2 pos, Texture2D image, SpriteGroup groups, Settings settings, SpriteGroup waterSprites) : base(graphicsDevice, pos)
		{
			groups.Add(this);
			waterSprites.Add(this);

			_image = image;
			_rect = new((int)pos.X, (int)pos.Y, _image.Width, _image.Height);
			_pos = pos;
			X = (int)_pos.X;
			Y = (int)_pos.Y;
			Z = settings.LAYERS["soil water"];
		}

		public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(_image, offsetTmp, Color.White);
			spriteBatch.End();
		}
	}

	public class Plant : Sprite
	{
		private GraphicsDevice _graphicsDevice;
		private Settings _settings;
		private string _plantType;
		private Rectangle _soil;
		private List<Texture2D> _frames = new();
		private double _age = 0;
		private int _maxAge;
		private double _growSpeed;
		private Texture2D _image;
		private Dictionary<int, Texture2D> _frameSorted = new();
		private Func<Point, bool> _CheckWatered;
		public bool Harvestable = false;

		public Plant(GraphicsDevice graphicsDevice, string plantType, SpriteGroup groups, SpriteGroup plantSprites, Rectangle soil, Settings settings, Func<Point, bool> CheckWatered) : base(graphicsDevice, new(0, 0))
		{
			_graphicsDevice = graphicsDevice;
			_settings = settings;

			groups.Add(this);
			plantSprites.Add(this);
			_plantType = plantType;
			_frames = Support.ImportFolder($"{_settings.Path}graphics/fruit/{_plantType}", _graphicsDevice);
			_soil = soil;

			_frameSorted.Add(0, _frames[0]);
			_frameSorted.Add(1, _frames[1]);
			_frameSorted.Add(2, _frames[2]);
			_frameSorted.Add(3, _frames[3]);

			_maxAge = _frames.Count - 1;
			_growSpeed = settings.GROW_SPEED[_plantType];

			_image = _frameSorted[(int)_age];

			X = (soil.X + soil.Width / 2) - _image.Width / 2;
			Y = (soil.Y + soil.Height / 2) - _image.Height / 2;
			Z = _settings.LAYERS["ground plant"];

			_CheckWatered = CheckWatered;
		}

		public Rectangle Rect { get => new Rectangle(X, Y, _image.Width, _image.Height); }
		public string Name { get => _plantType; }

		public void Grow()
		{
			Rectangle tmprect = new Rectangle(X, Y, _image.Width, _image.Height);

			if (_CheckWatered(tmprect.Center))
			{
				_age += _growSpeed;

				if ((int)_age > 0)
				{
					Z = _settings.LAYERS["main"];
				}

				if (_age >= _maxAge)
				{
					_age = _maxAge;
					Harvestable = true;
				}

				_image = _frameSorted[(int)_age];
				X = (_soil.X + _soil.Width / 2) - _image.Width / 2;
				Y = (_soil.Y + _soil.Height / 2) - _image.Height / 2;
			}
		}

		public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(_image, offsetTmp, Color.White);
			spriteBatch.End();
		}
	}

	public class Soil
	{
		private SpriteGroup _allSprites;
		private SpriteGroup _soilSprites;
		private SpriteGroup _waterSprites;
		private SpriteGroup _plantSprites;
		private Dictionary<string, Texture2D> _soilSurfs;
		private GraphicsDevice _graphicsDevice;
		private Settings _settings;
		private List<List<List<string>>> _grid = new();
		private List<Rectangle> _hitRects = new();
		private int _hTiles;
		private int _vTiles;
		private List<Texture2D> _waterSurfs = new();
		public bool raining = false;

		public Soil(SpriteGroup allSprites, GraphicsDevice graphicsDevice, Settings settings)
		{
			_allSprites = allSprites;
			_soilSprites = new();
			_waterSprites = new();
			_plantSprites = new();

			_graphicsDevice = graphicsDevice;
			_settings = settings;

			_soilSurfs = Support.ImportFolderDict($"{settings.Path}graphics/soil", graphicsDevice);
			_waterSurfs = Support.ImportFolder($"{settings.Path}graphics/soil_water", graphicsDevice);

			CreateSoilGrid();
		}

		public SpriteGroup PlantSprites { get => _plantSprites; }
		public List<List<List<string>>> Grid { get => _grid; }


		public void CreateSoilGrid()
		{
			Texture2D ground = Texture2D.FromFile(_graphicsDevice, $"{_settings.Path}graphics/world/ground.png");

			_hTiles = ground.Width / _settings.TILE_SIZE;
			_vTiles = ground.Height / _settings.TILE_SIZE;

			for (int i = 0; i < _vTiles; i++)
			{
				List<List<string>> tmp1 = new();

				for (int j = 0; j < _hTiles; j++)
				{
					List<string> tmp2 = new();
					tmp1.Add(tmp2);
				}

				_grid.Add(tmp1);

			}

			foreach (TiledLayer layer in Game1._map.Layers)
			{
				if (layer.name == "Farmable")
				{
					for (var i = 0; i < layer.data.Length; i++)
					{
						int gid = layer.data[i];

						// Empty tile, do nothing
						if (gid == 0)
						{

						}
						else
						{
							int tileFrame = gid - 1;
							int column = tileFrame % Game1.InteractionTileTilesWide;
							int row = (int)Math.Floor((double)tileFrame / Game1.InteractionTileTilesWide);

							float x = (i % Game1._map.Width) * Game1._map.TileWidth;
							float y = (float)Math.Floor(i / (double)Game1._map.Width) * Game1._map.TileHeight;

							_grid[(int)y / 64][(int)x / 64].Add("F");
							//new Generic(_graphicsDevice, $"{_settings.Path}graphics/soil/o.png", new Vector2(x, y), _allSprites, _settings);
							_hitRects.Add(new((int)x, (int)y, Game1.TileWidth, Game1.TileHeight));
						}
					}
				}
			}
		}

		public void GetHit(Rectangle point)
		{
			foreach (Rectangle rect in _hitRects)
			{
				if (rect.Intersects(point))
				{
					SoundEffectInstance soundInstance = Game1.hoeSound.CreateInstance();
					soundInstance.Volume = 0.5f;
					soundInstance.Play();

					int x = rect.X / Game1.TileWidth;
					int y = rect.Y / Game1.TileHeight;

					if (_grid[y][x].Contains("F"))
					{
						_grid[y][x].Clear();
						_grid[y][x].Add("X");
						CreateSoilTiles();
						if (raining)
						{
							WaterAll();
						}
					}
				}
			}
		}

		public void Water(Rectangle point)
		{
			foreach (SoilTile soilSprite in _soilSprites.GetSprites)
			{
				if (soilSprite.Rect.Intersects(point))
				{
					int x = soilSprite.Rect.X / _settings.TILE_SIZE;
					int y = soilSprite.Rect.Y / _settings.TILE_SIZE;

					_grid[y][x].Add("W");

					Vector2 pos = new(soilSprite.Rect.X, soilSprite.Rect.Y);
					Random rnd = new Random();
					Texture2D surf = _waterSurfs[rnd.Next(0, 3)];

					WaterTile tmp = new(_graphicsDevice, pos, surf, _allSprites, _settings, _waterSprites);
				}
			}
		}

		public void WaterAll()
		{
			foreach (var row in _grid.Select((value, i) => new { i, value }))
			{
				foreach (var cell in row.value.Select((value, j) => new { j, value }))
				{
					if (cell.value.Contains("X") && !cell.value.Contains("W"))
					{
						cell.value.Add("W");

						Random rnd = new Random();
						Texture2D surf = _waterSurfs[rnd.Next(0, 3)];
						Vector2 pos = new(cell.j * _settings.TILE_SIZE, row.i * _settings.TILE_SIZE);
						WaterTile tmp = new(_graphicsDevice, pos, surf, _allSprites, _settings, _waterSprites);
					}
				}
			}
		}

		public void RemoveWater()
		{
			foreach (WaterTile waterSprite in _waterSprites.GetSprites.ToList())
			{
				_waterSprites.Remove(waterSprite);
				_allSprites.Remove(waterSprite);
			}

			foreach (var row in _grid)
			{
				foreach (var cell in row)
				{
					if (cell.Contains("W"))
					{
						cell.Clear();
						cell.Add("X");
					}
				}
			}
		}

		public bool CheckWatered(Point pos)
		{
			int x = (int)pos.X / _settings.TILE_SIZE;
			int y = (int)pos.Y / _settings.TILE_SIZE;

			var cell = _grid[y][x];
			bool isWatered = false;
			if (cell.Contains("W")) isWatered = true;
			return isWatered;
		}

		public void PlantSeed(Rectangle point, string seed)
		{
			foreach (SoilTile soilSprite in _soilSprites.GetSprites)
			{
				if (soilSprite.Rect.Intersects(point))
				{
					SoundEffectInstance soundInstance = Game1.plantSound.CreateInstance();
					soundInstance.Volume = 0.3f;
					soundInstance.Play();

					int x = soilSprite.Rect.X / _settings.TILE_SIZE;
					int y = soilSprite.Rect.Y / _settings.TILE_SIZE;

					if (!_grid[y][x].Contains("P"))
					{
						_grid[y][x].Add("P");
						Plant tmp = new(_graphicsDevice, seed, _allSprites, _plantSprites, soilSprite.Rect, _settings, CheckWatered);
					}
				}
			}
		}

		public void UpdatePlants()
		{
			foreach (Plant plant in _plantSprites.GetSprites)
			{
				plant.Grow();
			}
		}

		public void CreateSoilTiles()
		{
			foreach (var row in _grid.Select((value, i) => new { i, value }))
			{
				foreach (var cell in row.value.Select((value, j) => new { j, value }))
				{
					if (cell.value.Contains("X"))
					{
						bool t = _grid[row.i - 1][cell.j].Contains("X");
						bool b = _grid[row.i + 1][cell.j].Contains("X");
						bool r = _grid[row.i][cell.j + 1].Contains("X");
						bool l = _grid[row.i][cell.j - 1].Contains("X");

						string tileType = "o";

						//all sides
						if (t && b && l && r) tileType = "x";

						//horizontal tiles only
						if (l && !t && !b && !r) tileType = "r";
						if (r && !t && !b && !l) tileType = "l";
						if (l && !t && !b && r) tileType = "lr";

						//vertical tiles only
						if (t && !l && !b && !r) tileType = "b";
						if (b && !l && !t && !l) tileType = "t";
						if (t && !l && b && !r) tileType = "tb";

						//corners
						if (l && b && !t && !r) tileType = "tr";
						if (r && b && !t && !l) tileType = "tl";
						if (l && t && !b && !r) tileType = "br";
						if (r && t && !b && !l) tileType = "bl";

						//T shapes
						if (!l && t && b && r) tileType = "tbr";
						if (l && t && b && !r) tileType = "tbl";
						if (l && t && !b && r) tileType = "lrb";
						if (l && !t && b && r) tileType = "lrt";


						SoilTile tmp = new(_graphicsDevice, new(cell.j * 64, row.i * 64), _soilSurfs[tileType], _allSprites, _soilSprites, _settings);
					}
				}

			}

		}
	}
}

