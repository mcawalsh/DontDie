using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

	private void GenerateWalls()
	{
		foreach(var room in map.Rooms)
		{
			SetRoomWallTiles(room);
		}

		foreach(var hall in map.corridors)
		{
			SetCorridorWallTiles(hall);
		}
	}

	private void SetCorridorWallTiles(Corridor hall)
	{
		
	}

	private bool isDone = false;

	private void SetRoomWallTiles(Room room)
	{
		TileType wallType = TileType.Wall;

		if (!isDone)
		{
			wallType = TileType.Empty;
			isDone = true;
		}

		var roomPositions = room.GetSurroundPositions();

		foreach (var pos in roomPositions)
		{
			var tile = map.GetTile(pos);

			if (tile == null || tile.TileType == TileType.Empty)
			{
				map.SetTile(pos.x, pos.y, wallType);
			}
		}
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

	private void SetScaledTile(DungeonMap theMap, int x, int y, TileType tileType)
	{
		for (int xScale = 0; xScale < scale; xScale++)
		{
			for (int yScale = 0; yScale < scale; yScale++)
			{
				theMap.SetTile(x + xScale, y + yScale, tileType);
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
		}

		foreach (var corridor in map.corridors)
		{
			CreateCorridorTile(corridor);
		}
	}

	private void CreateCorridorTile(Corridor corridor)
	{
		var start = corridor.Start;
		var end = corridor.End;

		var width = (end.x - start.x) * scale;
		var height = (end.y - start.y) * scale;

		// ALWAYS DRAW TILES LEFT TO RIGHT, TOP TO BOTTOM
		if (width < 0) // X is End to Start
		{
			if (height < 0) // Drawing End to Start
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					SetScaledTile(map, end.x * scale + x, end.y * scale, TileType.Floor);
				}

				for (int y = 0; y < Math.Abs(height); y++)
				{
					SetScaledTile(map, start.x * scale, start.y * scale + y, TileType.Floor);
				}
			}
			else if (height > 0) // Drawing End to Start
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					SetScaledTile(map, end.x * scale + x, start.y * scale, TileType.Floor);
				}

				for (int y = 0; y < Math.Abs(height); y++)
				{
					SetScaledTile(map, end.x * scale, start.y * scale + y, TileType.Floor);
				}
			}
			else // height == 0
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					SetScaledTile(map, end.x * scale + x, end.y * scale, TileType.Floor);
				}
			}
		}
		else if (width > 0)// X is Start to End
		{
			if (height < 0) // Drawing End to Start
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					SetScaledTile(map, start.x * scale + x, start.y * scale, TileType.Floor);
				}

				for (int y = 0; y < Math.Abs(height) + 1; y++)
				{
					SetScaledTile(map, end.x * scale, end.y * scale + y, TileType.Floor);
				}
			}
			else if (height > 0)// Drawing Start to End
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					SetScaledTile(map, start.x * scale + x, start.y * scale, TileType.Floor);
				}
				for (int y = 0; y < Math.Abs(height); y++)
				{
					SetScaledTile(map, end.x * scale, start.y * scale + y, TileType.Floor);
				}
			}
			else // height == 0
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					SetScaledTile(map, start.x * scale + x, start.y * scale, TileType.Floor);
				}
			}

		}
		else // width = 0
		{
			if (height > 0)
			{
				for (int y = 0; y < Math.Abs(height); y++)
				{
					SetScaledTile(map, end.x * scale, start.y * scale + y, TileType.Floor);
				}
			}
			else if (height < 0)
			{
				for (int y = 0; y < Math.Abs(height); y++)
				{
					SetScaledTile(map, end.x * scale, end.y * scale + y, TileType.Floor);
				}
			}
			else // height == 0
			{
				for (int y = 0; y < Math.Abs(height); y++)
				{
					SetScaledTile(map, end.x * scale, start.y * scale + y, TileType.Floor);
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
