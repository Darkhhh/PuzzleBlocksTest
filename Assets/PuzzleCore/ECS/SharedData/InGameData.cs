using UnityEngine;

namespace PuzzleCore.ECS.SharedData
{
    public class InGameData
    {
        public int CurrentScore { get; set; }
        
        public Vector3 CurrentMousePosition { get; set; }
        
        public int CoinsAmount { get; set; }
    }
}