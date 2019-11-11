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

	public List<Vector2Int> GetWallPositions(int scale)
	{
		var result = new List<Vector2Int>();

		// Scale Origin -- Origin.x * scale - scale = Room Origin
		var scaledOrigin = new Vector2Int(Origin.x * scale - scale - 1, Origin.y * scale - scale - 1);

		for (int heightPos = 0; heightPos <= Height * scale + 2; heightPos++)
		{
			if (heightPos == 0 || heightPos == (Height * scale + 2))
			{
				for (int widthPos = 0; widthPos <= Width * scale + 2; widthPos++)
				{
					result.Add(new Vector2Int(scaledOrigin.x + widthPos, scaledOrigin.y + heightPos));
				}
			} else
			{
				result.Add(new Vector2Int(scaledOrigin.x, scaledOrigin.y + heightPos)); // Left Wall
				result.Add(new Vector2Int(scaledOrigin.x + Width * scale + 2, scaledOrigin.y + heightPos)); // Right Wall
			}
		}

		return result;
	}
}
