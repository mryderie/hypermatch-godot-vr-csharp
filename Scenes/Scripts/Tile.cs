using Godot;
using System;

public abstract class Tile : StaticBody
{
	protected Camera playerCamera;
	protected TileSlot TileSlot;
	protected Controller Controller { get; set; }
	protected TraySlot TraySlot { get; set; }

	public override void _Ready()
	{
		base._Ready();

		playerCamera = GetNode<Camera>("/root/Game/ARVROrigin/PlayerCamera");
	}

	public override void _PhysicsProcess(float delta)
	{
		// TileSlot could be null if the Tile has just been matched, but not yet freed
		if (TileSlot != null
			 && !TileSlot.ContainsTile(this)) // i.e. if Tile is being Held, or is in Tray
		{
			var tileOrigin = IsHeld() ? Controller.GlobalTransform.origin : GlobalTransform.origin;

			LookAtFromPosition(tileOrigin,
								playerCamera.GlobalTransform.origin,
								new Vector3(0, 1, 0));
		}
	}

	public abstract void Initialize<T>(TraySlot traySlot, T tileType) where T : System.Enum;

	public bool IsHeld()
	{
		return Controller != null;
	}

	public void Hold(Controller controller)
	{
		Controller = controller;

		if(TileSlot.ContainsTile(this))
			TileSlot.Vacate();
	}

	public void Release()
	{
		Controller = null;

		// check if it's been dropped in the Tile Slot, if so, "snap" into centre
		var bodies = TileSlot.SlotCollisionArea.GetOverlappingBodies();
		foreach (var body in bodies)
		{
			if (body == this)
			{
				// move any the currently occupying Tile back to the Tray
				TileSlot.EjectTile();

				// Set the Tile transform to match the Slot (this "snaps" the Tile into place)
				GlobalTransform = TileSlot.GlobalTransform;
				TileSlot.Occupy(this);

				return;
			}
		}

		// if we reach this point, the Tile hasn't been placed in the slot, so it should return to the Tray
		GlobalTransform = TraySlot.GlobalTransform;
		if (TileSlot.ContainsTile(this))
			TileSlot.Vacate();
	}

	public void ReturnToTraySlot()
	{
		GlobalTransform = TraySlot.GlobalTransform;
	}

	public void Matched()
	{
		TraySlot.Matched();
		TraySlot = null;
		TileSlot = null;
		
		QueueFree();
	}
}
