using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValleyClone
{
    public class Player : Sprite
    {
        private GraphicsDevice _graphicsDevice;
        private Settings _settings;
        private Dictionary<string, List<Texture2D>> _animations = new();
        private string _status;
        private float _frameIndex;
        private Texture2D _image;
        private Rectangle _rectangle;
        private Color _color = Color.White;
        private Vector2 _direction = new(0, 0);
        private Vector2 _pos = new(0, 0);
        private int _speed = 50;
        private string[] _tools;
        private int _toolIndex;
        private string _selectedTool;
        private string[] _seeds;
        private int _seedIndex;
        private string _selectedSeed;
        private Dictionary<string, Timer> _timers = new();
        private Rectangle _hitbox;
        private SpriteGroup _collisionSprites;
        private SpriteGroup _treeSprites;
        private Rectangle _targetPos;
        private Dictionary<string, int> _itemInventory;
        private SpriteGroup _interActionSprites;
        public bool sleep = false;
        private Soil _soilLayer;
        private Dictionary<string, int> _seedInventory = new();
        private float _money = 200;
        private Action _ToggleShop;

        public Player(GraphicsDevice graphicsDevice, string path, Vector2 pos, SpriteGroup group, Settings settings, SpriteGroup collisionSprites, SpriteGroup treeSprites, SpriteGroup interActionSprites, Soil soilLayer, Action ToggleShop) : base(graphicsDevice, path, pos)
        {
            _collisionSprites = collisionSprites;
            _treeSprites = treeSprites;

            _graphicsDevice = graphicsDevice;
            _settings = settings;

            group.Add(this);

            ImportAsserts();
            _status = "down_idle";
            _frameIndex = 0;

            //general setup
            _image = _animations[_status][(int)_frameIndex];
            Z = _settings.LAYERS["main"];
            _rectangle = new Rectangle(
                (int)pos.X - _image.Width / 2,
                (int)pos.Y - _image.Height / 2,
                _image.Width,
                _image.Height);
            _hitbox = _rectangle;
            _hitbox.Inflate(-126, -70);

            //movement attributes
            _pos.X = _rectangle.X + _image.Width / 2;
            _pos.Y = _rectangle.Y + _image.Height / 2;

            //tool
            _tools = new[] { "hoe", "axe", "water" };
            _toolIndex = 1;
            _selectedTool = _tools[_toolIndex];

            _seeds = new[] { "corn", "tomato" };
            _seedIndex = 0;
            _selectedSeed = _seeds[_seedIndex];

            _timers.Add("tooluse", new Timer(350, UseTool));
            _timers.Add("toolswitch", new Timer(350, doNothing));
            _timers.Add("seeduse", new Timer(350, UseSeed));
            _timers.Add("seedswitch", new Timer(350, doNothing));

            _itemInventory = new();
            _itemInventory.Add("wood", 0);
            _itemInventory.Add("apple", 0);
            _itemInventory.Add("corn", 0);
            _itemInventory.Add("tomato", 0);

            _interActionSprites = interActionSprites;

            _soilLayer = soilLayer;

            _seedInventory.Add("corn", 5);
            _seedInventory.Add("tomato", 5);

            _ToggleShop = ToggleShop;
        }

        public Dictionary<string, int> ItemInventory { get => _itemInventory; }
        public Dictionary<string, int> SeedInventory { get => _seedInventory; }
        public float Money { get => _money; set => _money = value; }

        Action doNothing = () => { };

        public void UseTool()
        {
            if (SelectedTool == "hoe")
            {
                _soilLayer.GetHit(_targetPos);
            }
            if (SelectedTool == "axe")
            {
                foreach (Tree tree in _treeSprites.GetSprites)
                {
                    if (tree.TreeRect.Intersects(_targetPos))
                    {
                        tree.Damage();
                    }
                }
            }
            if (SelectedTool == "water")
            {
                _soilLayer.Water(_targetPos);
                Game1.watering.CreateInstance().Play();
            }
        }

        public void GetTargetPos()
        {
            _targetPos.X = (int)_pos.X + (int)_settings.PLAYER_TOOL_OFFSET[_status.Split(new string[] { "_" }, StringSplitOptions.None)[0]].X;
            _targetPos.Y = (int)_pos.Y + (int)_settings.PLAYER_TOOL_OFFSET[_status.Split(new string[] { "_" }, StringSplitOptions.None)[0]].Y;
            _targetPos.Width = 0;
            _targetPos.Height = 0;
        }

        public void UseSeed()
        {
            if (_seedInventory[_selectedSeed] > 0)
            {
                _soilLayer.PlantSeed(_targetPos, _selectedSeed);
                _seedInventory[_selectedSeed] -= 1;
            }
        }

        public void ImportAsserts()
        {
            _animations.Add("up", new List<Texture2D> { });
            _animations.Add("down", new List<Texture2D> { });
            _animations.Add("left", new List<Texture2D> { });
            _animations.Add("right", new List<Texture2D> { });

            _animations.Add("right_idle", new List<Texture2D> { });
            _animations.Add("left_idle", new List<Texture2D> { });
            _animations.Add("up_idle", new List<Texture2D> { });
            _animations.Add("down_idle", new List<Texture2D> { });

            _animations.Add("right_hoe", new List<Texture2D> { });
            _animations.Add("left_hoe", new List<Texture2D> { });
            _animations.Add("up_hoe", new List<Texture2D> { });
            _animations.Add("down_hoe", new List<Texture2D> { });

            _animations.Add("right_axe", new List<Texture2D> { });
            _animations.Add("left_axe", new List<Texture2D> { });
            _animations.Add("up_axe", new List<Texture2D> { });
            _animations.Add("down_axe", new List<Texture2D> { });

            _animations.Add("right_water", new List<Texture2D> { });
            _animations.Add("left_water", new List<Texture2D> { });
            _animations.Add("up_water", new List<Texture2D> { });
            _animations.Add("down_water", new List<Texture2D> { });

            foreach (string animation in _animations.Keys)
            {
                string fullPath = $"{_settings.Path}graphics/character/{animation}";
                _animations[animation] = Support.ImportFolder(fullPath, _graphicsDevice);
            }
        }

        public void Animate(float dt)
        {
            _frameIndex += 2 * dt;
            if (_frameIndex >= _animations[_status].Count) _frameIndex = 0;

            _image = _animations[_status][(int)_frameIndex];
        }

        public void Input()
        {
            if (!_timers["tooluse"].Active && !sleep)
            {
                //directions
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    _direction.Y = -1;
                    _status = "up";
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    _direction.Y = 1;
                    _status = "down";
                }
                else
                {
                    _direction.Y = 0;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    _direction.X = -1;
                    _status = "left";
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    _direction.X = 1;
                    _status = "right";
                }
                else
                {
                    _direction.X = 0;
                }

                //tool use
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    _timers["tooluse"].Activate();
                    _direction.X = 0;
                    _direction.Y = 0;
                    _frameIndex = 0;
                }

                //change tool
                if (Keyboard.GetState().IsKeyDown(Keys.W) && !_timers["toolswitch"].Active)
                {
                    _timers["toolswitch"].Activate();
                    _toolIndex += 1;
                    if (_toolIndex >= _tools.Length) _toolIndex = 0;
                    _selectedTool = _tools[_toolIndex];
                }

                //seed use
                if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    _timers["seeduse"].Activate();
                    _direction.X = 0;
                    _direction.Y = 0;
                    _frameIndex = 0;
                }

                //change seed
                if (Keyboard.GetState().IsKeyDown(Keys.R) && !_timers["seedswitch"].Active)
                {
                    _timers["seedswitch"].Activate();
                    _seedIndex += 1;
                    if (_seedIndex >= _seeds.Length) _seedIndex = 0;
                    _selectedSeed = _seeds[_seedIndex];
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    foreach (Interaction sprite in _interActionSprites.GetSprites)
                    {
                        Rectangle rect = new(X, Y, 2, 64);

                        if (rect.Intersects(sprite.GetRect))
                        {
                            if (sprite.Name == "Trader")
                            {
                                _ToggleShop();
                            }
                            else if (sprite.Name == "Bed")
                            {
                                _status = "left_idle";
                                sleep = true;
                            }
                        }
                    }
                }
            }
        }

        public string SelectedTool { get => _selectedTool; }
        public string SelectedSeed { get => _selectedSeed; }
        public string[] Tools { get => _tools; }
        public string[] Seeds { get => _seeds; }

        public void GetStatus()
        {
            //idle
            double directionLength = Math.Sqrt(_direction.X * _direction.X + _direction.Y * _direction.Y);
            if (directionLength == 0)
            {
                _status = _status.Split(new string[] { "_" }, StringSplitOptions.None)[0] + "_idle";
            }

            //tool use
            if (_timers["tooluse"].Active)
            {
                _status = _status.Split(new string[] { "_" }, StringSplitOptions.None)[0] + "_" + _selectedTool;
            }
        }

        public void UpdateTimers()
        {
            foreach (Timer timer in _timers.Values)
            {
                timer.Update();
            }
        }

        public void Collision(string direction)
        {
            foreach (Sprite s in _collisionSprites.GetSprites)
            {
                if (s.HitboxStt)
                {
                    if (_hitbox.Intersects(s.Hitbox))
                    {
                        if (direction == "horizontal")
                        {
                            if (_direction.X == 1)
                            {
                                _hitbox.X = s.Hitbox.Left - _hitbox.Width;
                            }
                            else if (_direction.X == -1)
                            {
                                _hitbox.X = s.Hitbox.Right;
                            }
                        }

                        if (direction == "vertical")
                        {
                            if (_direction.Y == 1)
                            {
                                _hitbox.Y = s.Hitbox.Top - _hitbox.Height;
                            }
                            else if (_direction.Y == -1)
                            {
                                _hitbox.Y = s.Hitbox.Bottom;
                            }
                        }
                    }
                }
            }

            _pos.X = _hitbox.X;
            X = _hitbox.X;
            _pos.Y = _hitbox.Y;
            Y = _hitbox.Y;
        }

        public void Move(float dt)
        {
            float directionLength = (float)Math.Sqrt(_direction.X * _direction.X + _direction.Y * _direction.Y);
            if (directionLength != 0)
            {
                // normalize vector
                float normalizedX = _direction.X / directionLength;
                float normalizedY = _direction.Y / directionLength;

                _pos.X += normalizedX * _speed * dt;
                _hitbox.X = (int)Math.Round(_pos.X, 1);
                Collision("horizontal");

                _pos.Y += normalizedY * _speed * dt;
                _hitbox.Y = (int)Math.Round(_pos.Y, 0);
                Collision("vertical");

                X = _hitbox.X;
                Y = _hitbox.Y;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp)
        {
            offsetTmp.X -= _image.Width / 2;
            offsetTmp.Y -= _image.Height / 2;
            spriteBatch.Begin();
            spriteBatch.Draw(_image, offsetTmp, _color);
            spriteBatch.End();
        }

        public override void Update(float dt)
        {
            Input();
            GetStatus();
            UpdateTimers();
            GetTargetPos();

            Move(dt);
            Animate(dt);
        }
    }
}

