using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

namespace PuzzleCore.ECS.Systems.Drag
{
    public class DetectDraggableOverGridObjectSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DraggingObjectComponent>> _draggingObjectFilter = default;
        
        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectComponents = default;
        private readonly EcsPoolInject<DraggableOverGridComponent> _draggableOverGridObjectComponents = default;
        private readonly EcsPoolInject<DraggingOverGridComponent> _draggingOverGridObjectComponents = default;
        
        public void Run(IEcsSystems systems)
        {
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