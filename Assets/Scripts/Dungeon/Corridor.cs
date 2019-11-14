using System.Collections.Generic;
using UnityEngine;

public interface IWallTiles
{
	public List<Vector2Int> wallTiles { get; }
}

public class Corridor : IWallTiles
{
	public Vector2Int start;
	public Vector2Int end;

	public List<Vector2Int> wallTiles { get; private set; }

	public Corridor(Vector2Int start, Vector2Int end)
	{
		this.start = start;
		this.end = end;
		this.wallTiles = new List<Vector2Int>();
	}
}
