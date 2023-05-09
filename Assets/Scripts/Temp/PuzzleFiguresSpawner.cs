using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Temp
{
    public class PuzzleFiguresSpawner
    {
        #region Private Values

        private readonly PuzzleFigure[] _figurePrefabs;
        
        private readonly Transform[] _spawnPoints;

        private float[] _figureWeights;

        private float _allWeight;

        #endregion


        #region Constructor

        public PuzzleFiguresSpawner(PuzzleFigure[] figurePrefabs, Transform[] spawnPoints)
        {
            _figurePrefabs = figurePrefabs;
            _spawnPoints = spawnPoints;

            CountFiguresWeights();
        }

        private void CountFiguresWeights()
        {
            _allWeight = _figurePrefabs.Sum(puzzleFigure => puzzleFigure.Weight);
            _figureWeights = new float[_figurePrefabs.Length + 1];
            _figureWeights[0] = 0;
            for (var i = 1; i < _figurePrefabs.Length + 1; i++)
            {
                _figureWeights[i] = _figureWeights[i - 1] + _figurePrefabs[i - 1].Weight;
            }
        }

        #endregion


        #region Handling Puzzle Figures

        public PuzzleFigure[] SpawnFigures()
        {
            var activeFigures = new PuzzleFigure[_spawnPoints.Length];
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
                var figure = Object.Instantiate(_figurePrefabs[index]);
                figure.Initialize();
                figure.SetPositionByCenter(_spawnPoints[i].position);
                activeFigures[i] = figure;
            }
        
            return activeFigures;
        }

        public void RemoveFigure(PuzzleFigure figure)
        {
            Object.Destroy(figure.gameObject);
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

        // public PuzzleFigure[] SpawnFigures()
        // {
        //     var activeFigures = new List<PuzzleFigure>(_spawnPoints.Length);
        //
        //     foreach (var spawnPoint in _spawnPoints)
        //     {
        //         var puzzleFigure = SpawnFigure();
        //         while (true)
        //         {
        //             if (!activeFigures.Contains(puzzleFigure)) break;
        //             puzzleFigure = SpawnFigure();
        //         }
        //         var figure = Object.Instantiate(puzzleFigure);
        //         figure.Initialize();
        //         figure.SetPositionByCenter(spawnPoint.position);
        //         activeFigures.Add(figure);
        //     }
        //
        //     return activeFigures.ToArray();
        // }
    }
}

