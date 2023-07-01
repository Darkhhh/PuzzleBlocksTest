using UnityEngine;

namespace Source.Code.Views
{
    public interface IGridPlaceableObject
    {
        public Vector3Int GetAnchorBlockPosition();

        public Vector3Int[] GetRelativeBlockPositions();
    }
}