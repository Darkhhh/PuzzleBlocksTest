using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.SharedData;
using Temp.PostGameplaySystems;

namespace Temp.Entrance.EntrySystems
{
    public class PostGameplayEntryEcsSystems : IEntryEcsSystems
    {
        public EcsSystems Systems { get; set; }
        public void Init(EcsWorld world, SystemsSharedData sharedData)
        {
            var sceneData = sharedData.SceneData;
            
            Systems = new EcsSystems(world, sharedData);
            
            Systems
                .Add(new ResetCellsToDefaultSystem())
                .Add(new LightCellsSystem())
                .Add(new CheckOnEndGameSystem())
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