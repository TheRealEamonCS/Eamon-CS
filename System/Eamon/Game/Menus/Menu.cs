
// Menu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Framework.Menus;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.Menus
{
	public abstract class Menu : IMenu
	{
		public virtual string Title { get; set; }

		public virtual StringBuilder Buf { get; set; }

		public virtual IList<IMenuItem> MenuItemList { get; set; }

		public virtual bool IsCharMenuItem(char ch)
		{
			return MenuItemList != null && MenuItemList.FirstOrDefault(mi => mi.SelectChar == ch) != null;
		}

		public virtual void PrintSubtitle()
		{

		}

		public virtual bool ShouldBreakMenuLoop()
		{
			return false;
		}

		public virtual void Startup()
		{

		}

		public virtual void Shutdown()
		{

		}

		public virtual void Execute()
		{
			RetCode rc;
			long i;

			Startup();

			while (true)
			{
				gOut.WriteLine();

				if (! string.IsNullOrWhiteSpace(Title))
				{
					gEngine.PrintTitle(Title, true);
				}

				PrintSubtitle();

				for (i = 0; i < MenuItemList.Count; i++)
				{
					gOut.Write("{0}", MenuItemList[(int)i].LineText);
				}

				gOut.Write("{0}{1}[{2}X]: ", Environment.NewLine, gEngine.EnableScreenReaderMode ? "Your choice " : "", gEngine.EnableScreenReaderMode ? "Default " : "");

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, "X", gEngine.ModifyCharToUpper, IsCharMenuItem, IsCharMenuItem);

				Debug.Assert(gEngine.IsSuccess(rc));

				var menuItem = MenuItemList.FirstOrDefault(mi => mi.SelectChar == Buf[0]);

				Debug.Assert(menuItem != null);

				if (menuItem.SubMenu == null)
				{
					break;
				}

				menuItem.SubMenu.Execute();

				if (ShouldBreakMenuLoop())
				{
					break;
				}
			}

			Shutdown();
		}

		public Menu()
		{

		}
	}
}
