using System.Linq;
using Leopotam.EcsLite;
using Temp.Components;
using Temp.Mono;
using Temp.Views.Cell;
using UnityEngine;

namespace Temp.InitializeSystems
{
    public class PrepareCellsSystem : IEcsInitSystem
    {
        private readonly Transform _grid;
        private readonly PuzzleFiguresHandler _handler;
        private readonly GameObject _target;

        public PrepareCellsSystem(Transform grid, PuzzleFiguresHandler handler, GameObject target)
        {
            _grid = grid;
            _handler = handler;
            _target = target;
        }

        public void Init(IEcsSystems systems)
        {
            var cells = (from Transform child 
                    in _grid.transform 
                select child.GetComponent<CellView>()).ToArray();
            
            var world = systems.GetWorld();
            var cellsPool = world.GetPool<CellComponent>();
            foreach (var cellView in cells)
            {
                var entity = world.NewEntity();
                ref var cell = ref cellsPool.Add(entity);
                cell.View = cellView;
                world.GetPool<DefaultCellStateComponent>().Add(entity);
                cell.View.Init(_handler.GetPuzzleBlock(), Object.Instantiate(_target));
            }
        }
    }
}