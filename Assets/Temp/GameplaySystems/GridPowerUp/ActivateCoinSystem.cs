using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.SharedData;
using PuzzleCore.ECS.Systems.Experimental.CellHandling;
using PuzzleCore.ECS.Views;
using SevenBoldPencil.EasyEvents;
using Temp.Components.Events;

namespace Temp.GameplaySystems.GridPowerUp
{
    public class ActivateCoinSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter = default;

        private readonly EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent, CellPowerUpComponent>> _destroyablePowerUpCellsFilter = default;

        private EventsBus _events;
        
        public void Run(IEcsSystems systems)
        {
            if (_removingFigureFilter.Value.GetEntitiesCount() == 0) return;
            var coinsAmount = 0;
            foreach (var cellEntity in _destroyablePowerUpCellsFilter.Value)
            {
                ref var cellPowerUp = ref _destroyablePowerUpCellsFilter.Pools.Inc3.Get(cellEntity);
                if (cellPowerUp.Type != PowerUpType.Coin) continue;

                coinsAmount++;
            }

            if (coinsAmount == 0) return;

            if (!_events.HasEventSingleton<IntermediateResultEvent>())
            {
                ref var data = ref _events.NewEventSingleton<IntermediateResultEvent>();
                data.CoinsAmount = coinsAmount;
            }
            else
            {
                ref var data = ref _events.GetEventBodySingleton<IntermediateResultEvent>();
                data.CoinsAmount = coinsAmount;
            }
        }

        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
    }
}