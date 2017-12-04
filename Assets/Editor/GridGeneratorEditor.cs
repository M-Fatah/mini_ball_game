using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GridGenerator gridGenerator = target as GridGenerator;

		if(GUILayout.Button("Generate Grid"))
		{
			gridGenerator.GenerateGrid();
		}
	}
}
