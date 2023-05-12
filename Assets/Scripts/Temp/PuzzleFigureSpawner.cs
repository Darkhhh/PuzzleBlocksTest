using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Temp
{
    public class PuzzleFigureSpawner
    {
        #region Private Values

        private readonly GameObject[] _figurePrefabs;
        
        private readonly Transform[] _spawnPoints;

        private float[] _figureWeights;

        private float _allWeight;

        #endregion


        #region Constructor

        public PuzzleFigureSpawner(GameObject[] figurePrefabs, Transform[] spawnPoints, float[] weights)
        {
            _figurePrefabs = figurePrefabs;
            _spawnPoints = spawnPoints;

            CountFiguresWeights(weights);
        }

        private void CountFiguresWeights(float[] weights)
        {
            _allWeight = weights.Sum();
            _figureWeights = new float[_figurePrefabs.Length + 1];
            _figureWeights[0] = 0;
            for (var i = 1; i < _figurePrefabs.Length + 1; i++)
            {
                _figureWeights[i] = _figureWeights[i - 1] + weights[i - 1];
            }
        }

        #endregion


        #region Handling Puzzle Figures

        public Figure[] SpawnFigures()
        {
            var activeFigures = new Figure[_spawnPoints.Length];
            var indices = new List<int>(_spawnPoints.Length);
        
            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                var index = GetNewSpawnFigureIndex();
                while (true)
                {
                    if (!indices.Contains(index)) break;
                    index = GetNewSpawnFigureIndex();
                }
                indices.Add(index);
                var gameObject = Object.Instantiate(_figurePrefabs[index]);
                var figure = new Figure(gameObject, 0.5f);
                figure.SetNewPosition(_spawnPoints[i].position, true, false);
                activeFigures[i] = figure;
                SetPowerUps(figure);
            }
        
            return activeFigures;
        }

        public void RemoveFigure(Figure figure)
        {
            Object.Destroy(figure.View);
        }

        #endregion
        
        private int GetNewSpawnFigureIndex()
        {
            var weight = Random.Range(float.Epsilon, _allWeight - float.Epsilon);

            for (var i = 1; i < _figureWeights.Length; i++)
            {
                if (weight > _figureWeights[i - 1] && weight <= _figureWeights[i])
                {
                    return i - 1;
                }
            }

            throw new Exception("Incorrect weight array or weight generation");
        }

        private void SetPowerUps(Figure figure)
        {
            
        }
    }
}