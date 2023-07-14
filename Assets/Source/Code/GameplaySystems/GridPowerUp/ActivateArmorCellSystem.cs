using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Common.Audio;
using Source.Code.Common.Utils;
using Source.Code.Components;
using Source.Code.Mono;
using Source.Code.SharedData;
using Source.Code.Views;
using Source.Code.Views.Cell;

namespace Source.Code.GameplaySystems.GridPowerUp
{
    public class ActivateArmorCellSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter =
            default;

        private readonly
            EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent, CellPowerUpComponent>>_destroyablePowerUpCellsFilter = default;

        private EcsWorld _world;
        private readonly PowerUpsHandler _handler;
        private AudioManager _audio;
        
        public ActivateArmorCellSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_removingFigureFilter.Value.GetEntitiesCount() == 0) return;
            var armoredBlocks = 0;
            foreach (var cellEntity in _destroyablePowerUpCellsFilter.Value)
            {
                ref var cellPowerUp = ref _destroyablePowerUpCellsFilter.Pools.Inc3.Get(cellEntity);
                if (cellPowerUp.Type != PowerUpType.ArmoredBlock) continue;

                armoredBlocks++;
                CellEntity.SetState(_world.PackEntityWithWorld(cellEntity), CellStateEnum.Occupied);
                
                // TODO Add RemovePowerUpComponent
                _handler.ReturnPowerUp(cellPowerUp.View);
                _destroyablePowerUpCellsFilter.Pools.Inc3.Del(cellEntity);
            }

            if (armoredBlocks > 0)
            {
                _audio.Play(SoundTag.DestroyingArmorBlock);
            }
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _audio = systems.GetShared<SystemsSharedData>().SceneData.audioManager;
        }
    }
}