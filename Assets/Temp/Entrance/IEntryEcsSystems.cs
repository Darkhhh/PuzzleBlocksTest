using Leopotam.EcsLite;
using PuzzleCore.ECS.SharedData;

namespace Temp.Entrance
{
    public interface IEntryEcsSystems
    {
        public EcsSystems Systems { get; set; }
        
        public void Init(EcsWorld world, SystemsSharedData sharedData);
        
        public void Run();

        public void Destroy();
    }
}