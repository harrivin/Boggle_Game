using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	//declare the number of rows and columns for the boggle game - SETTING IT TO A 4x4
	public int ROWS = 4;
	public int COLUMNS = 4;

	public GameObject gridTileGO;

	//space between tiles
	public  float GRID_OFFSET_X = 8f;
	public  float GRID_OFFSET_Y = 8f;

	[HideInInspector]
	public List<Tile> tiles;

	[HideInInspector]
	public List<List<Tile>> gridTiles;

	private string wordSource;

	private int wordSourceIndex;

	private struct Cell
	{
		public int row;
		public int column;
	}

	private List<Cell> gridIndexes;


	void Awake () {
		BuildSuffledIndexes ();
	}

	public void BuildGrid ()
	{
		var wordData = GetComponent<WordData> ();
		wordSource = wordData.GetRandomWord ();
		
		foreach (var index in gridIndexes) 
		{
			gridTiles[index.column][index.row].SetTileData(wordSource[wordSourceIndex]);
			wordSourceIndex++;
			if (wordSourceIndex == wordSource.Length)
			{
				wordSource = wordData.GetRandomWord();
				wordSourceIndex = 0;
			}
		}
	}

	private void BuildSuffledIndexes ()
	{
		tiles = new List<Tile> ();
		gridTiles = new List<List<Tile>> ();
		
		gridIndexes = new List<Cell> ();
		Cell indexer;
		for (int column = 0; column < COLUMNS; column++) {
			
			var columnTiles = new List<Tile>();
			
			for (int row = 0; row < ROWS; row++) {
				indexer = new Cell();
				indexer.column = column;
				indexer.row = row;
				gridIndexes.Add (indexer);
				
				var item = Instantiate (gridTileGO) as GameObject;
				var tile = item.GetComponent<Tile>();
				tile.SetTilePosition(this, column, row);
				tile.transform.parent = gameObject.transform;
				tiles.Add (tile);
				columnTiles.Add (tile);
			}
			gridTiles.Add(columnTiles);
		}

		WordData.ShuffleList (gridIndexes);
	}

}
