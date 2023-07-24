using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Common.Animations;
using Source.Code.Common.Audio;
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
        private AudioManager _audio;


        public void Init(IEcsSystems systems)
        {
            var shared = systems.GetShared<SystemsSharedData>();
            _events = shared.EventsBus;
            _audio = shared.SceneData.audioManager;
        }
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var figureEntity in _releasedFigureFilter.Value)
            {
                ref var puzzleFigure = ref _releasedFigureFilter.Pools.Inc1.Get(figureEntity);

                if (_destroyableCellsFilter.Value.GetEntitiesCount() > 0 || _highlightedCellsFilter.Value.GetEntitiesCount() > 0)
                {
                    _removedFiguresComponents.Value.Add(figureEntity);
                    _audio.Play(SoundTag.FigureSet);
                }
                else
                {
                    ref var draggingInfo = ref _releasedFigureFilter.Pools.Inc2.Get(figureEntity);
                    puzzleFigure.View.ReturnBack(draggingInfo.InitialPosition, 0.5f);

                    ref var data = ref _events.NewEventSingleton<ChangeFigureScaleComponent>();
                    data.Entity = figureEntity;
                    data.Increase = false;
                }
                _releasedFigureFilter.Pools.Inc2.Del(figureEntity);
            }
        }
    }
}