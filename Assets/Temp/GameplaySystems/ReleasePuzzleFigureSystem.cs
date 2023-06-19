using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

namespace Temp.GameplaySystems
{
    public class ReleasePuzzleFigureSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ReleasedObjectComponent>> _releasedFigureFilter =
            default;

        private readonly EcsFilterInject<Inc<DestroyableCellStateComponent>> _destroyableCellsFilter = default;

        private readonly EcsPoolInject<ReleasedObjectComponent> _releasedFiguresPool = default;
        
        private readonly EcsPoolInject<PuzzleFigureComponent> _puzzleFiguresPool = default;
        
        private readonly EcsPoolInject<ShouldBeRemovedFigureComponent> _removedFiguresComponents = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var figureEntity in _releasedFigureFilter.Value)
            {
                ref var puzzleFigure = ref _puzzleFiguresPool.Value.Get(figureEntity);

                if (_destroyableCellsFilter.Value.GetEntitiesCount() > 0)
                {
                    _removedFiguresComponents.Value.Add(figureEntity);
                }
                else
                {
                    ref var draggingInfo = ref _releasedFiguresPool.Value.Get(figureEntity);
                    puzzleFigure.View.transform.DOMove(draggingInfo.InitialPosition - puzzleFigure.View.Offset,
                        0.5f);
                }
            }
        }
    }
}