using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Temp.Components;
using Temp.Utils;
using Temp.Views.Cell;

namespace Temp.GameplaySystems
{
    /// <summary>
    /// При отпускании ручного усиления, все клетки, которые находились под его влиянием, лишаются компонента
    /// TargetedCellStateComponent и получают DefaultCellStateComponent и ShouldBeClearedCellComponent
    /// </summary>
    public class HandleManualPowerUpReleaseSystem: IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, ReleasedObjectComponent>> _releasedManualPowerUpFilter =
            default;

        private readonly EcsFilterInject<Inc<TargetedCellStateComponent>> _targetedCellsFilter = default;

        private readonly EcsPoolInject<TargetedCellStateComponent> _targetedCellsPool = default;
        private readonly EcsPoolInject<DefaultCellStateComponent> _defaultCellsPool = default;
        private readonly EcsPoolInject<ChangeCellStateComponent> _changeCellsPool = default;
        //private readonly EcsPoolInject<ShouldBeClearedCellComponent> _clearingCellsPool = default;

        public void Run(IEcsSystems systems)
        {
            if (_releasedManualPowerUpFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var cellEntity in _targetedCellsFilter.Value)
            {
                // _targetedCellsPool.Value.Del(cellEntity);
                // _defaultCellsPool.Value.Add(cellEntity);
                // _changeCellsPool.Value.Add(cellEntity);
                
                CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(cellEntity), CellStateEnum.Default);
                    //_clearingCellsPool.Value.Add(cellEntity);
            }
        }
    }
}