using System.Collections.Generic;
using P2.Rankings;

namespace P2.Network
{
    public class Reply
    {
    }

    public class Request
    {
    }
    
    public class GetRankingRequest : Request
    {
    }
    
    public class GetRankingReply : Reply
    {
        public List<RankingEntry> ranking;
    }
    
    public class AddRankingEntryRequest : Request
    {
        public string playerName;
        public int score;
        
        public AddRankingEntryRequest(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }
    }
    
    public class AddRankingEntryReply : Reply
    {
    }
}