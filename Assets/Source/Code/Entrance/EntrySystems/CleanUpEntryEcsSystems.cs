using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.CleanUpSystems;
using Source.Code.Components.Events;
using Source.Code.SharedData;

namespace Source.Code.Entrance.EntrySystems
{
    public class CleanUpEntryEcsSystems : IEntryEcsSystems
    {
        public EcsSystems Systems { get; set; }
        public void Init(EcsWorld world, SystemsSharedData sharedData)
        {
            Systems = new EcsSystems(world, sharedData);
            Systems
                .Add(new CleanReleasedObjectsSystem())
                .Add(new DeletePuzzleFigureSystem(sharedData.SceneData.powerUpsHandler))
                .Add(new RemovePowerUpsFromCellsSystem(sharedData.SceneData.powerUpsHandler))
                .Add(new HandleEndGameSystem(sharedData.SceneData.pageManager))
                .Add(new CheckOnEndGameSystem())
                
                .Add(sharedData.EventsBus.GetDestroyEventsSystem()
                    .IncSingleton<LeftMouseDownEvent>()
                    .IncSingleton<RightMouseDownEvent>()
                    .IncSingleton<DraggableObjectWasTakenEvent>()
                    .IncSingleton<FiguresWereSpawnedEvent>()
                )
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