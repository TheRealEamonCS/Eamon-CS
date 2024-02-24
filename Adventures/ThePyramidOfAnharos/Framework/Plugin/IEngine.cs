
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace ThePyramidOfAnharos.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		string MapData { get; set; }

		bool TaxLevied { get; set; }

		bool GuardsAttack { get; set; }

		void PrintGuideMonsterDirection();

		void PrintTheGlyphsRead(long effectUid);
	}
}
