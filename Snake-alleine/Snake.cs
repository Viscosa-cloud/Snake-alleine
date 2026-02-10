using System;
using System.Collections.Generic;
using System.Text;

namespace Snake_alleine
{
    class Snake  // Klasse für die Schlange, enthält die Logik für Bewegung und Kollisionserkennung
    {
        public List<Position> Body { get; private set; } = new List<Position>();  // Liste der Positionen der Schlangensegmente, wobei das erste Element der Kopf ist
        public Direction CurrentDirection { get; set; } = Direction.Right;  // Aktuelle Bewegungsrichtung der Schlange, standardmäßig nach rechts

        public Snake(int startX, int startY)  // Konstruktor, der die Schlange mit einer Anfangsposition und drei Segmenten erstellt
        {
            // Start mit 3 Segmenten
            Body.Add(new Position(startX, startY));
            Body.Add(new Position(startX - 1, startY));
            Body.Add(new Position(startX - 2, startY));
        }

        public void Move(bool grow)  // Methode zum Bewegen der Schlange, die den Kopf in die aktuelle Richtung bewegt und das letzte Segment entfernt, es sei denn, die Schlange soll wachsen (z.B. nach dem Essen)
        {
            Position head = Body[0];
            Position newHead = new Position(head.X, head.Y);

            switch (CurrentDirection)
            {
                case Direction.Up: newHead.Y--; break;
                case Direction.Down: newHead.Y++; break;
                case Direction.Left: newHead.X--; break;
                case Direction.Right: newHead.X++; break;
            }

            Body.Insert(0, newHead);
            if (!grow) Body.RemoveAt(Body.Count - 1);
        }

        public bool CheckCollision(int width, int height)  // Methode zur Überprüfung von Kollisionen, sowohl mit dem Rahmen als auch mit sich selbst
        {
            Position head = Body[0];
            // Kollision mit Rahmen (Rahmen ist bei 0 und width/height)
            if (head.X <= 0 || head.X >= width || head.Y <= 0 || head.Y >= height) return true;

            // Kollision mit sich selbst
            return Body.Skip(1).Any(p => p.X == head.X && p.Y == head.Y);
        }
    }
}
