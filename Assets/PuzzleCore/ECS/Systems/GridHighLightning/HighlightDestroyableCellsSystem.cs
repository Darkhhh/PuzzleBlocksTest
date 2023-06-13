using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Common;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using PuzzleCore.ECS.Views;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.GridHighLightning
{
    public class HighlightDestroyableCellsSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        
        private readonly EcsFilterInject<Inc<ShouldBeRemovedFigureComponent>> _removedFiguresFilter = default;
        
        private readonly EcsFilterInject<Inc<CellOrderedForPlacementComponent, CellComponent>> _orderedCellsFilter = default;

        #endregion

        
        #region ECS Pools

        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        
        private readonly EcsPoolInject<ShouldBeClearedCellComponent> _clearingCellsComponents = default;
        
        private readonly EcsPoolInject<CellOrderedForPlacementComponent> _orderedCellsComponents = default;

        #endregion

        
        #region Private Values

        private readonly Dictionary<Vector3Int, int> _entityCellsByPosition = new();
        private int _xOffset, _yOffset, _edge;
        private EventsBus _events;

        #endregion
        
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
            ref var firstCell = ref _cellComponents.Value.Get(_cellsFilter.Value.GetRawEntities()[0]);
            _xOffset = (int) firstCell.View.ParentPosition.x;
            _yOffset = (int) firstCell.View.ParentPosition.y;
            _edge = Mathf.RoundToInt((float)Math.Sqrt(_cellsFilter.Value.GetEntitiesCount())) / 2;
            foreach (var entity in _cellsFilter.Value)
            {
                ref var c = ref _cellComponents.Value.Get(entity);
                _entityCellsByPosition.Add(c.Position.GetIntVector(), entity);
            }
        }

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<HighlightGridEvent>()) return;

            foreach (var entity in _cellsFilter.Value)
            {
                ref var cell = ref _cellComponents.Value.Get(entity);
                cell.View.ChangeState(CellView.CellState.Occupied);
            }
            
            var clearingCells = new List<int>();
            for (var x = -_edge; x <= _edge; x += (int)CellView.Size)
            {
                var columnClearingCellsEntities = new List<int>();
                
                for (var y = -_edge; y <= _edge; y += (int)CellView.Size)
                {
                    var cellPosition = new Vector3Int(x + _xOffset, y + _yOffset);
                    if (!_entityCellsByPosition.TryGetValue(cellPosition, out var entity))
                        throw new Exception($"Can't reach cell by {cellPosition} position");
                    
                    ref var cell = ref _cellComponents.Value.Get(entity);
                    if (cell.Available && ! _orderedCellsComponents.Value.Has(entity))
                    {
                        columnClearingCellsEntities.Clear();
                        break;
                    }
                    columnClearingCellsEntities.Add(entity);
                }
                
                clearingCells.AddRange(columnClearingCellsEntities);
            }
            
            for (var y = -_edge; y <= _edge; y += (int)CellView.Size)
            {
                var rowClearingCellsEntities = new List<int>();
                
                for (var x = -_edge; x <= _edge; x += (int)CellView.Size)
                {
                    var cellPosition = new Vector3Int(x + _xOffset, y + _yOffset);
                    if (!_entityCellsByPosition.TryGetValue(cellPosition, out var entity))
                        throw new Exception($"Can't reach cell by {cellPosition} position");
                    ref var cell = ref _cellComponents.Value.Get(entity);
                    if (cell.Available && ! _orderedCellsComponents.Value.Has(entity))
                    {
                        rowClearingCellsEntities.Clear();
                        break;
                    }
                    rowClearingCellsEntities.Add(entity);
                }
                
                clearingCells.AddRange(rowClearingCellsEntities);
            }

            foreach (var entity in clearingCells)
            {
                ref var cell = ref _cellComponents.Value.Get(entity);
                //TODO Experiments
                cell.View.ChangeState(CellView.CellState.Destroyable);
                //cell.View.SetDestroyable(cell.Available);
            }
        }
    }
}