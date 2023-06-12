using UnityEngine;

namespace PuzzleCore.ECS.Views
{
    public interface IGridPlaceableObject
    {
        public Vector3Int GetAnchorBlockPosition();

        public Vector3Int[] GetRelativeBlockPositions();
    }
}