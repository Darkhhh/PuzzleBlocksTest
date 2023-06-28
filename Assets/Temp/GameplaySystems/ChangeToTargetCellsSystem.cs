using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Temp.Components;
using Temp.Utils;
using Temp.Views.Cell;

namespace Temp.GameplaySystems
{
    public class ChangeToTargetCellsSystem: IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, DraggingObjectComponent>> _draggingPowerUpFilter = default;

        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent, ChangeCellStateComponent>> _highlightedCellsFilter = default;

        private readonly EcsPoolInject<HighlightedCellStateComponent> _highlightedCellsPool = default;
        
        private readonly EcsPoolInject<TargetedCellStateComponent> _targetedCellsPool = default;
        
        private readonly EcsPoolInject<DefaultCellStateComponent> _defaultCellsPool = default;
        
        private readonly EcsPoolInject<OccupiedCellStateComponent> _occupiedCellsPool = default;

        
        
        public void Run(IEcsSystems systems)
        {
            if (_draggingPowerUpFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var entity in _highlightedCellsFilter.Value)
            {
                ref var data = ref _highlightedCellsFilter.Pools.Inc1.Get(entity);

                CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity),
                    data.PreviousState == CellStateEnum.Default ? CellStateEnum.Default : CellStateEnum.Targeted);
            }
        }
    }
}