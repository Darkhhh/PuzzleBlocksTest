using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Common.Utils;
using Source.Code.Components;
using Source.Code.Views.Cell;

namespace Source.Code.PostGameplaySystems
{
    public class ResetCellsToDefaultSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent>, Exc<ChangeCellStateComponent>> _highlightedCellsFilter = default;
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent, ChangeCellStateComponent>> _needToHighlightCellsFilter = default;
        private readonly EcsFilterInject<Inc<TargetedCellStateComponent>, Exc<ChangeCellStateComponent>> _targetedCellsFilter = default;
        private readonly EcsFilterInject<Inc<TargetedCellStateComponent, ChangeCellStateComponent>> _needToTargetCellsFilter = default;
        private readonly EcsFilterInject<Inc<AnchoredBlockCellComponent, CellComponent>> _anchoredCellsFilter = default;
        

        public void Run(IEcsSystems systems)
        {
            // Если появились новые подсвечиваемые клетки, убрать подсветку со старых
            if (_highlightedCellsFilter.Value.GetEntitiesCount() > 0 &&
                _needToHighlightCellsFilter.Value.GetEntitiesCount() > 0)
            {
                foreach (var entity in _highlightedCellsFilter.Value)
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity), CellStateEnum.Default);
                }
            }
            
            // Если появились новые целевые клетки, убрать цель со старых
            if (_targetedCellsFilter.Value.GetEntitiesCount() > 0 &&
                _needToTargetCellsFilter.Value.GetEntitiesCount() > 0)
            {
                foreach (var entity in _targetedCellsFilter.Value)
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity), CellStateEnum.Occupied);
                }
            }

            // Если фигура находится вне доски и есть подсвеченные клетки
            if (_anchoredCellsFilter.Value.GetEntitiesCount() == 0 &&
                _highlightedCellsFilter.Value.GetEntitiesCount() > 0)
            {
                foreach (var entity in _highlightedCellsFilter.Value)
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity), CellStateEnum.Default);
                }
            }
            
            // Если ручное усиление находится вне доски и есть целевые клетки
            if (_anchoredCellsFilter.Value.GetEntitiesCount() == 0 &&
                _targetedCellsFilter.Value.GetEntitiesCount() > 0)
            {
                foreach (var entity in _targetedCellsFilter.Value)
                {
                    CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(entity), CellStateEnum.Occupied);
                }
            }
        }
    }
}