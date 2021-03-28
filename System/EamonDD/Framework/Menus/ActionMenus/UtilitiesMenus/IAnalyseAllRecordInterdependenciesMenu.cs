
// IAnalyseAllRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Menus;

namespace EamonDD.Framework.Menus.ActionMenus
{
	/// <summary></summary>
	public interface IAnalyseAllRecordInterdependenciesMenu : IMenu
	{
		/// <summary></summary>
		IAnalyseRecordInterdependenciesMenu01<IGameBase>[] AnalyseMenus { get; set; }

		/// <summary></summary>
		IList<string> SkipFieldNameList { get; set; }

		/// <summary></summary>
		bool ModifyFlag { get; set; }

		/// <summary></summary>
		bool ExitFlag { get; set; }
	}
}
