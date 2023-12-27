using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValleyClone
{
    public class Menu
    {
        private Settings _settings;
        private Player _player;
        private Action _ToggleMenu;
        private GraphicsDevice _graphicsDevice;

        private int _width = 400;
        private int _space = 10;
        private int _padding = 8;

        private List<string> _options = new();
        private int _sellBorder;

        private int _totalHeight = 0;
        private int _menuTop;
        private Rectangle _mainRect;

        private int _index;

        private Timer _timer;

        public Menu(Settings settings, Player player, Action ToggleMenu, GraphicsDevice graphicsDevice)
        {
            _settings = settings;
            _player = player;
            _ToggleMenu = ToggleMenu;
            _graphicsDevice = graphicsDevice;

            foreach (string key in _player.ItemInventory.Keys)
            {
                _options.Add(key);
            }

            foreach (string key in _player.SeedInventory.Keys)
            {
                _options.Add(key);
            }

            _sellBorder = _player.ItemInventory.Count - 1;

            Setup();

            _index = 0;
            Action doNothing = () => { };
            _timer = new(200, doNothing);
        }

        public void DisplayMoney(SpriteBatch spriteBatch)
        {
            string text = $"${_player.Money}";
            Vector2 textSize = Game1.spriteFont.MeasureString(text);
            Texture2D moneySurfBgr = new Texture2D(_graphicsDevice, (int)textSize.X + 40, (int)textSize.Y + 22);
            Color[] data = new Color[((int)textSize.X + 40) * ((int)textSize.Y + 22)];
            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;
            moneySurfBgr.SetData(data);

            spriteBatch.Draw(moneySurfBgr, new Vector2(_settings.SCREEN_WIDTH / 2 - (int)textSize.X / 2, _settings.SCREEN_HEIGHT - 60), Color.White);
            spriteBatch.DrawString(Game1.spriteFont, text, new Vector2(_settings.SCREEN_WIDTH / 2 - (int)textSize.X / 2 + 2, _settings.SCREEN_HEIGHT - 60), Color.Black, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
        }

        public void Setup()
        {
            for (int i = 0; i < _options.Count; i++)
            {
                _totalHeight += 30 + _padding * 2;
            }

            _totalHeight += _space * (_options.Count - 1);
            _menuTop = _settings.SCREEN_HEIGHT / 2 - _totalHeight / 2;
            _mainRect = new(_settings.SCREEN_WIDTH / 2 - _width / 2, _menuTop, _width, _totalHeight);
        }

        public void Input()
        {
            _timer.Update();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _ToggleMenu();
            }

            if (!_timer.Active)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    _index += 1;
                    if (_index > _options.Count - 1) _index = _options.Count - 1;
                    _timer.Activate();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    _index -= 1;
                    if (_index < 0) _index = 0;
                    _timer.Activate();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    _timer.Activate();

                    string currentItem = _options[_index];
                    if (_index <= _sellBorder)
                    {
                        if (_player.ItemInventory[currentItem] > 0)
                        {
                            _player.ItemInventory[currentItem] -= 1;
                            _player.Money += _settings.SALE_PRICES[currentItem];
                        }
                    }
                    else
                    {
                        float seedPrice = _settings.PURCHASE_PRICES[currentItem];
                        if (_player.Money >= seedPrice)
                        {
                            SoundEffectInstance soundInstance = Game1.buySound.CreateInstance();
                            soundInstance.Volume = 0.5f;
                            soundInstance.Play();
                            _player.SeedInventory[currentItem] += 1;
                            _player.Money -= _settings.PURCHASE_PRICES[currentItem];
                        }
                    }
                }
            }
        }

        public void ShowEntry(SpriteBatch spriteBatch, string text, int amount, int top, bool selected)
        {
            Texture2D bgr = new Texture2D(_graphicsDevice, _mainRect.Width, 30 + _padding * 2);
            Color[] data = new Color[_mainRect.Width * (30 + _padding * 2)];
            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;
            bgr.SetData(data);

            spriteBatch.Draw(bgr, new Vector2(_mainRect.X, top), Color.White);
            float scale = 2.5f;
            spriteBatch.DrawString(Game1.spriteFont, text, new Vector2(_mainRect.X + 20, top + 4), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Game1.spriteFont, amount.ToString(), new Vector2(_mainRect.X + _mainRect.Width - 50, top + 4), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            if (selected)
            {
                Rectangle rect = new Rectangle(_mainRect.X, top, _mainRect.Width, 30 + _padding * 2);
                Texture2D pixel = new Texture2D(_graphicsDevice, 1, 1);
                pixel.SetData(new[] { Color.White }); // Use white to fill the texture

                // Draw the border of the rectangle
                spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, 4), Color.Black); // Top
                spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, 4, rect.Height), Color.Black); // Left
                spriteBatch.Draw(pixel, new Rectangle(rect.X + rect.Width - 4, rect.Y, 4, rect.Height), Color.Black); // Right
                spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height - 4, rect.Width, 4), Color.Black); // Bottom

                if (_index <= _sellBorder)
                {
                    spriteBatch.DrawString(Game1.spriteFont, "Sell", new Vector2(_mainRect.X + _mainRect.Width - 200, top + 4), Color.Blue, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.DrawString(Game1.spriteFont, "Buy", new Vector2(_mainRect.X + _mainRect.Width - 200, top + 4), Color.Red, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
            }
        }
        public void Update(SpriteBatch spriteBatch)
        {
            Input();
            spriteBatch.Begin();
            for (int i = 0; i < _options.Count; i++)
            {
                int top = _mainRect.Y + i * (30 + _padding * 2 + _space);

                List<int> amountList = new();
                foreach (int value in _player.ItemInventory.Values)
                {
                    amountList.Add(value);
                }

                foreach (int value in _player.SeedInventory.Values)
                {
                    amountList.Add(value);
                }

                int amount = amountList[i];
                ShowEntry(spriteBatch, _options[i], amount, top, _index == i);
            }
            DisplayMoney(spriteBatch);
            spriteBatch.End();
        }
    }
}

