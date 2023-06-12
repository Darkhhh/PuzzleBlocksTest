using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;
using PuzzleCore.ECS.Views;
using UnityEngine;

namespace PuzzleCore.ECS.Systems.ManualPowerUpsHandling
{
    public class AssignManualPowerUpsSystem : IEcsInitSystem
    {
        private readonly EcsPoolInject<ManualPowerUp> _manualPowerUpComponents = default;
        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectComponents = default;
        private readonly EcsPoolInject<DraggableOverGridComponent> _draggableOverGridObjectComponents = default;
        
        private readonly Transform _storage;
        
        public AssignManualPowerUpsSystem(Transform storage)
        {
            _storage = storage;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            foreach (Transform child in _storage.transform)
            {
                var entity = world.NewEntity();
                ref var manualPowerUp = ref _manualPowerUpComponents.Value.Add(entity);
                manualPowerUp.View = child.gameObject.GetComponent<ManualPowerUpView>();
                manualPowerUp.View.Init();
                manualPowerUp.AvailableAmount = 1;
                
                ref var draggableObject = ref _draggableObjectComponents.Value.Add(entity);
                draggableObject.View = manualPowerUp.View;

                ref var draggableOverGridObject = ref _draggableOverGridObjectComponents.Value.Add(entity);
                draggableOverGridObject.MustBeFullOnGrid = false;
                draggableOverGridObject.CheckOnCellAvailability = false;
                draggableOverGridObject.PlaceableObject = manualPowerUp.View;
            }
        }
    }
}