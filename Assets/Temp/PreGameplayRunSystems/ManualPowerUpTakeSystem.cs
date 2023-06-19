using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;

namespace Temp.PreGameplayRunSystems
{
    /// <summary>
    /// Проверяет может ли быть взят объект ручного усиления, должен выполнятся до DetectDraggableOverGridObjectSystem
    /// </summary>
    public class ManualPowerUpTakeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, DraggingObjectComponent>> _draggingManualPowerUp = default;
        private readonly EcsPoolInject<ManualPowerUp> _manualPowerUpComponents = default;
        private readonly EcsPoolInject<DraggingObjectComponent> _draggingObjectComponents = default;
        
        private EventsBus _events;
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<DraggableObjectWasTakenEvent>()) return;
            
            foreach (var entity in _draggingManualPowerUp.Value)
            {
                ref var powerUp = ref _manualPowerUpComponents.Value.Get(entity);
                
                if (powerUp.AvailableAmount == 0) _draggingObjectComponents.Value.Del(entity);
            }
        }
    }
}