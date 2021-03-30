using Godot;
using System.Collections.Generic;

public class ColorTile : Tile
{
	public Color Color { get; protected set; }

	public override void _Ready()
	{
		base._Ready();

		TileSlot = GetNode<ColorTileSlot>("/root/Game/MatchTileStream/ColorTileSlot");
	}

	public override void Initialize<T>(TraySlot traySlot, T color)
	{
		var meshInstance = GetNode<MeshInstance>("./MeshInstance");
		var colorEnum = (Color)(int)(object)color;
		Color = colorEnum;

		var meshMaterial = meshInstance.Mesh.SurfaceGetMaterial(0);
		if (meshMaterial is SpatialMaterial spatialMaterial)
		{
			// the material must be duplicated for the mesh instance, otherwise the changes will affect all instances of the mesh
			var instanceSpatialMaterial = spatialMaterial.Duplicate() as SpatialMaterial;
			instanceSpatialMaterial.AlbedoColor = ColorTileMaterials.Colors[colorEnum];
			meshInstance.MaterialOverride = instanceSpatialMaterial;
		}

		TraySlot = traySlot;
	}
}
