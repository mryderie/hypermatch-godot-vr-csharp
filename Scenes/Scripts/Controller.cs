using Godot;
using System;

public class Controller : ARVRController
{
	protected Game Game;
	protected Area CollisionArea;
	protected Tile HeldTile;
	protected Spatial GrabPosition;
	protected Camera PlayerCamera;
	protected Tween RumbleTween;
	protected bool PlayInProgress = false;

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");
		CollisionArea = GetNode<Area>("CollisionArea");
		GrabPosition = GetNode<Spatial>("GrabPosition");
		PlayerCamera = GetNode<Camera>("../PlayerCamera");
		RumbleTween = GetNode<Tween>("RumbleTween");
	}

	public void Initialize()
	{
		PlayInProgress = true;
		HeldTile = null;
		Visible = true;
	}

	public void GameOver()
	{
		PlayInProgress = false;
	}

	private void OnControllerButtonPressed(int buttonIndex)
	{
		switch(buttonIndex)
		{
			case 1:
				Game.Quit();
				break;

			case 7:
				Game.StartGame();
				break;
			
			case 2:
				OnButtonPressedGrab();
				break;
		}
	}

	private void OnControllerButtonRelease(int buttonIndex)
	{
		if (buttonIndex == 2)
			OnButtonReleasedGrab();
	}

	private void OnButtonPressedGrab()
	{
		if (!PlayInProgress)
			return;

		if (HeldTile == null)
		{
			var bodies = CollisionArea.GetOverlappingBodies();
			foreach (var body in bodies)
			{
				if (body == this)
					continue;

				if (body is Tile tile
					&& !tile.IsHeld())
				{
					HeldTile = tile;
					HeldTile.Hold(this);
					Visible = false;

					RumbleTween.InterpolateProperty(this,
										"rumble",
										0.3f,
										0,
										0.25f);

					RumbleTween.Start();
				}
			}
		}
	}

	private void OnButtonReleasedGrab()
	{
		if (HeldTile != null)
		{
			HeldTile.Release();
			HeldTile = null;
			Visible = true;
		}
	}
}
