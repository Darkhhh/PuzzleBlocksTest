using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Temp.Components;

namespace Temp.CleanUpSystems
{
    public class CleanReleasedObjectsSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ReleasedObjectComponent>> _releasedObjectsFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _releasedObjectsFilter.Value)
            {
                _releasedObjectsFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}