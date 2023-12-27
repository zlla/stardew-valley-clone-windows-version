using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Tiled;
using TiledCS;

namespace StardewValleyClone
{
    public class Level
    {
        GraphicsDevice _graphicsDevice;
        private Settings _settings;
        private SpriteGroup _allSprites = new();
        private SpriteGroup _collisionSprites = new();
        private SpriteGroup _treeSprites = new();
        private SpriteGroup _interActionSprites = new();
        private Overlay _overlay;
        private Player _player;
        private Vector2 _offset;
        private Transition _transition;
        private Soil _soilLayer;
        private Sky _rain;
        private bool _raining = false;
        Random rnd = new Random();
        private bool _shopActive = false;
        private Menu _menu;

        public Level(GraphicsDevice graphicsDevice, Settings settings)
        {
            _graphicsDevice = graphicsDevice;
            _settings = settings;
            _soilLayer = new(_allSprites, graphicsDevice, _settings);
            Setup();
            _transition = new(_graphicsDevice, _settings, Reset, _player);
            _overlay = new(_player, _settings, graphicsDevice);

            //sky
            if (rnd.Next(0, 10) > 7)
            {
                _raining = true;
            }

            _rain = new(_allSprites, graphicsDevice, _settings);
            _soilLayer.raining = _raining;

            _menu = new(_settings, _player, ToggleShop, _graphicsDevice);

            SoundEffectInstance soundInstance = Game1.music.CreateInstance();
            soundInstance.Volume = 0.3f;
            soundInstance.IsLooped = true;
            soundInstance.Play();
        }

        public void Setup()
        {
            //house
            foreach (TiledLayer layer in Game1._map.Layers)
            {
                //6 & 8
                if (layer.name == "HouseFloor")
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
                            int tileFrame = gid;
                            int column = tileFrame % Game1.HouseFloorTileTilesWide;
                            int row = (int)Math.Floor((double)tileFrame / (double)Game1.HouseFloorTileTilesWide);

                            float x = (i % Game1._map.Width) * Game1._map.TileWidth;
                            float y = (float)Math.Floor(i / (double)Game1._map.Width) * Game1._map.TileHeight;

                            Rectangle tilesetRec = new Rectangle(Game1.TileWidth * 1, Game1.TileHeight * 2, Game1.TileWidth, Game1.TileHeight);
                            LayerGeneric tmp = new(_graphicsDevice, Game1.HouseFloorTexture, new Rectangle((int)x, (int)y, Game1.TileWidth, Game1.TileHeight), tilesetRec, _allSprites, _settings, _settings.LAYERS["house bottom"]);
                        }
                    }
                }
                else if (layer.name == "HouseFurnitureBottom")
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
                            int tileFrame = gid;
                            int column = tileFrame % Game1.HouseFurnitureBottomTileTilesWide;
                            int row = (int)Math.Floor((double)tileFrame / (double)Game1.HouseFurnitureBottomTileTilesWide);

                            float x = (i % Game1._map.Width) * Game1._map.TileWidth;
                            float y = (float)Math.Floor(i / (double)Game1._map.Width) * Game1._map.TileHeight;

