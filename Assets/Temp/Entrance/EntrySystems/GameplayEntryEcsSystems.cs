using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.SharedData;
using Temp.DragSystems;
using Temp.GameplaySystems;
using Temp.GameplaySystems.GridPowerUp;

namespace Temp.Entrance.EntrySystems
{
    public class GameplayEntryEcsSystems : IEntryEcsSystems
    {
        public EcsSystems Systems { get; set; }
        
        
        public void Init(EcsWorld world, SystemsSharedData sharedData)
        {
            var sceneData = sharedData.SceneData;
            Systems = new EcsSystems(world, sharedData);
            Systems
                .Add(new MoveDraggingObjectSystem())
                .Add(new DragOverGridHandleSystem())
                
                .Add(new ChangeToTargetCellsSystem())
                .Add(new ChangeToDestroyableCellsSystem(sceneData.grid))
                .Add(new SuggestCellsForFigureSystem())
                
                .Add(new ReleaseDraggingOverGridObjectSystem())
                .Add(new ReleaseDraggingObjectSystem())
                .Add(new ReleasePuzzleFigureSystem())
                .Add(new ReleaseManualPowerUpSystem())
                
                .Add(new HandleManualPowerUpReleaseSystem())
                .Add(new SetPowerUpOnGridSystem(sceneData.powerUpsHandler))
                .Add(new HandlePuzzleFigureReleaseSystem())
                
                .Add(new ActivateCrossSystem())
                .Add(new ActivateCoinSystem())
                .Add(new ActivateArmorCellSystem(sceneData.powerUpsHandler))
                .Add(new ActivateMultipliersSystem())
                
                .Inject()
                .Init();
        }

        
        public void Run()
        {
            Systems?.Run();
        }

        public void Destroy()
        {
            Systems?.Destroy();
        }
    }
}