using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Components;
using Source.Code.Views;
using Source.Code.Views.ManualPowerUp;
using UnityEngine;

namespace Source.Code.InitializeSystems
{
    public class PrepareManualPowerUpsSystem : IEcsInitSystem
    {
        private readonly EcsPoolInject<ManualPowerUp> _manualPowerUpComponents = default;
        private readonly EcsPoolInject<DraggableObjectComponent> _draggableObjectComponents = default;
        private readonly EcsPoolInject<DraggableOverGridComponent> _draggableOverGridObjectComponents = default;
        
        private readonly Transform _storage;
        
        public PrepareManualPowerUpsSystem(Transform storage)
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
                manualPowerUp.View.SetAmountText(manualPowerUp.AvailableAmount);
                manualPowerUp.View.SetActiveCanvas(false);
                
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