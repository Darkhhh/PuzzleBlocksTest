﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Components;
using Source.Code.Mono;
using Source.Code.Utils;
using Source.Code.Views;
using Source.Code.Views.Cell;

namespace Source.Code.GameplaySystems.GridPowerUp
{
    public class ActivateArmorCellSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter =
            default;

        private readonly
            EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent, CellPowerUpComponent>>_destroyablePowerUpCellsFilter = default;

        private EcsWorld _world;
        private readonly PowerUpsHandler _handler;
        
        public ActivateArmorCellSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_removingFigureFilter.Value.GetEntitiesCount() == 0) return;
            foreach (var cellEntity in _destroyablePowerUpCellsFilter.Value)
            {
                ref var cellPowerUp = ref _destroyablePowerUpCellsFilter.Pools.Inc3.Get(cellEntity);
                if (cellPowerUp.Type != PowerUpType.ArmoredBlock) continue;

                CellEntity.SetState(_world.PackEntityWithWorld(cellEntity), CellStateEnum.Occupied);
                
                // TODO Add RemovePowerUpComponent
                _handler.ReturnPowerUp(cellPowerUp.View);
                _destroyablePowerUpCellsFilter.Pools.Inc3.Del(cellEntity);
            }
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }
    }
}