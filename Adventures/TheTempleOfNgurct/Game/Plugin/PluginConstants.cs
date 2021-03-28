
// PluginConstants.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace TheTempleOfNgurct.Game.Plugin
{
	public class PluginConstants : EamonRT.Game.Plugin.PluginConstants, Framework.Plugin.IPluginConstants
	{
		public PluginConstants()
		{
			ToughDesc = string.Format("Monsters usually fall into one of the following categories, but it is possible to create hybrids that are weak in some areas and strong in others:{0}{0}Weak Monsters - wimps and small creatures like rats, kobolds, etc.{0}Medium Monsters - petty thugs, orcs, goblins, etc.{0}Tough Monsters - giants, trolls, highly skilled warriors, etc.{0}Exceptional Monsters - dragons, demons, special villians, etc.", Environment.NewLine);

			CourageDesc = string.Format("Courage works as follows:{0}{0}1-100% - the chance the Monster won't flee combat and/or follow a fleeing player (if enemy).", Environment.NewLine);
		}
	}
}
