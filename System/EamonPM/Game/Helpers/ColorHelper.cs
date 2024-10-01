
// ColorHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Avalonia.Media;

namespace EamonPM.Game.Helpers
{
	public static class ColorHelper
	{
		public static string ToHexString(Color color)
		{
			return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
		}

		public static Color FromHexString(string hex, Color defaultColor)
		{
			Color color;

			if (string.IsNullOrWhiteSpace(hex) || !Color.TryParse(hex, out color))
			{
				color = defaultColor;
			}

			return color;
		}
	}
}
