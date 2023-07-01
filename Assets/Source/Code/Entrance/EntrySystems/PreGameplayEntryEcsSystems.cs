using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.DragSystems;
using Source.Code.PostGameplaySystems;
using Source.Code.PreGameplayRunSystems;
using Source.Code.SharedData;
using UnityEngine;

namespace Source.Code.Entrance.EntrySystems
{
    public class PreGameplayEntryEcsSystems : IEntryEcsSystems
    {
        public EcsSystems Systems { get; set; }
        public void Init(EcsWorld world, SystemsSharedData sharedData)
        {
            var sceneData = sharedData.SceneData;
            Systems = new EcsSystems(world, sharedData);
            Systems
                .Add(new HandleUserInputSystem(sceneData.sceneCamera))
                .Add(new SpawnFiguresSystem(sceneData.handler, sceneData.spawnPoints))
                .Add(new AssignPowerUpToFigureSystem())
                .Add(new AddPowerUpOnFigureSystem(sceneData.powerUpsHandler))
                .Add(new FindPlaceForFiguresSystem())
                .Add(new DetectDraggableObjectSystem(LayerMask.GetMask("PuzzleFigure", "ManualPowerUp")))
                .Add(new PuzzleFigureTakeSystem())
                .Add(new ManualPowerUpTakeSystem())
                .Add(new DetectDraggableOverGridObjectSystem())
                .Add(new ResetCellsToDefaultSystem())
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