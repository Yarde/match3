using Common.Common.Code;

namespace P2.Objectives
{
    public class ObjectivesFactory
    {
        private readonly Match3 _match3;

        public ObjectivesFactory(Match3 match3)
        {
            _match3 = match3;
        }
        
        public Objective CreateChipMatchedObjective(int matchesNeeded)
        {
            return new ChipMatchedObjective(matchesNeeded, _match3);
        }

        public Objective CreateMoveLimitObjective(int moveLimit)
        {
            return new MoveLimitObjective(moveLimit, _match3);

        }
    }
}