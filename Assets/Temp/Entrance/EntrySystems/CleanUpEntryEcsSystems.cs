using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Temp.CleanUpSystems;
using Temp.Components.Events;
using Temp.SharedData;

namespace Temp.Entrance.EntrySystems
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