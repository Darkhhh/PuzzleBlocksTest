using System;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components.Events;
using Source.Code.Mono;
using Source.Code.SharedData;
using Source.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Code.PostGameplaySystems
{
    public class CountCoinsAndScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EventsBus _events;
        private InGameData _gameData;
        private GameData _data;
        private ScorePopUpHandler _effectHandler;
        
        
        public void Init(IEcsSystems systems)
        {
            var shared = systems.GetShared<SystemsSharedData>();
            _events = shared.EventsBus;
            _gameData = shared.GameData;
            _data = shared.SceneData.DataManager.GetData().GameData;
            _effectHandler = shared.SceneData.scorePopUpHandler;
            _effectHandler.Init();
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
            
            _effectHandler.PlayEffect(new Vector3(Random.Range(-4, 4), Random.Range(-4, 4)), points);

            _gameData.CurrentScore += points;
            _gameData.CoinsAmount += coins;
            _data.coinsAmount += coins;

            ref var updateData = ref _events.NewEventSingleton<UpdateInGameUIEvent>();

            updateData.NewScore = _gameData.CurrentScore;
            updateData.NewCoins = _gameData.CoinsAmount;
        }
    }
}