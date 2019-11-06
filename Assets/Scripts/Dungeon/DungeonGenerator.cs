using System;

public class DungeonGenerator
{
	internal static DungeonMap GenerateDungeon(int width, int height)
	{
		DungeonMap map = new DungeonMap(width, height);

		for (int x = 1; x < width - 1; x++)
		{
			for (int y = 1; y < height -1; y++)
			{
				map.SetTile(x, y, TileType.Floor);
			}
		}

		for (int y = 0; y < height; y++)
		{
			if (y < 1 || y >= height - 1)
			{
				for (int x = 0; x < width; x++)
				{
					map.SetTile(x, y, TileType.Wall);
				}
			}
			else
			{
				map.SetTile(0, y, TileType.Wall);
				map.SetTile(width - 1, y, TileType.Wall);
			}
		}

		return map;
	}
}
