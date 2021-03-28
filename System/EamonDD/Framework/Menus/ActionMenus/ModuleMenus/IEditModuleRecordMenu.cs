
// IEditModuleRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Menus;

namespace EamonDD.Framework.Menus.ActionMenus
{
	/// <summary></summary>
	public interface IEditModuleRecordMenu : IMenu
	{
		/// <summary></summary>
		IModule EditRecord { get; set; }
	}
}
