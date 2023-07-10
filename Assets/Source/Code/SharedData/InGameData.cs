using UnityEngine;

namespace Source.Code.SharedData
{
    public class InGameData
    {
        public int CurrentScore { get; set; } = 0;
        
        public Vector3 CurrentMousePosition { get; set; }

        public int CoinsAmount { get; set; } = 0;

        public bool Pause { get; set; } = false;
    }
}