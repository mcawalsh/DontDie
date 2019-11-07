using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	public Tilemap groundMap;
	public Tilemap collisionMap;

	public Tile groundTile;
	public Tile wallTile;

	public GameObject player;

	[Min(10)]
	public int gridWidth = 100;
	[Min(10)]
	public int gridHeight = 100;

	private DungeonGenerator generator;
	private DungeonMap map;

	void Start()
	{
		generator = new DungeonGenerator(gridWidth, gridHeight);
		map = generator.GenerateDungeon();
		DrawMap(map.Tiles);
		InitialisePlayer();
		AddCollisions();
	}

	private void InitialisePlayer()
	{
		player.gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
	}

	private void DrawMap(DungeonTile[,] tiles)
	{
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				DungeonTile tile = tiles[x, y];

				if (tile != null)
				{
					switch (tile.TileType)
					{
						case TileType.Floor:
							groundMap.SetTile(new Vector3Int(x - gridWidth / 2, y - gridHeight / 2, 0), groundTile);
							break;
						case TileType.Wall:
							collisionMap.SetTile(new Vector3Int(x - gridWidth / 2, y - gridHeight / 2, 0), wallTile);
							break;
						default:
							break;
					}
				}
			}
		}
	}

	private void AddCollisions()
	{
		var rb = collisionMap.gameObject.AddComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Static;
		rb.simulated = true;

		var collider = collisionMap.gameObject.AddComponent<TilemapCollider2D>();
		collider.usedByComposite = true;

		var comp = collisionMap.gameObject.AddComponent<CompositeCollider2D>();
	}
}
