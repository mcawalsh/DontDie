using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	public Tilemap groundMap;
	public Tilemap collisionMap;

	public Tile groundTile;
	public Tile wallTile;

	public GameObject player;

	int gridWidth = 10;
	int gridHeight = 10;

	void Start()
	{
		InitialiseGrid();
		InitialiseWalls();
		InitialisePlayer();
		AddCollisions();
	}

	private void InitialisePlayer()
	{
		player.gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
	}

	private void InitialiseGrid()
	{
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				groundMap.SetTile(new Vector3Int(x - gridWidth / 2, y - gridHeight / 2, 0), groundTile);
			}
		}
	}

	private void InitialiseWalls()
	{
		for (int y = -1; y < gridHeight +1; y++)
		{
			if (y < 0 || y >= gridHeight)
			{
				for (int x = -1; x < gridWidth + 1; x++)
				{
					collisionMap.SetTile(new Vector3Int(x - gridWidth / 2, y - gridHeight / 2, 0), wallTile);
				}
			}
			else
			{
				collisionMap.SetTile(new Vector3Int(-1 - gridWidth / 2, y - gridHeight / 2, 0), wallTile);
				collisionMap.SetTile(new Vector3Int(gridWidth - gridWidth / 2, y - gridHeight / 2, 0), wallTile);
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

	void Update()
    {
        
    }
}
