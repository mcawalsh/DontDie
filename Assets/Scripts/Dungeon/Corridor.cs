using System.Collections.Generic;
using UnityEngine;

public class Corridor
{
	public Vector2Int start;
	public Vector2Int end;

	public List<Vector2Int> wallTiles;

	public Corridor(Vector2Int start, Vector2Int end)
	{
		this.start = start;
		this.end = end;
		this.wallTiles = new List<Vector2Int>();
	}
}
