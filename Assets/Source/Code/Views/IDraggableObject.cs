using UnityEngine;

namespace Source.Code.Views
{
    public interface IDraggableObject
    {
        public Vector3 GetObjectPosition();

        public void SetNewPosition(Vector3 newPosition);

        public Transform GetTransform();
    }
}