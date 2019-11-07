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
}
