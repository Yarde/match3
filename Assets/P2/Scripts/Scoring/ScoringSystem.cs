using Common.Common.Code;

namespace P2.Scoring
{
    public class ScoringSystem
    {
        private readonly Match3 _match3;
        
        public int Score { get; private set; }

        public ScoringSystem(Match3 match3)
        {
            _match3 = match3;
        }

    }
}