using System;
using Leopotam.EcsLite;
using Source.Code.Components;
using Source.Code.Views.Cell;

namespace Source.Code.Common.Utils
{
    public static class CellEntity
    {
        public static void SetState(EcsPackedEntityWithWorld packedEntity, CellStateEnum state, CellStateEnum previousState = CellStateEnum.Default)
        {
            if (!packedEntity.Unpack(out var world, out var entity))
                throw new Exception("Incorrect input data");
            
            world.GetPool<DefaultCellStateComponent>().Del(entity);
            world.GetPool<HighlightedCellStateComponent>().Del(entity);
            world.GetPool<OccupiedCellStateComponent>().Del(entity);
            world.GetPool<DestroyableCellStateComponent>().Del(entity);
            world.GetPool<SuggestedCellStateComponent>().Del(entity);
            world.GetPool<TargetedCellStateComponent>().Del(entity);

            if (!world.GetPool<ChangeCellStateComponent>().Has(entity))
            {
                ref var data = ref world.GetPool<ChangeCellStateComponent>().Add(entity);
                data.State = state;
            }
            else
            {
                ref var data = ref world.GetPool<ChangeCellStateComponent>().Get(entity);
                data.State = state;
            }
            

            switch (state)
            {
                case CellStateEnum.Default:
                    world.GetPool<DefaultCellStateComponent>().Add(entity);
                    break;
                case CellStateEnum.Suggested:
                    world.GetPool<SuggestedCellStateComponent>().Add(entity);
                    break;
                case CellStateEnum.Highlighted:
                    ref var data = ref world.GetPool<HighlightedCellStateComponent>().Add(entity);
                    data.PreviousState = previousState;
                    break;
                case CellStateEnum.Occupied:
                    world.GetPool<OccupiedCellStateComponent>().Add(entity);
                    break;
                case CellStateEnum.Destroyable:
                    ref var component = ref world.GetPool<DestroyableCellStateComponent>().Add(entity);
                    component.PreviousState = previousState;
                    break;
                case CellStateEnum.Targeted:
                    world.GetPool<TargetedCellStateComponent>().Add(entity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}