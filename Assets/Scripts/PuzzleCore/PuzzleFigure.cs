using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleFigure : DraggableGameObject
{
    [SerializeField] private GameObject[] blocks;

    private Vector3 _centerOffset;
    private void Awake()
    {
        CountCenterOffset();
    }

    public void Initialize()
    {
        CountCenterOffset();
        // Cache => GetRelativeBlocksPositions
    }

    public Vector3[] GetRelativeBlocksPositions()
    {
        var positions = new Vector3[blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
        {
            positions[i] = blocks[i].transform.position - gameObject.transform.position;
        }

        return positions;
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

    /// <summary>
    /// Computing vector offset to the center from anchor block of figure.
    /// gameObject.transform.position + _centerOffset => Geometric center of the puzzle figure.
    /// </summary>
    public void CountCenterOffset()
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
}
