using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Components;
using Source.Code.Utils;
using Source.Code.Views.Cell;

namespace Source.Code.GameplaySystems
{
    /// <summary>
    /// При отпускании ручного усиления, все клетки, которые находились под его влиянием, лишаются компонента
    /// TargetedCellStateComponent и получают DefaultCellStateComponent и ShouldBeClearedCellComponent
    /// </summary>
    public class HandleManualPowerUpReleaseSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ManualPowerUp, ReleasedObjectComponent>> _releasedManualPowerUpFilter = default;
        private readonly EcsFilterInject<Inc<TargetedCellStateComponent>> _targetedCellsFilter = default;

        private EcsWorld _world;
        
        
        public void Init(IEcsSystems systems) => _world = systems.GetWorld();
        

        public void Run(IEcsSystems systems)
        {
            if (_releasedManualPowerUpFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var cellEntity in _targetedCellsFilter.Value)
            {
                CellEntity.SetState(_world.PackEntityWithWorld(cellEntity), CellStateEnum.Default);
            }
        }
    }
}