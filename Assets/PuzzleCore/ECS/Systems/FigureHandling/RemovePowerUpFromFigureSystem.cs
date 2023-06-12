using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PuzzleCore.ECS.Components;

namespace PuzzleCore.ECS.Systems.FigureHandling
{
    public class RemovePowerUpFromFigureSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent, FigurePowerUpComponent>>
            _powerUpFiguresFilter = default;

        private readonly EcsPoolInject<FigurePowerUpComponent> _powerUpToFigureComponents = default;


        private readonly PowerUpsHandler _handler;
        
        public RemovePowerUpFromFigureSystem(PowerUpsHandler handler)
        {
            _handler = handler;
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_powerUpFiguresFilter.Value.GetEntitiesCount() == 0) return;

            foreach (var entity in _powerUpFiguresFilter.Value)
            {
                ref var powerUpToFigure = ref _powerUpToFigureComponents.Value.Get(entity);

                _handler.ReturnPowerUp(powerUpToFigure.View);
                
                _powerUpToFigureComponents.Value.Del(entity);
            }
        }
    }
}