                            Rectangle tilesetRec = new Rectangle(Game1.TileWidth * column, Game1.TileHeight * row, Game1.TileWidth, Game1.TileHeight);
                            tilesetRec.Y = 5 * 64;
                            LayerGeneric tmp = new(_graphicsDevice, Game1.HouseFurnitureBottomTexture, new Rectangle((int)x, (int)y, Game1.TileWidth, Game1.TileHeight), tilesetRec, _allSprites, _settings, _settings.LAYERS["house bottom"]);
                        }
                    }
                }

                if (layer.name == "HouseWalls")
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
                            int tileFrame = gid;
                            int column = tileFrame % Game1.HouseWallsTileTilesWide;
                            int row = (int)Math.Floor((double)tileFrame / (double)Game1.HouseWallsTileTilesWide);

                            float x = (i % Game1._map.Width) * Game1._map.TileWidth;
                            float y = (float)Math.Floor(i / (double)Game1._map.Width) * Game1._map.TileHeight;

                            column -= 4;
                            row -= 24;

                            Rectangle tilesetRec = new Rectangle(Game1.TileWidth * column, Game1.TileHeight * row, Game1.TileWidth, Game1.TileHeight);
                            LayerGeneric tmp = new(_graphicsDevice, Game1.HouseWallsTexture, new Rectangle((int)x, (int)y, Game1.TileWidth, Game1.TileHeight), tilesetRec, _allSprites, _settings);
                        }
                    }
                }
                else if (layer.name == "HouseFurnitureTop")
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
                            int tileFrame = gid;
                            int column = tileFrame % Game1.HouseFurnitureTopTileTilesWide;
                            int row = (int)Math.Floor((double)tileFrame / (double)Game1.HouseFurnitureTopTileTilesWide);

                            float x = (i % Game1._map.Width) * Game1._map.TileWidth;
                            float y = (float)Math.Floor(i / (double)Game1._map.Width) * Game1._map.TileHeight;

                            row -= 23;
                            column += 0;

                            Rectangle tilesetRec = new Rectangle(Game1.TileWidth * column, Game1.TileHeight * row, Game1.TileWidth, Game1.TileHeight);
                            LayerGeneric tmp = new(_graphicsDevice, Game1.HouseFurnitureTopTexture, new Rectangle((int)x, (int)y, Game1.TileWidth, Game1.TileHeight), tilesetRec, _allSprites, _settings);
                        }
                    }
                }

                if (layer.name == "Fence")
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
                            int tileFrame = gid;
                            int column = tileFrame % Game1.FencesTileTilesWide;
                            int row = (int)Math.Floor((double)tileFrame / (double)Game1.FencesTileTilesWide);

                            float x = (i % Game1._map.Width) * Game1._map.TileWidth;
                            float y = (float)Math.Floor(i / (double)Game1._map.Width) * Game1._map.TileHeight;

                            column -= 1;
                            row -= 29;
                            x -= 32;
                            y -= 32;

                            Rectangle tilesetRec = new Rectangle(Game1.TileWidth * column, Game1.TileHeight * row, Game1.TileWidth, Game1.TileHeight);
                            LayerGeneric tmp = new(_graphicsDevice, Game1.FencesTexture, new Rectangle((int)x, (int)y, Game1.TileWidth, Game1.TileHeight), tilesetRec, _allSprites, _settings, _collisionSprites);
                        }
                    }
                }

                if (layer.name == "Water")
                {
                    List<Texture2D> waterFrames = Support.ImportFolder($"{_settings.Path}/graphics/water/", _graphicsDevice);

                    for (var i = 0; i < layer.data.Length; i++)
                    {
                        int gid = layer.data[i];

                        // Empty tile, do nothing
                        if (gid == 0)
                        {

                        }
                        else
                        {
                            int tileFrame = gid;
                            float x = (i % Game1._map.Width) * Game1._map.TileWidth;
                            float y = (float)Math.Floor(i / (double)Game1._map.Width) * Game1._map.TileHeight;

                            Rectangle tilesetRec = new Rectangle(Game1.TileWidth * 0, Game1.TileHeight * 0, Game1.TileWidth, Game1.TileHeight);
                            y -= 32;
                            Water tmp = new(_graphicsDevice, waterFrames, new Rectangle((int)x, (int)y, Game1.TileWidth, Game1.TileHeight), tilesetRec, _allSprites, _settings);
                        }
                    }
                }

                if (layer.name == "Decoration")
                {
                    foreach (TiledObject obj in layer.objects)
                    {
                        if (obj.name == "1")
                        {
                            Texture2D img = Texture2D.FromFile(_graphicsDevice, $"{_settings.Path}graphics/objects/sunflower.png");
                            Rectangle rectCus = new((int)obj.x - 64 - img.Width - 18, (int)obj.y - 64 * 3, (int)(img.Width * 2.9 + 18), (int)(img.Height * 1.3 - 2));
                            WildFlower tmp = new(_graphicsDevice, img, new Rectangle((int)obj.x - 64, (int)obj.y - 64 * 3, img.Width, img.Height), _allSprites, _settings, _settings.LAYERS["main"], _collisionSprites, rectCus);
                        }
                        else if (obj.name == "2")
                        {
                            Texture2D img = Texture2D.FromFile(_graphicsDevice, $"{_settings.Path}graphics/objects/bush.png");
                            Rectangle rectCus = new((int)obj.x - img.Width - 10, (int)obj.y - 90, (int)(img.Width * 2.9 + 12), (int)(img.Height * 1.3 + 8));
                            WildFlower tmp = new(_graphicsDevice, img, new Rectangle((int)obj.x, (int)obj.y - 64, img.Width, img.Height), _allSprites, _settings, _settings.LAYERS["main"], _collisionSprites, rectCus);
                        }
                        else if (obj.name == "3")
                        {
                            Texture2D img = Texture2D.FromFile(_graphicsDevice, $"{_settings.Path}graphics/objects/flower.png");
                            Rectangle rectCus = new((int)obj.x - img.Width - 32, (int)obj.y - 96, (int)(img.Width * 2.9 + 34), (int)(img.Height * 1.3 + 8));
                            WildFlower tmp = new(_graphicsDevice, img, new Rectangle((int)obj.x, (int)obj.y - 64, img.Width, img.Height), _allSprites, _settings, _settings.LAYERS["main"], _collisionSprites, rectCus);
                        }
                        else if (obj.name == "4")
                        {
                            Texture2D img = Texture2D.FromFile(_graphicsDevice, $"{_settings.Path}graphics/objects/mushroom.png");
                            Rectangle rectCus = new((int)obj.x - img.Width - 32, (int)obj.y - 96, (int)(img.Width * 2.9 + 34), (int)(img.Height * 1.3 + 8));
                            WildFlower tmp = new(_graphicsDevice, img, new Rectangle((int)obj.x, (int)obj.y - 64, img.Width, img.Height), _allSprites, _settings, _settings.LAYERS["main"], _collisionSprites, rectCus);
                        }
                        else if (obj.name == "5")
                        {
                            Texture2D img = Texture2D.FromFile(_graphicsDevice, $"{_settings.Path}graphics/objects/mushrooms.png");
                            Rectangle rectCus = new((int)obj.x - img.Width - 32, (int)obj.y - 96, (int)(img.Width * 2.9 + 34), (int)(img.Height * 1.3 + 8));
                            WildFlower tmp = new(_graphicsDevice, img, new Rectangle((int)obj.x, (int)obj.y - 64, img.Width, img.Height), _allSprites, _settings, _settings.LAYERS["main"], _collisionSprites, rectCus);
                        }
                    }
                }

                if (layer.name == "Trees")
                {
                    foreach (TiledObject obj in layer.objects)
                    {
                        if (obj.name == "Small")
                        {
                            Texture2D img = Texture2D.FromFile(_graphicsDevice, $"{_settings.Path}graphics/objects/tree_small.png");
                            Rectangle rectCus = new((int)obj.x - 64 - img.Width - 18, (int)obj.y - 64 * 3, (int)(img.Width * 2.9 + 18), (int)(img.Height * 1.3 - 2));
                            Tree tmp = new(_graphicsDevice, img, new Rectangle((int)obj.x - 64, (int)obj.y - 64 * 3, img.Width, img.Height), _allSprites, _settings, _settings.LAYERS["main"], _collisionSprites, rectCus, obj.name, _treeSprites, PlayerAdd);
                        }
                        else if (obj.name == "Large")
                        {
                            Texture2D img = Texture2D.FromFile(_graphicsDevice, $"{_settings.Path}graphics/objects/tree_medium.png");
                            Rectangle rectCus = new((int)obj.x - img.Width - 28, (int)obj.y - 64 * 3 + 4, (int)(img.Width * 2.9 - 48), (int)(img.Height * 1.3 - 2));
                            Tree tmp = new(_graphicsDevice, img, new Rectangle((int)obj.x - 64, (int)obj.y - 64 * 3, img.Width, img.Height), _allSprites, _settings, _settings.LAYERS["main"], _collisionSprites, rectCus, obj.name, _treeSprites, PlayerAdd);
                        }
                    }
                }

                if (layer.name == "Collision")
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
                            int tileFrame = gid;
                            int column = tileFrame % 1;
                            int row = (int)Math.Floor((double)tileFrame / 1);

                            float x = (i % Game1._map.Width) * Game1._map.TileWidth - 32;
                            float y = (float)Math.Floor(i / (double)Game1._map.Width) * Game1._map.TileHeight;

                            LayerGeneric tmp = new(_graphicsDevice, new Texture2D(_graphicsDevice, 64, 64), new Rectangle((int)x, (int)y, Game1.TileWidth, Game1.TileHeight), _allSprites, _collisionSprites);
                        }
                    }
                }

                if (layer.name == "Player")
                {
                    foreach (TiledObject obj in layer.objects)
                    {
                        if (obj.name == "Start")
                        {
                            _player = new(_graphicsDevice, $"{_settings.Path}graphics/character/down_idle/0.png", new(obj.x, obj.y), _allSprites, _settings, _collisionSprites, _treeSprites, _interActionSprites, _soilLayer, ToggleShop);
                        }
                        if (obj.name == "Bed")
                        {
                            Interaction tmp = new(_graphicsDevice, new((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height), _interActionSprites, obj.name);
                        }
                        if (obj.name == "Trader")
                        {
                            Interaction tmp = new(_graphicsDevice, new((int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height), _interActionSprites, obj.name);
                        }
                    }
                }
            }

            Generic ground = new(_graphicsDevice, $"{_settings.Path}graphics/world/ground.png", new(0, 0), _allSprites, _settings, _settings.LAYERS["ground"]);
        }

        public void PlayerAdd(string item)
        {
            _player.ItemInventory[item.ToLower()] += 1;
            Game1.successSound.CreateInstance().Play();
        }

        public void ToggleShop()
        {
            _shopActive = !_shopActive;
        }

        public void Reset()
        {
            //plants
            _soilLayer.UpdatePlants();

            //soil
            _soilLayer.RemoveWater();
            if (rnd.Next(0, 10) > 4)
            {
                _raining = true;
            }
            else _raining = false;
            _soilLayer.raining = _raining;
            if (_raining) _soilLayer.WaterAll();

            //apples on trees
            foreach (Tree tree in _treeSprites.GetSprites)
            {
                foreach (Apple apple in tree.GetAppleSprites.GetSprites.ToList())
                {
                    tree.GetAppleSprites.Remove(apple);
                    _allSprites.Remove(apple);
                }

                tree.CreateFruit();
            }
        }

        public void PlantCollision()
        {
            if (_soilLayer.PlantSprites.GetSprites.Count > 0)
            {
                foreach (Plant plant in _soilLayer.PlantSprites.GetSprites.ToList())
                {
                    if (plant.Harvestable && plant.Rect.Intersects(new Rectangle(_player.X, _player.Y, 64, 64)))
                    {
                        PlayerAdd(plant.Name);
                        _allSprites.Remove(plant);
                        _soilLayer.PlantSprites.Remove(plant);
                        new Partical(_graphicsDevice, new Vector2(plant.Rect.X, plant.Rect.Y), _allSprites, _settings.LAYERS["main"], 200, plant.Name.ToLower(), _settings);
                        _soilLayer.Grid[plant.Rect.Y / _settings.TILE_SIZE][plant.Rect.X / _settings.TILE_SIZE].Remove("P");
                    }
                }
            }
        }

        public void Run(float dt)
        {
            //_allSprites.Update(dt);
            //PlantCollision();
        }

        public void Draw(float dt, SpriteBatch spriteBatch)
        {
            _offset.X = _player.X - _settings.SCREEN_WIDTH / 2;
            _offset.Y = _player.Y - _settings.SCREEN_HEIGHT / 2;

            foreach (int layer in _settings.LAYERS.Values)
            {
                var orderedSprites = _allSprites.GetSprites.Where(s => s.Z == layer).OrderBy(s => s.Y);
                foreach (Sprite s in orderedSprites)
                {
                    Vector2 offsetTmp = new();
                    offsetTmp.X = s.X;
                    offsetTmp.Y = s.Y;
                    offsetTmp.X -= _offset.X;
                    offsetTmp.Y -= _offset.Y;

                    s.Draw(spriteBatch, offsetTmp);
                }
            }

            if (_shopActive)
            {
                _menu.Update(spriteBatch);
            }
            else
            {
                _allSprites.Update(dt);
                PlantCollision();
            }

            _overlay.Display(spriteBatch);
            if (_raining && !_shopActive)
            {
                _rain.Update();
            }

            if (_player.sleep)
            {
                _transition.Play(spriteBatch);
            }
        }
    }
}

