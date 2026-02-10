using System;
using System.Collections.Generic;
using System.Text;

namespace Snake_alleine
{
    public struct Position  // Struktur für die Position von Schlangensegmenten und Essen, enthält X- und Y-Koordinaten
    {
        public int X;
        public int Y;
        public Position(int x, int y) { X = x; Y = y; }
    }

    // Richtungs-Enum
    public enum Direction { Up, Down, Left, Right }  // Enum für die Bewegungsrichtung der Schlange, mit den vier möglichen Richtungen: oben, unten, links und rechts
}

