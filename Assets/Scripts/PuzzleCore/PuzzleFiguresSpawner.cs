using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuzzleFiguresSpawner : MonoBehaviour
{
    [SerializeField] private PuzzleFigure[] figurePrefabs;
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var figure = Instantiate(figurePrefabs[Random.Range(0, figurePrefabs.Length)]);
            figure.CountCenterOffset();
            figure.SetPositionByCenter(spawnPoint.position);
        }
    }
}
