using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValleyClone
{
    public class Sprite
    {
        private Texture2D _image;
        private Rectangle _rectangle;
        private Color _color = Color.White;
        private int _x;
        private int _y;
        private int _z;
        private bool _hitboxStt = false;
        private Rectangle _hitbox;

        public Sprite(GraphicsDevice graphicsDevice, string path, Vector2 position)
        {
            _image = Texture2D.FromFile(graphicsDevice, path);
            _rectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (_image.Width),
                (_image.Height));
            _x = _rectangle.X;
            _y = _rectangle.Y;
        }

        public Sprite(GraphicsDevice graphicsDevice, Vector2 position)
        {
            _image = new(graphicsDevice, 1, 1);
            _rectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (_image.Width),
                (_image.Height));
            _x = _rectangle.X;
            _y = _rectangle.Y;
        }

        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public int Z { get => _z; set => _z = value; }
        public Texture2D Image { get => _image; set => _image = value; }
        public bool HitboxStt { get => _hitboxStt; set => _hitboxStt = value; }
        public Rectangle Hitbox { get => _hitbox; set => _hitbox = value; }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offsetTmp) { }
        public virtual void Update(float dt) { }
    }
}
