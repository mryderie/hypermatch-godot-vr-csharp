using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MatchTileStream : Spatial
{
	protected RandomNumberGenerator Rng = new RandomNumberGenerator();

	protected const int VISIBLE_MATCHES = 3;
	protected static readonly int MaxMatchQueueLength = TileTray.SlotCount;
	protected static readonly int StartingSlotCount = TileTray.StartingSlotCount;

	protected PackedScene MatchTileScene = ResourceLoader.Load<PackedScene>("res://Scenes/MatchTile.tscn");

	protected ColorTileSlot ColorTileSlot;
	protected SymbolTileSlot SymbolTileSlot;

	protected ColorTileTray ColorTray;
	protected SymbolTileTray SymbolTray;

	protected Queue<MatchTile> VisibleMatchQueue = new Queue<MatchTile>(VISIBLE_MATCHES);
	protected Queue<(Color color, Symbol symbol)> MatchQueue = new Queue<(Color, Symbol)>(MaxMatchQueueLength);
	protected Tween Tween;

	protected const float MatchTileSpacing = 0.3f;
	public bool IsMoving { get { return Tween.IsActive(); } }
	public override void _Ready()
	{
		base._Ready();

		Rng.Seed = (ulong)DateTime.UtcNow.Ticks;

		ColorTray = GetNode<ColorTileTray>("ColorTray");
		SymbolTray = GetNode<SymbolTileTray>("SymbolTray");

		ColorTileSlot = GetNode<ColorTileSlot>("ColorTileSlot");
		SymbolTileSlot = GetNode<SymbolTileSlot>("SymbolTileSlot");

		Tween = GetNode<Tween>("Tween");
	}

	public void Initialize()
	{
		// clear the slots
		ColorTileSlot.Initialize();
		SymbolTileSlot.Initialize();

		// clear the trays
		ColorTray.Initialize();
		SymbolTray.Initialize();

		// clear the match stream
		MatchQueue.Clear();
		foreach(var matchTile in VisibleMatchQueue)
		{
			matchTile.QueueFree();
		}
		VisibleMatchQueue.Clear();

		// stop any Tweens
		Tween.RemoveAll();

		for (var i = 0; i < StartingSlotCount; i++)
		{
			AddNewTile();
			UpdateMatchQueue();
		}
	}

	protected MatchTile AddMatchTileVisibleInstance((Color color, Symbol symbol) tileParams, int queuePosition)
	{
		var matchTileInstance = MatchTileScene.Instance() as MatchTile;
		AddChild(matchTileInstance);

		matchTileInstance.Initialize(tileParams);

		// move match tile to correct position
		matchTileInstance.Translation = new Vector3(0, (queuePosition * MatchTileSpacing), 0);

		return matchTileInstance;
	}

	public bool CurrentTileMatch()
	{
		var currentMatchTile = MatchQueue.Peek();
		
		var slotColor = ColorTileSlot.Color;
		var slotSymbol = SymbolTileSlot.Symbol;

		var isMatch = slotColor != null
						&& slotSymbol != null
						&& slotColor.Value == currentMatchTile.color
						&& slotSymbol.Value == currentMatchTile.symbol;

		if (isMatch)
		{
			// remove the matched tile from the queue
			MatchQueue.Dequeue();
			var matchedTile = VisibleMatchQueue.Dequeue();
			matchedTile.QueueFree();

			// free the Tile Slots
			ColorTileSlot.Matched();
			SymbolTileSlot.Matched();

			AddNewTile();

			// Slide the remaing Match Tiles downward
			MoveTileStream();
		}

		return isMatch;
	}

	protected void MoveTileStream()
	{
		foreach (var matchTile in VisibleMatchQueue.ToArray())
		{

			Tween.InterpolateProperty(matchTile,
										"translation:y",
										matchTile.Translation.y,
										matchTile.Translation.y - MatchTileSpacing,
										0.25f);
		}
		Tween.Start();
	}

	protected void OnStreamMovementCompleted()
	{
		UpdateMatchQueue();
	}

	public void Timeup()
	{
		// Move the current tile to the back of the queue
		MatchQueue.Enqueue(MatchQueue.Dequeue());

		// Remove the current visible Tile from view
		var matchedTile = VisibleMatchQueue.Dequeue();
		matchedTile.QueueFree();

		AddNewTile();

		// Slide the remaing Match Tiles downward
		MoveTileStream();
	}

	protected void AddNewTile()
	{
		if (MatchQueue.Count < MaxMatchQueueLength)
		{
			var newTileParams = CreateNewTile();
			MatchQueue.Enqueue(newTileParams);

			AddTileToTrays(newTileParams);
		}
	}

	protected void UpdateMatchQueue()
	{
		if (VisibleMatchQueue.Count < VISIBLE_MATCHES)
		{
			var tileToShow = MatchQueue.ElementAt(VisibleMatchQueue.Count);
			VisibleMatchQueue.Enqueue(AddMatchTileVisibleInstance(tileToShow, VisibleMatchQueue.Count));
		}
	}

	protected (Color color, Symbol symbol) CreateNewTile()
	{
		var color = (Color)Rng.RandiRange((int)Color.Color01, (int)Color.Color16);
		var symbol = (Symbol)Rng.RandiRange((int)Symbol.A, (int)Symbol.P);
		return (color, symbol);
	}

	protected void AddTileToTrays((Color color, Symbol symbol) newTileParams)
	{
		ColorTray.AddTile<Color, ColorTile>(newTileParams.color);
		SymbolTray.AddTile<Symbol, SymbolTile>(newTileParams.symbol);
	}
}



