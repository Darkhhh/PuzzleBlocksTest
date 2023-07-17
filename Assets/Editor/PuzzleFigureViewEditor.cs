using Source.Code.Views;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PuzzleFigureView))]
    public class PuzzleFigureViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var figure = (PuzzleFigureView)target;
            
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Apply Scale"))
            {
                figure.transform.localScale = new Vector3(figure.SpawnScale, figure.SpawnScale);
            }

            if (GUILayout.Button("Reset"))
            {
                figure.transform.localScale = Vector3.one;
            }
            
            GUILayout.EndHorizontal();
        }
    }
}