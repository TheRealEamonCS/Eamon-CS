
// RecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class RecordMenu<T> : Menu, IRecordMenu<T> where T : class, IGameBase
	{
		public virtual IDbTable<T> RecordTable { get; set; }

		public virtual string RecordTypeName { get; set; }

		public virtual void PrintPostListLineSep()
		{
			gOut.WriteLine();

			gOut.Print("{0}", Globals.LineSep);
		}

		public virtual void UpdateGlobals()
		{

		}

		public RecordMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
