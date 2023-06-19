using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using PuzzleCore.ECS.Systems.Drag;
using PuzzleCore.ECS.Systems.FigureHandling;
using PuzzleCore.ECS.Systems.GridHighLightning;
using PuzzleCore.ECS.Systems.Main;
using PuzzleCore.ECS.Systems.ManualPowerUpsHandling;
using PuzzleCore.ECS.Systems.PowerUpHandling;
using PuzzleCore.ECS.Systems.PuzzleGridHandling;
using PuzzleCore.ECS.Systems.UI.InGame;
using SevenBoldPencil.EasyEvents;
using UI.InGame;
using UnityEngine;

namespace PuzzleCore.ECS
{
    public class EcsStartUp : MonoBehaviour
    {
        #region Systems

        private IEcsSystems _systems, _initSystems, _clearUpSystems, _userInterfaceSystems;

        #endregion

        #region Serialized Fields

        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Transform grid;
        
        //TODO Take To Const C# File
        [SerializeField] private float magnetDistance;
        
        
        [SerializeField] private Transform[] spawnPoints;

        //TODO Take To Const C# File
        [SerializeField] private float powerUpProbability;
        
        
        [SerializeField] private PuzzleFiguresHandler handler;
        [SerializeField] private PowerUpsHandler powerUpsHandler;
        [SerializeField] private Transform manualPowerUpsStorage;
        [SerializeField] private EditorEcsStartUp editorEcsStartUp;
        [SerializeField] private InGameUserInterfaceHandler uiHandler;
        [SerializeField] private GameObject targetPrefab;
        #endregion
        
        private void Awake() 
        {
            var sharedData = new SystemsSharedData { EventsBus = new EventsBus(), GameData = new InGameData() };
            _systems = new EcsSystems(new EcsWorld(), sharedData);
            _initSystems = new EcsSystems(_systems.GetWorld(), sharedData);
            _userInterfaceSystems = new EcsSystems(_systems.GetWorld(), sharedData);
            _clearUpSystems = new EcsSystems(_systems.GetWorld(), sharedData);
            
            #region UNITY_EDITOR

#if UNITY_EDITOR
            editorEcsStartUp.Initialize(_systems.GetWorld());
#endif

            #endregion

            #region Init Systems

            _initSystems
                .Add(new AssignCellsSystem(grid))                                                                                       // +
                .Add(new InjectPuzzleBlocksToCells(handler))                                                                            // +
                .Add(new InjectTargetBlockSystem(targetPrefab))                                                                         // +
                .Add(new AssignManualPowerUpsSystem(manualPowerUpsStorage))                                                             // +
                .Add(new InGameUserInterfaceInitSystem(uiHandler))                                                                      // +
                .Inject()
                .Init();

            #endregion

            #region GamePlay Systems

            _systems
                .Add(new DetectUserInputSystem(sceneCamera))                                                                            // +
                .Add(new AssignPowerUpToFigureSystem(spawnPoints.Length, powerUpProbability))                                           // +
                .Add(new CreateFiguresSystem(handler, spawnPoints))                                                                     // +
                .Add(new AddPowerUpOnFigureSystem(powerUpsHandler))                                                                     // +
                
                
                
                .Add(new DetectDraggableObjectSystem(LayerMask.GetMask("PuzzleFigure", "ManualPowerUp")))        // +
                .Add(new DetectDraggableOverGridObjectSystem())                                                                         // +
                .Add(new HandleManualPowerUpDragSystem())                                                                               // +
                .Add(new MoveDraggingObjectSystem())                                                                                    // +
                .Add(new DragOverGridHandleSystem(magnetDistance))                                                                      // +
                
                
                .Add(new CountPointsSystem())
                .Add(new ClearPuzzleGridSystem())
                
                .Add(new ReleaseDraggingOverGridObjectSystem())                                                                         // +
                .Add(new ReleaseDraggingObjectSystem())                                                                                 // +
                .Add(new ReleaseFigureSystem())                                                                                         // +
                
                .Add(new HandlePuzzleFigureDragSystem())                                                                                // +
                .Add(new HandleManualPowerUpReleaseSystem())                                                                            // +
                .Add(new RoughClearPuzzleGridSystem(powerUpsHandler))
                
                .Add(new HighlightDestroyableCellsSystem())                                                                             // +
                .Add(new ReHighlightGridSystem())
                .Add(new DeHighlightGridSystem())

                .Add(new SetPowerUpOnGridSystem(powerUpsHandler))
                .Add(new CheckFullRowAndColumnsSystem())                                                                                // +

                .Add(new ActivateCrossPowerUpSystem(powerUpsHandler))
                .Add(new ActivateMultiplierPowerUpSystem(powerUpsHandler))
                .Add(new ActivateCoinPowerUpSystem(powerUpsHandler))
                .Add(new ActivateArmoredBlockPowerUpSystem(powerUpsHandler))
                
                .Add(new ChangeFigureScaleSystem())
                
                .Add(new RemovePowerUpFromFigureSystem(powerUpsHandler))
                .Add(new DeSpawnFigureSystem())                                                                                         // +
                .Add(new CheckOnEndGameSystem())                                                                                        // +
                .Add(new HighlightSingleFigurePlaceSystem())                                                                            // +
                
                .Inject()
                .Init();

            #endregion

            #region UI Systems

            _userInterfaceSystems
                .Add(new UpdateScoreSystem(uiHandler))
                .Add(new HandleRestartButtonSystem(uiHandler, powerUpsHandler))
                .Inject()
                .Init();

            #endregion
            
            RegisterClearUpSystems(sharedData);
        }
    
        void Update() 
        {
            _systems?.Run();
            _userInterfaceSystems?.Run();
            _clearUpSystems?.Run();
        }
    
        void OnDestroy() 
        {
            if (_systems != null)
            {
                var sharedData = _systems.GetShared<SystemsSharedData>();
                sharedData.EventsBus.Destroy();
                _systems.Destroy();
                _systems.GetWorld().Destroy();
                _systems = null;
                _clearUpSystems = null;
            }
        }


        private void RegisterClearUpSystems(SystemsSharedData systemsSharedData)
        {
            _clearUpSystems
                .Add(systemsSharedData.EventsBus.GetDestroyEventsSystem()
                    .IncSingleton<CheckOnEndGameComponent>()
                    .IncSingleton<DraggableObjectWasTakenEvent>()
                    .IncSingleton<DecreaseAllFiguresScaleComponent>()
                    .IncSingleton<ChangeFigureScaleComponent>()
                    .IncSingleton<LeftMouseDownEvent>()
                    .IncSingleton<RightMouseDownEvent>()
                    .IncSingleton<CurrentMousePositionEvent>()
                    .IncSingleton<HighlightGridEvent>()
                    .IncSingleton<DeHighlightGridEvent>()
                    .IncSingleton<UpdateScoreEvent>()
                    .IncSingleton<RoughClearEvent>())
                .Inject()
                .Init();
        }
    }
}
