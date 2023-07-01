using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;

namespace Source.Code.DragSystems
{
    /// <summary>
    /// Если найден объект, который теперь перетаскивается по сцене, определяет является ли он перетаскиваемым по
    /// доске (DraggableOverGridComponent), если да, то вешает на него DraggingOverGridComponent
    /// </summary>
    public class DetectDraggableOverGridObjectSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DraggingObjectComponent>> _draggingObjectFilter = default;
        
        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectComponents = default;
        private readonly EcsPoolInject<DraggableOverGridComponent> _draggableOverGridObjectComponents = default;
        private readonly EcsPoolInject<DraggingOverGridComponent> _draggingOverGridObjectComponents = default;

        private EventsBus _events;
        
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
        
        
        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<DraggableObjectWasTakenEvent>()) return;
            if (_draggingObjectFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var entity in _draggingObjectFilter.Value)
            {
                if (_draggableOverGridObjectComponents.Value.Has(entity) &&
                    !_draggingOverGridObjectComponents.Value.Has(entity))
                {
                    ref var t = ref _draggableOverGridObjectComponents.Value.Get(entity);
                    ref var v = ref _draggableObjectComponents.Value.Get(entity);
                    ref var o = ref _draggingOverGridObjectComponents.Value.Add(entity);
                    o.PlaceableObject = t.PlaceableObject;
                    o.CheckOnCellAvailability = t.CheckOnCellAvailability;
                    o.MustBeFullOnGrid = t.MustBeFullOnGrid;
                    o.CurrentPosition = v.View.GetObjectPosition();
                }
            }
        }
    }
}