using Godot;
using System;

public class ScoreBoard : Spatial
{
	[Export]
	protected NodePath ViewportPath;
	protected MeshInstance MeshInstance;
	protected Viewport Viewport;

	protected Label TimeValueLabel;
	protected Label ScoreValueLabel;
	protected Label LevelValueLabel;

	protected Godot.Color TimeLeftColor = new Godot.Color("ffffff");
	protected Godot.Color TimeUpColor = new Godot.Color("ff3300");

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


		TimeValueLabel = GetNode<Label>("./Viewport/Control/VBoxContainer/TimeValue");
		ScoreValueLabel = GetNode<Label>("./Viewport/Control/VBoxContainer/ScoreValue");
		LevelValueLabel = GetNode<Label>("./Viewport/Control/VBoxContainer/LevelValue");
	}
	public void SetTime(float time)
	{
		TimeValueLabel.Set("custom_colors/font_color", time == 0 ? TimeUpColor : TimeLeftColor);
		TimeValueLabel.Text = $"{time:00.00}";
	}

	public void SetScore(uint score)
	{
		ScoreValueLabel.Text = $"{score:N0}";
	}

	internal void SetLevel(uint level)
	{
		LevelValueLabel.Text = $"Level {level}";
	}
}
