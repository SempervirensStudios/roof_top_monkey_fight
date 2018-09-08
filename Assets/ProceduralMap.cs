using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralMap : MonoBehaviour {

	public RuleTile ruleTile;
	public int columns;
	public int rows;
    private Tilemap tileMap;
	private int[,] gridArray;
	private int[] buildingHeights;

    // Use this for initialization
    void Start () {
		Debug.Log("Loaded rule tile " + ruleTile.name );

		tileMap = gameObject.GetComponent<Tilemap>();

		gridArray = InitializeGrid(rows, columns);

		buildingHeights = GenerateBuildingHeights(columns, rows);

		gridArray = PopulateGrid(gridArray, buildingHeights);

		FillTileMap(tileMap, gridArray, ruleTile);

	}

	static int CappedAutoRegressiveProcess(int previousHeight, int maxHeight, int variance = 5) {

		int newHeight = previousHeight + Random.Range(-variance, variance);

		newHeight = (newHeight > maxHeight)? maxHeight : newHeight;

		newHeight = (newHeight < 0)? 0 : newHeight;

		return newHeight;
	}

	static int[] GenerateBuildingHeights(int ncols, int nrows) {

		int[] heights = new int[ncols];

		heights[0] = Random.Range(0, ncols / 3);

		for (int i = 1; i < heights.GetLength(0); i++) {

			heights[i] = CappedAutoRegressiveProcess(heights[i - 1], nrows - 1);

		}

		return heights;
	}

	static int[,] InitializeGrid(int nrows, int ncols) {

		return new int[nrows, ncols];
		
	}

	static int[,] PopulateGrid(int[,] grid, int[] colHeights) {

		int gridRows = grid.GetLength(0);
		int gridCols = grid.GetLength(1);

		Debug.Log("Array: (" + gridRows + ", " + gridCols + ")");

		for (int i = 0; i < gridRows; i++) {

			for (int j = 0; j < gridCols; j++) {

				grid[i, j] = (i <= colHeights[j])? 1 : 0;

			}
		}

		return grid;
	}

	void FillTileMap(Tilemap tMap, int[,] boolGrid, RuleTile rTile) {

		int gridRows = boolGrid.GetLength(0);
		int gridCols = boolGrid.GetLength(1);

		for (int i = 0; i < gridRows; i++) {

			for (int j = 0; j < gridCols; j++) {

				if (boolGrid[i, j] == 1) {

					tMap.SetTile(new Vector3Int(j, i, 0), rTile);
				}
			}
		}
	}
	
}
