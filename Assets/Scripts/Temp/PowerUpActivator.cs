using System.Collections.Generic;
using Temp;
using UnityEngine;

namespace PuzzleCore
{
    public static class PowerUpActivator
    {
        public static void Activate(PuzzleGrid grid)
        {
            grid.GetRawRepresentation(out var cells, out var cellsByPosition);

            foreach (var cell in cells)
            {
                if (cell.PowerUp != PowerUp.None) Activate(cell, cellsByPosition);
            }
        }

        private static void Activate(Cell powerUpCell, Dictionary<Vector3Int, Cell> cellsByPosition)
        {
            
        }
    }
}