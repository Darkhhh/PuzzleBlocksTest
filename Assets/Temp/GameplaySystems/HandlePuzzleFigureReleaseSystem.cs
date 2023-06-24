using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Systems.Experimental.CellHandling;
using Temp.Utils;

namespace Temp.GameplaySystems
{
    public class HandlePuzzleFigureReleaseSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _releasedFiguresFilter =
            default;

        private readonly EcsFilterInject<Inc<AnchoredBlockCellComponent>> _anchoredBlocksFilter = default;

        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent>> _highlightedCellsFilter = default;
        private readonly EcsPoolInject<OccupiedCellStateComponent> _occupiedCellsPool = default;
        private readonly EcsPoolInject<ChangeCellStateComponent> _changeStateCellsPool = default;

        public void Run(IEcsSystems systems)
        {
            if (_releasedFiguresFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var entity in _highlightedCellsFilter.Value)
            {
                CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity), CellStateEnum.Occupied);
                
                // _occupiedCellsPool.Value.Add(entity);
                // _highlightedCellsFilter.Pools.Inc1.Del(entity);
                // if (!_changeStateCellsPool.Value.Has(entity)) _changeStateCellsPool.Value.Add(entity);
            }

            foreach (var entity in _anchoredBlocksFilter.Value)
            {
                _anchoredBlocksFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}