using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore;
using PuzzleCore.ECS.Common;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Views;
using UnityEngine;

namespace Temp.GameplaySystems
{
    public class SetPowerUpOnGridSystem : IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<ShouldBeRemovedFigureComponent, PuzzleFigureComponent, FigurePowerUpComponent>> 
                    _removingFigureWithPowerUpFilter = default;
        
        private readonly EcsFilterInject<Inc<CellComponent, HighlightedCellStateComponent>> _highlightedCellsFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent>> _destroyableCellsFilter = default;
        
        #endregion

        
        #region ECS Pools

        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpComponents = default;

        #endregion
        

        private readonly PowerUpsHandler _handler;
        public SetPowerUpOnGridSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_removingFigureWithPowerUpFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var figureEntity in _removingFigureWithPowerUpFilter.Value)
            {
                ref var powerUp = ref _removingFigureWithPowerUpFilter.Pools.Inc3.Get(figureEntity);
                ref var puzzleFigure = ref _removingFigureWithPowerUpFilter.Pools.Inc2.Get(figureEntity);

                Vector3Int powerUpPosition;
                if (powerUp.BlockNumber == -1)
                {
                    powerUpPosition = puzzleFigure.View.transform.position.GetIntVector();
                }
                else
                {
                    powerUpPosition = puzzleFigure.View.transform.position.GetIntVector() + 
                                      puzzleFigure.RelativeBlockPositions[powerUp.BlockNumber].GetIntVector();
                }

                foreach (var cellEntity in _highlightedCellsFilter.Value)
                {
                    if (CellCheck(cellEntity, powerUpPosition, powerUp.Type)) return;
                }
                
                foreach (var cellEntity in _destroyableCellsFilter.Value)
                {
                    if (CellCheck(cellEntity, powerUpPosition, powerUp.Type)) return;
                }
            }
        }

        private bool CellCheck(int cellEntity, Vector3Int powerUpPosition, PowerUpType powerUpType)
        {
            ref var cell = ref _highlightedCellsFilter.Pools.Inc1.Get(cellEntity);

            if (cell.Position.GetIntVector() != powerUpPosition) return false;
            
            ref var cellPowerUp = ref _cellPowerUpComponents.Value.Add(cellEntity);
            cellPowerUp.View = _handler.GetPowerUp(powerUpType);
            cellPowerUp.Type = powerUpType;
            cellPowerUp.View.transform.position = cell.Position;
            cellPowerUp.View.gameObject.SetActive(true);
            
            return true;
        }
    }
}