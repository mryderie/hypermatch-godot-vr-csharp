using Godot;
using System;

public class ColorTileSlot : TileSlot
{
	public override void _Ready()
	{
		base._Ready();
	}

	public Color? Color
	{
		get
		{
			return IsEmpty ? null : (Tile as ColorTile)?.Color;
		}
	}
}
