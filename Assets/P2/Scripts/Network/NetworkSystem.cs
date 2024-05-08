using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using P2.Rankings;

namespace P2.Network
{
    public class NetworkSystem
    {
        private readonly Dictionary<Type, Reply> _mockData = new();

        private NetworkSystem()
        {
            CreateMockData();
        }

        private void CreateMockData()
        {
            var rankingMock = new List<RankingEntry>();
            var places = 10;
            for (var i = 0; i < places; i++)
            {
                rankingMock.Add(new RankingEntry
                {
                    position = i + 1,
                    playerName = $"Player{i}",
                    score = (places - i) * 100
                });
            }
            
            _mockData[typeof(GetRankingRequest)] = new GetRankingReply {ranking = rankingMock};
            _mockData[typeof(AddRankingEntryRequest)] = new AddRankingEntryReply();
        }

        public async UniTask<T> SendRequest<T>(Request request) where T : Reply
        {
            // no server to send request to return mock data
            await UniTask.Delay(500);
            return _mockData[request.GetType()] as T;
        }
    }
}