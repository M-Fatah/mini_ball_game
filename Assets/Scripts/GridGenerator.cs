using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
	public Transform cellPrefab;
	public Transform turretPrefab;
	public Transform coinPrefab;
	public Grid grid;

	private Queue<GridCellPos> shuffledCellPositions;
    private Queue<TurretPosition> shuffledTurretPositions;
	private List<GridCellPos> allGridCellPoses;
	private TurretPosition[] turretPositions;

	void Start()
	{
		// Loads map data from a file.
		if(LoadDataFromFile.instance.IsReady())
		{
			float width = LoadDataFromFile.instance.GetGridElement("width");
            float height = LoadDataFromFile.instance.GetGridElement("height");
            int turretsNumber = LoadDataFromFile.instance.GetGridElement("turretsNumber");

			grid.width = (width == 0)? grid.width : width;
            grid.height = (height == 0) ? grid.height : height;
			grid.turretsNumber = (turretsNumber == 0)? grid.turretsNumber : turretsNumber;

			turretPositions = LoadDataFromFile.instance.GetTurretPosition();
		}
		
		GenerateGrid();
	}

	public void GenerateGrid()
	{

		allGridCellPoses = new List<GridCellPos>();

        // Getting random empty grid cell positions.
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
			{
				// Making sure the cell is not occupied by a turret.
				for(int i = 0; i < turretPositions.Length; i++)
				{
					GridCellPos turretCellPos = new GridCellPos(turretPositions[i].x, turretPositions[i].y);
					GridCellPos randomPossibleCell = new GridCellPos(x, y);
					if(turretCellPos != randomPossibleCell)
					{
						allGridCellPoses.Add(randomPossibleCell);
					}
				}
			}
		}

		shuffledCellPositions = new Queue<GridCellPos>(Utils.ShuffleArray(allGridCellPoses.ToArray(), 10));
		shuffledTurretPositions = new Queue<TurretPosition>(Utils.ShuffleArray(turretPositions, 10));


        // Ground collider.
        BoxCollider groundCollider = GetComponent<BoxCollider>();
		groundCollider.size = new Vector3(grid.width, cellPrefab.transform.localScale.y, grid.height);

		
		// Creates an empty gameObject to hold all the current grid cells.
		if(transform.Find("GridMapContainer"))
		{
			DestroyImmediate(transform.Find("GridMapContainer").gameObject);
		}
		
		GameObject gridMapContainer = new GameObject("GridMapContainer");
		gridMapContainer.transform.parent = transform;
		
		// Wall colliders.
		if(transform.Find("WallColliders"))
		{
			DestroyImmediate(transform.Find("WallColliders"));
		}
		GameObject wallColliders = new GameObject("WallColliders");
		wallColliders.transform.parent = gridMapContainer.transform;
        BoxCollider leftWallCollider = wallColliders.AddComponent<BoxCollider>();
        BoxCollider rightWallCollider = wallColliders.AddComponent<BoxCollider>();
        BoxCollider topWallCollider = wallColliders.AddComponent<BoxCollider>();
        BoxCollider bottomWallCollider = wallColliders.AddComponent<BoxCollider>();

		leftWallCollider.center = new Vector3(-grid.width / 2, 0.5f, 0);
		leftWallCollider.size = new Vector3(cellPrefab.localScale.y, cellPrefab.localScale.x, grid.height);
		
		rightWallCollider.center = new Vector3(grid.width / 2, 0.5f, 0);
        rightWallCollider.size = new Vector3(cellPrefab.localScale.y, cellPrefab.localScale.x, grid.height);

        topWallCollider.center = new Vector3(0, 0.5f, grid.height / 2);
        topWallCollider.size = new Vector3(grid.width, cellPrefab.localScale.z, cellPrefab.localScale.y);

		bottomWallCollider.center = new Vector3(0, 0.5f, -grid.height / 2);
        bottomWallCollider.size = new Vector3(grid.width, cellPrefab.localScale.z, cellPrefab.localScale.y);

		// Creating the grid map.
		for (int x = 0; x < grid.width; x++)
		{
			
			for(int y = 0; y < grid.height; y++)
			{
				// Creating grid vertical walls.
				if(x == 0 || x == grid.width - 1)
				{	
					Vector3 wallCellPos = (x == 0)? new Vector3(-grid.width / 2 + x, 0.5f, -grid.height / 2 + 0.5f + y) : new Vector3(-grid.width / 2 + x + 1, 0.5f, -grid.height / 2 + 0.5f + y);
					Transform wallCell = (Transform)Instantiate(cellPrefab, wallCellPos, Quaternion.Euler(Vector3.forward * 90f)); 
					wallCell.parent = gridMapContainer.transform;
				}

                // Creating grid horizontal walls.
                if(y == 0 || y == grid.height - 1)
				{
					Vector3 wallCellPos = (y == 0)? new Vector3(-grid.width / 2 + x + 0.5f, 0.5f, -grid.height / 2 + y) : new Vector3(-grid.width / 2 + x + 0.5f, 0.5f, -grid.height / 2 + y + 1);
					Transform wallCell = (Transform)Instantiate(cellPrefab, wallCellPos, Quaternion.Euler(Vector3.right * 90f)); 
					wallCell.parent = gridMapContainer.transform;
				}

				// Creating ground cells.
				Vector3 gridCellPos = CellPosToWorldPos(new GridCellPos(x, y));
				Transform gridCell = (Transform)Instantiate(cellPrefab, gridCellPos, Quaternion.identity);
				gridCell.parent = gridMapContainer.transform;


			}
		}


		// Instantiating turrets at the given random position.
		for(int i = 0; i < grid.turretsNumber; i++)
		{
			GridCellPos turretRandomCellPos = GetRandomTurretGridCell();

			if(!(turretRandomCellPos.x < 0 || turretRandomCellPos.x > grid.width))
			{
				if(!(turretRandomCellPos.y < 0 || turretRandomCellPos.y > grid.height))
				{
					Transform turret = (Transform)Instantiate(turretPrefab, CellPosToWorldPos(turretRandomCellPos) + Vector3.up * 0.55f, Quaternion.Euler(0, Random.Range(0.0f, 180.0f), 0));
					turret.parent = gridMapContainer.transform;
				}
			}
		}

		// Instantiating coins at the given random position.
		for(int i = 0; i < grid.totalCoinsNumber; i++)
		{
			Transform coin = (Transform)Instantiate(coinPrefab, CellPosToWorldPos(GetRandomGridCellPosition()) + Vector3.up * 0.5f, Quaternion.identity);
            coin.parent = gridMapContainer.transform;
		}
	}

    // Returns a random cell position in world space from x, y coords.
    public GridCellPos GetRandomGridCellPosition()
	{
		GridCellPos randomGridCellPosition = shuffledCellPositions.Dequeue();

		shuffledCellPositions.Enqueue(randomGridCellPosition);

		return randomGridCellPosition;
	}

	// Returns a random turret position in world space from x, y coords.
	public GridCellPos GetRandomTurretGridCell()
    {
		TurretPosition randomTurretPos = shuffledTurretPositions.Dequeue(); 
        shuffledTurretPositions.Enqueue(randomTurretPos);

        GridCellPos randomGridCellPos = new GridCellPos(randomTurretPos.x, randomTurretPos.y);
		
		return randomGridCellPos;
    }

	// Returns a world space position from x, y coords.
	public Vector3 CellPosToWorldPos(GridCellPos gridCellPos)
	{
		return new Vector3(-grid.width / 2 + 0.5f + gridCellPos.x, 0, -grid.height / 2 + 0.5f + gridCellPos.y);
	}

	
	public class GridCellPos
	{
		public int x;
		public int y;

		public GridCellPos(int _x, int _y)
		{
			x = _x;
			y = _y;
		}

		public static bool operator ==(GridCellPos gridCell1, GridCellPos gridCell2)
		{
			return (gridCell1.x == gridCell2.x) && (gridCell1.y == gridCell2.y);
		}

		public static bool operator !=(GridCellPos gridCell1, GridCellPos gridCell2)
        {
            return !(gridCell1 == gridCell2);
        }
	}

	[System.Serializable]
	public class Grid
	{
		public float width = 10;
		public float height = 10;
		public int turretsNumber = 3;
		public int totalCoinsNumber = 10;
	}
}
