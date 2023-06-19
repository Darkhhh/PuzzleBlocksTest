using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

namespace Temp.GameplaySystems
{
    public class HandlePuzzleFigureReleaseSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ReleasedObjectComponent>> _releasedFiguresFilter =
            default;
        
        public void Run(IEcsSystems systems)
        {
            if (_releasedFiguresFilter.Value.GetEntitiesCount() == 0) return;
        }
    }
}