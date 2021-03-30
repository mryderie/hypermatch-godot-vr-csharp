using Godot;
using System;

public class TraySlot : Spatial
{
	public bool IsEmpty
	{
		get
		{
			return Tile == null;
		}
	}
	public bool StartingSlot { get; protected set; }
	public Tile Tile { get; protected set; }

	public void Initialize()
	{
		Tile?.QueueFree();
		Tile = null;
	}

	public void Initialize(bool startingSlot)
	{
		StartingSlot = startingSlot;
	}

	public void Occupy(Tile tile)
	{
		Tile = tile;
	}
	
	public void Matched()
	{
		Tile = null;
	}
}
