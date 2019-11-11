﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMap
{
	public DungeonTile[,] Tiles { get; private set; }
	public Leaf Leaf { get; internal set; }

	public List<Room> Rooms { get; set; }
	public List<Corridor> corridors;

	public int NumberOfRooms { get; set; }

	public DungeonMap(int width, int height, int scale)
	{
		this.Tiles = new DungeonTile[width * scale, height * scale];
		NumberOfRooms = 0;
		this.Rooms = new List<Room>();
		this.corridors = new List<Corridor>();
	}

	internal void SetTile(int x, int y, TileType value)
	{
		var tile = Tiles[x, y];
		if (tile == null)
			tile = new DungeonTile();

		tile.TileType = value;

		Tiles[x, y] = tile;
	}

	internal DungeonTile GetTile(Vector2Int pos)
	{
		return Tiles[pos.x, pos.y];
	}
}