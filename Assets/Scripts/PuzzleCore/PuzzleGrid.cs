using System;
using System.Collections.Generic;
using System.Linq;
using Temp;
using UnityEngine;

namespace PuzzleCore
{
    public class PuzzleGrid
    {
        #region Private Values

        private readonly Cell[] _cells;
        private readonly Dictionary<Vector3Int, Cell> _cellsByPosition = new();
        private readonly float _magnetDistance;
        private PuzzleFigure _capturedFigure;

        #endregion


        #region Actions

        public Action<PowerUp> PowerUpWasSet;

        #endregion


        #region Cached Utility Values

        // 9 - probably max puzzle figure size, it's just for tiny optimization
        private readonly List<Cell> _placementCells = new(9);

        #endregion


        #region Contructors

        public PuzzleGrid(Cell[] cells, float magnetDistance)
        {
            _cells = cells;
            _magnetDistance = magnetDistance;
            foreach (var cell in cells)
            {
                _cellsByPosition.Add(cell.Position.GetIntVector(), cell);
            }
        }

        public PuzzleGrid(Transform cellsParentObject, float magnetDistance)
        {
            _magnetDistance = magnetDistance;
            _cells = (from Transform child 
                    in cellsParentObject.transform 
                    select child.GetComponent<Cell>()).ToArray();

            foreach (var cell in _cells) _cellsByPosition.Add(cell.Position.GetIntVector(), cell);
        }

        #endregion

        
        #region Figure Can Be Placed

        public bool FigureCanBePlaced(PuzzleFigure figure)
        {
            return _cells.Where(cell => cell.Available).Any(cell => FigureCanBePlaced(figure, cell));
        }
        
        private bool CapturedFigureCanBePlaced(Cell cell, out List<Cell> figureCells)
        {
            if (_capturedFigure is null) throw new NullReferenceException();
            
            var blockPositions = _capturedFigure.BlocksRelativePositions;
            figureCells = new List<Cell>(blockPositions.Length);
            var placementCells = 0;
            
            for (var i = 0; i < blockPositions.Length; i++)
            {
                if (!_cellsByPosition.TryGetValue(cell.Position.GetIntVector() + blockPositions[i].GetIntVector(),
                        out var c)) continue;
                if (!c.Available) continue;
                placementCells++;
                figureCells.Add(c);
            }

            return placementCells == blockPositions.Length;
        }

        private bool FigureCanBePlaced(PuzzleFigure figure, out int availablePositions)
        {
            availablePositions = _cells.Where(cell => cell.Available).Count(cell => FigureCanBePlaced(figure, cell));
            return availablePositions > 0;
        }

        private bool FigureCanBePlaced(PuzzleFigure figure, Cell anchoredCell)
        {
            if (!anchoredCell.Available) return false;
            var blockPositions = figure.BlocksRelativePositions;
            var positionsForBlocks = 0;
                
            for (var i = 0; i < blockPositions.Length; i++)
            {
                if (!_cellsByPosition
                        .TryGetValue(anchoredCell.Position.GetIntVector() + blockPositions[i].GetIntVector(), out var c)) 
                    continue;
                if (c.Available) positionsForBlocks++;
            }

            return positionsForBlocks == blockPositions.Length;
        }

        #endregion
        
        
        #region Figure Handling

        
        public void SetCapturedFigure(PuzzleFigure figure)
        {
            _capturedFigure = figure ? figure : 
                throw new NullReferenceException("PuzzleGrid: Got null instead puzzle figure");
            _capturedFigure.FigureMoved += FigureMoved;
        }

