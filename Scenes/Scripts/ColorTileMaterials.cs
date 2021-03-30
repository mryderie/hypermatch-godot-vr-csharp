using System.Collections.Generic;

public static class ColorTileMaterials
{
	public static Dictionary<Color, Godot.Color> Colors = new Dictionary<Color, Godot.Color>
	{
		{ Color.Color01, new Godot.Color("336600")}, // dark green
		{ Color.Color02, new Godot.Color("33ff00")}, // bright green
		{ Color.Color03, new Godot.Color("8C001A")}, // burgundy
		{ Color.Color04, new Godot.Color("ff3300")}, // red

		{ Color.Color05, new Godot.Color("ffff33")}, // yellow
		{ Color.Color06, new Godot.Color("3300ff")}, // blue
		{ Color.Color07, new Godot.Color("33ffff")}, // cyan
		{ Color.Color08, new Godot.Color("330066")}, // plum

		{ Color.Color09, new Godot.Color("ff33ff")}, // magenta/pink
		{ Color.Color10, new Godot.Color("99ccff")}, // pale blue
		{ Color.Color11, new Godot.Color("9900ff")}, // purple
		{ Color.Color12, new Godot.Color("00cc99")}, // cyan/green

		{ Color.Color13, new Godot.Color("cc6666")}, // dark salmon
		{ Color.Color14, new Godot.Color("cc9900")}, // gold
		{ Color.Color15, new Godot.Color("333333")}, // dark grey
		{ Color.Color16, new Godot.Color("cccccc")}, // light grey
	};
}
