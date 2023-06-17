using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Common;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Views;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.PowerUpHandling
{
    public class ActivateCrossPowerUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<CellComponent, ShouldBeClearedCellComponent, CellPowerUpComponent>>
            _clearedCellsWithPowerUpsFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;

        #endregion


        #region ECS Pools

        private readonly EcsPoolInject<CellComponent> _cellComponents = default;

        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpComponents = default;

        private readonly EcsPoolInject<ShouldBeClearedCellComponent> _shouldBeClearedCellComponents = default;

        #endregion
        

        private readonly PowerUpsHandler _handler;
        
        private readonly Dictionary<Vector3Int, int> _entityCellsByPosition = new();
        private int _xOffset, _yOffset, _edge;
        
        public ActivateCrossPowerUpSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Init(IEcsSystems systems)
        {
            if (_entityCellsByPosition.Count > 0) return;
            
            _entityCellsByPosition.Clear();
            foreach (var entity in _cellsFilter.Value)
            {
                ref var c = ref _cellComponents.Value.Get(entity);
                _entityCellsByPosition.Add(c.Position.GetIntVector(), entity);
            }
            
            ref var firstCell = ref _cellComponents.Value.Get(_cellsFilter.Value.GetRawEntities()[0]);
            //_xOffset = (int) firstCell.View.ParentPosition.x;
            //_yOffset = (int) firstCell.View.ParentPosition.y;
            _edge = Mathf.RoundToInt((float)Math.Sqrt(_cellsFilter.Value.GetEntitiesCount())) / 2;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_clearedCellsWithPowerUpsFilter.Value.GetEntitiesCount() == 0) return;
            
            foreach (var cellEntity in _clearedCellsWithPowerUpsFilter.Value)
            {
                ref var cellPowerUp = ref _cellPowerUpComponents.Value.Get(cellEntity);
                
                if (cellPowerUp.Type != PowerUpType.Cross) continue;
                
                _handler.ReturnPowerUp(cellPowerUp.View);
                _cellPowerUpComponents.Value.Del(cellEntity);
                
                ref var cell = ref _cellComponents.Value.Get(cellEntity);
                CompleteCellsCross(cell.Position.GetIntVector());
            }

            foreach (var cellEntity in _clearedCellsWithPowerUpsFilter.Value)
            {
                ref var cellPowerUp = ref _cellPowerUpComponents.Value.Get(cellEntity);
                if (cellPowerUp.Type != PowerUpType.Cross) continue;
                
                _handler.ReturnPowerUp(cellPowerUp.View);
                _cellPowerUpComponents.Value.Del(cellEntity);
            }
        }

        private void CreateCellsDictionary()
        {
            if (_entityCellsByPosition.Count > 0) return;
            
            _entityCellsByPosition.Clear();
            foreach (var entity in _cellsFilter.Value)
            {
                ref var c = ref _cellComponents.Value.Get(entity);
                _entityCellsByPosition.Add(c.Position.GetIntVector(), entity);
            }
        }

        private void CompleteCellsCross(Vector3Int crossCellPosition)
        {
            for (var x = -_edge; x <= _edge; x += (int)CellView.Size)
            {
                var position = new Vector3Int(x + _xOffset, crossCellPosition.y + _yOffset);
                if (!_entityCellsByPosition.TryGetValue(position, out var entity))
                    throw new Exception($"Can't reach cell by {position} position");
                
                ref var cell = ref _cellComponents.Value.Get(entity);
                //if (cell.Available) continue;
                
                if (_shouldBeClearedCellComponents.Value.Has(entity)) continue;
                _shouldBeClearedCellComponents.Value.Add(entity);

                if (_cellPowerUpComponents.Value.Has(entity))
                {
                    ref var p = ref _cellPowerUpComponents.Value.Get(entity);
                    if (p.Type != PowerUpType.Cross) continue;
                    CompleteCellsCross(cell.Position.GetIntVector());
                }
            }
            
            for (var y = -_edge; y <= _edge; y += (int)CellView.Size)
            {
                var position = new Vector3Int(crossCellPosition.x + _xOffset, y + _yOffset);
                if (!_entityCellsByPosition.TryGetValue(position, out var entity))
                    throw new Exception($"Can't reach cell by {position} position");
                
                ref var cell = ref _cellComponents.Value.Get(entity);
                //if (cell.Available) continue;
                
                
                if (_shouldBeClearedCellComponents.Value.Has(entity)) continue;
                _shouldBeClearedCellComponents.Value.Add(entity);
                
                if (_cellPowerUpComponents.Value.Has(entity))
                {
                    ref var p = ref _cellPowerUpComponents.Value.Get(entity);
                    if (p.Type != PowerUpType.Cross) continue;
                    CompleteCellsCross(cell.Position.GetIntVector());
                }
            }
        }

        
    }
}