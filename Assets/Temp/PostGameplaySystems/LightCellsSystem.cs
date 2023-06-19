using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Systems.Experimental.CellHandling;
using UnityEngine;

namespace Temp.PostGameplaySystems
{
    public class LightCellsSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ChangeCellStateComponent, CellComponent>> _changeCellsFilter = default;
        
        private readonly EcsPoolInject<CellComponent> _cellsPool = default;

        private readonly EcsPoolInject<ChangeCellStateComponent> _changeCellsPool = default;
        private readonly EcsPoolInject<DefaultCellStateComponent> _defaultCellsPool = default;
        private readonly EcsPoolInject<SuggestedCellStateComponent> _suggestedCellsPool = default;
        private readonly EcsPoolInject<HighlightedCellStateComponent> _highlightedCellsPool = default;
        private readonly EcsPoolInject<OccupiedCellStateComponent> _occupiedCellsPool = default;
        private readonly EcsPoolInject<DestroyableCellStateComponent> _destroyableCellsPool = default;
        private readonly EcsPoolInject<TargetedCellStateComponent> _targetedCellsPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _changeCellsFilter.Value)
            {
                ref var cell = ref _cellsPool.Value.Get(entity);
                
                if (_defaultCellsPool.Value.Has(entity)) cell.View.ChangeState(CellStateEnum.Default);
                
                else if (_suggestedCellsPool.Value.Has(entity)) cell.View.ChangeState(CellStateEnum.Suggested);
                
                else if (_highlightedCellsPool.Value.Has(entity)) cell.View.ChangeState(CellStateEnum.Highlighted);
                
                else if (_occupiedCellsPool.Value.Has(entity)) cell.View.ChangeState(CellStateEnum.Occupied);
                
                else if (_destroyableCellsPool.Value.Has(entity)) cell.View.ChangeState(CellStateEnum.Destroyable);
                
                else if (_targetedCellsPool.Value.Has(entity)) cell.View.ChangeState(CellStateEnum.Targeted);
                
                _changeCellsPool.Value.Del(entity);
                
                Debug.Log("Changing Cell State");
            }
        }
    }
}