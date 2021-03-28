
// MhMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonMH.Framework.Menus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus
{
	[ClassMappings]
	public class MhMenu : IMhMenu
	{
		public virtual void PrintMainHallMenuSubtitle()
		{
			gOut.Print("Character: {0}", Globals.Character != null ? Globals.Character.Name : gEngine.UnknownName);

			gOut.Print("As you wander about the hall, you realize you can do one of seven things:");
		}

		public virtual void PrintVillageMenuSubtitle()
		{
			gOut.Print("Character: {0}", Globals.Character != null ? Globals.Character.Name : gEngine.UnknownName);

			gOut.Print("As you wander about the village, you realize you can do one of seven things:");
		}

		public virtual void PrintPracticeAreaMenuSubtitle()
		{
			gOut.Print("Character: {0}", Globals.Character != null ? Globals.Character.Name : gEngine.UnknownName);

			gOut.Print("As you wander about the alleys, you realize you can do one of six things:");
		}
	}
}
