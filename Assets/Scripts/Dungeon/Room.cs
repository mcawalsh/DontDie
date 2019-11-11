using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public Vector2Int Origin;
	public int Width;
	public int Height;

	public Room(Vector2Int origin, int width, int height)
	{
		this.Origin = origin;
		this.Width = width;
		this.Height = height;
	}

	public List<Vector2Int> GetSurroundPositions()
	{
		var result = new List<Vector2Int>();

		for (int y = -1; y <= Height; y++)
		{
			if (y == -1 || y == Height)
			{
				for (int x = -1; x <= Width; x++)
				{
					result.Add(new Vector2Int(Origin.x + x - 1, Origin.y + y - 1));
				}
			} else
			{
				result.Add(new Vector2Int(Origin.x - 2, Origin.y + y - 1));
				result.Add(new Vector2Int(Origin.x + Width - 1, Origin.y + y - 1));
			}
		}

		return result;
	}
}
