using Leopotam.EcsLite;
using PuzzleCore.ECS.Components;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.PuzzleGridHandling
{
    public class InjectTargetBlockSystem : IEcsInitSystem
    {
        private readonly GameObject _target;
        public InjectTargetBlockSystem(GameObject target)
        {
            _target = target;
        }
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var cellsEntities = world.Filter<CellComponent>().End();
            var cells = world.GetPool<CellComponent>();

            foreach (var entity in cellsEntities)
            {
                ref var cell = ref cells.Get(entity);
                var target = Object.Instantiate(_target);
                cell.View.InjectTarget(target);
            }
        }
    }
}