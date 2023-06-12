using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;

namespace PuzzleCore.ECS.Systems.Drag
{
    public class MoveDraggingObjectSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DraggingObjectComponent>> _draggingObjectFilter = default;
        
        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectComponents = default;
        private readonly EcsPoolInject<DraggingObjectComponent> _draggingObjectComponents = default;
        private readonly EcsPoolInject<DraggingOverGridComponent> _draggingOverGridObjectComponents = default;
        
        private EventsBus _events;
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_draggingObjectFilter.Value.GetEntitiesCount() == 0) return;
            
            ref var position = ref _events.GetEventBodySingleton<CurrentMousePositionEvent>();
            foreach (var entity in _draggingObjectFilter.Value)
            {
                ref var o = ref _draggableObjectComponents.Value.Get(entity);
                ref var draggingData = ref _draggingObjectComponents.Value.Get(entity);
                
                o.View.GetTransform().position = position.Position + draggingData.Offset;

                if (_draggingOverGridObjectComponents.Value.Has(entity))
                {
                    ref var t = ref _draggingOverGridObjectComponents.Value.Get(entity);
                    t.CurrentPosition = o.View.GetTransform().position;
                }
            }
        }
    }
}