using UnityEngine;

namespace PuzzleCore.ECS.Views
{
    public interface IDraggableObject
    {
        public Vector3 GetObjectPosition();

        public void SetNewPosition(Vector3 newPosition);

        public Transform GetTransform();
    }
}