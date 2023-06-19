using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

namespace Temp.GameplaySystems
{
    public class HandleManualPowerUpReleaseSystem: IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, ReleasedObjectComponent>> _releasedManualPowerUpFilter =
            default;
        
        public void Run(IEcsSystems systems)
        {
            if (_releasedManualPowerUpFilter.Value.GetEntitiesCount() == 0) return;
        }
    }
}