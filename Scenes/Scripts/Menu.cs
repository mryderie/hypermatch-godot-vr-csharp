using Godot;
using System;

public class Menu : Spatial
{
	[Export]
	protected NodePath ViewportPath;
	protected MeshInstance MeshInstance;
	protected Viewport Viewport;

	public override void _Ready()
	{
		// Set the Mesh to use viewport as its textue
		// Note - tried to do this through the GUI, but ended up with a plain black texture
		MeshInstance = GetNode<MeshInstance>("MeshInstance");
		Viewport = GetNode<Viewport>(ViewportPath);

		var viewportMaterial = new SpatialMaterial
		{
			FlagsUnshaded = true,
			AlbedoTexture = Viewport.GetTexture(),
			FlagsTransparent = true,
		};

		MeshInstance.SetSurfaceMaterial(0, viewportMaterial);
	}
}
