using System.Collections.Generic;
using Source.Code.Common.Effects;
using UnityEngine;

namespace Source.Code.Mono
{
    public class DestroyingArmorBlocksHandler : MonoBehaviour
    {
        [SerializeField] private DestroyableArmorBlockHandler prefab;

        private readonly Queue<DestroyableArmorBlockHandler> _blocks = new ();

        public void Init(int capacity)
        {
            prefab.gameObject.SetActive(false);
            for (var i = 0; i < capacity; i++)
            {
                _blocks.Enqueue(Instantiate(prefab, transform).Init());
            }
        }
        
        public void ActivateEffect(Vector2 position)
        {
            var block = Get();
            block.SetPosition(position);
            block.StartDestroying(() =>
            {
                Return(block);
            });
        }
        
        
        private DestroyableArmorBlockHandler Get()
        {
            if (_blocks.TryDequeue(out var block))
            {
                block.gameObject.SetActive(true);
                return block;
            }

            var b = Instantiate(prefab, transform).Init();
            b.gameObject.SetActive(true);
            return b;
        }
        
        private void Return(DestroyableArmorBlockHandler block)
        {
            block.gameObject.SetActive(false);
            block.ResetPositions();
            _blocks.Enqueue(block);
        }
    }
}