using System;

public class DungeonMap
{
	public DungeonTile[,] Tiles { get; private set; }

	public DungeonMap(int width, int height)
	{
		this.Tiles = new DungeonTile[width, height];
	}

	internal void SetTile(int x, int y, TileType value)
	{
		var tile = Tiles[x, y];
		if (tile == null)
			tile = new DungeonTile();
		
		tile.TileType = value;

		Tiles[x, y] = tile;
	}
}
