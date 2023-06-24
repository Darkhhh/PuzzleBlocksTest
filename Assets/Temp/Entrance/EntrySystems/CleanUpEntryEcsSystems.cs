using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using Temp.CleanUpSystems;
using Temp.Components.Events;
using Temp.PostGameplaySystems;

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