
// IAnalyseCharacterRecordTreeMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework.Menus;

namespace EamonDD.Framework.Menus.ActionMenus
{
	/// <summary></summary>
	public interface IAnalyseCharacterRecordTreeMenu : IMenu
	{
		/// <summary></summary>
		IList<string> RecordTreeStringList { get; set; }
	}
}
