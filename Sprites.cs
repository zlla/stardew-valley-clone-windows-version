using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValleyClone
{
    public class Generic : Sprite
    {
        private Texture2D _image;
        private Vector2 _pos;
        private Settings _settings;

        public Generic(GraphicsDevice graphicsDevice, string path, Vector2 pos, SpriteGroup groups, Settings settings, int z) : base(graphicsDevice, path, pos)
        {
            _settings = settings;
            groups.Add(this);
            _image = Texture2D.FromFile(graphicsDevice, path);
            _pos = pos;
            Z = z;
            X = (int)pos.X;
            Y = (int)pos.Y;
        }

        public Generic(GraphicsDevice graphicsDevice, string path, Vector2 pos, SpriteGroup groups, Settings settings) : base(graphicsDevice, path, pos)
        {
            _settings = settings;
            groups.Add(this);
            _image = Texture2D.FromFile(graphicsDevice, path);
            Z = _settings.LAYERS["main"];
            X = (int)pos.X;
            Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_image, offsetTmp, Color.White);
            spriteBatch.End();
        }
    }

    public class Interaction : Sprite
    {
        private Texture2D _surf;
        private string _name;
        private Rectangle _rect;

        public Interaction(GraphicsDevice graphicsDevice, Rectangle rect, SpriteGroup interActionSprites, string name) : base(graphicsDevice, new(rect.X, rect.Y))
        {
            interActionSprites.Add(this);
            _surf = new(graphicsDevice, rect.Width, rect.Height);
            _name = name;
            _rect = rect;
            _rect.X += 46;
        }

        public Rectangle GetRect { get => _rect; }
        public string Name { get => _name; }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_surf, offsetTmp, Color.White);
            spriteBatch.End();
        }
    }

    public class LayerGeneric : Sprite
    {
        private Texture2D _image;
        private Rectangle _rect;
        private Rectangle _tilesetRec;
        private Settings _settings;

        public LayerGeneric(GraphicsDevice graphicsDevice, Texture2D img, Rectangle rect, Rectangle tilesetRec, SpriteGroup groups, Settings settings, int z) : base(graphicsDevice, new(rect.X, rect.Y))
        {
            _settings = settings;
            groups.Add(this);
            _image = img;
            Z = z;
            _rect = rect;
            _tilesetRec = tilesetRec;
            X = rect.Center.X;
            Y = rect.Center.Y;
        }

        public LayerGeneric(GraphicsDevice graphicsDevice, Texture2D img, Rectangle rect, Rectangle tilesetRec, SpriteGroup groups, Settings settings, int z, SpriteGroup collisionSprites) : base(graphicsDevice, new(rect.X, rect.Y))
        {
            _settings = settings;
            groups.Add(this);
            collisionSprites.Add(this);
            _image = img;
            Z = z;
            _rect = rect;
            _tilesetRec = tilesetRec;
            X = rect.Center.X;
            Y = rect.Center.Y;
            Hitbox = _rect;
            HitboxStt = true;
        }

        public LayerGeneric(GraphicsDevice graphicsDevice, Texture2D img, Rectangle rect, Rectangle tilesetRec, SpriteGroup groups, Settings settings) : base(graphicsDevice, new(rect.X, rect.Y))
        {
            _settings = settings;
            groups.Add(this);
            _image = img;
            Z = _settings.LAYERS["main"];
            _rect = rect;
            _tilesetRec = tilesetRec;
            X = rect.Center.X;
            Y = rect.Center.Y;
        }

        public LayerGeneric(GraphicsDevice graphicsDevice, Texture2D img, Rectangle rect, Rectangle tilesetRec, SpriteGroup groups, Settings settings, SpriteGroup collisionSprites) : base(graphicsDevice, new(rect.X, rect.Y))
        {
            _settings = settings;
            groups.Add(this);
            collisionSprites.Add(this);
            _image = img;
            Z = _settings.LAYERS["main"];
            _rect = rect;
            _tilesetRec = tilesetRec;
            X = rect.Center.X;
            Y = rect.Center.Y;
            Hitbox = new(_rect.X - 84, _rect.Y - 64, img.Width - 64, img.Height - 64 * 2);

            HitboxStt = true;
        }

        public LayerGeneric(GraphicsDevice graphicsDevice, Texture2D img, Rectangle rect, SpriteGroup groups, Settings settings, int z) : base(graphicsDevice, new(rect.X, rect.Y))
        {
            _settings = settings;
            groups.Add(this);
            _image = img;
            Z = z;
            _rect = rect;
            X = rect.Center.X;
            Y = rect.Center.Y;
        }

        public LayerGeneric(GraphicsDevice graphicsDevice, Texture2D img, Rectangle rect, SpriteGroup groups, Settings settings, int z, SpriteGroup collisionSprites) : base(graphicsDevice, new(rect.X, rect.Y))
        {
            _settings = settings;
            groups.Add(this);
            collisionSprites.Add(this);
            _image = img;
            Z = z;
            _rect = rect;
            X = rect.Center.X;
            Y = rect.Center.Y;
            Hitbox = new(_rect.X, _rect.Y, img.Width + 50, img.Height + 50);
            HitboxStt = true;
        }

        public LayerGeneric(GraphicsDevice graphicsDevice, Texture2D img, Rectangle rect, SpriteGroup groups, SpriteGroup collisionSprites) : base(graphicsDevice, new(rect.X, rect.Y))
        {
            groups.Add(this);
            collisionSprites.Add(this);
            _image = img;
            Z = 7;
            _rect = rect;
            X = rect.Center.X;
            Y = rect.Center.Y;
            Hitbox = new(_rect.X - 25, _rect.Y - 34, img.Width + 90, img.Height + 60);
            HitboxStt = true;
        }

        public LayerGeneric(GraphicsDevice graphicsDevice, Vector2 pos, SpriteGroup groups, int z) : base(graphicsDevice, new(pos.X, pos.Y))
        {
            groups.Add(this);
            Z = z;
            X = (int)pos.X;
            Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_image, offsetTmp, _tilesetRec, Color.White);
            spriteBatch.End();
        }
    }

    public class Water : LayerGeneric
    {
        private float _frameIndex;
        private List<Texture2D> _frames;
        private Rectangle _tilesetRec;
        private Texture2D _image;

        public Water(GraphicsDevice graphicsDevice, List<Texture2D> frames, Rectangle rect, Rectangle tilesetRec, SpriteGroup groups, Settings settings) : base(graphicsDevice, frames[0], rect, tilesetRec, groups, settings)
        {
            _frameIndex = 0;
            _frames = frames;
            _image = _frames[(int)_frameIndex];
            Z = settings.LAYERS["water"];
            _tilesetRec = tilesetRec;
        }

        public void Animate(float dt)
        {
            _frameIndex += 2 * dt;
            if (_frameIndex >= _frames.Count) _frameIndex = 0;

            _image = _frames[(int)_frameIndex];
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_image, offsetTmp, Color.White);
            spriteBatch.End();
        }

        public override void Update(float dt)
        {
            Animate(dt);
        }
    }

    public class WildFlower : LayerGeneric
    {
        private Texture2D _image;

        public WildFlower(GraphicsDevice graphicsDevice, Texture2D img, Rectangle rect, SpriteGroup groups, Settings settings, int z, SpriteGroup collisionSprites, Rectangle rectCus) : base(graphicsDevice, img, rect, groups, settings, z, collisionSprites)
        {
            _image = img;
            Hitbox = rectCus;
            HitboxStt = true;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_image, offsetTmp, Color.White);
            spriteBatch.End();
        }
    }

    public class Partical : LayerGeneric
    {
        Stopwatch stopwatch = new Stopwatch();
        private float _startTime;
        private float _duration;
        private SpriteGroup _groups;
        private Texture2D _image;

        public Partical(GraphicsDevice graphicsDevice, Vector2 pos, SpriteGroup groups, int z, int duaration, string name, Settings settings) : base(graphicsDevice, pos, groups, z)
        {
            stopwatch.Start();
            _startTime = stopwatch.ElapsedMilliseconds;
            _duration = duaration;
            _groups = groups;
            Z = z;

            if (name == "apple") _image = Texture2D.FromFile(graphicsDevice, $"{settings.Path}graphics/partical/apple.png");
            else if (name == "large") _image = Texture2D.FromFile(graphicsDevice, $"{settings.Path}graphics/partical/large.png");
            else if (name == "small") _image = Texture2D.FromFile(graphicsDevice, $"{settings.Path}graphics/partical/small.png");
            else if (name == "corn") _image = Texture2D.FromFile(graphicsDevice, $"{settings.Path}graphics/partical/corn.png");
            else if (name == "tomato") _image = Texture2D.FromFile(graphicsDevice, $"{settings.Path}graphics/partical/tomato.png");
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_image, offsetTmp, Color.White);
            spriteBatch.End();
        }

        public override void Update(float dt)
        {
            float currentTime = stopwatch.ElapsedMilliseconds;
            if (currentTime - _startTime >= _duration)
            {
                _groups.Remove(this);
            }
        }
    }

    public class Apple : Sprite
    {
        Texture2D _image;
        public Apple(GraphicsDevice graphicsDevice, Texture2D img, Vector2 pos, SpriteGroup groups, SpriteGroup appleSprites, int z) : base(graphicsDevice, new(pos.X, pos.Y))
        {
            groups.Add(this);
            appleSprites.Add(this);
            _image = img;
            X = (int)pos.X;
            Y = (int)pos.Y;
            Z = z;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_image, offsetTmp, Color.White);
            spriteBatch.End();
        }
    }

    public class Tree : LayerGeneric
    {
        private Settings _settings;
        GraphicsDevice _graphicsDevice;
        private Texture2D _image;
        private Texture2D _appleSurf;
        private List<(float, float)> _applePos;
        private string _name;
        private SpriteGroup _appleSprites;
        private int _health = 5;
        private bool _alive = true;
        private Texture2D _stumpSurf;
        private string _stumpPath;
        private Timer _invulTimer;
        private SpriteGroup _group;
        private Action<string> PlayerAdd;
        Rectangle _rect;

        public Tree(GraphicsDevice graphicsDevice, Texture2D img, Rectangle rect, SpriteGroup groups, Settings settings, int z, SpriteGroup collisionSprites, Rectangle rectCus, string name, SpriteGroup treeSprites, Action<string> PlayerAdd) : base(graphicsDevice, img, rect, groups, settings, z, collisionSprites)
        {
            _graphicsDevice = graphicsDevice;

            _group = groups;
            treeSprites.Add(this);

            _settings = settings;
            _name = name;
            _image = img;
            Hitbox = rectCus;
            HitboxStt = true;

            _rect = rect;

            //tree attr
            _stumpPath = $"{_settings.Path}graphics/stumps/";
            if (_name == "Small")
            {
                _stumpSurf = Texture2D.FromFile(graphicsDevice, $"{_stumpPath}small.png");
            }
            else if (_name == "Large")
            {
                _stumpSurf = Texture2D.FromFile(graphicsDevice, $"{_stumpPath}large.png");
            }
            _invulTimer = new(200, doNothing);

            //apples
            _appleSurf = Texture2D.FromFile(graphicsDevice, $"{_settings.Path}graphics/fruit/apple.png");
            _applePos = settings.APPLE_POS[_name];
            _appleSprites = new();
            CreateFruit();

            this.PlayerAdd = PlayerAdd;
        }

        Action doNothing = () => { };

        public SpriteGroup GetAppleSprites { get => _appleSprites; }
        public Rectangle TreeRect { get => new(this.X, this.Y, _image.Width, _image.Height); }

        public void Damage()
        {
            _health -= 1;

            Game1.axeSound.CreateInstance().Play();

            Random rnd = new Random();
            if (_appleSprites.GetSprites.Count > 0)
            {
                int mIndex = rnd.Next(_appleSprites.GetSprites.Count);
                Sprite apple = _appleSprites.GetSprites[mIndex];
                new Partical(_graphicsDevice, new Vector2(_appleSprites.GetSprites[mIndex].X, _appleSprites.GetSprites[mIndex].Y), _group, _settings.LAYERS["fruit"], 200, "apple", _settings);
                PlayerAdd("apple");
                _group.Remove(apple);
                _appleSprites.GetSprites.RemoveAt(mIndex);
            }
        }

        public void CheckDeath()
        {
            if (_health <= 0)
            {
                new Partical(_graphicsDevice, new Vector2(X, Y), _group, _settings.LAYERS["fruit"], 300, _name.ToLower(), _settings);
                _image = _stumpSurf;
                if (_name == "Small") Hitbox = new Rectangle(Hitbox.X, Hitbox.Y + 64, 156, 64);
                else Hitbox = new Rectangle(Hitbox.X + 4, Hitbox.Y + 64, 168, 64);
                Y += 64;
                _alive = false;
                PlayerAdd("wood");
            }
        }

        public void CreateFruit()
        {
            var rand = new Random();
            foreach ((float, float) pos in _applePos)
            {
                if (_health > 0)
                {
                    if (rand.Next(10) <= 2)
                    {
                        int x = (int)pos.Item1 + _rect.X + 24;
                        int y = (int)pos.Item2 + _rect.Y + 38;
                        Apple tmp = new(_graphicsDevice, _appleSurf, new(x, y), _group, _appleSprites, _settings.LAYERS["fruit"]);
                    }
                }
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
            if (_alive) CheckDeath();
        }
    }
}

