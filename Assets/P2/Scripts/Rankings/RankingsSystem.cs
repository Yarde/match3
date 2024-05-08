using System;
using System.Collections.Generic;
using Common.Common.Code;
using Cysharp.Threading.Tasks;
using P2.Network;
using P2.Scoring;
using UnityEngine;

namespace P2.Rankings
{
    public class RankingsSystem : IDisposable
    {
        private readonly NetworkSystem _networkManager;
        private readonly Match3 _match3;
        private readonly ScoringSystem _scoringSystem;

        private List<RankingEntry> _ranking;
        private float _lastFetchTime;

        public RankingsSystem(Match3 match3, NetworkSystem networkManager, ScoringSystem scoringSystem)
        {
            _match3 = match3;
            _networkManager = networkManager;
            _scoringSystem = scoringSystem;
            
            _match3.OnGameEnded += OnGameEnded;
        }
        
        private void OnGameEnded(bool successful)
        {
            if (successful)
            {
                AddRankingEntry("Player", _scoringSystem.Score.Value).Forget();
            }
        }

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

        private async UniTask AddRankingEntry(string playerName, int score)
        {
            await _networkManager.SendRequest<AddRankingEntryReply>(new AddRankingEntryRequest(playerName, score));
        }
        
        private async UniTask FetchRankingData()
        {
            var request = new GetRankingRequest();
            var reply = await _networkManager.SendRequest<GetRankingReply>(request);
            _ranking = reply.ranking;
        }

        public void Dispose()
        {
            _match3.OnGameEnded -= OnGameEnded;
        }
    }
}