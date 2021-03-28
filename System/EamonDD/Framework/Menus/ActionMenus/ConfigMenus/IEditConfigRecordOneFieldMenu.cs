
// IEditConfigRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonDD.Framework.Menus.ActionMenus
{
	/// <summary></summary>
	public interface IEditConfigRecordOneFieldMenu : IEditConfigRecordMenu
	{
		/// <summary></summary>
		string EditFieldName { get; set; }
	}
}
