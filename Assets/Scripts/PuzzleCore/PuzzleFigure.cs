using System;
using System.Collections.Generic;
using System.Linq;
using Temp;
using UnityEditor.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PuzzleCore
{
    public class PuzzleFigure : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private GameObject[] blocks;
        
        [SerializeField][Range(0f, 1f)] private float weight;

        #endregion


        #region Properties
        
        public Vector3 PositionCenter => gameObject.transform.position + _centerOffset;
        
        public Vector3[] BlocksRelativePositions => _blocksRelativePositions;
        
        public float Weight => weight;

        #endregion


        #region Actions

        public Action<Vector3> FigureMoved;

        #endregion


        #region Private Values

        private Vector3[] _blocksRelativePositions;
        
        private Vector3 _centerOffset;

        private Dictionary<int, PowerUp> _powerUps;

        #endregion


        #region Initializing

        private void Awake()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            CountCenterOffset();
            _blocksRelativePositions = GetRelativeBlocksPositions();
            ClearPowerUps();
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

        private void ClearPowerUps()
        {
            _powerUps = new Dictionary<int, PowerUp>(blocks.Length);
            for (var i = 0; i < blocks.Length; i++) _powerUps.Add(i, PowerUp.None);
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

        public void SetPowerUp(PowerUp powerUp)
        {
            var attempts = 0;
            int index;
            while (true)
            {
                index = Random.Range(0, blocks.Length);
                attempts++;
                if (_powerUps[index] == PowerUp.None) break;
                if (attempts > blocks.Length) throw new Exception("Trying to add more power ups, then blocks");
            }

            _powerUps[index] = powerUp;
        }

        public bool GetPowerUps(out List<(int blockIndex, PowerUp powerUp)> powerUps)
        {
            powerUps = new List<(int blockIndex, PowerUp powerUp)>(blocks.Length);
            powerUps.AddRange(
                from pair in _powerUps 
                where pair.Value != PowerUp.None 
                select (pair.Key, pair.Value));
            return powerUps.Count > 0;
        }
        
        #endregion
    }
}

