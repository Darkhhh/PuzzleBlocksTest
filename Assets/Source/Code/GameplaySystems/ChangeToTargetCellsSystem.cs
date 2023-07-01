using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Components;
using Source.Code.Utils;
using Source.Code.Views.Cell;

namespace Source.Code.GameplaySystems
{
    public class ChangeToTargetCellsSystem: IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, DraggingObjectComponent>> _draggingPowerUpFilter = default;
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent, ChangeCellStateComponent>> _highlightedCellsFilter = default;

        private EcsWorld _world;
        
        
        public void Init(IEcsSystems systems) => _world = systems.GetWorld();

        
        public void Run(IEcsSystems systems)
        {
            if (_draggingPowerUpFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var entity in _highlightedCellsFilter.Value)
            {
                ref var data = ref _highlightedCellsFilter.Pools.Inc1.Get(entity);

                CellEntity.SetState(_world.PackEntityWithWorld(entity),
                    data.PreviousState == CellStateEnum.Default ? CellStateEnum.Default : CellStateEnum.Targeted);
            }
        }
    }
}