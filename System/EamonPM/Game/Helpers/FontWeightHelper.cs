
// FontWeightHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Avalonia.Media;

namespace EamonPM.Game.Helpers
{
	public static class FontWeightHelper
	{
		public static FontWeight FromString(string fontWeight)
		{
			return fontWeight.ToLower() switch
			{
				"normal" => FontWeight.Normal,
				"bold" => FontWeight.Bold,
				"black" => FontWeight.Black,
				_ => throw new ArgumentException($"Unknown font weight: {fontWeight}")
			};
		}
	}
}
