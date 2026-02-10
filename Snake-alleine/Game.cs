using Snake_alleine;
using System;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    class Game  // Main game logic and loop
    {
        private int width, height, offsetTop = 6;
        private int score;
        private int speed;
        private int currentLevel;
        private Snake snake = null!;
        private Food food = new Food();
        private HighScoreManager hs = new HighScoreManager();

        public void Start()  //Startpunkt des Spiels
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            hs.Load();

            while (true)
            {
                ShowLevelMenu();
                Setup();
                Run();
                GameOverScreen();
            }
        }

        private void ShowLevelMenu()   // Level-Auswahlmenü
        {
            Console.Clear();  // Bildschirm löschen
            Console.ForegroundColor = ConsoleColor.Gray;  // Textfarbe setzen
            string[] menu = {   // Menü-Layout
                "╔══════════════════════════════════════════╗",
                "║          S N A K E   M E N Ü             ║",
                "╠══════════════════════════════════════════╣",
                "║                                          ║",
                "║       Wähle dein Level (1-3):            ║",
                "║                                          ║",
                "║       [1] LEICHT  (Entspannt)            ║",
                "║       [2] MEDIUM  (Herausfordernd)       ║",
                "║       [3] SCHWER  (Extrem)               ║",
                "║                                          ║",
                "╚══════════════════════════════════════════╝"
            };

            int startY = (Console.WindowHeight / 2) - (menu.Length / 2);
            for (int i = 0; i < menu.Length; i++)
            {
                int startX = (Console.WindowWidth / 2) - (menu[i].Length / 2);
                SafeDraw(startX, startY + i, menu[i]);
            }

            bool valid = false;   // Gültigkeitsprüfung für Eingabe
            while (!valid)   // Warte auf gültige Eingabe
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) { currentLevel = 1; speed = 130; valid = true; }
                if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) { currentLevel = 2; speed = 80; valid = true; }
                if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3) { currentLevel = 3; speed = 40; valid = true; }
            }
        }

        private void Setup()  // Spiel-Setup
        {
            UpdateSize();
            score = 0;
            hs.NewRecordAchieved = false;
            snake = new Snake(width / 2, height / 2);
            Console.Clear();
            DrawInterface();
            food.Spawn(width, height, snake.Body);
            food.Draw(offsetTop);
        }

        private void Run()   // Hauptspiel-Schleife
        {
            while (true)
            {
                if (Console.KeyAvailable) HandleInput();

                // WICHTIG: Altes Ende merken, bevor die Schlange wandert
                Position tailBeforeMove = snake.Body.Last();
                Position head = snake.Body[0];
                bool eating = (head.X == food.Pos.X && head.Y == food.Pos.Y);

                snake.Move(eating);

                if (!eating)
                {
                    // Altes Ende löschen
                    SafeDraw(tailBeforeMove.X * 2, tailBeforeMove.Y + offsetTop, "  ");
                }
                else
                {
                    // Essen-Logik mit Power-Ups
                    if (food.IsSpecial)
                    {
                        score += 50;
                        try { Console.Beep(1000, 100); } catch { }
                        FlashScreen();
                    }
                    else
                    {
                        score += 10;
                        try { Console.Beep(600, 50); } catch { }
                    }

                    if (speed > 25) speed -= 1;

                    CheckForNewHighscore();
                    UpdateScoreUI();
                    food.Spawn(width, height, snake.Body);
                    food.Draw(offsetTop);
                }

                if (snake.CheckCollision(width, height))
                {
                    try { Console.Beep(200, 400); } catch { }
                    break;
                }

                Render();
                Thread.Sleep(speed);
            }
        }

        private void FlashScreen()  // Bildschirm kurz aufhellen für Power-Up-Effekt
        {
            Console.ForegroundColor = ConsoleColor.White;
            DrawBorders();
            Thread.Sleep(20);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            DrawBorders();
        }

        private void CheckForNewHighscore()  // Überprüft, ob ein neuer Highscore erreicht wurde
        {
            if (score > hs.Score && !hs.NewRecordAchieved)
            {
                hs.NewRecordAchieved = true;
                Console.ForegroundColor = ConsoleColor.Yellow;
                SafeDraw(2, 4, "!!! NEUER HIGHSCORE !!!");
                try { Console.Beep(1200, 200); } catch { }
            }
        }

        private void HandleInput()  // Verarbeitet die Benutzereingaben für die Steuerung der Schlange
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow: if (snake.CurrentDirection != Direction.Down) snake.CurrentDirection = Direction.Up; break;
                case ConsoleKey.DownArrow: if (snake.CurrentDirection != Direction.Up) snake.CurrentDirection = Direction.Down; break;
                case ConsoleKey.LeftArrow: if (snake.CurrentDirection != Direction.Right) snake.CurrentDirection = Direction.Left; break;
                case ConsoleKey.RightArrow: if (snake.CurrentDirection != Direction.Left) snake.CurrentDirection = Direction.Right; break;
                case ConsoleKey.Escape: Environment.Exit(0); break;
            }
        }

        private void Render()  // Zeichnet die Schlange auf dem Bildschirm
        {
            // Zeichnet nur den neuen Kopf der Schlange
            Console.ForegroundColor = GetLevelColor();
            SafeDraw(snake.Body[0].X * 2, snake.Body[0].Y + offsetTop, "██");
        }

        private ConsoleColor GetLevelColor()  // Gibt die Farbe basierend auf dem aktuellen Level und Highscore zurück 
        {
            if (hs.NewRecordAchieved) return ConsoleColor.Yellow;
            if (score >= 100) return ConsoleColor.Magenta;
            if (score >= 50) return ConsoleColor.Cyan;
            return ConsoleColor.Green;
        }

        private void UpdateSize() // Berechnet die Spielfeldgröße basierend auf der Konsolengröße
        {
            int availableWidth = Console.WindowWidth - 2;
            width = availableWidth / 2;
            height = Console.WindowHeight - offsetTop - 2;
        }

        private void DrawInterface() // Zeichnet die statische Benutzeroberfläche mit Score, Level und Highscore
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            SafeDraw(2, 1, "╔══════════════════════════════════════════╗");
            SafeDraw(2, 2, $"║  SCORE: {score:D4}   │ HIGHSCORE: {hs.Score:D4}  ║");
            SafeDraw(2, 3, $"║  LEVEL: {currentLevel}      │ SPEED: {speed}ms       ║");
            SafeDraw(2, 4, "╚══════════════════════════════════════════╝");
            DrawBorders();
        }

        private void DrawBorders() // Zeichnet die Spielfeldgrenzen
        {
            for (int i = 0; i <= width * 2; i++)
            {
                SafeDraw(i, offsetTop, "▀");
                SafeDraw(i, height + offsetTop, "▄");
            }
            for (int i = 0; i <= height; i++)
            {
                SafeDraw(0, i + offsetTop, "█");
                SafeDraw(width * 2, i + offsetTop, "█");
            }
        }

        private void UpdateScoreUI()  // Aktualisiert die Anzeige von Score und Highscore, hebt den Score hervor, wenn ein neuer Rekord erreicht wurde
        {
            Console.ForegroundColor = hs.NewRecordAchieved ? ConsoleColor.Yellow : ConsoleColor.White;
            SafeDraw(12, 2, score.ToString("D4"));
            if (hs.NewRecordAchieved)
            {
                SafeDraw(31, 2, score.ToString("D4"));
            }
        }

        private void GameOverScreen()  // Zeigt den Game-Over-Bildschirm an, speichert den Highscore und wartet auf die Eingabe zum Neustart
        {
            hs.Save(score);
            Console.ForegroundColor = ConsoleColor.Red;
            int cx = Math.Max(0, (width * 2 / 2) - 14);
            int cy = (height / 2) + offsetTop;

            string[] msg = {
                "╔═══════════════════════════╗",
                "║      G A M E  O V E R      ║",
                hs.NewRecordAchieved ? "║   !!! NEUER REKORD !!!    ║" : "║                           ║",
                "║    ENTER ZUM NEUSTART      ║",
                "╚═══════════════════════════╝"
            };
            for (int i = 0; i < msg.Length; i++) SafeDraw(cx, cy + i, msg[i]);

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }

        public static void SafeDraw(int x, int y, string s)  // Zeichnet Text an einer bestimmten Position, überprüft vorher die Grenzen der Konsole
        {
            if (x >= 0 && x < Console.BufferWidth && y >= 0 && y < Console.BufferHeight)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
        }
    }
}