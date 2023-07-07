using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Common.Utils;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;
using Source.Code.Views;

namespace Source.Code.GameplaySystems.GridPowerUp
{
    public class ActivateMultipliersSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent, CellPowerUpComponent>>_destroyablePowerUpCellsFilter = default;
        
        private EventsBus _events;
        
        
        public void Init(IEcsSystems systems) => _events = systems.GetShared<SystemsSharedData>().EventsBus;
        
        
        public void Run(IEcsSystems systems)
        {
            if (_removingFigureFilter.Value.GetEntitiesCount() == 0) return;
            var multiplier = 1;
            foreach (var cellEntity in _destroyablePowerUpCellsFilter.Value)
            {
                ref var cellPowerUp = ref _destroyablePowerUpCellsFilter.Pools.Inc3.Get(cellEntity);
                if (cellPowerUp.Type is PowerUpType.Coin or PowerUpType.Cross or PowerUpType.ArmoredBlock) continue;

                multiplier *= cellPowerUp.Type.Multiplier();
            }

            if (multiplier == 1) return;
            
            if (!_events.HasEventSingleton<IntermediateResultEvent>())
            {
                ref var data = ref _events.NewEventSingleton<IntermediateResultEvent>();
                data.Multiplier = multiplier;
            }
            else
            {
                ref var data = ref _events.GetEventBodySingleton<IntermediateResultEvent>();
                data.Multiplier = multiplier;
            }
        }

        
    }
}