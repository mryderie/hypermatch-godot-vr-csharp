using Godot;
using System;

public class GameOverMenu : Menu
{
	protected Label ScoreValue;
	protected Label TopScoreValue;
	public override void _Ready()
	{
		base._Ready();
		
		ScoreValue = GetNode<Label>("./Viewport/Control/VBoxContainer/ScoreValue");
		TopScoreValue = GetNode<Label>("./Viewport/Control/VBoxContainer/TopScoreValue");
	}
	public void SetScore(uint score)
	{
		ScoreValue.Text = $"Score {score}";
	}

	public void SetTopScore(uint score)
	{
		TopScoreValue.Text = $"Top Score {score}";
	}
}
