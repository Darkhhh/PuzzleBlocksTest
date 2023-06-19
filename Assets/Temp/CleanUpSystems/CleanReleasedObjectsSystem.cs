using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

namespace Temp.CleanUpSystems
{
    public class CleanReleasedObjectsSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ReleasedObjectComponent>> _releasedObjectsFilter = default;

        private readonly EcsPoolInject<ReleasedObjectComponent> _releasedObjectsPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _releasedObjectsFilter.Value)
            {
                _releasedObjectsPool.Value.Del(entity);
            }
        }
    }
}