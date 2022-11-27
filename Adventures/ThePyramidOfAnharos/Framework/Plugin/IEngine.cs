
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace ThePyramidOfAnharos.Framework.Plugin
{
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		void PrintGuideMonsterDirection();

		void PrintTheGlyphsRead(long effectUid);
	}
}
