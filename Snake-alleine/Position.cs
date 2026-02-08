using System;
using System.Collections.Generic;
using System.Text;

namespace Snake_alleine
{
    public struct Position
    {
        public int X;
        public int Y;
        public Position(int x, int y) { X = x; Y = y; }
    }

    // Richtungs-Enum
    public enum Direction { Up, Down, Left, Right }
}

