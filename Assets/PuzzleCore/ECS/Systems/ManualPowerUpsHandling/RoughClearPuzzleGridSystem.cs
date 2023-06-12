using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;

namespace PuzzleCore.ECS.Systems.ManualPowerUpsHandling
{
    public class RoughClearPuzzleGridSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, ReleasedObjectComponent>> _releasedPowerUpFilter;
        private readonly EcsFilterInject<Inc<CellComponent, CellOrderedForPlacementComponent>> _orderedCellsFilter = default;
        
        #region ECS Pools

        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        
        private readonly EcsPoolInject<CellOrderedForPlacementComponent> _orderedCellComponents = default;
        
        private readonly EcsPoolInject<ShouldBeClearedCellComponent> _clearingCellsComponents = default;
        
        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpComponents = default;

        #endregion
        
        private readonly PowerUpsHandler _handler;
        private EventsBus _events;
        
        public RoughClearPuzzleGridSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
        
        public void Run(IEcsSystems systems)
        {
            //if (_releasedPowerUpFilter.Value.GetEntitiesCount() == 0) return;
            if (!_events.HasEventSingleton<RoughClearEvent>()) return;

            foreach (var entity in _orderedCellsFilter.Value)
            {
                ref var c = ref _cellComponents.Value.Get(entity);
                c.View.SetSimple();
                c.Available = true;
                _orderedCellComponents.Value.Del(entity);
                if (_clearingCellsComponents.Value.Has(entity)) _clearingCellsComponents.Value.Del(entity);

                if (_cellPowerUpComponents.Value.Has(entity))
                {
                    ref var cellPowerUp = ref _cellPowerUpComponents.Value.Get(entity);
                    _handler.ReturnPowerUp(cellPowerUp.View);
                    _cellPowerUpComponents.Value.Del(entity);
                }
            }
        }
    }
}