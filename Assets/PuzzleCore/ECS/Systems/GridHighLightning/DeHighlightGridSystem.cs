using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Components.Events;
using PuzzleCore.ECS.SharedData;
using PuzzleCore.ECS.Views;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.GridHighLightning
{
    public class DeHighlightGridSystem : IEcsInitSystem, IEcsRunSystem
    {
        #region ECS Filters
        
        private readonly EcsFilterInject<Inc<CellComponent>> _cellsFilter = default;

        #endregion


        #region ECS Pools

        private readonly EcsPoolInject<CellComponent> _cellComponents = default;

        #endregion
        

        private EventsBus _events;
        public void Init(IEcsSystems systems)
        {
            _events = systems.GetShared<SystemsSharedData>().EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            if (!_events.HasEventSingleton<DeHighlightGridEvent>()) return;
            
            foreach (var entity in _cellsFilter.Value)
            {
                ref var c = ref _cellComponents.Value.Get(entity);
                //TODO Experiments
                c.View.ChangeState(CellView.CellState.Default);
                // if (c.View.Suggested) continue;
                // c.View.SetAvailable(c.Available);
            }
        }
    }
}