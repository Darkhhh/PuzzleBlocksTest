using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;

namespace Source.Code.GameplaySystems
{
    /// <summary>
    /// Проверяет все объекты PuzzleFigureComponent + ReleasedObjectComponent.
    /// Если фигуру есть куда ставить, добавляется компонент ShouldBeRemovedFigureComponent.
    /// Иначе Фигура возвращается на изначальную позицию.
    /// </summary>
    public class ReleasePuzzleFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ReleasedObjectComponent>> _releasedFigureFilter = default;
        private readonly EcsFilterInject<Inc<DestroyableCellStateComponent>> _destroyableCellsFilter = default;
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent>> _highlightedCellsFilter = default;

        private readonly EcsPoolInject<ShouldBeRemovedFigureComponent> _removedFiguresComponents = default;
        
        private EventsBus _events;
        
        
        public void Init(IEcsSystems systems) => _events = systems.GetShared<SystemsSharedData>().EventsBus;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var figureEntity in _releasedFigureFilter.Value)
            {
                ref var puzzleFigure = ref _releasedFigureFilter.Pools.Inc1.Get(figureEntity);

                if (_destroyableCellsFilter.Value.GetEntitiesCount() > 0 || _highlightedCellsFilter.Value.GetEntitiesCount() > 0)
                {
                    _removedFiguresComponents.Value.Add(figureEntity);
                }
                else
                {
                    ref var draggingInfo = ref _releasedFigureFilter.Pools.Inc2.Get(figureEntity);
                    puzzleFigure.View.transform.DOMove(draggingInfo.InitialPosition - puzzleFigure.View.Offset,
                        0.5f);

                    ref var data = ref _events.NewEventSingleton<ChangeFigureScaleComponent>();
                    data.Entity = figureEntity;
                    data.Increase = false;
                }
                _releasedFigureFilter.Pools.Inc2.Del(figureEntity);
            }
        }
    }
}