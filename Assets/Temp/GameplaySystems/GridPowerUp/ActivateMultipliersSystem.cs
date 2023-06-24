﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Common;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.SharedData;
using PuzzleCore.ECS.Views;
using SevenBoldPencil.EasyEvents;
using Temp.Components.Events;

namespace Temp.GameplaySystems.GridPowerUp
{
    public class ActivateMultipliersSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter =
            default;

        private readonly
            EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent, CellPowerUpComponent>>_destroyablePowerUpCellsFilter = default;
        
        private EventsBus _events;
        
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

        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
    }
}