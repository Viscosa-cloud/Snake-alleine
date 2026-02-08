using Snake_alleine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    class Food
    {
        public Position Pos { get; private set; }
        public bool IsSpecial { get; private set; }
        private Random rand = new Random();

        public void Spawn(int width, int height, List<Position> snakeBody)
        {
            do
            {
                Pos = new Position(rand.Next(1, width), rand.Next(1, height));
            } while (snakeBody.Any(p => p.X == Pos.X && p.Y == Pos.Y));

            // 10% Chance auf ein Spezial-Essen (Stern)
            IsSpecial = rand.Next(0, 10) == 0;
        }

        public void Draw(int offsetTop)
        {
            if (IsSpecial)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Game.SafeDraw(Pos.X * 2, Pos.Y + offsetTop, "🌟");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Game.SafeDraw(Pos.X * 2, Pos.Y + offsetTop, "🍎");
            }
        }
    }
}