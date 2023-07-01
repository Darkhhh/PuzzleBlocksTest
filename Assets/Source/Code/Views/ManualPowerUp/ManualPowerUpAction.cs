using UnityEngine;

namespace Source.Code.Views.ManualPowerUp
{
    public abstract class ManualPowerUpAction
    {
        public abstract Vector3Int[] GetActivationBlocks();
    }
}