
// IMenuItem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Menus
{
	/// <summary></summary>
	public interface IMenuItem
	{
		/// <summary></summary>
		char SelectChar { get; set; }

		/// <summary></summary>
		string LineText { get; set; }

		/// <summary></summary>
		IMenu SubMenu { get; set; }
	}
}
