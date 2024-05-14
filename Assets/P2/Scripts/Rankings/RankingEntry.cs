namespace P2.Rankings
{
    public class RankingEntry
    {
        public int Position { get; private set; }
        public string PlayerName { get; private set; }
        public int Score { get; private set; }

        public RankingEntry(int position, string playerName, int score)
        {
            Position = position;
            PlayerName = playerName;
            Score = score;
        }
    }
}