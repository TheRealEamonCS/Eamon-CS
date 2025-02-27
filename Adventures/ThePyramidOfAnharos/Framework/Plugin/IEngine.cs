
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;

namespace ThePyramidOfAnharos.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		/// <summary></summary>
		new StringBuilder Buf01 { get; set; }

		string MapData { get; set; }

		bool TaxLevied { get; set; }

		bool GuardsAttack { get; set; }

		void PrintGuideMonsterDirection();

		void PrintTheGlyphsRead(long effectUid);
	}
}
