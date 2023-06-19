using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

namespace Temp.GameplaySystems
{
    public class ReleaseManualPowerUpSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, ReleasedObjectComponent>> _releasedManualPowerUpFilter =
            default;
        private readonly EcsFilterInject<Inc<TargetedCellStateComponent>> _targetedCellsFilter = default;

        private readonly EcsPoolInject<ReleasedObjectComponent> _releasedObjectsPool = default;
        private readonly EcsPoolInject<ManualPowerUp> _manualPowerUpsPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var powerUpEntity in _releasedManualPowerUpFilter.Value)
            {
                ref var dragData = ref _releasedObjectsPool.Value.Get(powerUpEntity);
                ref var manualPowerUp = ref _manualPowerUpsPool.Value.Get(powerUpEntity);
                
                manualPowerUp.View.transform.position = dragData.InitialPosition;

                if (_targetedCellsFilter.Value.GetEntitiesCount() > 0)
                {
                    manualPowerUp.AvailableAmount--;
                }
            }
        }
    }
}