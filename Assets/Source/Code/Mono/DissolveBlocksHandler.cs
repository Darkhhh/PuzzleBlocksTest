using System.Collections.Generic;
using Source.Code.Common.Effects;
using UnityEngine;

namespace Source.Code.Mono
{
    public class DissolveBlocksHandler : MonoBehaviour
    {
        [SerializeField] private Transform storage;
        [SerializeField] private GameObject blockPrefab;

        private readonly Queue<DissolvingPuzzleBlockScript> _blocks = new ();

        public void Init(int blocksAmount)
        {
            for (var i = 0; i < blocksAmount; i++)
            {
                var block = Instantiate(blockPrefab, storage).GetComponent<DissolvingPuzzleBlockScript>();
                block.Init();
                block.gameObject.SetActive(false);
                _blocks.Enqueue(block);
            }
        }

        public DissolvingPuzzleBlockScript Get()
        {
            if (_blocks.TryDequeue(out var block))
            {
                block.gameObject.SetActive(true);
                return block;
            }

            var b = Instantiate(blockPrefab, storage).GetComponent<DissolvingPuzzleBlockScript>();
            b.Init();
            b.gameObject.SetActive(true);
            return b;
        }

        public void Return(DissolvingPuzzleBlockScript block)
        {
            block.gameObject.SetActive(false);
            _blocks.Enqueue(block);
        }
    }
}