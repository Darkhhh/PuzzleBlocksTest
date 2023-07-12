using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;

namespace Source.Code.GameplaySystems
{
    public class ReleaseManualPowerUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, ReleasedObjectComponent>> _releasedManualPowerUpFilter = default;
        private readonly EcsFilterInject<Inc<TargetedCellStateComponent>> _targetedCellsFilter = default;
        
        private EventsBus _events;
        
        
        public void Init(IEcsSystems systems) => _events = systems.GetShared<SystemsSharedData>().EventsBus;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var powerUpEntity in _releasedManualPowerUpFilter.Value)
            {
                ref var dragData = ref _releasedManualPowerUpFilter.Pools.Inc2.Get(powerUpEntity);
                ref var manualPowerUp = ref _releasedManualPowerUpFilter.Pools.Inc1.Get(powerUpEntity);
                
                manualPowerUp.View.transform.position = dragData.InitialPosition;

                if (_targetedCellsFilter.Value.GetEntitiesCount() > 0)
                {
                    manualPowerUp.AvailableAmount--;
                    manualPowerUp.View.SetAmountText(manualPowerUp.AvailableAmount);
                    _events.NewEventSingleton<ClearTargetedCellsEvent>();
                }
                _releasedManualPowerUpFilter.Pools.Inc2.Del(powerUpEntity);
            }
        }
    }
}