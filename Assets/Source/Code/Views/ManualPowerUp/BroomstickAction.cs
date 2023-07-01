using UnityEngine;

namespace Source.Code.Views.ManualPowerUp
{
    public class BroomstickAction :ManualPowerUpAction
    {
        public override Vector3Int[] GetActivationBlocks()
        {
            var relativeBlocksPositions = new Vector3Int[16];
            var index = 0;
            for (var i = -8; i < 9; i++)
            {
                if (i == 0) continue;
                relativeBlocksPositions[index] = new Vector3Int(i, 0);
                index++;
            }

            return relativeBlocksPositions;
        }
    }
}