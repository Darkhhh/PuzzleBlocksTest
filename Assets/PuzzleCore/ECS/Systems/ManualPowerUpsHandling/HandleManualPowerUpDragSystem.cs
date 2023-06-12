using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

namespace PuzzleCore.ECS.Systems.ManualPowerUpsHandling
{
    /// <summary>
    /// Проверяет взятое ручное усиление на возможность использования. Если кол-во равно нулю, сбрасывает с объекта
    /// усиления компонент DraggingObjectComponent.
    /// </summary>
    public class HandleManualPowerUpDragSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, DraggingObjectComponent>> _draggingManualPowerUp = default;
        private readonly EcsPoolInject<ManualPowerUp> _manualPowerUpComponents = default;
        private readonly EcsPoolInject<DraggingObjectComponent> _draggingObjectComponents = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _draggingManualPowerUp.Value)
            {
                ref var powerUp = ref _manualPowerUpComponents.Value.Get(entity);
                
                if (powerUp.AvailableAmount == 0) _draggingObjectComponents.Value.Del(entity);
            }
        }
    }
}