#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RocketDatabase))]
public class RocketDatabaseEditor : Editor
{
    public float closenessRange = 5f;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RocketDatabase rocketDatabase = (RocketDatabase)target;
        
        if (GUILayout.Button("Sort Landmarks"))
        {
            SortLandMarks(rocketDatabase);
        }

        GUILayout.Label("Landmark Closeness Range (m)");
        closenessRange = EditorGUILayout.FloatField(closenessRange);
        
        if (GUILayout.Button("Print Close Landmarks"))
        {
            PrintCloseLandmarks(rocketDatabase, closenessRange);
        }
    }
    
    public void SortLandMarks(RocketDatabase rocketDatabase)
    {
        rocketDatabase.landMarks = rocketDatabase.landMarks.OrderBy(x => x.height).ToList();
    }

    public void PrintCloseLandmarks(RocketDatabase rocketDatabase, float range)
    {
        SortLandMarks(rocketDatabase);
        
        for (int i = 0; i < rocketDatabase.landMarks.Count - 1; i++)
        {
            if (rocketDatabase.landMarks[i + 1].height - rocketDatabase.landMarks[i].height < range * 100f)
            {
                Debug.Log(rocketDatabase.landMarks[i].name);
            }
        }
    }
}
#endif