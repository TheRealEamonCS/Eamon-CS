
// IMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Text;

namespace Eamon.Framework.Menus
{
	/// <summary></summary>
	public interface IMenu
	{
		/// <summary></summary>
		string Title { get; set; }

		/// <summary></summary>
		StringBuilder Buf { get; set; }

		/// <summary></summary>
		IList<IMenuItem> MenuItemList { get; set; }

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharMenuItem(char ch);

		/// <summary></summary>
		void PrintSubtitle();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldBreakMenuLoop();

		/// <summary></summary>
		void Startup();

		/// <summary></summary>
		void Shutdown();

		/// <summary></summary>
		void Execute();
	}
}
