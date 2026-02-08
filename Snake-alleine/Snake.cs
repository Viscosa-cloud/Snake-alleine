using System;
using System.Collections.Generic;
using System.Text;

namespace Snake_alleine
{
    class Snake
    {
        public List<Position> Body { get; private set; } = new List<Position>();
        public Direction CurrentDirection { get; set; } = Direction.Right;

        public Snake(int startX, int startY)
        {
            // Start mit 3 Segmenten
            Body.Add(new Position(startX, startY));
            Body.Add(new Position(startX - 1, startY));
            Body.Add(new Position(startX - 2, startY));
        }

        public void Move(bool grow)
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

        public bool CheckCollision(int width, int height)
        {
            Position head = Body[0];
            // Kollision mit Rahmen (Rahmen ist bei 0 und width/height)
            if (head.X <= 0 || head.X >= width || head.Y <= 0 || head.Y >= height) return true;

            // Kollision mit sich selbst
            return Body.Skip(1).Any(p => p.X == head.X && p.Y == head.Y);
        }
    }
}
