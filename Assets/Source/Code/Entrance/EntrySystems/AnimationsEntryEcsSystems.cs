using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.AnimationSystems;
using Source.Code.SharedData;
using UnityEngine;

namespace Source.Code.Entrance.EntrySystems
{
    public class AnimationsEntryEcsSystems : IEntryEcsSystems
    {
        public EcsSystems Systems { get; set; }
        
        
        public void Init(EcsWorld world, SystemsSharedData sharedData)
        {
            Systems = new EcsSystems(world, sharedData);
            
            Systems
                .Add(new AnimateDestroyableCellsSystem(sharedData.SceneData.dissolveBlocksHandler, new Vector3(4,4)))
                .Add(new SwapFiguresAndPowerUpsSystem())
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