using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.ManualPowerUpsHandling
{
    public class HandleManualPowerUpReleaseSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellOrderedForPlacementComponent>> _orderedCellsFilter = default;
        private readonly EcsFilterInject<Inc<ManualPowerUp, ReleasedObjectComponent>> _releasedPowerUpFilter = default;
        private readonly EcsPoolInject<ManualPowerUp> _manualPowerUpComponents = default;
        private readonly EcsPoolInject<ReleasedObjectComponent> _releasedObjectComponents = default;
        private EventsBus _events;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _releasedPowerUpFilter.Value)
            {
                ref var manualPowerUp = ref _manualPowerUpComponents.Value.Get(entity);
                ref var dragData = ref _releasedObjectComponents.Value.Get(entity);

                //TODO If released not in puzzle grid, do not change AvailableAmount
                manualPowerUp.View.transform.position = dragData.InitialPosition;
                Debug.Log($"Ordered cells: {_orderedCellsFilter.Value.GetEntitiesCount()}");
                if (_orderedCellsFilter.Value.GetEntitiesCount() > 0)
                {
                    manualPowerUp.AvailableAmount--;
                    _events.NewEventSingleton<RoughClearEvent>();
                }
                _releasedObjectComponents.Value.Del(entity);
            }
        }

        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
    }
}