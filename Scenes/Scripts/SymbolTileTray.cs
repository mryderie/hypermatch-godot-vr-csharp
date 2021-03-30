using Godot;

public class SymbolTileTray : TileTray
{
	protected PackedScene SymbolTileScene = ResourceLoader.Load<PackedScene>("res://Scenes/SymbolTile.tscn");

	public override void _Ready()
	{
		base._Ready();
	}

	protected override T CreateTile<T>()
	{
		return SymbolTileScene.Instance() as T;
	}
}
