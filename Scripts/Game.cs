using Godot;
using System;

public class Game : Node
{
	protected const float LEVEL_START_TIME = 10f;
	protected const float TIME_LIMIT_REDUCTION_RATE = 0.1f;
	protected const float INITIAL_MATCH_TIME_LIMIT = 4f;
	protected float MatchTimeLimit = INITIAL_MATCH_TIME_LIMIT;
	protected float TimeRemaining = LEVEL_START_TIME;

	protected Controller LeftController;
	protected Controller RightController;
	protected MatchTileStream MatchTileStream;
	protected Timer TileMatchTimer;
	protected TileMatchCountdown TileMatchCountdown;
	protected ScoreBoard ScoreBoard;
	protected Menu StartMenu;
	protected GameOverMenu GameOverMenu;

	public AudioStreamPlayer3D MatchSuccessSound { get; set; }
	public AudioStreamPlayer3D MatchFailSound { get; set; }
	public AudioStreamPlayer3D NextLevelSound { get; set; }

	protected bool PlayInProgress = false;
	protected bool AdvancingToNextMatchTile = false;
	protected uint Score = 0;
	protected uint Level = 1;

	//used to stop the timer being updated on every frame (too flickery)
	protected const int UPDATE_COUNTDOWN_EVERY_X_FRAME = 8;
	protected uint Framecount = 0;
	public override void _Ready()
	{
		base._Ready();

		var vr = ARVRServer.FindInterface("OpenVR");
		if (vr != null && vr.Initialize())
		{
			GetViewport().Arvr = true;
			GetViewport().Hdr = false;

			OS.VsyncEnabled = false;
			Engine.IterationsPerSecond = 90;
		}

		LeftController = GetNode<Controller>("./ARVROrigin/LeftController");
		RightController = GetNode<Controller>("./ARVROrigin/RightController");
		MatchTileStream = GetNode<MatchTileStream>("MatchTileStream");
		TileMatchTimer = GetNode<Timer>("TileMatchTimer");
		TileMatchCountdown = GetNode<TileMatchCountdown>("TileMatchCountdown");
		ScoreBoard = GetNode<ScoreBoard>("ScoreBoard");
		StartMenu = GetNode<Menu>("StartMenu");
		GameOverMenu = GetNode<GameOverMenu>("GameOverMenu");

		MatchSuccessSound = GetNode<AudioStreamPlayer3D>("MatchTileStream/MatchSuccessSound");
		MatchFailSound = GetNode<AudioStreamPlayer3D>("MatchTileStream/MatchFailSound");
		NextLevelSound = GetNode<AudioStreamPlayer3D>("MatchTileStream/NextLevelSound");
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		if (PlayInProgress)
		{
			TimeRemaining -= delta;

			if (TimeRemaining <= 0)
			{
				// out of time, but game doesn't end until final match timer runs out, giving the player a chance to get back in
				TimeRemaining = 0;
			}

			// update score board countdown
			Framecount = (Framecount + 1) % UPDATE_COUNTDOWN_EVERY_X_FRAME;
			if (Framecount == 0)
				ScoreBoard.SetTime(TimeRemaining);
			
			if (AdvancingToNextMatchTile)
			{
				AdvancingToNextMatchTile = MatchTileStream.IsMoving;
			}
			else
			{
				if (MatchTileStream.CurrentTileMatch())
				{
					TimeRemaining += TileMatchTimer.TimeLeft; //add the remaining time for the Tile to the overall time left
					TileMatchTimer.Start(MatchTimeLimit); // restarts the timer
					TileMatchCountdown.Restart(MatchTimeLimit);
					AdvancingToNextMatchTile = true;
					Score++;
					ScoreBoard.SetScore(Score);
					
					if(Score % 10 == 0)
					{
						NextLevelSound.Play();

						Level++;
						ScoreBoard.SetLevel(Level);
						MatchTimeLimit = MatchTimeLimit - (MatchTimeLimit * TIME_LIMIT_REDUCTION_RATE);
						TimeRemaining += LEVEL_START_TIME;
						ScoreBoard.SetTime(TimeRemaining);
					}
					else
					{
						MatchSuccessSound.Play();
					}
				}
			}
		}
	}

	protected void OnTileMatchTimerTimeout()
	{
		if (TimeRemaining <= 0)
		{
			GameOver();
		}
		else if (PlayInProgress && !AdvancingToNextMatchTile)
		{
			AdvancingToNextMatchTile = true;
			MatchFailSound.Play();
			MatchTileStream.Timeup();
			TileMatchCountdown.Restart(MatchTimeLimit);
		}
	}

	public void StartGame()
	{
		if (PlayInProgress)
			return;

		GameOverMenu.Hide();
		StartMenu.Hide();

		TimeRemaining = LEVEL_START_TIME;
		MatchTimeLimit = INITIAL_MATCH_TIME_LIMIT;
		Score = 0;
		Level = 1;		
		ScoreBoard.SetScore(Score);
		ScoreBoard.SetLevel(Level);

		AdvancingToNextMatchTile = false;

		LeftController.Initialize();
		RightController.Initialize();

		MatchTileStream.Initialize();
		TileMatchTimer.Start(MatchTimeLimit);
		TileMatchCountdown.Restart(MatchTimeLimit);
		PlayInProgress = true;
	}

	protected void GameOver()
	{
		PlayInProgress = false;
		LeftController.GameOver();
		RightController.GameOver();

		var topScore = RefreshTopScore(Score);
		
		GameOverMenu.SetTopScore(topScore);
		GameOverMenu.SetScore(Score);
		GameOverMenu.Show();
	}

	protected uint RefreshTopScore(uint score)
	{
		uint topScore;
		var scoresFilePath = "user://scores.data";
		
		var scoresFile = new File();
		if(scoresFile.FileExists(scoresFilePath))
		{
			// file exists
			scoresFile.Open(scoresFilePath, File.ModeFlags.ReadWrite);
			topScore = scoresFile.Get32();

			if(score > topScore)
			{
				scoresFile.Seek(0);
				scoresFile.Store32(score);
			}

			topScore = Math.Max(score, topScore);
		}
		else
		{
			// file does not exist
			scoresFile.Open(scoresFilePath, File.ModeFlags.Write);
			scoresFile.Store32(score);
			topScore = score;
		}
		scoresFile.Close();

		return topScore;
	}

	public void Quit()
	{
		if (PlayInProgress)
			return;

		GetTree().Quit();
	}
}
