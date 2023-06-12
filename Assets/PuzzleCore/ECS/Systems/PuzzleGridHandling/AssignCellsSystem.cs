using System.Linq;
using Leopotam.EcsLite;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Views;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.PuzzleGridHandling
{
    public class AssignCellsSystem : IEcsInitSystem
    {
        private readonly Transform _grid;
        private CellView[] _cells;
        
        public AssignCellsSystem(Transform cellsParentObject)
        {
            _grid = cellsParentObject;
        }
        
        public void Init(IEcsSystems systems)
        {
            _cells = (from Transform child 
                    in _grid.transform 
                select child.GetComponent<CellView>()).ToArray();
            
            var world = systems.GetWorld();
            var cells = world.GetPool<CellComponent>();
            foreach (var cellView in _cells)
            {
                var entity = world.NewEntity();
                ref var cell = ref cells.Add(entity);
                cell.View = cellView;
                cell.Available = true;
                cell.View.Init();
            }
        }
    }
}