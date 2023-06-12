using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Common;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.PuzzleGridHandling
{
    public class CountPointsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent, ShouldBeClearedCellComponent>> _clearingCellsFilter = default;

        private EventsBus _events;

        private InGameData _gameData;
        
        public void Run(IEcsSystems systems)
        {
            if (_clearingCellsFilter.Value.GetEntitiesCount() == 0) return;

            ref var e = ref _events.NewEventSingleton<UpdateScoreEvent>();
            e.NewScore = _gameData.CurrentScore + _clearingCellsFilter.Value.GetEntitiesCount();
            _gameData.CurrentScore = e.NewScore;
            
            Debug.Log($"Earned {_clearingCellsFilter.Value.GetEntitiesCount()} points!");
        }

        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
            _gameData = systems.GetShared<SystemsSharedData>().GameData;
        }
    }
}