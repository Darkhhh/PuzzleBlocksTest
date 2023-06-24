using System;
using Leopotam.EcsLite;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using Temp.Components.Events;

namespace Temp.PostGameplaySystems
{
    public class CountCoinsAndScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EventsBus _events;
        private InGameData _gameData;
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
            _gameData = systems.GetShared<SystemsSharedData>().GameData;
        }

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<IntermediateResultEvent>()) return;

            ref var data = ref _events.GetEventBodySingleton<IntermediateResultEvent>();
            
            if (data.DestroyedCells == 0) throw new Exception("Incorrect input data");

            int coins = data.CoinsAmount, points = data.DestroyedCells;
            if (data.Multiplier != 0)
            {
                coins *= data.Multiplier;
                points *= data.Multiplier;
            }
            
            _events.DestroyEventSingleton<IntermediateResultEvent>();

            _gameData.CurrentScore += points;
            _gameData.CoinsAmount += coins;
            

            ref var updateData = ref _events.NewEventSingleton<UpdateInGameUIEvent>();

            updateData.NewScore = _gameData.CurrentScore;
            updateData.NewCoins = _gameData.CoinsAmount;
        }
    }
}