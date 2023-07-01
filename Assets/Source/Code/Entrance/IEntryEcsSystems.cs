using Leopotam.EcsLite;
using Source.Code.SharedData;

namespace Source.Code.Entrance
{
    public interface IEntryEcsSystems
    {
        public EcsSystems Systems { get; set; }
        
        public void Init(EcsWorld world, SystemsSharedData sharedData);
        
        public void Run();

        public void Destroy();
    }
}