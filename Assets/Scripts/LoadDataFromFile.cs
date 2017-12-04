using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LoadDataFromFile : MonoBehaviour
{
  	public static LoadDataFromFile instance;


	private string jsonData;
    private GridData gridData;
    private Dictionary<string, int> gridDataDictionary;
    private TurretPosition[] turretPositions;

    private bool isReady = false;

	void Awake()
	{
        // Singelton pattern.
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        
		ReadGridDataFromFile("GridData.json");
	}

    
	public void ReadGridDataFromFile(string fileName)
    {
        gridDataDictionary = new Dictionary<string, int>();

        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        jsonData = File.ReadAllText(filePath);

        if (!string.IsNullOrEmpty(jsonData))
        {
            gridData = JsonUtility.FromJson<GridData>(jsonData);

            for(int i = 0; i < gridData.gridElements.Length; i++)
            {
                gridDataDictionary.Add(gridData.gridElements[i].element, gridData.gridElements[i].value);
            }

            turretPositions = gridData.turretPositions;

        }
        else
        {
            Debug.Log("Could not load a grid data file, using default values!");
        }

        isReady = true;
    }

    public int GetGridElement(string key)
    {
        int result = 0;

        if (gridDataDictionary.ContainsKey(key))
        {
            result = gridDataDictionary[key];
        }

        return result;
    }

    public TurretPosition[] GetTurretPosition()
    {
        return turretPositions;
    } 

    public bool IsReady()
    {
        return isReady;
    }
}

[System.Serializable]
public class GridData
{
    public GridElements[] gridElements;
    public TurretPosition[] turretPositions;
}

[System.Serializable]
public class GridElements
{
    public string element;
    public int value;
}

[System.Serializable]
public class TurretPosition
{
    public int x;
    public int y;
}
