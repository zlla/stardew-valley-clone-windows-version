using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValleyClone
{
    public class SpriteGroup
    {
        private List<Sprite> _allSprites = new();

        public List<Sprite> GetSprites { get => _allSprites; }

        public SpriteGroup() { }

        public void Add(Sprite s)
        {
            _allSprites.Add(s);
        }

        public void Remove(Sprite s)
        {
            if (_allSprites.Contains(s))
            {
                _allSprites.Remove(s);
            }
        }

        public void RemoveAt(int index)
        {
            _allSprites.RemoveAt(index);
        }

        public void Update(float dt)
        {
            foreach (Sprite sprite in _allSprites.ToList())
            {
                sprite.Update(dt);
            }
        }
    }
}

