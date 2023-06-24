﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.SharedData;
using SevenBoldPencil.EasyEvents;
using Temp.Components.Events;

namespace Temp.PostGameplaySystems
{
    public class DeletePuzzleFigureSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter =
            default;

        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectPool = default;
        
        private readonly EcsPoolInject<DraggableOverGridComponent> _draggableOverGridObjectPool = default;
        
        private readonly EcsPoolInject<AvailablePlacesComponent> _figurePlacePool = default;

        private readonly EcsPoolInject<FigurePowerUpComponent> _figurePowerUpsPool = default;
        
        
        private EventsBus _events;
        private readonly PowerUpsHandler _handler;
        
        public DeletePuzzleFigureSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }
        

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _removingFigureFilter.Value)
            {
                ref var puzzleFigure = ref _removingFigureFilter.Pools.Inc1.Get(entity);
                
                
                puzzleFigure.View.gameObject.SetActive(false);

                _removingFigureFilter.Pools.Inc1.Del(entity);
                _removingFigureFilter.Pools.Inc2.Del(entity);
                
                _draggableObjectPool.Value.Del(entity);
                _draggableOverGridObjectPool.Value.Del(entity);
                _figurePlacePool.Value.Del(entity);

                if (_figurePowerUpsPool.Value.Has(entity))
                {
                    ref var powerUpToFigure = ref _figurePowerUpsPool.Value.Get(entity);
                    _handler.ReturnPowerUp(powerUpToFigure.View);
                    _figurePowerUpsPool.Value.Del(entity);
                }
                

                if (!_events.HasEventSingleton<FindPlaceForFiguresEvent>())
                    _events.NewEventSingleton<FindPlaceForFiguresEvent>();
            }
        }
    }
}