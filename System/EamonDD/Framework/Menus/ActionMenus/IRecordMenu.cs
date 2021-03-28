
// IRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Menus;

namespace EamonDD.Framework.Menus.ActionMenus
{
	/// <summary></summary>
	public interface IRecordMenu<T> : IMenu where T : class, IGameBase
	{
		/// <summary></summary>
		IDbTable<T> RecordTable { get; set; }

		/// <summary></summary>
		string RecordTypeName { get; set; }

		/// <summary></summary>
		void PrintPostListLineSep();

		/// <summary></summary>
		void UpdateGlobals();
	}
}
