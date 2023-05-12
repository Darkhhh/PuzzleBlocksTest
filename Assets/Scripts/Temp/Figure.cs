using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Temp
{
    public class Figure
    {
        #region Private Values

        private readonly List<Vector3> _blockPositions = new();
        
        private readonly Vector3 _centerOffset;

        #endregion


        #region Properties

        public GameObject View { get; set; }
        
        public float Weight { get; set; }
        
        public Vector3[] BlocksRelativePositions { get; }
        
        public Vector3 PositionCenter => View.transform.position + _centerOffset;

        #endregion


        #region Actions

        public Action<Vector3> FigureMoved;

        #endregion


        #region Initializing

        public Figure(GameObject view, float weight)
        {
            foreach (Transform child in view.transform)
            {
                _blockPositions.Add(child.position);
            }

            View = view;
            Weight = weight;
            _centerOffset = CountCenterOffset();
            BlocksRelativePositions = GetRelativeBlocksPositions();
        }
        
        private Vector3 CountCenterOffset()
        {
            if (_blockPositions.Count < 1) return Vector3.zero;
            
            var anchorPosition = View.transform.position;

            (float Min, float Max) xAxis = (anchorPosition.x, anchorPosition.x);
            (float Min, float Max) yAxis = (anchorPosition.y, anchorPosition.y);
            
            foreach (var t in _blockPositions.Select(block => block - anchorPosition))
            {
                if (t.x < xAxis.Min) xAxis = (t.x, xAxis.Max);
                if (t.x > xAxis.Min) xAxis = (xAxis.Min, t.x);
                
                if (t.y < yAxis.Min) yAxis = (t.y, yAxis.Max);
                if (t.y > yAxis.Min) yAxis = (yAxis.Min, t.y);
            }

            return new Vector3((xAxis.Max + xAxis.Min) / 2, (yAxis.Max + yAxis.Min) / 2);
        }
        
        private Vector3[] GetRelativeBlocksPositions()
        {
            var positions = new Vector3[_blockPositions.Count];
            for (int i = 0; i < _blockPositions.Count; i++)
            {
                positions[i] = _blockPositions[i] - View.transform.position;
            }

            return positions;
        }

        #endregion


        #region Public Methods

        public void SetNewPosition(Vector3 position, bool byCenter = false,bool withActionInvocation = true)
        {
            if (byCenter) View.transform.position = position - _centerOffset;
            else View.transform.position = position;
            
            if (withActionInvocation) FigureMoved?.Invoke(position);
        }
        
        public Vector3[] GetSceneBlocksPositions()
        {
            return _blockPositions.ToArray();
        }

        #endregion
    }
}