        private void FigureMoved(Vector3 newPosition)
        {
            #region Clearing Values

            foreach (var cell in _cells)
            {
                if (cell.Available) cell.SetAvailable();
            }
            _placementCells.Clear();
            Cell placementCell = null;
            var flag = false;

            #endregion

            #region Search For Placement Cell

            foreach (var cell in _cells)
            {
                if (!cell.Available || !(Math.Sqrt(Math.Pow(cell.Position.x - newPosition.x, 2) +
                                                   Math.Pow(cell.Position.y - newPosition.y, 2)) < _magnetDistance)) continue;
                placementCell = cell;
                flag = true;
            }

            if (!flag) return;

            #endregion

            #region Check If There Available Cells For Figure

            var blockPositions = _capturedFigure.BlocksRelativePositions;
            
            for (int i = 0; i < blockPositions.Length; i++)
            {
                if (_cellsByPosition.TryGetValue(placementCell.Position.GetIntVector() + blockPositions[i].GetIntVector(),
                        out var c))
                {
                    if (c.Available) _placementCells.Add(c);
                }
            }

            if (_placementCells.Count < blockPositions.Length) return;

            #endregion

            #region Highlight Place And Store Placement Cell

            placementCell.AnchorBlockPlacement = true;
            placementCell.SetHighlighted();
            foreach (var cell in _placementCells) cell.SetHighlighted();

            #endregion
        }

        #endregion


        #region Releasing Figure

        public bool ReleaseFigure(out int clearedCellsNumber)
        {
            var anchorCell = (from cell in _cells where cell.AnchorBlockPlacement select cell).FirstOrDefault();

            var placed = SetCapturedFigureOnGrid(anchorCell);
            SetPuzzleFigurePowerUps(_capturedFigure, anchorCell);
            _capturedFigure.FigureMoved -= FigureMoved;
            ActivatePuzzleFigurePowerUps();
            CheckForFullRowsOrColumns();
            ClearCells(out clearedCellsNumber);
            return placed;
        }
        
        private bool SetCapturedFigureOnGrid(Cell anchoredCell)
        {
            if (_capturedFigure is null) throw new NullReferenceException();
            
            var blockPositions = _capturedFigure.BlocksRelativePositions;
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

        private void SetPuzzleFigurePowerUps(PuzzleFigure figure, Cell anchorCell)
        {
            if (!figure.GetPowerUps(out var powerUps)) return;

            var blocksPositions = figure.BlocksRelativePositions;

            foreach (var tuple in powerUps)
            {
                if (!_cellsByPosition.TryGetValue(
                        (anchorCell.Position + blocksPositions[tuple.blockIndex]).GetIntVector(),
                        out var cell)) throw new Exception("Could not reach cell");
                cell.PowerUp = tuple.powerUp;
                PowerUpWasSet?.Invoke(cell.PowerUp);
            }
        }

        private void ActivatePuzzleFigurePowerUps()
        {
            PowerUpActivator.Activate(this);
        }
        
        private void CheckForFullRowsOrColumns()
        {
            var xOffset = (int) _cells[0].ParentPosition.x;
            var yOffset = (int)_cells[0].ParentPosition.y;
            var edge = Mathf.RoundToInt((float)Math.Sqrt(_cells.Length)) / 2;
            var clearingCells = new List<Cell>();
            for (var x = -edge; x <= edge; x += (int)Cell.Size)
            {
                var columnClearingCells = new List<Cell>();
                
                for (var y = -edge; y <= edge; y += (int)Cell.Size)
                {
                    var cellPosition = new Vector3Int(x + xOffset, y + yOffset);
                    if (!_cellsByPosition.TryGetValue(cellPosition, out var cell))
                        throw new Exception($"Can't reach cell by {cellPosition} position");
                    if (cell.Available)
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
                    if (cell.Available)
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
                cell.ShouldBeCleared = true;
            }
        }
        
        private void ClearCells(out int clearedCells)
        {
            clearedCells = 0;
            foreach (var cell in _cells)
            {
                if (!cell.ShouldBeCleared) continue;
                clearedCells++;
                cell.SetAvailable();
            }
        }

        #endregion


        #region Power Ups

        public void GetRawRepresentation(out Cell[] cells, out Dictionary<Vector3Int, Cell> cellsByPosition)
        {
            cells = _cells;
            cellsByPosition = _cellsByPosition;
        }

        #endregion
    }
}