using System.Linq;
using Leopotam.EcsLite;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Systems.Experimental.CellHandling;
using PuzzleCore.ECS.Views;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.PuzzleGridHandling
{
    public class AssignCellsSystem : IEcsInitSystem
    {
        private readonly Transform _grid;

        
        //private CellView[] _cells;
        private Cell[] _cells;      
        private readonly GameObject _puzzleBlock, _target;
        
        public AssignCellsSystem(Transform cellsParentObject)
        {
            _grid = cellsParentObject;
        }
        public AssignCellsSystem(Transform grid, GameObject puzzleBlock, GameObject target)
        {
            _grid = grid;
            _puzzleBlock = puzzleBlock;
            _target = target;
        }
        
        public void Init(IEcsSystems systems)
        {
            // _cells = (from Transform child 
            //         in _grid.transform 
            //     select child.GetComponent<CellView>()).ToArray();
            
            _cells = (from Transform child 
                    in _grid.transform 
                select child.GetComponent<Cell>()).ToArray();
            
            var world = systems.GetWorld();
            var cells = world.GetPool<CellComponent>();
            foreach (var cellView in _cells)
            {
                var entity = world.NewEntity();
                ref var cell = ref cells.Add(entity);
                cell.View = cellView;
                world.GetPool<DefaultCellStateComponent>().Add(entity);
                //cell.Available = true;
                //cell.View.Init();
            }
        }
    }
}