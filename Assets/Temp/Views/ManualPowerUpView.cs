using System;
using Temp.Utils;
using UnityEngine;

namespace Temp.Views
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
        
        public ManualPowerUpType Type => type;

        public Vector3Int[] _relativeBlocksPositions;

        public void Init()
        {
            switch (type)
            {
                case ManualPowerUpType.CanonBall:
                    _relativeBlocksPositions = Array.Empty<Vector3Int>();
                    break;
                case ManualPowerUpType.Dynamite:
                {
                    _relativeBlocksPositions = new Vector3Int[8];
                    var index = 0;
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (i == 0 && j == 0) continue;
                            _relativeBlocksPositions[index] = new Vector3Int(i, j);
                            index++;
                        }
                    }

                    break;
                }
                case ManualPowerUpType.LargeDynamite:
                {
                    _relativeBlocksPositions = new Vector3Int[24];
                    var index = 0;
                    for (int i = -2; i < 3; i++)
                    {
                        for (int j = -2; j < 3; j++)
                        {
                            if (i == 0 && j == 0) continue;
                            _relativeBlocksPositions[index] = new Vector3Int(i, j);
                            index++;
                        }
                    }

                    break;
                }
                case ManualPowerUpType.Broomstick:
                {
                    _relativeBlocksPositions = new Vector3Int[16];
                    var index = 0;
                    for (var i = -8; i < 9; i++)
                    {
                        if (i == 0) continue;
                        _relativeBlocksPositions[index] = new Vector3Int(i, 0);
                        index++;
                    }

                    break;
                }
                default:
                    _relativeBlocksPositions = Array.Empty<Vector3Int>();
                    break;
            }
        }

        public Vector3Int GetAnchorBlockPosition()
        {
            return transform.position.GetIntVector();
        }

        public Vector3Int[] GetRelativeBlockPositions()
        {
            return _relativeBlocksPositions;
            //return new[] { new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0) };
        }

        public Vector3 GetObjectPosition()
        {
            return transform.position;
        }

        public void SetNewPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}