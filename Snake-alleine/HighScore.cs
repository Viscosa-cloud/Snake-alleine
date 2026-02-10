using System.IO;

namespace SnakeGame
{
    class HighScoreManager // Highscore-Manager, der den Highscore verwaltet und speichert
    {
        private string filePath = "highscore.txt";
        public int Score { get; private set; }
        public bool NewRecordAchieved { get; set; } = false;

        public void Load()  // Lädt den Highscore aus der Datei, wenn sie existiert, andernfalls wird der Highscore auf 0 gesetzt
        {
            if (File.Exists(filePath))
            {
                int.TryParse(File.ReadAllText(filePath), out int s);
                Score = s;
            }
            else Score = 0;
            NewRecordAchieved = false;
        }

        public void Save(int currentScore) // Speichert den Highscore in der Datei, wenn der aktuelle Score höher ist als der gespeicherte Highscore
        {
            if (currentScore > Score)
            {
                Score = currentScore;
                File.WriteAllText(filePath, Score.ToString());
            }
        }
    }
}