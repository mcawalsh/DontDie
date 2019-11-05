using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	public Tilemap groundMap;
	public Tilemap itemMap;

	public Tile groundTile;
	public Tile wallTile;

	public GameObject player;
    
    void Start()
	{
		InitialisGrid();
		// InitialisePlayer();
	}

	private void InitialisePlayer()
	{
		Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
	}

	private void InitialisGrid()
	{
		int gridWidth = 10;
		int gridHeight = 10;

		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				groundMap.SetTile(new Vector3Int(x - gridWidth / 2, y - gridHeight / 2, 0), groundTile);
			}
		}
	}

	void Update()
    {
        
    }
}
