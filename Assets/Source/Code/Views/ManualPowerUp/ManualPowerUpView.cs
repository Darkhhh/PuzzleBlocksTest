using System;
using Source.Code.Utils;
using UnityEngine;

namespace Source.Code.Views.ManualPowerUp
{
    public enum ManualPowerUpType : byte
    {
        CanonBall,
        Broomstick,
        Dynamite,
        LargeDynamite
    }
    
    public class ManualPowerUpView : MonoBehaviour, IDraggableObject, IGridPlaceableObject
    {
        [SerializeField] private ManualPowerUpType type;

        private ManualPowerUpAction _action;
        
        [HideInInspector]
        public Vector3Int[] relativeBlocksPositions;

        public void Init()
        {
            _action = type switch
            {
                ManualPowerUpType.CanonBall => new CanonBallAction(),
                ManualPowerUpType.Broomstick => new BroomstickAction(),
                ManualPowerUpType.Dynamite => new DynamiteAction(),
                ManualPowerUpType.LargeDynamite => new LargeDynamiteAction(),
                _ => throw new ArgumentOutOfRangeException()
            };

            relativeBlocksPositions = _action.GetActivationBlocks();
        }

        
        public Vector3Int GetAnchorBlockPosition() => transform.position.GetIntVector();

        
        public Vector3Int[] GetRelativeBlockPositions() => relativeBlocksPositions;

        
        public Vector3 GetObjectPosition() => transform.position;

        
        public void SetNewPosition(Vector3 newPosition) => transform.position = newPosition;

        
        public Transform GetTransform() => transform;
    }
}