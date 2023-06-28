using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Temp.Components;
using Temp.Components.Events;
using Temp.SharedData;
using UnityEngine;

namespace Temp.DragSystems
{
    /// <summary>
    /// При нажатии левой кнопки мыши ищет DraggableObjectComponent объект из заданных слоев
    /// </summary>
    public class DetectDraggableObjectSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DraggingObjectComponent>> _draggingObjectFilter = default;
        private readonly EcsFilterInject<Inc<DraggableObjectComponent>> _draggableObjectFilter = default;
        
        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectComponents = default;
        private readonly EcsPoolInject<DraggingObjectComponent> _draggingObjectComponents = default;
        
        private readonly LayerMask _draggableObjectsLayers;
        private EventsBus _events;
        
        public DetectDraggableObjectSystem(LayerMask draggableObjectsLayers)
        {
            _draggableObjectsLayers = draggableObjectsLayers;
        }
        
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
        
        
        public void Run(IEcsSystems systems)
        {
            if (_draggingObjectFilter.Value.GetEntitiesCount() > 0) return;
            if (!_events.HasEventSingleton<LeftMouseDownEvent>()) return;
            
            ref var e = ref _events.GetEventBodySingleton<LeftMouseDownEvent>();
            var mousePosition = e.Position;
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, float.PositiveInfinity, 
                _draggableObjectsLayers);
            if (!hit) return;
            
            foreach (var entity in _draggableObjectFilter.Value)
            {
                ref var o = ref _draggableObjectComponents.Value.Get(entity);
                if (o.View.GetTransform() != hit.transform) continue;
                
                ref var component = ref _draggingObjectComponents.Value.Add(entity);
                var position = o.View.GetTransform().position;
                component.Offset = position - mousePosition;
                component.InitialPosition = o.View.GetObjectPosition();

                // TODO Should Be Cleared
                _events.NewEventSingleton<DraggableObjectWasTakenEvent>();
                break;
            }
        }
    }
}