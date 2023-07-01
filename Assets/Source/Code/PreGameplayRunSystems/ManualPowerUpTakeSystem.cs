using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;

namespace Source.Code.PreGameplayRunSystems
{
    /// <summary>
    /// Проверяет может ли быть взят объект ручного усиления, должен выполнятся до DetectDraggableOverGridObjectSystem
    /// </summary>
    public class ManualPowerUpTakeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, DraggingObjectComponent>> _draggingManualPowerUp = default;
        
        private EventsBus _events;
        
        
        public void Init(IEcsSystems systems) => _events = systems.GetShared<SystemsSharedData>().EventsBus;

        
        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<DraggableObjectWasTakenEvent>()) return;
            
            foreach (var entity in _draggingManualPowerUp.Value)
            {
                ref var powerUp = ref _draggingManualPowerUp.Pools.Inc1.Get(entity);
                
                if (powerUp.AvailableAmount == 0) _draggingManualPowerUp.Pools.Inc2.Del(entity);
            }
        }
    }
}