using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerStats))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PlayerStats grid = (PlayerStats)target;
        if(GUILayout.Button("Reset"))
        {
            grid.ResetPlayerAllStats();
        }
    }
}
