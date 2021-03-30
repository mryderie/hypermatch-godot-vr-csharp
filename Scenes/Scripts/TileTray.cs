using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class TileTray : Spatial
{
	protected RandomNumberGenerator Rng = new RandomNumberGenerator();
	protected PackedScene TraySlotScene = ResourceLoader.Load<PackedScene>("res://Scenes/TraySlot.tscn");

	protected static (float x, float y, bool starting)[] SlotDefs = new (float x, float y, bool starting) []
	{
		// top row
		(-0.125f, 0.43302f, false),
		(0.125f, 0.43302f, false),

		// upper middle row
		(-0.25f, 0.21651f, false),
		(0, 0.21651f, true),
		(0.25f,0.21651f, true),

		// middle row
		(-0.375f, 0, false),
		(-0.125f, 0, true),
		(0.125f, 0, true),

		// lower middle row
		(-0.25f, -0.21651f, false),
		(0, -0.21651f, true),
		(0.25f, -0.21651f, true),

		// bottom row
		(-0.125f, -0.43302f, false),
		(0.125f, -0.43302f, false),
	};
	
	public static int StartingSlotCount { get { return SlotDefs.Count(sd => sd.starting); } }
	public static int SlotCount { get { return SlotDefs.Count(); } }

	protected List<TraySlot> Slots = new List<TraySlot>();

	protected abstract T CreateTile<T>() where T : Tile;

	public override void _Ready()
	{
		Rng.Seed = (ulong)DateTime.UtcNow.Ticks;

		foreach(var slotDef in SlotDefs)
		{
			var traySlot = TraySlotScene.Instance() as TraySlot;
			traySlot.Initialize(slotDef.starting);
			traySlot.Translation = new Vector3(slotDef.x, slotDef.y, 0);

			Slots.Add(traySlot);
			AddChild(traySlot);
		}
	}

	public void Initialize()
	{
		foreach (var slot in Slots)
		{
			slot.Initialize();
		}
	}

	protected TraySlot GetEmptyTraySlot()
	{
		// always fill the starting slots before moving to the outer slots
		var emptyStartingSlots = Slots.Where(s => s.IsEmpty && s.StartingSlot).ToArray();
		if (emptyStartingSlots.Any())
		{
			return emptyStartingSlots[Rng.RandiRange(0, emptyStartingSlots.Length - 1)];
		}

		var emptyNonStartingSlots = Slots.Where(s => s.IsEmpty && !s.StartingSlot).ToArray();
		if (emptyNonStartingSlots.Any())
		{
			return emptyNonStartingSlots[Rng.RandiRange(0, emptyNonStartingSlots.Length - 1)];
		}

		return null;
	}

	public void AddTile<T,U>(T tileType)
		where T : Enum
		where U : Tile
	{
		var traySlot = GetEmptyTraySlot();

		if (traySlot == null)
		{
			// shouldn't happen!
			GD.PrintErr($"Too many Tiles added to TileTray");
			return;
		}

		var newTile = CreateTile<U>();
		newTile.Initialize<T>(traySlot, tileType);

		traySlot.AddChild(newTile);
		traySlot.Occupy(newTile);
	}
}
