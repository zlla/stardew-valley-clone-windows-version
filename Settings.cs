using System;
using System.Collections.Generic;
using System.Numerics;

namespace StardewValleyClone
{
    public class Settings
    {
        private string _path = "C:\\Users\\hoangan\\Downloads\\StardewValleyClone\\StardewValleyClone\\";

        // screen
        private int _screenWidth;
        private int _screenHeight;
        private int _tileSize;

        private Dictionary<string, Tuple<int, int>> _overlayPositions = new();
        private Dictionary<string, Vector2> _playerToolOffset = new();
        private Dictionary<string, int> _layers = new();
        private Dictionary<string, List<(float, float)>> _applePos = new();
        private Dictionary<string, double> _growSpeed = new();
        private Dictionary<string, float> _salePrices = new();
        private Dictionary<string, float> _purchasePrices = new();

        public Settings()
        {
            _screenWidth = 1280;
            _screenHeight = 720;
            _tileSize = 64;

            _overlayPositions.Add("tool", Tuple.Create(10, _screenHeight - 70));
            _overlayPositions.Add("seed", Tuple.Create(80, _screenHeight - 70));

            Vector2 leftVector = new Vector2();
            leftVector.X = (float)-40;
            leftVector.Y = (float)20;
            _playerToolOffset.Add("left", leftVector);
            Vector2 rightVector = new Vector2();
            rightVector.X = (float)40;
            rightVector.Y = (float)20;
            _playerToolOffset.Add("right", rightVector);
            Vector2 upVector = new Vector2();
            upVector.X = (float)0;
            upVector.Y = (float)-10;
            _playerToolOffset.Add("up", upVector);
            Vector2 downVector = new Vector2();
            downVector.X = (float)0;
            downVector.Y = (float)50;
            _playerToolOffset.Add("down", downVector);

            _layers.Add("water", 0);
            _layers.Add("ground", 1);
            _layers.Add("soil", 2);
            _layers.Add("soil water", 3);
            _layers.Add("rain floor", 4);
            _layers.Add("house bottom", 5);
            _layers.Add("ground plant", 6);
            _layers.Add("main", 7);
            _layers.Add("house top", 8);
            _layers.Add("fruit", 9);
            _layers.Add("rain drops", 10);

            _applePos.Add("Small", new List<(float, float)> { (18, 17), (30, 37), (12, 50), (30, 45), (20, 30), (30, 10) });
            _applePos.Add("Large", new List<(float, float)> { (30, 24), (60, 65), (50, 50), (16, 40), (45, 50), (42, 70) });

            _growSpeed.Add("corn", 1);
            _growSpeed.Add("tomato", 0.7);

            _salePrices.Add("wood", 4);
            _salePrices.Add("apple", 2);
            _salePrices.Add("corn", 10);
            _salePrices.Add("tomato", 20);

            _purchasePrices.Add("corn", 4);
            _purchasePrices.Add("tomato", 5);

        }

        public string Path { get => _path; }
        public int SCREEN_WIDTH { get => _screenWidth; }
        public int SCREEN_HEIGHT { get => _screenHeight; }
        public int TILE_SIZE { get => _tileSize; }
        public Dictionary<string, Tuple<int, int>> OVERLAY_POSITIONS { get => _overlayPositions; }
        public Dictionary<string, Vector2> PLAYER_TOOL_OFFSET { get => _playerToolOffset; }
        public Dictionary<string, int> LAYERS { get => _layers; }
        public Dictionary<string, List<(float, float)>> APPLE_POS { get => _applePos; }
        public Dictionary<string, double> GROW_SPEED { get => _growSpeed; }
        public Dictionary<string, float> SALE_PRICES { get => _salePrices; }
        public Dictionary<string, float> PURCHASE_PRICES { get => _purchasePrices; }
    }
}

