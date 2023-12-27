using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace StardewValleyClone
{
    public class Timer
    {
        private GameTime _gameTime = new();
        private float _duration;
        private float _startTime;
        private bool _active;
        public Action _func;
        Stopwatch stopwatch = new Stopwatch();

        public Timer(float duration, Action FunCParam)
        {
            _duration = duration;
            _startTime = 0;
            _active = false;
            _func = FunCParam;
            stopwatch.Start();
        }

        public void Activate()
        {
            _active = true;
            _startTime = stopwatch.ElapsedMilliseconds;
        }

        public void Deactivate()
        {
            _active = false;
            _startTime = 0;
        }

        public void Update()
        {
            float currentTime = stopwatch.ElapsedMilliseconds;
            if (currentTime - _startTime >= _duration && _startTime != 0)
            {
                _func?.Invoke();
                Deactivate();
            }
        }

        public bool Active { get => _active; }
    }
}

