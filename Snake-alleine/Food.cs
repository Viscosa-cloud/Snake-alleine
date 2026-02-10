using Snake_alleine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    class Food  // Klasse für das Essen (Apfel und Spezial-Essen)
    {
        public Position Pos { get; private set; }  // Position des Essens
        public bool IsSpecial { get; private set; }  // Gibt an, ob es sich um ein Spezial-Essen (Stern) handelt
        private Random rand = new Random();  // Zufallszahlengenerator für die Position und die Art des Essens

        public void Spawn(int width, int height, List<Position> snakeBody)  // Methode zum Erzeugen eines neuen Essens an einer zufälligen Position, die nicht von der Schlange besetzt ist
        {
            do
            {
                Pos = new Position(rand.Next(1, width), rand.Next(1, height));
            } while (snakeBody.Any(p => p.X == Pos.X && p.Y == Pos.Y));

            // 10% Chance auf ein Spezial-Essen (Stern)
            IsSpecial = rand.Next(0, 10) == 0;
        }

        public void Draw(int offsetTop)   // Methode zum Zeichnen des Essens auf der Konsole, abhängig davon, ob es sich um ein Spezial-Essen handelt oder nicht
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