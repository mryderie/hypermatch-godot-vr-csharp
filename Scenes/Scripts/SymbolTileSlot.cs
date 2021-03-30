using Godot;
using System;

public class SymbolTileSlot : TileSlot
{
	public override void _Ready()
	{
		base._Ready();
	}

	public Symbol? Symbol
	{
		get
		{
			return IsEmpty ? null : (Tile as SymbolTile)?.Symbol;
		}
	}
}
