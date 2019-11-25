using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	public CameraMovement cameraMovement;
	public Tilemap groundMap;
	public Tilemap collisionMap;

	public Tile groundTile;
	public Tile wallTile;

	public Rigidbody2D player;
	public Rigidbody2D enemy;

	[Min(10)]
	public int gridWidth = 100;
	[Min(10)]
	public int gridHeight = 100;

	[Min(1)]
	public int gridScale = 2;

	private DungeonGenerator generator;
	private DungeonMap map;

	void Start()
	{
		SetupCamera();
		SetupMap();
		InitialisePlayer();
	}

	private void SetupMap()
	{
		generator = new DungeonGenerator(gridWidth, gridHeight, gridScale);
		map = generator.GenerateDungeon();
		DrawMap(map.Tiles);
		AddCollisions();
	}

	private void SetupCamera()
	{
		cameraMovement.maxPosition = new Vector2(gridWidth * gridScale, gridHeight * gridScale);
		cameraMovement.minPosition = new Vector2(-gridWidth * gridScale, -gridHeight * gridScale);
		cameraMovement.target = player.transform;
	}

	private void InitialisePlayer()
	{
		var room = map.Leaf.GetRoom();

		player.transform.SetPositionAndRotation(GridToTileSpace(room.Origin), Quaternion.identity);

		Vector2Int enemyPos = new Vector2Int(room.Origin.x + 2, room.Origin.y + 2);
		enemy.transform.SetPositionAndRotation(GridToTileSpace(enemyPos), Quaternion.identity);
	}

	private Vector3Int GridToTileSpace(Vector2Int origin)
	{
		return GridToTileSpace(origin.x, origin.y);
	}

	private Vector3Int GridToTileSpace(int x, int y)
	{
		return new Vector3Int(x * gridScale - gridWidth / 2, y * gridScale - gridHeight / 2, 0);
	}

	private void DrawMap(DungeonTile[,] tiles)
	{
		for (int x = 0; x < gridWidth * gridScale; x++)
		{
			for (int y = 0; y < gridHeight * gridScale; y++)
			{
				DungeonTile tile = tiles[x, y];

				if (tile != null)
				{
					switch (tile.TileType)
					{
						case TileType.Floor:
							int tileX = x - gridWidth / 2;
							int tileY = y - gridHeight / 2;
							groundMap.SetTile(new Vector3Int(tileX, tileY, 0), groundTile);
							break;
						case TileType.Wall:
							collisionMap.SetTile(new Vector3Int(x - gridWidth / 2, y - gridHeight / 2, 0), wallTile);
							break;
						case TileType.Empty:
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

		collisionMap.gameObject.AddComponent<CompositeCollider2D>();
	}
}
