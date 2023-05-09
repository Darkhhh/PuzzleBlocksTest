using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Grid : MonoBehaviour
{
    private Dictionary<Vector3Int, Cell> _cellsByPosition = new Dictionary<Vector3Int, Cell>();


    [SerializeField] private Cell[] cells;
    [SerializeField] private PuzzleFigure dragObject;
    [SerializeField] private float magnetDistance;
    private bool _dragging = false;
    private Vector3 _initialPosition;
    
    private void Start()
    {
        dragObject.StartedDragging = ObjectStartedDragging;
        dragObject.StoppedDragging = ObjectStoppedDragging;
        dragObject.Dragging = ObjectMoved;

        foreach (var cell in cells)
        {
            _cellsByPosition.Add(cell.Position.GetIntVector(), cell);
        }
    }

    private void ObjectStartedDragging(Vector3 position)
    {
        _initialPosition = position;
        _dragging = true;
    }

    private void ObjectMoved(Vector3 position)
    {
        foreach (var cell in cells)
        {
            if (cell.Available) cell.ChangeColor(false);
            cell.OrderedForPlacement = false;
            cell.AnchorBlockPlacement = false;
        }
        
        Cell placementCell = null;
        var flag = false;
        foreach (var cell in cells)
        {
            if (!cell.Available || !(Math.Sqrt(Math.Pow(cell.Position.x - position.x, 2) +
                                               Math.Pow(cell.Position.y - position.y, 2)) < magnetDistance)) continue;
            placementCell = cell;
            flag = true;
        }

        if (!flag) return;
        
        var blockPositions = dragObject.GetRelativeBlocksPositions(); // Can be cached
        var placementCells = new List<Cell>(blockPositions.Length);
        for (int i = 0; i < blockPositions.Length; i++)
        {
            if (_cellsByPosition.TryGetValue(placementCell.Position.GetIntVector() + blockPositions[i].GetIntVector(),
                    out var c))
            {
                if (c.Available) placementCells.Add(c);
            }
        }

        // Если доступных ячеек меньше, чем необходимых позиций -- выход
        if (placementCells.Count < blockPositions.Length) return;

        placementCell.AnchorBlockPlacement = true;
        placementCell.OrderedForPlacement = true;
        placementCell.ChangeColor(true);
        foreach (var cell in placementCells)
        {
            cell.OrderedForPlacement = true;
            cell.ChangeColor(true);
        }
        
        
        // foreach (var cell in cells)
        // {
        //     if (!cell.Available) continue;
        //     cell.ChangeColor(false);
        //     cell.OrderedForPlacement = false;
        // }
        
        // foreach (var cell in cells)
        // {
        //     if (cell.Available && Math.Sqrt(Math.Pow(cell.Position.x - position.x, 2) +
        //                                     Math.Pow(cell.Position.y - position.y, 2)) < magnetDistance)
        //     {
        //         var blocks = dragObject.GetRelativeBlocksPositions();
        //         var figureCells = new Cell[blocks.Length];
        //         var placeable = true;
        //         for (int i = 0; i < blocks.Length; i++)
        //         {
        //             if (_cellsByPosition.TryGetValue(cell.Position.GetIntVector() + blocks[i].GetIntVector(),
        //                     out var c))
        //             {
        //                 figureCells[i] = c;
        //             }
        //             else
        //             {
        //                 placeable = false;
        //             }
        //         }
        //
        //         if (placeable)
        //         {
        //             foreach (var figureCell in figureCells)
        //             {
        //                 figureCell.ChangeColor(true);
        //                 figureCell.OrderedForPlacement = true;
        //             }
        //             cell.ChangeColor(true);
        //             cell.OrderedForPlacement = true;
        //             break;
        //         }
        //         
        //         // foreach (var blockPosition in blocks)
        //         // {
        //         //     if(_cellsByPosition.TryGetValue(cell.Position.GetIntVector() + blockPosition.GetIntVector(),
        //         //            out var c)) c.ChangeColor(true);
        //         // }        
        //             
        //         // cell.ChangeColor(true);
        //         // cell.OrderedForPlacement = true;
        //         // break;
        //     }
        // }
    }

    private void ObjectStoppedDragging(Vector3 position)
    {
        var placed = false;
        foreach (var cell in cells)
        {
            if (cell.OrderedForPlacement)
            {
                if (cell.AnchorBlockPlacement) dragObject.transform.position = cell.Position;
                cell.Available = false;
                cell.OrderedForPlacement = false;
                cell.AnchorBlockPlacement = false;
                cell.SetBlock();
                placed = true;
            }
            else
            {
                if (cell.Available) cell.ChangeColor(false);
            }
        }

        if (!placed) dragObject.transform.position = _initialPosition;

        // _dragging = false;
        //
        // foreach (var cell in cells)
        // {
        //     if (!cell.OrderedForPlacement && cell.Available)
        //     {
        //         cell.ChangeColor(false);
        //         continue;
        //     }
        //     
        //     dragObject.transform.position = cell.Position;
        //     cell.Available = false;
        //     cell.OrderedForPlacement = false;
        //     cell.SetBlock();
        //     break;
        // }

        // foreach (var cell in cells)
        // {
        //     cell.ChangeColor(false);
        //     cell.OrderedForPlacement = false;
        // }
    }

    private void Update()
    {
        if (_dragging)
        {
            //var position = dragObject.transform.position;
            //Cell targetedCell = null;
            
        }
    }
}
