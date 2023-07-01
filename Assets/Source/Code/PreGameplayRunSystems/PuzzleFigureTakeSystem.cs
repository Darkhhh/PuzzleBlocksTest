using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;

namespace Source.Code.PreGameplayRunSystems
{
    /// <summary>
    /// Проверяет может ли быть взят объект фигуры, должен выполнятся до DetectDraggableOverGridObjectSystem
    /// </summary>
    public class PuzzleFigureTakeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, DraggingObjectComponent>> _draggingFiguresFilter = default;
        private readonly EcsPoolInject<CanNotBeTakenComponent> _cantTakeComponents = default;
        
        private EventsBus _events;
        
        
        public void Init(IEcsSystems systems) => _events = systems.GetShared<SystemsSharedData>().EventsBus;

        
        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<DraggableObjectWasTakenEvent>()) return;

            foreach (var entity in _draggingFiguresFilter.Value)
            {
                if (!_cantTakeComponents.Value.Has(entity))
                {
                    ref var data = ref _events.NewEventSingleton<ChangeFigureScaleComponent>();
                    data.Entity = entity;
                    data.Increase = true;
                    continue;
                }
                _draggingFiguresFilter.Pools.Inc2.Del(entity);
            }
        }
    }
}