using UnityEngine;

namespace Source.Code.Views.ManualPowerUp
{
    public class DynamiteAction : ManualPowerUpAction
    {
        public override Vector3Int[] GetActivationBlocks()
        {
            var relativeBlocksPositions = new Vector3Int[8];
            var index = 0;
            for (var i = -1; i < 2; i++)
            {
                for (var j = -1; j < 2; j++)
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