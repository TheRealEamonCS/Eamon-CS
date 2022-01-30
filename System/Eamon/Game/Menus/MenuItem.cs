
// MenuItem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Menus;
using Eamon.Game.Attributes;

namespace Eamon.Game.Menus
{
	[ClassMappings]
	public class MenuItem : IMenuItem
	{
		public virtual char SelectChar { get; set; }

		public virtual string LineText { get; set; }

		public virtual IMenu SubMenu { get; set; }

		public MenuItem()
		{
			LineText = "";
		}
	}
}
