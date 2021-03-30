using Godot;

public class ColorTileTray : TileTray
{
	protected PackedScene ColorTileScene = ResourceLoader.Load<PackedScene>("res://Scenes/ColorTile.tscn");

	public override void _Ready()
	{
		base._Ready();
	}

	protected override T CreateTile<T>()
	{
		return ColorTileScene.Instance() as T;
	}
}
