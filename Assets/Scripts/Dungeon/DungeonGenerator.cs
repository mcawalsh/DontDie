using System;
using System.Linq;
using UnityEngine;

public class DungeonGenerator
{
	public int width;
	public int height;
	[Min(1)]
	public int minRooms = 1;
	public int maxRooms = 25;
	DungeonMap map;

	public DungeonGenerator(int width, int height)
	{
		this.width = width;
		this.height = height;
		map = new DungeonMap(width, height);
	}

	internal DungeonMap GenerateDungeon()
	{
		GenerateLeaves();
		GenerateRooms(map.Leaf);
		GenerateCorridors(map.Leaf);

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

	private void GenerateTiles()
	{
		foreach (var room in map.Rooms)
		{
			for (int x = 0; x < room.Width; x++)
			{
				for (int y = 0; y < room.Height; y++)
				{
					map.SetTile(x + room.Origin.x - 1, y + room.Origin.y - 1, TileType.Floor);
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

		var width = Mathf.Clamp(end.x - start.x, 1, 200);
		var height = Mathf.Clamp(end.y - start.y, 1, 200);

		// ALWAYS DRAW TILES LEFT TO RIGHT, TOP TO BOTTOM

		if (width < 0) // X is End to Start
		{
			if (height < 0) // Drawing End to Start
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					map.SetTile(end.x + x, start.y, TileType.Wall);
				}

				for (int y = 0; y < Math.Abs(height); y++)
				{
					map.SetTile(start.x, start.y + y, TileType.Wall);
				}
			}
			else // Drawing End to Start
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					map.SetTile(end.x + x, start.y, TileType.Wall);
				}

				for (int y = 0; y < Math.Abs(height); y++)
				{
					map.SetTile(end.x, start.y + y, TileType.Wall);
				}
			}
		}
		else // X is Start to End
		{
			if (height < 0) // Drawing End to Start
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					map.SetTile(end.x + x, end.y, TileType.Wall);
				}

				for (int y = 0; y < Math.Abs(height); y++)
				{
					map.SetTile(end.x, start.y + y, TileType.Wall);
				}
			}
			else // Drawing Start to End
			{
				for (int x = 0; x < Math.Abs(width); x++)
				{
					map.SetTile(start.x + x, start.y, TileType.Wall);
				}
				for (int y = 0; y < Math.Abs(height); y++)
				{
					map.SetTile(end.x, start.y + y, TileType.Wall);
				}
			}
		}
	}

	private void GenerateTiles(Leaf parentLeaf)
	{
		foreach (var leaf in parentLeaf.Children)
		{
			if (leaf.Children == null || leaf.Children.Count() == 0)
			{
				// Draw the tile
				GenerateLeafTiles(leaf);
			}

			foreach (var l in leaf.Children)
			{
				GenerateTiles(l);
			}
		}
	}

	private void GenerateLeafTiles(Leaf leaf)
	{
		for (int x = 0; x < leaf.Width; x++)
		{
			for (int y = 0; y < leaf.Height; y++)
			{
				map.SetTile(x + leaf.Origin.x, y + leaf.Origin.y, TileType.Floor);
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
