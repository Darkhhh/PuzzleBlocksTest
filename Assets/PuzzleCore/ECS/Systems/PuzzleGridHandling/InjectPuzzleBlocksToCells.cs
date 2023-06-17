using Leopotam.EcsLite;
using PuzzleCore.ECS.Components;

namespace PuzzleCore.ECS.Systems.PuzzleGridHandling
{
    public class InjectPuzzleBlocksToCells : IEcsInitSystem
    {
        private readonly PuzzleFiguresHandler _handler;
        
        public InjectPuzzleBlocksToCells(PuzzleFiguresHandler handler)
        {
            _handler = handler;
        }
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var cellsEntities = world.Filter<CellComponent>().End();
            var cells = world.GetPool<CellComponent>();

            foreach (var entity in cellsEntities)
            {
                ref var cell = ref cells.Get(entity);
                
                //cell.View.InjectPuzzleBlock(_handler.GetPuzzleBlock());
            }
        }
    }
}