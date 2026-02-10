using System;
using Snake_alleine;

namespace SnakeGame
{
    class Program
    {
        static void Main()  // Main-Methode, der Einstiegspunkt des Programms
        {
            // Startet das Spiel
            Game snakeGame = new Game();
            snakeGame.Start();
        }
    }
}