using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Components;

namespace Source.Code.PostGameplaySystems
{
    public class LightCellsSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ChangeCellStateComponent, CellComponent>> _changeCellsFilter = default;
        
        
        #if UNITY_EDITOR
        
        private readonly EcsFilterInject<Inc<DefaultCellStateComponent, OccupiedCellStateComponent>> _1Filter = default;
        private readonly EcsFilterInject<Inc<DefaultCellStateComponent, SuggestedCellStateComponent>> _2Filter = default;
        private readonly EcsFilterInject<Inc<DefaultCellStateComponent, DestroyableCellStateComponent>> _3Filter = default;
        private readonly EcsFilterInject<Inc<DefaultCellStateComponent, TargetedCellStateComponent>> _4Filter = default;
        private readonly EcsFilterInject<Inc<DefaultCellStateComponent, HighlightedCellStateComponent>> _5Filter = default;
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent, OccupiedCellStateComponent>> _6Filter = default;
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent, DestroyableCellStateComponent>> _7Filter = default;
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent, SuggestedCellStateComponent>> _8Filter = default;
        private readonly EcsFilterInject<Inc<HighlightedCellStateComponent, TargetedCellStateComponent>> _9Filter = default;
        private readonly EcsFilterInject<Inc<SuggestedCellStateComponent, DestroyableCellStateComponent>> _10Filter = default;
        private readonly EcsFilterInject<Inc<SuggestedCellStateComponent, TargetedCellStateComponent>> _11Filter = default;
        private readonly EcsFilterInject<Inc<SuggestedCellStateComponent, OccupiedCellStateComponent>> _12Filter = default;
        private readonly EcsFilterInject<Inc<DestroyableCellStateComponent, OccupiedCellStateComponent>> _13Filter = default;
        private readonly EcsFilterInject<Inc<DestroyableCellStateComponent, TargetedCellStateComponent>> _14Filter = default;
        private readonly EcsFilterInject<Inc<OccupiedCellStateComponent, TargetedCellStateComponent>> _15Filter = default;
        
        #endif

        public void Run(IEcsSystems systems)
        {
            #if UNITY_EDITOR

            if (_1Filter.Value.GetEntitiesCount() > 0 || _2Filter.Value.GetEntitiesCount() > 0 ||
                _3Filter.Value.GetEntitiesCount() > 0 ||
                _4Filter.Value.GetEntitiesCount() > 0 || _5Filter.Value.GetEntitiesCount() > 0 ||
                _6Filter.Value.GetEntitiesCount() > 0 ||
                _7Filter.Value.GetEntitiesCount() > 0 || _8Filter.Value.GetEntitiesCount() > 0 ||
                _9Filter.Value.GetEntitiesCount() > 0 ||
                _10Filter.Value.GetEntitiesCount() > 0 || _12Filter.Value.GetEntitiesCount() > 0 ||
                _13Filter.Value.GetEntitiesCount() > 0 || _11Filter.Value.GetEntitiesCount() > 0 ||
                _14Filter.Value.GetEntitiesCount() > 0 ||
                _15Filter.Value.GetEntitiesCount() > 0) throw new Exception("Some cells have two states in one time");
            
            #endif
            
            
            foreach (var entity in _changeCellsFilter.Value)
            {
                ref var cell = ref _changeCellsFilter.Pools.Inc2.Get(entity);
                ref var data = ref _changeCellsFilter.Pools.Inc1.Get(entity);
                
                cell.View.ChangeState(data.State);
                _changeCellsFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}