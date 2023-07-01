using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Components;
using Source.Code.SharedData;

namespace Source.Code.DragSystems
{
    /// <summary>
    /// Перемещает перетаскиваемый объект (DraggingObjectComponent) по сцене
    /// </summary>
    public class MoveDraggingObjectSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DraggingObjectComponent>> _draggingObjectFilter = default;
        
        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectComponents = default;
        private readonly EcsPoolInject<DraggingOverGridComponent> _draggingOverGridObjectComponents = default;
        
        private InGameData _gameData;
        
        
        public void Init(IEcsSystems systems)
        {
            _gameData = systems.GetShared<SystemsSharedData>().GameData;
        }
        
        
        public void Run(IEcsSystems systems)
        {
            if (_draggingObjectFilter.Value.GetEntitiesCount() == 0) return;

            var position = _gameData.CurrentMousePosition;
            
            foreach (var entity in _draggingObjectFilter.Value)
            {
                ref var o = ref _draggableObjectComponents.Value.Get(entity);
                ref var draggingData = ref _draggingObjectFilter.Pools.Inc1.Get(entity);
                
                o.View.GetTransform().position = position + draggingData.Offset;

                if (_draggingOverGridObjectComponents.Value.Has(entity))
                {
                    ref var t = ref _draggingOverGridObjectComponents.Value.Get(entity);
                    t.CurrentPosition = o.View.GetTransform().position;
                }
            }
        }
    }
}