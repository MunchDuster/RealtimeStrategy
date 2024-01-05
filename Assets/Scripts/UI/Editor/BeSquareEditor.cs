using UnityEngine;
using UnityEditor;

namespace Munchy.UI
{
    /// <summary>
    /// Adds button to make the script work in editor easily    
    /// </summary>
    [CustomEditor(typeof(BeSquare))]
    public class BeSquareEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            BeSquare yourScript = (BeSquare)target;

            if (GUILayout.Button("Make square"))
                yourScript.MakeSquare();

            GUILayout.Space(10);

            DrawDefaultInspector();
        }
    }
}