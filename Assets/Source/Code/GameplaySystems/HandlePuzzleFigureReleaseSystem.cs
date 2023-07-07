using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Common.Utils;
using Source.Code.Components;
using Source.Code.Views.Cell;

namespace Source.Code.GameplaySystems
{
    public class HandlePuzzleFigureReleaseSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _releasedFiguresFilter = default;
        private readonly EcsFilterInject<Inc<AnchoredBlockCellComponent>> _anchoredBlocksFilter = default;
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent>> _highlightedCellsFilter = default;

        private EcsWorld _world;
        
        
        public void Init(IEcsSystems systems) => _world = systems.GetWorld();
        
        
        public void Run(IEcsSystems systems)
        {
            if (_releasedFiguresFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var entity in _highlightedCellsFilter.Value)
            {
                CellEntity.SetState(_world.PackEntityWithWorld(entity), CellStateEnum.Occupied);
            }

            foreach (var entity in _anchoredBlocksFilter.Value)
            {
                _anchoredBlocksFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}