using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

namespace PuzzleCore.ECS.Systems.Experimental
{
    public class DefaultCellStateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        
        private readonly EcsPoolInject<CellComponent> _cellsPool = default;

        private readonly EcsPoolInject<ChangeCellStateComponent> _changeCellsPool = default;
        private readonly EcsPoolInject<DefaultCellStateComponent> _defaultCellsPool = default;
        private readonly EcsPoolInject<SuggestedCellStateComponent> _suggestedCellsPool = default;
        private readonly EcsPoolInject<HighlightedCellStateComponent> _highlightedCellsPool = default;
        private readonly EcsPoolInject<OccupiedCellStateComponent> _occupiedCellsPool = default;
        private readonly EcsPoolInject<DestroyableCellStateComponent> _destroyableCellsPool = default;
        private readonly EcsPoolInject<TargetedCellStateComponent> _targetedCellsPool = default;
        
        public void Run(IEcsSystems systems)
        {

            throw new System.NotImplementedException();
        }
    }
}