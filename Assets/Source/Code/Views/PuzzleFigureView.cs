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

        [SerializeField] private Vector3Int[] blocks;

        #endregion


        #region Properties

        public float SpawnScale => spawnScale;
        
        public float Weight => weight;


        private Vector3 PositionCenter => gameObject.transform.position + offset;

        #endregion


        #region Private Values

        private PuzzleFigureBlockScript[] _figureBlocks;

        #endregion


        
        public Vector3Int GetAnchorBlockPosition() => gameObject.transform.position.GetIntVector();
        
        public void SetNewPosition(Vector3 position) => transform.position = position;
        
        public Vector3 GetObjectPosition() => PositionCenter;
        
        public Transform GetTransform() => transform;

        public void SetPositionByCenter(Vector3 position) => gameObject.transform.position = position - offset * SpawnScale;

        public Vector3Int[] GetRelativeBlockPositions() => blocks;
        

        public void Init()
        {
            foreach (Transform child in transform) child.gameObject.SetActive(false);
        }



        public void ExecuteCoroutine(IEnumerator routine) => StartCoroutine(routine);
        
        
        public void InjectBlocks(PuzzleFigureBlockScript[] figureBlocks) => _figureBlocks = figureBlocks;


        public void SetToDefault()
        {
            foreach (var item in _figureBlocks) item.SetToDefault();
        }
        
        public void SetToUntouchable()
        {
            foreach (var item in _figureBlocks) item.SetToGrayScale();
        }

        public void ReturnBack(Vector3 initialPosition, float timeInSeconds)
        {
            StartCoroutine(MovingCoroutines.MoveTowardsOverTime(
                transform,
                initialPosition - offset,
                timeInSeconds));
        }
    }
}
