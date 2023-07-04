using Source.Code.Mono;
using UI.InGame;
using UnityEngine;

namespace Source.Code.SharedData
{
    public class SceneData : MonoBehaviour
    {
        [Header("Scene Setup")]
        public Camera sceneCamera;
        [Space]
        
        
        [Header("Puzzle Grid Handling")]
        public Transform grid;
        public GameObject targetPrefab;
        [Space]
        
        
        [Header("Puzzle Figures Handling")]
        public Transform[] spawnPoints;
        public PuzzleFiguresHandler handler;
        public PowerUpsHandler powerUpsHandler;
        [Space]
        
        
        [Header("Manual Power Ups Handling")]
        public Transform manualPowerUpsStorage;
        [Space]
        
        
        [Header("UI")]
        public InGameUserInterfaceHandler uiHandler;
        [Space]
        
        [Header("Effects")] 
        public DissolveBlocksHandler dissolveBlocksHandler;
    }
}