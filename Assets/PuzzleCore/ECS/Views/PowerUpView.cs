using UnityEngine;

namespace PuzzleCore.ECS.Views
{
    public enum PowerUpType : byte
    {
        Coin, 
        Cross,
        ArmoredBlock,
        MultiplierX2,
        MultiplierX5,
        MultiplierX10
    }
    
    public class PowerUpView : MonoBehaviour
    {
        public PowerUpType type;

        public float weight;
    }
}