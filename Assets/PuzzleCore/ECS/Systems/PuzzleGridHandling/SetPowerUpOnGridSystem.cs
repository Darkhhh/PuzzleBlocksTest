using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.PuzzleGridHandling
{
    public class SetPowerUpOnGridSystem : IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<CellComponent, CellOrderedForPlacementComponent>> _orderedCellsFilter = default;
        private readonly EcsFilterInject<Inc<ShouldBeRemovedFigureComponent, PuzzleFigureComponent, FigurePowerUpComponent>> 
                    _removingFigureWithPowerUpFilter = default;

        #endregion

        
        #region ECS Pools

        private readonly EcsPoolInject<FigurePowerUpComponent> _figurePowerUpsComponents = default;
        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpComponents = default;
        private readonly EcsPoolInject<CellComponent> _cellsComponents = default;

        #endregion
        

        private readonly PowerUpsHandler _handler;
        public SetPowerUpOnGridSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_removingFigureWithPowerUpFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var entity in _removingFigureWithPowerUpFilter.Value)
            {
                ref var powerUp = ref _figurePowerUpsComponents.Value.Get(entity);
                
                var cellPosition = Vector3.zero;
                var cellEntity = -1;
                var index = 0;

                foreach (var c in _orderedCellsFilter.Value)
                {
                    ref var cell = ref _cellsComponents.Value.Get(c);

                    if (index == powerUp.BlockNumber)
                    {
                        cellEntity = c;
                        cellPosition = cell.Position;
                        break;
                    }

                    index++;
                }
                
                if (powerUp.BlockNumber == -1)
                {
                    cellEntity = _orderedCellsFilter.Value.GetRawEntities()[0];
                    ref var cell = ref _cellsComponents.Value.Get(cellEntity);
                    cellPosition = cell.Position;
                }

                if (cellEntity == -1)
                    throw new Exception("Could not find correct cell");
                
                ref var cellPowerUp = ref _cellPowerUpComponents.Value.Add(cellEntity);
                cellPowerUp.View = _handler.GetPowerUp(powerUp.Type);
                cellPowerUp.Type = powerUp.Type;
                cellPowerUp.View.transform.position = cellPosition;
                cellPowerUp.View.gameObject.SetActive(true);
            }
        }
    }
}