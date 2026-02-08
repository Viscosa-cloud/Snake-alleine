using System.IO;

namespace SnakeGame
{
    class HighScoreManager
    {
        private string filePath = "highscore.txt";
        public int Score { get; private set; }
        public bool NewRecordAchieved { get; set; } = false;

        public void Load()
        {
            if (File.Exists(filePath))
            {
                int.TryParse(File.ReadAllText(filePath), out int s);
                Score = s;
            }
            else Score = 0;
            NewRecordAchieved = false;
        }

        public void Save(int currentScore)
        {
            if (currentScore > Score)
            {
                Score = currentScore;
                File.WriteAllText(filePath, Score.ToString());
            }
        }
    }
}