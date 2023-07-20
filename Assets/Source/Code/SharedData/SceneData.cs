using Source.Code.Common.Audio;
using Source.Code.Mono;
using Source.Data;
using Source.Localization;
using Source.UI.Code.InGamePageManagerScripts;
using Source.UI.Code.Pages;
using UnityEngine;
using Zenject;

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
        public InGameUIHandler uiHandler;
        [Space]
        
        [Header("Effects")] 
        public DissolveBlocksHandler dissolveBlocksHandler;
        public DestroyingArmorBlocksHandler destroyingArmorBlocksHandler;
        public ScorePopUpHandler scorePopUpHandler;

        
        [Space] [Header("Localization Handler")]
        public LocalizationHandler localizationHandler;

        [Space] [Header("Page Manager")] 
        public PageManager pageManager;


        [HideInInspector] [Inject] public AudioManager audioManager;
        
        [HideInInspector] [Inject] public IDataHandler DataManager;
    }
}