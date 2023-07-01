using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.InitializeSystems;
using Source.Code.SharedData;

namespace Source.Code.Entrance.EntrySystems
{
    /// <summary>
    /// Здесь находятся системы, которые отрабатывают только после загрузки сцены
    /// </summary>
    public class InitializeSceneEntryEcsSystems : IEntryEcsSystems
    {
        public EcsSystems Systems { get; set; }
        
        public void Init(EcsWorld world, SystemsSharedData sharedData)
        {
            var sceneData = sharedData.SceneData;
            
            Systems = new EcsSystems(world, sharedData);
            Systems
                .Add(new PrepareCellsSystem(sceneData.grid, sceneData.handler, sceneData.targetPrefab))
                .Add(new PrepareManualPowerUpsSystem(sceneData.manualPowerUpsStorage))
                .Add(new PrepareGameInterfaceSystem(sceneData.uiHandler))
                .Inject()
                .Init();
        }

        public void Run() { }
        
        public void Destroy()
        {
            Systems.Destroy();
        }
    }
}