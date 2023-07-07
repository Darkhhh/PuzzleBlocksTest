using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;
using UnityEngine;

namespace Source.Code.DragSystems
{
    /// <summary>
    /// При нажатии левой кнопки мыши ищет DraggableObjectComponent объект из заданных слоев
    /// </summary>
    public class DetectDraggableObjectSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DraggingObjectComponent>> _draggingObjectFilter = default;
        private readonly EcsFilterInject<Inc<DraggableObjectComponent>> _draggableObjectFilter = default;
        
        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectPool = default;
        private readonly EcsPoolInject<DraggingObjectComponent> _draggingObjectPool = default;
        private readonly EcsPoolInject<DoNotTakeObject> _doNotTakeObjectsPool = default;

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
                ref var o = ref _draggableObjectPool.Value.Get(entity);
                if (o.View.GetTransform() != hit.transform) continue;

                if (_doNotTakeObjectsPool.Value.Has(entity)) break;
                
                ref var component = ref _draggingObjectPool.Value.Add(entity);
                var position = o.View.GetTransform().position;
                component.Offset = position - mousePosition;
                component.InitialPosition = o.View.GetObjectPosition();

                _events.NewEventSingleton<DraggableObjectWasTakenEvent>();
                break;
            }
        }
    }
}