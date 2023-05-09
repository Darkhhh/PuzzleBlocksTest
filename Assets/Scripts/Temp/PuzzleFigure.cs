using System;
using System.Linq;
using UnityEngine;

namespace Temp
{
    public class PuzzleFigure : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private int idTag;

        [SerializeField] private GameObject[] blocks;
        
        [SerializeField][Range(0f, 1f)] private float weight;

        #endregion


        #region Properties
        
        public Vector3 PositionCenter => gameObject.transform.position + _centerOffset;
        
        public Vector3[] BlocksRelativePositions => _blocksRelativePositions;
        
        public float Weight => weight;

        public int IdTag => idTag;

        #endregion


        #region Actions

        public Action<Vector3> FigureMoved;

        #endregion


        #region Private Values

        private Vector3[] _blocksRelativePositions;
        
        private Vector3 _centerOffset;

        #endregion


        #region Initializing

        private void Awake()
        {
            Initialize();
            // foreach (Transform child in transform)
            // {
            //     // child.transform.position
            // }
        }
        
        public void Initialize()
        {
            CountCenterOffset();
            _blocksRelativePositions = GetRelativeBlocksPositions();
        }
        
        /// <summary>
        /// Computing vector offset to the center from anchor block of figure.
        /// gameObject.transform.position + _centerOffset => Geometric center of the puzzle figure.
        /// </summary>
        private void CountCenterOffset()
        {
            if (blocks.Length < 1)
            {
                _centerOffset = Vector3.zero;
                return;
            }
            var b = blocks.ToList();
            b.Add(gameObject);

            var v = blocks[0].transform.position - gameObject.transform.position;
            (float Min, float Max) xAxis = (v.x, v.x);
            (float Min, float Max) yAxis = (v.y, v.y);
            
            foreach (var t in b.Select(block => block.transform.position - gameObject.transform.position))
            {
                if (t.x < xAxis.Min) xAxis = (t.x, xAxis.Max);
                if (t.x > xAxis.Min) xAxis = (xAxis.Min, t.x);
                
                if (t.y < yAxis.Min) yAxis = (t.y, yAxis.Max);
                if (t.y > yAxis.Min) yAxis = (yAxis.Min, t.y);
            }

            _centerOffset = new Vector3((xAxis.Max + xAxis.Min) / 2, (yAxis.Max + yAxis.Min) / 2);
        }
        
        private Vector3[] GetRelativeBlocksPositions()
        {
            var positions = new Vector3[blocks.Length];
            for (int i = 0; i < blocks.Length; i++)
            {
                positions[i] = blocks[i].transform.position - gameObject.transform.position;
            }

            return positions;
        }


        #endregion


        #region Public Methods

        public void SetNewPosition(Vector3 position)
        {
            transform.position = position;
            FigureMoved?.Invoke(position);
        }
        
        public Vector3[] GetSceneBlocksPositions()
        {
            var positions = new Vector3[blocks.Length];
            for (int i = 0; i < blocks.Length; i++)
            {
                positions[i] = blocks[i].transform.position;
            }

            return positions;
        }
        
        public void SetPositionByCenter(Vector3 position)
        {
            gameObject.transform.position = position - _centerOffset;
        }
        
        #endregion
    }
}

