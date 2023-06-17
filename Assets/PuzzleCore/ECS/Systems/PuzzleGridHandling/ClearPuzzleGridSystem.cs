using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Views;

namespace PuzzleCore.ECS.Systems.PuzzleGridHandling
{
    /// <summary>
    /// Убирает блоки с клеток доски, которые помечены компонентом ShouldBeClearedCellComponent
    /// </summary>
    public class ClearPuzzleGridSystem : IEcsRunSystem
    {
        #region ECS Filters

        private readonly EcsFilterInject<Inc<CellComponent, ShouldBeClearedCellComponent>> _clearingCellsFilter = default;

        #endregion


        #region ECS Pools

        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        
        private readonly EcsPoolInject<ShouldBeClearedCellComponent> _clearingCellsComponents = default;

        #endregion
        
        

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _clearingCellsFilter.Value)
            {
                ref var c = ref _cellComponents.Value.Get(entity);
                //TODO Experiments
                //c.View.ChangeState(CellState.Default);
                //c.View.SetSimple();
                //c.Available = true;
                _clearingCellsComponents.Value.Del(entity);
            }
        }
    }
}