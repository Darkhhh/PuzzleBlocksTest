using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.SharedData;
using Source.Code.UpdateUserInterfaceSystems;

namespace Source.Code.Entrance.EntrySystems
{
    public class UpdateUserInterfaceEntryEcsSystems : IEntryEcsSystems
    {
        public EcsSystems Systems { get; set; }
        public void Init(EcsWorld world, SystemsSharedData sharedData)
        {
            Systems = new EcsSystems(world, sharedData);
            Systems
                .Add(new UpdateInGameUISystem(sharedData.SceneData.uiHandler))
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