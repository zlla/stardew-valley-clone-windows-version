using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValleyClone
{
    public class Overlay
    {
        private Settings _settings;
        private Support _support;
        private Player _player;
        private string _overlayPath;
        private Dictionary<string, Texture2D> _toolsSurf;
        private Dictionary<string, Texture2D> _seedsSurf;

        public Overlay(Player player, Settings settings, GraphicsDevice graphicsDevice)
        {
            _settings = settings;
            _support = new();
            _player = player;

            _overlayPath = $"{_settings.Path}graphics/overlay/";
            _toolsSurf = new();
            _seedsSurf = new();

            foreach (string fullPathImage in Directory.GetFiles(_overlayPath))
            {
                foreach (string tool in player.Tools)
                {
                    if (fullPathImage == $"{_overlayPath}{tool}.png")
                    {
                        _toolsSurf.Add(tool, (Texture2D.FromFile(graphicsDevice, fullPathImage)));
                    }
                }
                foreach (string seed in player.Seeds)
                {
                    if (fullPathImage == $"{_overlayPath}{seed}.png")
                    {
                        _seedsSurf.Add(seed, (Texture2D.FromFile(graphicsDevice, fullPathImage)));
                    }
                }
            }
        }

        public void Display(SpriteBatch spriteBatch)
        {
            Texture2D toolSurf = _toolsSurf[_player.SelectedTool];
            Rectangle toolRect = new(_settings.OVERLAY_POSITIONS["tool"].Item1, _settings.OVERLAY_POSITIONS["tool"].Item2, toolSurf.Width, toolSurf.Height);

            Texture2D seedSurf = _seedsSurf[_player.SelectedSeed];
            Rectangle seedRect = new(_settings.OVERLAY_POSITIONS["seed"].Item1, _settings.OVERLAY_POSITIONS["seed"].Item2, seedSurf.Width, seedSurf.Height);

            spriteBatch.Begin();
            spriteBatch.Draw(toolSurf, toolRect, Color.White);
            spriteBatch.Draw(seedSurf, seedRect, Color.White);
            spriteBatch.End();
        }

        public Dictionary<string, Texture2D> ToolsSurf { get => _toolsSurf; }
        public Dictionary<string, Texture2D> SeedsSurf { get => _seedsSurf; }
    }
}

