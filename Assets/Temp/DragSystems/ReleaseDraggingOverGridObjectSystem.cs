using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;

namespace Temp.DragSystems
{
    public class ReleaseDraggingOverGridObjectSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DraggingObjectComponent, DraggingOverGridComponent>> _draggingObjectFilter = default;
        
        private readonly EcsPoolInject<DraggingOverGridComponent> _draggingOverGridObjectComponents = default;
        
        private EventsBus _events;
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<RightMouseDownEvent>()) return;
            
            if (_draggingObjectFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var entity in _draggingObjectFilter.Value)
            {
                _draggingOverGridObjectComponents.Value.Del(entity);
            }

            // if (!_events.HasEventSingleton<DeHighlightGridEvent>())
            //     _events.NewEventSingleton<DeHighlightGridEvent>();
        }
    }
}