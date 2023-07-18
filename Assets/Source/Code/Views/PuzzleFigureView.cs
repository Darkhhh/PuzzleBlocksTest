using System.Collections;
using System.Linq;
using Source.Code.Common.Animations;
using Source.Code.Common.Effects;
using Source.Code.Common.Utils;
using UnityEngine;

namespace Source.Code.Views
{
    public class PuzzleFigureView : MonoBehaviour, IDraggableObject, IGridPlaceableObject
    {
        #region Serialize Fields

        [SerializeField] [Range(0.1f, 1f)] private float spawnScale = 0.6f;
        
        [SerializeField][Range(0.05f, 0.95f)] private float weight;

        [SerializeField] private Vector3 offset;

        #endregion


        #region Properties

        public float SpawnScale => spawnScale;
        
        public float Weight => weight;
        
        public Vector3[] BlocksRelativePositions { get; private set; }
        
        private Vector3Int[] RelativeBlocksPositions { get; set; }

        public Vector3[] BlocksScenePosition { get; private set; }

        private Vector3 PositionCenter => gameObject.transform.position + Offset;

        //public Vector3 Offset { get; private set; }
        public Vector3 Offset => offset;

        #endregion


        #region Private Values

        //private GameObject[] _blocks;
        private Vector3[] _blocksPositions;
        private PuzzleFigureBlockScript[] _figureBlocks;

        #endregion


        
        public Vector3Int GetAnchorBlockPosition() => gameObject.transform.position.GetIntVector();
        
        public void SetNewPosition(Vector3 position) => transform.position = position;
        
        public Vector3 GetObjectPosition() => PositionCenter;
        
        public Transform GetTransform() => transform;

        //public void SetPositionByCenter(Vector3 position) => gameObject.transform.position = position - Offset * 0.6f;
        public void SetPositionByCenter(Vector3 position) => gameObject.transform.position = position - Offset * SpawnScale;

        public Vector3Int[] GetRelativeBlockPositions() => RelativeBlocksPositions;
        

        public void Init()
        {
            var childCount = transform.childCount;
            //_blocks = new GameObject[childCount];
            _blocksPositions = new Vector3[childCount];
            BlocksRelativePositions = new Vector3[childCount];
            BlocksScenePosition = new Vector3[childCount];
            RelativeBlocksPositions = new Vector3Int[childCount];
            var index = 0;
            foreach (Transform child in transform)
            {
                var childPosition = child.position;
                //_blocks[index] = child.gameObject;
                _blocksPositions[index] = child.position;
                child.gameObject.SetActive(false);
                BlocksScenePosition[index] = childPosition;
                BlocksRelativePositions[index] = childPosition - gameObject.transform.position;
                RelativeBlocksPositions[index] = BlocksRelativePositions[index].GetIntVector();
                index++;
            }
        }



        public void ExecuteCoroutine(IEnumerator routine) => StartCoroutine(routine);


        public void SetOffset(Vector3 vector)
        {
            offset = vector;
        }
        
        public void InjectBlocks(PuzzleFigureBlockScript[] blocks) => _figureBlocks = blocks;


        public void SetToDefault()
        {
            foreach (var item in _figureBlocks) item.SetToDefault();
        }
        
        public void SetToUntouchable()
        {
            foreach (var item in _figureBlocks) item.SetToGrayScale();
        }

        public void ReturnBack(Vector3 initialPosition, float speed)
        {
            StartCoroutine(MovingCoroutines.MoveTowards(transform, initialPosition - Offset, speed));
        }
    }
}
