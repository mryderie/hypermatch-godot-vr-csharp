using Godot;

public class MatchTile : Spatial
{
	public void Initialize((Color color, Symbol symbol) tileParams)
	{
		// set the color
		var colorMesh = GetNode<MeshInstance>("./ColorMesh");
		var colorMeshMaterial = colorMesh.Mesh.SurfaceGetMaterial(0);
		if (colorMeshMaterial is SpatialMaterial colorMeshSpatialMaterial)
		{
			// the material must be duplicated for the mesh instance, otherwise the changes will affect all instances of the mesh
			var instanceSpatialMaterial = colorMeshSpatialMaterial.Duplicate() as SpatialMaterial;
			instanceSpatialMaterial.AlbedoColor = ColorTileMaterials.Colors[tileParams.color];
			colorMesh.MaterialOverride = instanceSpatialMaterial;
		}

		// set the symbol
		var symbolMesh = GetNode<MeshInstance>("./SymbolMesh");
		var symbolMeshMaterial = symbolMesh.Mesh.SurfaceGetMaterial(0);
		if (symbolMeshMaterial is SpatialMaterial symbolMeshSpatialMaterial)
		{
			// the material must be duplicated for the mesh instance, otherwise the changes will affect all instances of the mesh
			var instanceSpatialMaterial = symbolMeshSpatialMaterial.Duplicate() as SpatialMaterial;
			// set the UV1 offset so that the Tile displays the correct portion of the texture
			var uvOffset = SymbolTileMaterials.SymbolTextureUvOffset[tileParams.symbol];
			instanceSpatialMaterial.Uv1Offset = new Vector3(uvOffset.x, uvOffset.y, 0);
			symbolMesh.MaterialOverride = instanceSpatialMaterial;
		}
	}
}
