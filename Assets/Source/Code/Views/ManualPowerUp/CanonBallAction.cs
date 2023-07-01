using System;
using UnityEngine;

namespace Source.Code.Views.ManualPowerUp
{
    public class CanonBallAction : ManualPowerUpAction
    {
        public override Vector3Int[] GetActivationBlocks() => Array.Empty<Vector3Int>();
    }
}