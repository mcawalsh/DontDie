using System;
using System.Linq;
using UnityEngine;

public class DungeonGenerator
{
	public int width;
	public int height;
	public int scale;
	[Min(1)]
	public int minRooms = 1;
	public int maxRooms = 25;
	DungeonMap map;

	public DungeonGenerator(int width, int height, int gridScale)
	{
		this.width = width;
		this.height = height;
		this.scale = gridScale;

		map = new DungeonMap(width, height, scale);
	}

	internal DungeonMap GenerateDungeon()
	{
		GenerateLeaves();
		GenerateRooms(map.Leaf);

		GenerateTiles();

		return map;
	}

	private void GenerateCorridors(Leaf leaf)
	{
		if (leaf.childrenHaveRooms)
		{
			CreateCorridor(leaf.Children[0].room, leaf.Children[1].room);
		}
		else if (leaf.Children.Count() > 0)
		{
			foreach (var child in leaf.Children)
			{
				GenerateCorridors(child);
			}
		}
	}

	private void CreateCorridor(Room room1, Room room2)
	{
		Corridor corridor = new Corridor(room1.Origin, room2.Origin);

		map.corridors.Add(corridor);
	}

	private void GenerateRooms(Leaf leaf)
	{
		if (leaf.Children.Count() < 1)
		{
			map.Rooms.Add(leaf.CreateRoom());
		}
		else
		{
			foreach (var l in leaf.Children)
			{
				GenerateRooms(l);
			}

			CreateCorridor(leaf.Children[0].GetRoom(), leaf.Children[1].GetRoom());
		}
	}

	private void SetScaledTile(DungeonMap theMap, int x, int y, TileType tileType, IWallTiles wallTiles = null)
	{
		for (int xScale = 0; xScale < scale; xScale++)
		{
			for (int yScale = 0; yScale < scale; yScale++)
			{
				theMap.SetTile(x + xScale, y + yScale, tileType);

				if (wallTiles != null)
				{
					AddVerticalWallsToCorridor(wallTiles, x + xScale, y + yScale);
				}
			}
		}
	}

	private void GenerateTiles()
	{
		foreach (var room in map.Rooms)
		{
			for (int x = 0; x < room.Width * scale; x++)
			{
				for (int y = 0; y < room.Height * scale; y++)
				{
					int roomX = x + room.Origin.x * scale - scale;
					int roomY = y + room.Origin.y * scale - scale;

					SetScaledTile(map, roomX, roomY, TileType.Floor);
				}
			}

			CreateWallTiles(room);
		}

		foreach (var corridor in map.corridors)
		{
			CreateCorridorTile(corridor);
			CreateWallTiles(corridor);
		}
	}

	private void CreateWallTiles(Room room)
	{
		var positions = room.GetWallPositions(scale);

		foreach (var pos in positions)
		{
			DungeonTile tile = map.GetTile(pos);

			if (tile.TileType == TileType.Empty)
			{
				map.SetTile(pos, TileType.Wall);
			}
		}
	}

	private void CreateWallTiles(Corridor corridor)
	{
		foreach (var pos in corridor.wallTiles)
		{
			DungeonTile tile = map.GetTile(pos);

			if (tile.TileType == TileType.Empty)
			{
				map.SetTile(pos, TileType.Wall);
			}
		}
	}

	private void AddVerticalWallsToCorridor(IWallTiles wallTiles, int x, int y)
	{
		wallTiles.wallTiles.Add(new Vector2Int(x - 1, y));
		wallTiles.wallTiles.Add(new Vector2Int(x + 1, y));

		wallTiles.wallTiles.Add(new Vector2Int(x, y + 1));
		wallTiles.wallTiles.Add(new Vector2Int(x, y - 1));
	}

