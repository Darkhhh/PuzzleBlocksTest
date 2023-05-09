using System;
using System.Collections.Generic;
using UnityEngine;

namespace Temp
{
    public class PuzzleGrid
    {
        #region Private Values

        private readonly Cell[] _cells;
        private readonly Dictionary<Vector3Int, Cell> _cellsByPosition = new();
        private readonly float _magnetDistance;
        private PuzzleFigure _capturedFigure;

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

        #endregion


        #region Figure Handling

        public bool FigureCanBePlaced(PuzzleFigure figure)
        {
            var blockPositions = figure.BlocksRelativePositions;
            foreach (var cell in _cells)
            {
                if (!cell.Available) continue;
                var positionsForBlocks = 0;
                
                for (int i = 0; i < blockPositions.Length; i++)
                {
                    if (!_cellsByPosition.TryGetValue(cell.Position.GetIntVector() + blockPositions[i].GetIntVector(),
                            out var c)) continue;
                    if (c.Available) positionsForBlocks++;
                }

                if (positionsForBlocks == blockPositions.Length) return true;
            }

            return false;
        }
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
                if (cell.Available) cell.SetColor(Cell.AvailableColor);
                cell.OrderedForPlacement = false;
                cell.AnchorBlockPlacement = false;
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
            placementCell.OrderedForPlacement = true;
            placementCell.SetColor(Cell.HighlightedColor);
            foreach (var cell in _placementCells)
            {
                cell.OrderedForPlacement = true;
                cell.SetColor(Cell.HighlightedColor);
            }

            #endregion
        }

        public bool ReleaseFigure(out int clearedCellsNumber)
        {
            var placed = false;
            foreach (var cell in _cells)
            {
                if (cell.OrderedForPlacement)
                {
                    if (cell.AnchorBlockPlacement) _capturedFigure.transform.position = cell.Position;
                    cell.Available = false;
                    cell.OrderedForPlacement = false;
                    cell.AnchorBlockPlacement = false;
                    cell.SetColor(Cell.UnAvailableColor);
                    placed = true;
                }
                else
                {
                    if (cell.Available) cell.SetColor(Cell.AvailableColor);
                }
            }
            
            _capturedFigure.FigureMoved -= FigureMoved;
            CheckForFullRowsOrColumns(out clearedCellsNumber);
            return placed;
        }

        private void CheckForFullRowsOrColumns(out int clearedCellsNumber)
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

            clearedCellsNumber = clearingCells.Count;
            foreach (var cell in clearingCells)
            {
                cell.Available = true;
                cell.SetColor(Cell.AvailableColor);
                cell.OrderedForPlacement = false;
                cell.AnchorBlockPlacement = false;
            }
        }

        private void ClearCells()
        {
            
        }

        #endregion
    }
}