using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Temp
{
    public class PuzzleFiguresObserver : MonoBehaviour
    {
        #region Private Values

        private Vector3 _offset;
        private List<PuzzleFigure> _activeFigures;
        private PuzzleFigure _capturedFigure;
        private Vector3 _initialPosition;
        private bool _dragging;

        private PuzzleGrid _puzzleGrid;
        private PuzzleFiguresSpawner _spawner;

        #endregion

        #region Serialize Fields

        #region Required For Observer
        [Header("Required For Observer")]
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private LayerMask movableLayers;
        [Space]
        #endregion


        #region Required For Grid
        [Header("Required For Puzzle Grid")]
        [SerializeField] private Cell[] gridCells;
        [SerializeField] private float magnetDistance;
        [Space]
        #endregion


        #region Required For Spawner
        [Header("Required For Puzzle Figures Spawner")]
        [SerializeField] private PuzzleFigure[] prefabs;
        [SerializeField] private Transform[] spawnPoints;
        
        #endregion

        #endregion
        

        private void Start()
        {
            _puzzleGrid = new PuzzleGrid(gridCells, magnetDistance);
            _spawner = new PuzzleFiguresSpawner(prefabs, spawnPoints);

            _activeFigures = _spawner.SpawnFigures().ToList();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
                
                var hit = Physics2D.Raycast(mousePosition, Vector2.zero, float.PositiveInfinity, 
                    movableLayers);
                
                if (hit)
                {
                    _dragging = true;
                    CaptureFigure(hit.transform);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (!_puzzleGrid.ReleaseFigure(out var clearedCellsNumber))
                {
                    _capturedFigure.SetPositionByCenter(_initialPosition);
                }
                else
                {
                    if (!_activeFigures.Remove(_capturedFigure)) throw new Exception("Couldn't delete item");
                    _spawner.RemoveFigure(_capturedFigure);
                    
                    var availablePlaces = _activeFigures.Count(figure => _puzzleGrid.FigureCanBePlaced(figure));
                    if (_activeFigures.Count > 0 && availablePlaces == 0) Debug.Log("Game Over");
                    if (_activeFigures.Count < 1) SpawnNewFigures();
                }
                _capturedFigure = null;
                _dragging = false;
                if (clearedCellsNumber > 0) Debug.Log($"Earned Points: {clearedCellsNumber}");
            }
    
            if (_dragging) MoveFigure();
        }
    
        private void CaptureFigure(Transform hit)
        {
            foreach (var activeFigure in _activeFigures)
            {
                if (activeFigure.transform != hit) continue;
                _capturedFigure = activeFigure;
                _initialPosition = _capturedFigure.PositionCenter;
                _offset = _capturedFigure.transform.position - sceneCamera.ScreenToWorldPoint(Input.mousePosition);
                break;
            }
            _puzzleGrid.SetCapturedFigure(_capturedFigure);
        }
    
        private void MoveFigure()
        {
            if (_capturedFigure is null) throw new Exception("Could not reach Puzzle Figure");
            _capturedFigure.SetNewPosition(sceneCamera.ScreenToWorldPoint(Input.mousePosition) + _offset);
        }

        private void SpawnNewFigures()
        {
            _activeFigures = _spawner.SpawnFigures().ToList();
            var availablePlaces = _activeFigures.Count(figure => _puzzleGrid.FigureCanBePlaced(figure));
            //Debug.Log($"Figures can be placed: {availablePlaces}");
            if (availablePlaces == 0) Debug.Log("Game Over");
        }
    }
}

