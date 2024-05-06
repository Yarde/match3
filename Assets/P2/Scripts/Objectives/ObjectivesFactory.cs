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
        
        public ChipMatchedObjective CreateChipMatchedObjective(int matchesNeeded)
        {
            return new ChipMatchedObjective(matchesNeeded, _match3);
        }

        public MoveLimitObjective CreateMoveLimitObjective(int moveLimit)
        {
            return new MoveLimitObjective(moveLimit, _match3);

        }
    }
}