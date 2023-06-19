using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

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
                _highlightedCellsPool.Value.Del(entity);

                if (_occupiedCellsPool.Value.Has(entity))
                {
                    _occupiedCellsPool.Value.Del(entity);
                    _targetedCellsPool.Value.Add(entity);
                }
                else
                {
                    _defaultCellsPool.Value.Add(entity);
                }
            }
        }
    }
}