﻿using System.Collections.Generic;
using System.Linq;
using Source.Code.Common.Utils;
using Source.Code.Views;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PuzzleFigureView))] [CanEditMultipleObjects]
    public class PuzzleFigureViewEditor : UnityEditor.Editor
    {
        private const string PivotObjectName = "Pivot";

        private SerializedProperty _offsetProperty, _blocksProperty;

        private void OnEnable()
        {
            _offsetProperty = serializedObject.FindProperty("offset");
            _blocksProperty = serializedObject.FindProperty("blocks");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(20);
            
            var figure = (PuzzleFigureView)target;
            
            GUILayout.Label("Blocks Assignment");
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Find Child Blocks"))
                {
                    _blocksProperty.ClearArray();
                    var children = figure.transform.Cast<Transform>().ToList();
                    children.Reverse();
                    foreach (var child in children)
                    {
                        _blocksProperty.InsertArrayElementAtIndex(0);
                        var e = _blocksProperty.GetArrayElementAtIndex(0);
                        e.vector3IntValue = child.position.GetIntVector();
                    }
                }
            }
            GUILayout.EndHorizontal();
            
            
            GUILayout.Label("Scale Preview");
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Apply Scale"))
                {
                    figure.transform.localScale = new Vector3(figure.SpawnScale, figure.SpawnScale);
                }

                if (GUILayout.Button("Reset"))
                {
                    figure.transform.localScale = Vector3.one;
                }
            }
            GUILayout.EndHorizontal();
            
            GUILayout.Space(20);
            GUILayout.Label("Pivot Editor");
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Count Pivot"))
                {
                    var figureTransform = figure.transform;
                    var _ = new GameObject(PivotObjectName)
                    {
                        transform =
                        {
                            position = CountCenterOffset(figureTransform),
                            parent = figureTransform
                        }
                    };
                }

                if (GUILayout.Button("Save Pivot"))
                {
                    foreach (Transform child in figure.transform)
                    {
                        if (child.gameObject.name != PivotObjectName) continue;
                        
                        var pivotPosition = child.position;
                        _offsetProperty.vector3Value = pivotPosition;
                        break;
                    }
                }
            }
            GUILayout.EndHorizontal();

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        
        private static Vector2 CountCenterOffset(Transform figureTransform)
        {
            Vector2 offset;

            var blocksPositions = (
                from Transform child 
                in figureTransform 
                select child.position)
                .Select(dummy => (Vector2)dummy).ToList();

            if (blocksPositions.Count < 1)
            {
                offset = Vector3.zero;
                return offset;
            }
            var b = blocksPositions.ToList();
            Vector2 position = figureTransform.position;
            b.Add(position);

            var v = blocksPositions[0] - position;
            (float Min, float Max) xAxis = (v.x, v.x);
            (float Min, float Max) yAxis = (v.y, v.y);
            
            foreach (var t in b.Select(block => block - position))    
            {
                if (t.x < xAxis.Min) xAxis = (t.x, xAxis.Max);
                if (t.x > xAxis.Min) xAxis = (xAxis.Min, t.x);
                
                if (t.y < yAxis.Min) yAxis = (t.y, yAxis.Max);
                if (t.y > yAxis.Min) yAxis = (yAxis.Min, t.y);
            }

            offset = new Vector3((xAxis.Max + xAxis.Min) / 2, (yAxis.Max + yAxis.Min) / 2);
            return offset;
        }
    }
}