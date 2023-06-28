using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Temp.Components;
using Temp.Utils;
using Temp.Views.Cell;

namespace Temp.PostGameplaySystems
{
    public class ResetCellsToDefaultSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent>, Exc<ChangeCellStateComponent>>
            _highlightedCellsFilter = default;
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent, ChangeCellStateComponent>>
            _needToHighlightCellsFilter = default;
        
        private readonly EcsFilterInject<Inc<TargetedCellStateComponent>, Exc<ChangeCellStateComponent>>
            _targetedCellsFilter = default;
        private readonly EcsFilterInject<Inc<TargetedCellStateComponent, ChangeCellStateComponent>>
            _needToTargetCellsFilter = default;
        
        private readonly EcsFilterInject<Inc<AnchoredBlockCellComponent, CellComponent>> _anchoredCellsFilter = default;

        private readonly EcsPoolInject<ChangeCellStateComponent> _changeStateCellsPool = default;
        private readonly EcsPoolInject<DefaultCellStateComponent> _defaultCellsPool = default;
        private readonly EcsPoolInject<SuggestedCellStateComponent> _suggestedCellsPool = default;
        private readonly EcsPoolInject<HighlightedCellStateComponent> _highlightedCellsPool = default;
        private readonly EcsPoolInject<OccupiedCellStateComponent> _occupiedCellsPool = default;
        private readonly EcsPoolInject<DestroyableCellStateComponent> _destroyableCellsPool = default;
        private readonly EcsPoolInject<TargetedCellStateComponent> _targetedCellsPool = default;
        

        public void Run(IEcsSystems systems)
        {
            // Если появились новые подсвечиваемые клетки, убрать подсветку со старых
            if (_highlightedCellsFilter.Value.GetEntitiesCount() > 0 &&
                _needToHighlightCellsFilter.Value.GetEntitiesCount() > 0)
            {
                foreach (var entity in _highlightedCellsFilter.Value)
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity), CellStateEnum.Default);
                    
                    // _highlightedCellsPool.Value.Del(entity);
                    // _defaultCellsPool.Value.Add(entity);
                    // _changeStateCellsPool.Value.Add(entity);
                }
            }
            
            // Если появились новые целевые клетки, убрать цель со старых
            if (_targetedCellsFilter.Value.GetEntitiesCount() > 0 &&
                _needToTargetCellsFilter.Value.GetEntitiesCount() > 0)
            {
                foreach (var entity in _targetedCellsFilter.Value)
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity), CellStateEnum.Occupied);
                    
                    // _targetedCellsPool.Value.Del(entity);
                    // _defaultCellsPool.Value.Add(entity);
                    // _changeStateCellsPool.Value.Add(entity);
                }
            }

            // Если фигура находится вне доски и есть подсвеченные клетки
            if (_anchoredCellsFilter.Value.GetEntitiesCount() == 0 &&
                _highlightedCellsFilter.Value.GetEntitiesCount() > 0)
            {
                foreach (var entity in _highlightedCellsFilter.Value)
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity), CellStateEnum.Default);
                    
                    // _highlightedCellsPool.Value.Del(entity);
                    // _defaultCellsPool.Value.Add(entity);
                    // _changeStateCellsPool.Value.Add(entity);
                }
            }
            
            // Если ручное усиление находится вне доски и есть целевые клетки
            if (_anchoredCellsFilter.Value.GetEntitiesCount() == 0 &&
                _targetedCellsFilter.Value.GetEntitiesCount() > 0)
            {
                foreach (var entity in _targetedCellsFilter.Value)
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity), CellStateEnum.Occupied);
                    
                    // _targetedCellsPool.Value.Del(entity);
                    // _defaultCellsPool.Value.Add(entity);
                    // _changeStateCellsPool.Value.Add(entity);
                }
            }
        }
    }
}