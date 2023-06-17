using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.SharedData;
using PuzzleCore.ECS.Views;
using SevenBoldPencil.EasyEvents;
using UI.InGame;

namespace PuzzleCore.ECS.Systems.UI.InGame
{
    public class HandleRestartButtonSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;
        
        #region ECS Pools

        private readonly EcsPoolInject<CellComponent> _cellComponents = default;
        
        private readonly EcsPoolInject<CellOrderedForPlacementComponent> _orderedCellComponents = default;
        
        private readonly EcsPoolInject<ShouldBeClearedCellComponent> _clearingCellsComponents = default;
        
        private readonly EcsPoolInject<CellPowerUpComponent> _cellPowerUpComponents = default;

        #endregion
        
        private readonly PowerUpsHandler _powerUpsHandler;
        private readonly InGameUserInterfaceHandler _handler;
        private EventsBus _events;

        public HandleRestartButtonSystem(InGameUserInterfaceHandler handler, PowerUpsHandler powerUpsHandler)
        {
            _handler = handler;
            _powerUpsHandler = powerUpsHandler;
        }

        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            if (!_handler.RestartButtonClicked) return;
            ClearAllCells();
            systems.GetShared<SystemsSharedData>().GameData.CurrentScore = 0;
            _handler.SetScore(0);
            _handler.ResetClickProperty();
        }

        private void ClearAllCells()
        {
            foreach (var entity in _cellsFilter.Value)
            {
                ref var c = ref _cellComponents.Value.Get(entity);
                //TODO Experimental
                //c.View.ChangeState(CellState.Default);
                //c.View.SetSimple();
                //c.Available = true;
                _orderedCellComponents.Value.Del(entity);
                if (_clearingCellsComponents.Value.Has(entity)) _clearingCellsComponents.Value.Del(entity);

                if (_cellPowerUpComponents.Value.Has(entity))
                {
                    ref var cellPowerUp = ref _cellPowerUpComponents.Value.Get(entity);
                    _powerUpsHandler.ReturnPowerUp(cellPowerUp.View);
                    _cellPowerUpComponents.Value.Del(entity);
                }
            }
        }
    }
}