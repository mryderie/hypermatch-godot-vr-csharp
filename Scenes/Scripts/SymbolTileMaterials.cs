using System.Collections.Generic;

public static class SymbolTileMaterials
{
	public static Dictionary<Symbol, (float x, float y)> SymbolTextureUvOffset = new Dictionary<Symbol, (float x, float y)>
	{
		{ Symbol.A, (-1.5f, 0)},
		{ Symbol.B, (-1.25f, 0)},
		{ Symbol.C, (-1f, 0)},
		{ Symbol.D, (-0.75f, 0)},

		{ Symbol.E, (-1.5f, 0.25f)},
		{ Symbol.F, (-1.25f, 0.25f)},
		{ Symbol.G, (-1f, 0.25f)},
		{ Symbol.H, (-0.75f, 0.25f)},

		{ Symbol.I, (-1.5f, 0.5f)},
		{ Symbol.J, (-1.25f, 0.5f)},
		{ Symbol.K, (-1f, 0.5f)},
		{ Symbol.L, (-0.75f, 0.5f)},

		{ Symbol.M, (-1.5f, 0.75f)},
		{ Symbol.N, (-1.25f, 0.75f)},
		{ Symbol.O, (-1f, 0.75f)},
		{ Symbol.P, (-0.75f, 0.75f)},
	};
}