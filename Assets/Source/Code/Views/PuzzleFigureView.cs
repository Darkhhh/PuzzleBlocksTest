using System.Linq;
using Source.Code.Utils;
using UnityEngine;

namespace Source.Code.Views
{
    public class PuzzleFigureView : MonoBehaviour, IDraggableObject, IGridPlaceableObject
    {
        #region Serialize Fields

        [SerializeField][Range(0.05f, 0.95f)] private float weight;

        #endregion


        #region Properties

        public float Weight => weight;
        
        public Vector3[] BlocksRelativePositions { get; private set; }
        
        private Vector3Int[] RelativeBlocksPositions { get; set; }

        public Vector3[] BlocksScenePosition { get; private set; }

        private Vector3 PositionCenter => gameObject.transform.position + Offset;

        public Vector3 Offset { get; private set; }

        #endregion


        #region Private Values

        private GameObject[] _blocks;

        #endregion


        
        public Vector3Int GetAnchorBlockPosition() => gameObject.transform.position.GetIntVector();
        
        public void SetNewPosition(Vector3 position) => transform.position = position;
        
        public Vector3 GetObjectPosition() => PositionCenter;
        
        public Transform GetTransform() => transform;

        public void SetPositionByCenter(Vector3 position) => gameObject.transform.position = position - Offset;

        public Vector3Int[] GetRelativeBlockPositions() => RelativeBlocksPositions;
        

        public void Init()
        {
            var childCount = transform.childCount;
            _blocks = new GameObject[childCount];
            BlocksRelativePositions = new Vector3[childCount];
            BlocksScenePosition = new Vector3[childCount];
            RelativeBlocksPositions = new Vector3Int[childCount];
            var index = 0;
            foreach (Transform child in transform)
            {
                var childPosition = child.position;
                _blocks[index] = child.gameObject;
                BlocksScenePosition[index] = childPosition;
                BlocksRelativePositions[index] = childPosition - gameObject.transform.position;
                RelativeBlocksPositions[index] = BlocksRelativePositions[index].GetIntVector();
                index++;
            }
            
            CountCenterOffset();
        }
        
        private void CountCenterOffset()
        {
            if (_blocks.Length < 1)
            {
                Offset = Vector3.zero;
                return;
            }
            var b = _blocks.ToList();
            b.Add(gameObject);

            var v = _blocks[0].transform.position - gameObject.transform.position;
            (float Min, float Max) xAxis = (v.x, v.x);
            (float Min, float Max) yAxis = (v.y, v.y);
            
            foreach (var t in b.Select(block => block.transform.position - gameObject.transform.position))
            {
                if (t.x < xAxis.Min) xAxis = (t.x, xAxis.Max);
                if (t.x > xAxis.Min) xAxis = (xAxis.Min, t.x);
                
                if (t.y < yAxis.Min) yAxis = (t.y, yAxis.Max);
                if (t.y > yAxis.Min) yAxis = (yAxis.Min, t.y);
            }

            Offset = new Vector3((xAxis.Max + xAxis.Min) / 2, (yAxis.Max + yAxis.Min) / 2);
        }

        
    }
}
