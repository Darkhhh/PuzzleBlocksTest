using UnityEngine;

namespace Source.Code.Views.ManualPowerUp
{
    public class LargeDynamiteAction :ManualPowerUpAction
    {
        public override Vector3Int[] GetActivationBlocks()
        {
            var relativeBlocksPositions = new Vector3Int[24];
            var index = 0;
            for (var i = -2; i < 3; i++)
            {
                for (var j = -2; j < 3; j++)
                {
                    if (i == 0 && j == 0) continue;
                    relativeBlocksPositions[index] = new Vector3Int(i, j);
                    index++;
                }
            }

            return relativeBlocksPositions;
        }
    }
}