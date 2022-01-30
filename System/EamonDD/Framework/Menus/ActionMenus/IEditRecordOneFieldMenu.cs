
// IEditRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	/// <summary></summary>
	public interface IEditRecordOneFieldMenu<T> : IRecordMenu<T> where T : class, IGameBase
	{
		/// <summary></summary>
		T EditRecord { get; set; }

		/// <summary></summary>
		string EditFieldName { get; set; }
	}
}
