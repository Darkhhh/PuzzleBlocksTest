using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;
using Source.Code.Common.Utils;
using Source.Code.Components;
using Source.Code.Components.Events;
using Source.Code.SharedData;
using Source.Code.Views.Cell;

namespace Source.Code.PostGameplaySystems
{
    public class RestartGameSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent>> _figuresFilter = default;

        private readonly EcsPoolInject<ShouldBeRemovedFigureComponent> _removingFigures = default;
        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpsPool = default;
        private readonly EcsPoolInject<RemovePowerUpComponent> _clearCellPowerUpPool = default;


        private EventsBus _events;
        private EcsWorld _world;
        private InGameData _gameData;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
            _gameData = systems.GetShared<SystemsSharedData>().GameData;
        }

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<RestartGameEvent>()) return;
            _events.DestroyEventSingleton<RestartGameEvent>();

            foreach (var figureEntity in _figuresFilter.Value)
            {
                _removingFigures.Value.Add(figureEntity);
            }

            foreach (var cellEntity in _cellsFilter.Value)
            {
                CellEntity.SetState(_world.PackEntityWithWorld(cellEntity), CellStateEnum.Default);

                if (_cellPowerUpsPool.Value.Has(cellEntity))
                {
                    _clearCellPowerUpPool.Value.Add(cellEntity);
                }
            }

            _gameData.CurrentScore = 0;

            if (!_events.HasEventSingleton<UpdateInGameUIEvent>())
            {
                ref var updateData = ref _events.NewEventSingleton<UpdateInGameUIEvent>();

                updateData.NewScore = _gameData.CurrentScore;
                updateData.NewCoins = _gameData.CoinsAmount;
            }
            
            _events.DestroyEventSingleton<GameOverEvent>();
        }
    }
}