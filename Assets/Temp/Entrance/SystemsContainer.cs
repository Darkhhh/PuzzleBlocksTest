using System.Collections.Generic;
using Leopotam.EcsLite;
using PuzzleCore.ECS.SharedData;

namespace Temp.Entrance
{
    public class SystemsContainer
    {
        private readonly List<IEntryEcsSystems> _systems = new();
        private readonly EcsWorld _world;
        private readonly SystemsSharedData _sharedData;

        public SystemsContainer(EcsWorld world, SystemsSharedData sharedData)
        {
            _world = world;
            _sharedData = sharedData;
        }

        public SystemsContainer Add(IEntryEcsSystems systems)
        {
            _systems.Add(systems);
            return this;
        }

        public SystemsContainer Init()
        {
            foreach (var system in _systems) system.Init(_world, _sharedData);
            return this;
        }
        
        public void Run()
        {
            foreach (var system in _systems) system.Run();
        }

        public void Destroy()
        {
            foreach (var system in _systems) system.Destroy();
        }
    }
}