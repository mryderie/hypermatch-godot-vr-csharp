using Godot;
using System.Collections.Generic;

public class SymbolTile : Tile
{
	public Symbol Symbol { get; protected set; }

	public override void _Ready()
	{
		base._Ready();

		TileSlot = GetNode<SymbolTileSlot>("/root/Game/MatchTileStream/SymbolTileSlot");
	}

	public override void Initialize<T>(TraySlot traySlot, T symbol)
	{
		var meshInstance = GetNode<MeshInstance>("./MeshInstance");
		var symbolEnum = (Symbol)(int)(object)symbol;
		Symbol = symbolEnum;

		var meshMaterial = meshInstance.Mesh.SurfaceGetMaterial(0);
		if (meshMaterial is SpatialMaterial spatialMaterial)
		{
			// the material must be duplicated for the mesh instance, otherwise the changes will affect all instances of the mesh
			var instanceSpatialMaterial = spatialMaterial.Duplicate() as SpatialMaterial;

			// set the UV1 offset so that the Tile displays the correct portion of the texture
			var uvOffset = SymbolTileMaterials.SymbolTextureUvOffset[symbolEnum];
			instanceSpatialMaterial.Uv1Offset = new Vector3(uvOffset.x, uvOffset.y, 0);
			meshInstance.MaterialOverride = instanceSpatialMaterial;
		}

		TraySlot = traySlot;
	}
}