	private void CreateCorridorTile(Corridor corridor)
	{
		var start = corridor.start;
		var end = corridor.end;

		var width = (end.x - start.x) * scale;
		var height = (end.y - start.y) * scale;

		// ALWAYS DRAW TILES LEFT TO RIGHT, TOP TO BOTTOM
		if (width < 0) // X is End to Start
		{
			if (height < 0) // Drawing End to Start
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					int tileX = end.x * scale + x;
					int tileY = end.y * scale;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}

				for (int y = 0; y < Math.Abs(height); y++)
				{
					int tileX = start.x * scale;
					int tileY = start.y * scale + y;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}
			}
			else if (height > 0) // Drawing End to Start
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					int tileX = end.x * scale + x;
					int tileY = start.y * scale;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}

				for (int y = 0; y < Math.Abs(height); y++)
				{
					int tileX = end.x * scale;
					int tileY = start.y * scale + y;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}
			}
			else // height == 0
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					int tileX = end.x * scale + x;
					int tileY = end.y * scale;
					
					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}
			}
		}
		else if (width > 0)// X is Start to End
		{
			if (height < 0) // Drawing End to Start
			{
				for (int x = 0; x < Math.Abs(width) + 1; x++)
				{
					int tileX = start.x * scale + x;
					int tileY = start.y * scale;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}

				for (int y = 0; y < Math.Abs(height) + 1; y++)
				{
					int tileX = end.x * scale;
					int tileY = end.y * scale + y;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}
			}
			else if (height > 0)// Drawing Start to End
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					int tileX = start.x * scale + x;
					int tileY = start.y * scale;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}
				for (int y = 0; y < Math.Abs(height); y++)
				{
					int tileX = end.x * scale;
					int tileY = start.y * scale + y;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}
			}
			else // height == 0
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					int tileX = start.x * scale + x;
					int tileY = start.y * scale;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}
			}

		}
		else // width = 0
		{
			if (height > 0)
			{
				for (int y = 0; y < Math.Abs(height); y++)
				{
					int tileX = end.x * scale;
					int tileY = start.y * scale + y;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}
			}
			else if (height < 0)
			{
				for (int y = 0; y < Math.Abs(height); y++)
				{
					int tileX = end.x * scale;
					int tileY = end.y * scale + y;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}
			}
			else // height == 0
			{
				for (int y = 0; y < Math.Abs(height); y++)
				{
					int tileX = end.x * scale;
					int tileY = start.y * scale + y;

					SetScaledTile(map, tileX, tileY, TileType.Floor, corridor);
				}
			}
		}
	}

	private void GenerateLeaves()
	{
		// Start with a parent leaf
		map.Leaf = new Leaf(new Vector2Int(0, 0), width, height, null);

		while (map.NumberOfRooms < maxRooms)
		{
			SplitLeaf(map.Leaf);
		}
	}

	private void SplitLeaf(Leaf leaf)
	{
		// Split in 2 for now

		// Check if the leaf has children, if it does then split the child
		if (leaf.Children.Count() > 0)
		{
			foreach (var l in leaf.Children)
				SplitLeaf(l);
		}
		else
		{
			float splitPoint = UnityEngine.Random.Range(0.3f, 0.75f);

			// if it doesn't then split it in 2
			if (leaf.Height / leaf.Width < 0.75)
			{
				int lhsSplitWidth = (int)(leaf.Width * splitPoint);
				int rhsSplitWidth = (int)(leaf.Width * (1 - splitPoint));
				// Split Vertical
				leaf.Children.Add(new Leaf(leaf.Origin, lhsSplitWidth, leaf.Height, leaf));
				leaf.Children.Add(new Leaf(new Vector2Int(leaf.Origin.x + lhsSplitWidth, leaf.Origin.y), rhsSplitWidth, leaf.Height, leaf));
				map.NumberOfRooms++;
			}
			else
			{
				int lhsSplitWidth = (int)(leaf.Height * splitPoint);
				int rhsSplitWidth = (int)(leaf.Height * (1 - splitPoint));

				// Split horizontal
				leaf.Children.Add(new Leaf(leaf.Origin, leaf.Width, lhsSplitWidth, leaf));
				leaf.Children.Add(new Leaf(new Vector2Int(leaf.Origin.x, leaf.Origin.y + lhsSplitWidth), leaf.Width, rhsSplitWidth, leaf));
				map.NumberOfRooms++;
			}
		}
	}
}
