using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

namespace Temp.PostGameplaySystems
{
    public class DeletePuzzleFigureSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter =
            default;

        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectPool = default;
        
        private readonly EcsPoolInject<DraggableOverGridComponent> _draggableOverGridObjectPool = default;
        
        private readonly EcsPoolInject<AvailablePlacesComponent> _figurePlacePool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _removingFigureFilter.Value)
            {
                ref var puzzleFigure = ref _removingFigureFilter.Pools.Inc1.Get(entity);
                
                
                puzzleFigure.View.gameObject.SetActive(false);

                _removingFigureFilter.Pools.Inc1.Del(entity);
                _removingFigureFilter.Pools.Inc2.Del(entity);
                
                _draggableObjectPool.Value.Del(entity);
                _draggableOverGridObjectPool.Value.Del(entity);
                _figurePlacePool.Value.Del(entity);
            }
        }
    }
}