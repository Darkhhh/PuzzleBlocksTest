using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Temp.Components;
using Temp.Components.Events;
using Temp.SharedData;

namespace Temp.GameplaySystems
{
    public class ReleaseManualPowerUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, ReleasedObjectComponent>> _releasedManualPowerUpFilter =
            default;
        private readonly EcsFilterInject<Inc<TargetedCellStateComponent>> _targetedCellsFilter = default;

        private readonly EcsPoolInject<ReleasedObjectComponent> _releasedObjectsPool = default;
        private readonly EcsPoolInject<ManualPowerUp> _manualPowerUpsPool = default;

        private EventsBus _events;
        
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
                    _events.NewEventSingleton<ClearTargetedCellsEvent>();
                }
                _releasedObjectsPool.Value.Del(powerUpEntity);
            }
        }

        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
    }
}