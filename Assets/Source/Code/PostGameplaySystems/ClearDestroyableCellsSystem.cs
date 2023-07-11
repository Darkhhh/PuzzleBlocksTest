using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Common.Audio;
using Source.Code.Common.Utils;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.Mono;
using Source.Code.SharedData;
using Source.Code.Views;
using Source.Code.Views.Cell;

namespace Source.Code.PostGameplaySystems
{
    /// <summary>
    /// Очищает Destroyable клетки и усиления с них, должна отрабатывать после активации всех усилений
    /// </summary>
    public class ClearDestroyableCellsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent>>_destroyableCellsFilter = default;

        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpsPool = default;
        private readonly EcsPoolInject<RemovePowerUpComponent> _clearCellPowerUpPool = default;

        private EventsBus _events;
        private AudioManager _audio;

        public void Init(IEcsSystems systems)
        {
            var shared = systems.GetShared<SystemsSharedData>();
            _events = shared.EventsBus;
            _audio = shared.SceneData.audioManager;
        }
        
        
        public void Run(IEcsSystems systems)
        {
            if (_removingFigureFilter.Value.GetEntitiesCount() == 0) return;
            
            var earnedPoints = _destroyableCellsFilter.Value.GetEntitiesCount();

            if (earnedPoints > 0)
            {
                if (!_events.HasEventSingleton<IntermediateResultEvent>())
                {
                    ref var data = ref _events.NewEventSingleton<IntermediateResultEvent>();
                    data.DestroyedCells = earnedPoints;
                }
                else
                {
                    ref var data = ref _events.GetEventBodySingleton<IntermediateResultEvent>();
                    data.DestroyedCells = earnedPoints;
                }
            }

            bool crosses = false, coins = false, multipliers = false, armor = false;
            foreach (var cellEntity in _destroyableCellsFilter.Value)
            {
                CellEntity.SetState(systems.GetWorld().PackEntityWithWorld(cellEntity), CellStateEnum.Default);
                
                if (_cellPowerUpsPool.Value.Has(cellEntity))
                {
                    ref var powerUp = ref _cellPowerUpsPool.Value.Get(cellEntity);

                    switch (powerUp.Type)
                    {
                        case PowerUpType.Cross:
                            crosses = true;
                            break;
                        case PowerUpType.Coin:
                            coins = true;
                            break;
                        case PowerUpType.ArmoredBlock:
                            armor = true;
                            break;
                        case PowerUpType.MultiplierX2 or PowerUpType.MultiplierX5 or PowerUpType.MultiplierX10:
                            multipliers = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                    _clearCellPowerUpPool.Value.Add(cellEntity);
                }
            }
            
            if (crosses) _audio.Play(SoundTag.ActivatingCross);
            if (coins)
            {
                _audio.Play(multipliers ? SoundTag.CollectCoinsWithMultipliers : SoundTag.CollectingCoins);
            }
            if (armor) _audio.Play(SoundTag.DestroyingArmorBlock);
            if (!coins && multipliers) _audio.Play(SoundTag.CollectMultipliers);
        }

        
    }
}