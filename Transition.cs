using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValleyClone
{
    public class Transition
    {
        private GraphicsDevice _graphicsDevice;
        private Action Reset;
        private Player _player;

        private Texture2D _image;
        private int _color = 255;
        private int _speed = -2;

        public Transition(GraphicsDevice graphicsDevice, Settings settings, Action Reset, Player player)
        {
            _graphicsDevice = graphicsDevice;
            this.Reset = Reset;
            _player = player;

            _image = new Texture2D(graphicsDevice, 1, 1);
            _image.SetData(new Color[] { new Color(255, 255, 255) });
        }

        public void Play(SpriteBatch spriteBatch)
        {
            if (_color == 255 && _speed < 0)
            {
                System.Threading.Thread.Sleep(50);
            }

            _color += _speed;

            if (_color <= 0)
            {
                _speed *= -1;
                _color = 0;
                Reset();
            }

            if (_color >= 255)
            {
                _color = 255;
                _player.sleep = false;
                _speed = -2;
            }

            spriteBatch.Begin();
            spriteBatch.Draw(_image,
            new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height),
            new Color(
                (int)(Math.Sin(_color * Math.PI / 510) * 255),
                (int)(Math.Sin(_color * Math.PI / 510) * 255),
                (int)(Math.Sin(_color * Math.PI / 510) * 255)
                ));
            spriteBatch.End();
        }
    }
}
