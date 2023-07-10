using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.CleanUpSystems;
using Source.Code.PostGameplaySystems;
using Source.Code.SharedData;

namespace Source.Code.Entrance.EntrySystems
{
    public class PostGameplayEntryEcsSystems : IEntryEcsSystems
    {
        public EcsSystems Systems { get; set; }
        public void Init(EcsWorld world, SystemsSharedData sharedData)
        {
            var sceneData = sharedData.SceneData;
            
            Systems = new EcsSystems(world, sharedData);
            
            Systems
                .Add(new RestartGameSystem())
                .Add(new ChangeFiguresScaleSystem())
                .Add(new ClearTargetedCellsSystem(sceneData.powerUpsHandler))
                .Add(new ClearDestroyableCellsSystem())
                
                .Add(new LightCellsSystem())
                .Add(new CountCoinsAndScoreSystem())
                
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