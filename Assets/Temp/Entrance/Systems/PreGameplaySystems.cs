using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.SharedData;
using Temp.PreGameplayRunSystems;
using Temp.SharedData;

namespace Temp.Entrance.Systems
{
    public class PreGameplaySystems : IEntryEcsSystems
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