
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
		IList<IAnalyseRecordInterdependenciesMenu01<IGameBase>> AnalyseMenuList { get; set; }

		/// <summary></summary>
		IList<string> SkipFieldNameList { get; set; }

		/// <summary></summary>
		bool ModifyFlag { get; set; }

		/// <summary></summary>
		bool ExitFlag { get; set; }
	}
}
