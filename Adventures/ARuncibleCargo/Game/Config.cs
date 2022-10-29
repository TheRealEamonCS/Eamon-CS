
// Config.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static ARuncibleCargo.Game.Plugin.Globals;

namespace ARuncibleCargo.Game
{
	[ClassMappings]
	public class Config : Eamon.Game.Config, IConfig
	{
		public override RetCode DeleteGameState(string configFileName, bool startOver)
		{
			RetCode rc;

			try
			{
				gEngine.File.Delete(gEngine.SnapshotFileName);
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}

			rc = base.DeleteGameState(configFileName, startOver);

			return rc;
		}
	}
}
