using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
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
                .Add(new ChangeFiguresScaleSystem())
                .Add(new ClearTargetedCellsSystem(sceneData.powerUpsHandler))
                .Add(new ClearDestroyableCellsSystem(sceneData.powerUpsHandler))
                
                .Add(new LightCellsSystem())
                .Add(new CountCoinsAndScoreSystem())
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