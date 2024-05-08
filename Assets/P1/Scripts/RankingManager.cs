using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace P1
{
    public class RankingManager : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManager;
        
        private List<RankingEntry> _ranking;
        private float _lastFetchTime;
        
        public async UniTask<IReadOnlyList<RankingEntry>> GetRanking()
        {
            if (_ranking != null && _lastFetchTime + 60 > Time.time)
            {
                return _ranking;
            }
            _lastFetchTime = Time.time; 
            
            await FetchRankingData();
            return _ranking;
        }
        
        public async UniTask AddRankingEntry(string playerName, int score)
        {
            await _networkManager.SendRequest<AddRankingEntryReply>(new AddRankingEntryRequest(playerName, score));
        }
        
        private async UniTask FetchRankingData()
        {
            var request = new GetRankingRequest();
            var reply = await _networkManager.SendRequest<GetRankingReply>(request);
            _ranking = reply.ranking;
        }
    }

    public class RankingEntry
    {
        public int position;
        public string playerName;
        public int score;
    }
}