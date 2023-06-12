using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;

namespace PuzzleCore.ECS.Systems.FigureHandling
{
    public class HandlePuzzleFigureDragSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DraggingObjectComponent, PuzzleFigureComponent>> _draggingFigureFilter = default;
        
        private EventsBus _events;
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<DraggableObjectWasTakenEvent>()) return;

            foreach (var entity in _draggingFigureFilter.Value)
            {
                ref var e = ref _events.NewEventSingleton<ChangeFigureScaleComponent>();
                e.Entity = entity;
                e.Increase = true;
            }
        }
    }
}