using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Leaf
{
	public Vector2Int Origin { get; set; }
	public int Width { get; set; }
	public int Height { get; set; }
	public int MinRooms { get; set; } = 0;
	public int MaxRooms { get; set; } = 0;
	public List<Leaf> Children { get; private set;  }
	public Room room;
	public Leaf parent;
	public bool childrenHaveRooms = false;

	public Leaf(Vector2Int origin, int width, int height, Leaf parent)
	{
		this.Origin = origin;
		this.Width = width;
		this.Height = height;
		this.parent = parent;
		//this.MinRooms = minRooms;
		//this.MaxRooms = maxRooms;

		Children = new List<Leaf>();
	}

	internal Room CreateRoom()
	{
		// random width
		int roomWidth = Random.Range(2, this.Width - 2);

		// random height
		int roomHeight = Random.Range(2, this.Height - 2);

		// random origin position x and y
		int originX = Random.Range(2, this.Width - 2 - roomWidth);
		int originY = Random.Range(2, this.Height - 2 - roomHeight);

		// Create a new room
		room = new Room(new Vector2Int(this.Origin.x + originX, this.Origin.y + originY), roomWidth, roomHeight);

		parent.childrenHaveRooms = true;

		return room;
	}

	internal Room GetRoom()
	{
		if (room != null)
		{
			return room;
		}

		if (Children.Count > 0)
		{
			if(Random.Range(0, 1) > 0.5f)
			{
				return Children[0].GetRoom();
			} else
			{
				return Children[1].GetRoom();
			}
		}

		return null;
	}
}
