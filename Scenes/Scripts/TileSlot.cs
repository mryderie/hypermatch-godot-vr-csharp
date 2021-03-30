using Godot;
using System;

public abstract class TileSlot : Spatial
{
	public Area SlotCollisionArea { get; set; }
	public AudioStreamPlayer3D SlotOccupySound { get; set; }
	protected Tile Tile { get; set; }

	public bool IsEmpty
	{
		get
		{
			return Tile == null;
		}
	}

	public override void _Ready()
	{
		base._Ready();

		SlotCollisionArea = GetNode<Area>("CollisionArea");
		SlotOccupySound = GetNode<AudioStreamPlayer3D>("SlotOccupySound");
	}

	public void Initialize()
	{
		Tile = null;
	}

	public void Occupy(Tile tile)
	{
		Tile = tile;
		SlotOccupySound.Play();
	}

	/// <summary>
	/// Call when the player has successfully matched the Color and Symbol, so that the Slot can be cleared for the next Tile.
	/// </summary>
	public void Matched()
	{
		Tile.Matched();
		Tile = null;
	}

	/// <summary>
	/// Call when the player picks up the Tile in the Slot and releases it.
	/// </summary>
	public void Vacate()
	{
		Tile = null;
	}

	/// <summary>
	/// Used when the player has picked up the Tile from the Slot, and released it back into the Slot
	/// </summary>
	/// <param name="tile">Indicates if the Tile in the Slot is the same as the Tile passed in.</param>
	/// <returns></returns>
	public bool ContainsTile(Tile tile)
	{
		return Tile == tile;
	}

	public void EjectTile()
	{
		if (Tile != null)
		{
			Tile.ReturnToTraySlot();
			Tile = null;
		}
	}
}
