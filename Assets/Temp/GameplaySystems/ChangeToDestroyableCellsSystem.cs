using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Common;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Systems.Experimental.CellHandling;
using Temp.Utils;
using UnityEngine;

namespace Temp.GameplaySystems
{
    public class ChangeToDestroyableCellsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, DraggingObjectComponent>> _draggingFigureFilter = default;
        private readonly EcsFilterInject<Inc<AnchoredBlockCellComponent>> _anchoredCellsFilter = default;

        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        private readonly EcsFilterInject<Inc<DestroyableCellStateComponent>> _destroyableCellsFilter = default;

        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent, ChangeCellStateComponent>> _highlightedCellsFilter = default;

        private readonly EcsPoolInject<ChangeCellStateComponent> _changeStateCellsPool = default;
        private readonly EcsPoolInject<DefaultCellStateComponent> _defaultCellsPool = default;
        private readonly EcsPoolInject<SuggestedCellStateComponent> _suggestedCellsPool = default;
        private readonly EcsPoolInject<HighlightedCellStateComponent> _highlightedCellsPool = default;
        private readonly EcsPoolInject<OccupiedCellStateComponent> _occupiedCellsPool = default;
        private readonly EcsPoolInject<DestroyableCellStateComponent> _destroyableCellsPool = default;
        
        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        
        
        #region Private Values

        private readonly Dictionary<Vector3Int, int> _entityCellsByPosition = new();
        private readonly int _xOffset, _yOffset;
        private int _edge;
        // TODO Change to PackedEntity
        private int _previousAnchoredCellEntity = -1;

        #endregion

        public ChangeToDestroyableCellsSystem(Transform grid)
        {
            var gridPosition = grid.transform.position.GetIntVector();
            _xOffset = gridPosition.x;
            _yOffset = gridPosition.y;
        }
        

        public void Init(IEcsSystems systems)
        {
            ref var firstCell = ref _cellComponents.Value.Get(_cellsFilter.Value.GetRawEntities()[0]);
            _edge = Mathf.RoundToInt((float)Math.Sqrt(_cellsFilter.Value.GetEntitiesCount())) / 2;
            
            foreach (var entity in _cellsFilter.Value)
            {
                ref var c = ref _cellComponents.Value.Get(entity);
                _entityCellsByPosition.Add(c.Position.GetIntVector(), entity);
            }
        }
        
        
        public void Run(IEcsSystems systems)
        {
            if (_draggingFigureFilter.Value.GetEntitiesCount() == 0) return;
            
            // Если нет якорных клеток, значит фигура либо вне доски, либо не может быть установлена
            if (_anchoredCellsFilter.Value.GetEntitiesCount() == 0)
            {
                ClearOldValues(systems);
                _previousAnchoredCellEntity = -1;
                return;
            }
            // Если якорная клетка та же, то значит фигура место не меняла
            foreach (var anchorEntity in _anchoredCellsFilter.Value)
            {
                if (anchorEntity == _previousAnchoredCellEntity) return;

                _previousAnchoredCellEntity = anchorEntity;
                break;
            }
            

            // if (_highlightedCellsFilter.Value.GetEntitiesCount() == 0)
            // {
            //     ClearOldValues(systems);
            //     return;
            // }
            
            // Если якорная клетка та же, то значит фигура место не меняла
            // foreach (var anchorEntity in _anchoredCellsFilter.Value)
            // {
            //     if (anchorEntity == _previousAnchoredCellEntity) return;
            //
            //     _previousAnchoredCellEntity = anchorEntity;
            //     break;
            // }
            
            //ClearOldValues(systems);

            
            



            var clearingCells = new List<int>();
            for (var x = -_edge; x <= _edge; x += 1)
            {
                var columnClearingCellsEntities = new List<int>();
                
                for (var y = -_edge; y <= _edge; y += 1)
                {
                    var cellPosition = new Vector3Int(x + _xOffset, y + _yOffset);
                    if (!_entityCellsByPosition.TryGetValue(cellPosition, out var entity))
                        throw new Exception($"Can't reach cell by {cellPosition} position");
                    
                    if (!_occupiedCellsPool.Value.Has(entity) && !_highlightedCellsPool.Value.Has(entity))
                    {
                        columnClearingCellsEntities.Clear();
                        break;
                    }
                    columnClearingCellsEntities.Add(entity);
                }
                
                clearingCells.AddRange(columnClearingCellsEntities);
            }
            
            for (var y = -_edge; y <= _edge; y += 1)
            {
                var rowClearingCellsEntities = new List<int>();
                
                for (var x = -_edge; x <= _edge; x += 1)
                {
                    var cellPosition = new Vector3Int(x + _xOffset, y + _yOffset);
                    if (!_entityCellsByPosition.TryGetValue(cellPosition, out var entity))
                        throw new Exception($"Can't reach cell by {cellPosition} position");
                    
                    if (!_occupiedCellsPool.Value.Has(entity) && !_highlightedCellsPool.Value.Has(entity))
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
                if (_highlightedCellsPool.Value.Has(entity))
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity),
                        CellStateEnum.Destroyable, CellStateEnum.Highlighted);
                }

                if (_occupiedCellsPool.Value.Has(entity))
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity),
                        CellStateEnum.Destroyable, CellStateEnum.Occupied);
                }
                
                
                // var previousState = CellStateEnum.Default;
                // if (_highlightedCellsPool.Value.Has(entity))
                // {
                //     _highlightedCellsPool.Value.Del(entity);
                //     previousState = CellStateEnum.Highlighted;
                // }
                //
                // if (_occupiedCellsPool.Value.Has(entity))
                // {
                //     _occupiedCellsPool.Value.Del(entity);
                //     previousState = CellStateEnum.Occupied;
                // }
                //     
                //
                // if (!_changeStateCellsPool.Value.Has(entity)) _changeStateCellsPool.Value.Add(entity);
                // if (!_destroyableCellsPool.Value.Has(entity))
                // {
                //     ref var component = ref _destroyableCellsPool.Value.Add(entity);
                //     component.PreviousState = previousState;
                // }
            }
        }


        private void ClearOldValues(IEcsSystems systems)
        {
            // Очистить старые значения, если есть, добавить новые, если есть
            foreach (var destroyableCellEntity in _destroyableCellsFilter.Value)
            {
                ref var component = ref _destroyableCellsPool.Value.Get(destroyableCellEntity);
                switch (component.PreviousState)
                {
                    case CellStateEnum.Highlighted:
                        CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(destroyableCellEntity),
                            CellStateEnum.Default);
                        //_defaultCellsPool.Value.Add(destroyableCellEntity);
                        break;
                    case CellStateEnum.Occupied:
                        //_occupiedCellsPool.Value.Add(destroyableCellEntity);
                        CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(destroyableCellEntity),
                            CellStateEnum.Occupied);
                        break;
                    case CellStateEnum.Default or CellStateEnum.Suggested or CellStateEnum.Destroyable or CellStateEnum.Targeted:
                        throw new Exception("Incorrect previous state for destroyable cell");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                // if (!_changeStateCellsPool.Value.Has(destroyableCellEntity))
                //     _changeStateCellsPool.Value.Add(destroyableCellEntity);
                // _destroyableCellsPool.Value.Del(destroyableCellEntity);
            }
        }
    }
}