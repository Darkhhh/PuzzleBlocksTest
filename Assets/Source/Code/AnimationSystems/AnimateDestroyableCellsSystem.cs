using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Source.Code.Common.Audio;
using Source.Code.Components;
using Source.Code.Mono;
using Source.Code.SharedData;
using UnityEngine;

namespace Source.Code.AnimationSystems
{
    public class AnimateDestroyableCellsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PuzzleFigureComponent, ShouldBeRemovedFigureComponent>> _removingFigureFilter = default;
        private readonly EcsFilterInject<Inc<CellComponent, DestroyableCellStateComponent>> _destroyableCellsFilter = default;
        
        
        private readonly DissolveBlocksHandler _handler;
        private readonly Vector3 _targetPosition;
        private readonly Vector3 _offset = new (0, -0.05f);
        private AudioManager _audio;

        public AnimateDestroyableCellsSystem(DissolveBlocksHandler handler, Vector3 targetPosition)
        {
            _handler = handler;
            _targetPosition = targetPosition;
        }

        public void Init(IEcsSystems systems)
        {
            _handler.Init(48);
            _audio = systems.GetShared<SystemsSharedData>().SceneData.audioManager;
        }

        public void Run(IEcsSystems systems)
        {
            if (_removingFigureFilter.Value.GetEntitiesCount() == 0) return;

            var position = _targetPosition;
            
            foreach (var entity in _destroyableCellsFilter.Value)
            {
                ref var cell = ref _destroyableCellsFilter.Pools.Inc1.Get(entity);

                var destroyingCell = _handler.Get().Prepare(cell.Position);
                
                destroyingCell.MoveTowards(new Vector3(Random.Range(-6f, 6f), Random.Range(-6f, 6f)), () =>
                {
                    _audio.Play(SoundTag.DissolvingBlocks);
                    destroyingCell.PlayEffect(() =>
                    {
                        _handler.Return(destroyingCell);
                    });
                });
                position += _offset;
            }
            
            //if (_destroyableCellsFilter.Value.GetEntitiesCount() > 0) _audio.PlayWithDelay(SoundTag.DissolvingBlocks, 0.5f);
        }
    }
}