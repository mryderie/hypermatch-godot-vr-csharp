using Godot;
using System;

public class TileMatchCountdown : Spatial
{
	protected MeshInstance TimeBar;
	protected Tween ShrinkTween;

	protected Godot.Color StartColor = new Godot.Color("33ff00");
	protected Godot.Color EndColor = new Godot.Color("ff3300");

	public override void _Ready()
	{
		base._Ready();

		TimeBar = GetNode<MeshInstance>("TimeBar");
		ShrinkTween = GetNode<Tween>("ShrinkTween");
	}

	public void Restart(float shrinkTime)
	{
		// reset to full scale
		TimeBar.Scale = new Vector3(1, 1, 1);
		ShrinkTween.RemoveAll();

		// reset color to green
		var meshMaterial = TimeBar.Mesh.SurfaceGetMaterial(0);
		if (meshMaterial is SpatialMaterial spatialMaterial)
		{
			spatialMaterial.AlbedoColor = StartColor;

			// tween color from green to red
			ShrinkTween.InterpolateProperty(spatialMaterial,
										"albedo_color",
										StartColor,
										EndColor,
										shrinkTime);
		}


		// shrink the bar as the time runs out
		ShrinkTween.InterpolateProperty(TimeBar,
										"scale:x",
										1,
										0,
										shrinkTime);
		
		ShrinkTween.Start();
	}
}
