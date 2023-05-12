﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Temp
{
    public class PuzzleGrid
    {
        #region Private Values

        private readonly GameObject _view;
        
        private readonly List<Cell> _cells = new();
        
        private readonly Dictionary<Vector3Int, Cell> _cellsByPosition = new();
        
        private readonly float _magnetDistance;

        #endregion
        
        
        #region Cached Utility Values

        // 9 - probably max puzzle figure size, it's just for tiny optimization
        private readonly List<Cell> _placementCells = new(9);

        #endregion
        
        
        public PuzzleGrid(GameObject sceneObject, float magnetDistance)
        {
            _view = sceneObject;
            _magnetDistance = magnetDistance;
            foreach (Transform cell in sceneObject.transform)
            {
                _cells.Add(new Cell(cell.gameObject, sceneObject.transform));
            }
            foreach (var cell in _cells) _cellsByPosition.Add(cell.Position.GetIntVector(), cell);
        }
        
        
        #region Figure Can Be Placed

        public bool FigureCanBePlaced(Figure figure)
        {
            return _cells.Where(cell => cell.Status == CellStatus.Available).Any(cell => FigureCanBePlaced(figure, cell));
        }

        private bool FigureCanBePlaced(Figure figure, out int availablePositions)
        {
            availablePositions = _cells
                .Where(cell => cell.Status == CellStatus.Available)
                .Count(cell => FigureCanBePlaced(figure, cell));
            return availablePositions > 0;
        }

        private bool FigureCanBePlaced(Figure figure, Cell anchoredCell)
        {
            if (anchoredCell.Status == CellStatus.UnAvailable) return false;
            var blockPositions = figure.BlocksRelativePositions;
            var positionsForBlocks = 0;
                
            for (var i = 0; i < blockPositions.Length; i++)
            {
                if (!_cellsByPosition
                        .TryGetValue(anchoredCell.Position.GetIntVector() + blockPositions[i].GetIntVector(), out var c)) 
                    continue;
                if (c.Status == CellStatus.Available) positionsForBlocks++;
            }

            return positionsForBlocks == blockPositions.Length;
        }

        #endregion


        public void FigureMoved(Figure figure, Vector3 newPosition)
        {
            foreach (var cell in _cells.Where(cell => cell.Status == CellStatus.Available)) cell.SetAvailable();
            _placementCells.Clear();
            Cell placementCell = null;
            var flag = false;
            
            

            foreach (var cell in _cells)
            {
                if (cell.Status == CellStatus.UnAvailable || 
                    !(Math.Sqrt(Math.Pow(cell.Position.x - newPosition.x, 2) +
                                Math.Pow(cell.Position.y - newPosition.y, 2)) < _magnetDistance)) continue;
                placementCell = cell;
                flag = true;
            }

            if (!flag) return;

            

            var blockPositions = figure.BlocksRelativePositions;
            
            for (int i = 0; i < blockPositions.Length; i++)
            {
                if (_cellsByPosition.TryGetValue(placementCell.Position.GetIntVector() + blockPositions[i].GetIntVector(),
                        out var c))
                {
                    if (c.Status == CellStatus.Available) _placementCells.Add(c);
                }
            }

            if (_placementCells.Count < blockPositions.Length) return;
            

            placementCell.AdditionalStatus = AdditionalCellStatus.Anchored;
            placementCell.SetHighlighted();
            foreach (var cell in _placementCells) cell.SetHighlighted();
        }
        
        
        #region Releasing Figure

        public bool ReleaseFigure(Figure figure, out int clearedCellsNumber)
        {
            clearedCellsNumber = 0;
            var placed = (
                    from cell in _cells 
                    where cell.AdditionalStatus == AdditionalCellStatus.Anchored 
                    select SetCapturedFigureOnGrid(figure, cell))
                .FirstOrDefault();
            if (!placed) return false;
            
            SetPuzzleFigurePowerUps();
            ActivatePuzzleFigurePowerUps();
            CheckForFullRowsOrColumns();
            ClearCells(out clearedCellsNumber);
            return true;
        }
        
        private bool SetCapturedFigureOnGrid(Figure figure, Cell anchoredCell)
        {
            // TODO Change to _placementCells
            var blockPositions = figure.BlocksRelativePositions;
            var placementCells = new List<Cell>(blockPositions.Length) {anchoredCell};
            for (int i = 0; i < blockPositions.Length; i++)
            {
                if (_cellsByPosition
                    .TryGetValue(anchoredCell.Position.GetIntVector() + blockPositions[i].GetIntVector(), out var cell))
                {
                    placementCells.Add(cell);
                }
                else
                {
                    placementCells.Clear();
                    return false;
                }
            }

            foreach (var placementCell in placementCells) placementCell.SetUnavailable();
            
            return true;
        }

        private void SetPuzzleFigurePowerUps()
        {
            
        }

        private void ActivatePuzzleFigurePowerUps()
        {
            
        }
        
        private void CheckForFullRowsOrColumns()
        {
            var xOffset = (int) _cells[0].ParentPosition.x;
            var yOffset = (int)_cells[0].ParentPosition.y;
            var edge = Mathf.RoundToInt((float)Math.Sqrt(_cells.Count)) / 2;
            var clearingCells = new List<Cell>();
            for (var x = -edge; x <= edge; x += (int)Cell.Size)
            {
                var columnClearingCells = new List<Cell>();
                
                for (var y = -edge; y <= edge; y += (int)Cell.Size)
                {
                    var cellPosition = new Vector3Int(x + xOffset, y + yOffset);
                    if (!_cellsByPosition.TryGetValue(cellPosition, out var cell))
                        throw new Exception($"Can't reach cell by {cellPosition} position");
                    if (cell.Status == CellStatus.Available)
                    {
                        columnClearingCells.Clear();
                        break;
                    }
                    columnClearingCells.Add(cell);
                }
                
                clearingCells.AddRange(columnClearingCells);
            }
            
            for (var y = -edge; y <= edge; y += (int)Cell.Size)
            {
                var rowClearingCells = new List<Cell>();
                
                for (var x = -edge; x <= edge; x += (int)Cell.Size)
                {
                    var cellPosition = new Vector3Int(x + xOffset, y + yOffset);
                    if (!_cellsByPosition.TryGetValue(cellPosition, out var cell))
                        throw new Exception($"Can't reach cell by {cellPosition} position");
                    if (cell.Status == CellStatus.Available)
                    {
                        rowClearingCells.Clear();
                        break;
                    }
                    rowClearingCells.Add(cell);
                }
                
                clearingCells.AddRange(rowClearingCells);
            }

            foreach (var cell in clearingCells)
            {
                cell.AdditionalStatus = AdditionalCellStatus.ShouldBeCleared;
            }
        }
        
        private void ClearCells(out int clearedCells)
        {
            clearedCells = 0;
            foreach (var cell in _cells.Where(cell => cell.AdditionalStatus == AdditionalCellStatus.ShouldBeCleared))
            {
                clearedCells++;
                cell.SetAvailable();
            }
        }

        #endregion
    }
}