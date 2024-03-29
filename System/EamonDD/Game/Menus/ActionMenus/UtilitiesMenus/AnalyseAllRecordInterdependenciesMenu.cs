﻿
// AnalyseAllRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseAllRecordInterdependenciesMenu : Menu, IAnalyseAllRecordInterdependenciesMenu
	{
		public virtual IAnalyseRecordInterdependenciesMenu01<IGameBase>[] AnalyseMenus { get; set; }

		public virtual IList<string> SkipFieldNameList { get; set; }

		public virtual bool ModifyFlag { get; set; }

		public virtual bool ExitFlag { get; set; }

		public override void Execute()
		{
			RetCode rc;

			SkipFieldNameList.Clear();

			ExitFlag = false;

			while (true)
			{
				ModifyFlag = false;

				foreach (var menu in AnalyseMenus)
				{
					menu.Execute();

					if (menu.ExitFlag)
					{
						ExitFlag = true;
					}

					if (!ExitFlag)
					{
						if (menu.ModifyFlag)
						{
							ModifyFlag = true;
						}

						gOut.Print("{0}", gEngine.LineSep);

						gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

						Buf.Clear();

						rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

						Debug.Assert(gEngine.IsSuccess(rc));

						if (Buf.Length > 0 && Buf[0] == 'X')
						{
							ExitFlag = true;
						}
					}

					if (ExitFlag)
					{
						goto ExitLoop;
					}
				}

				if (!ModifyFlag)
				{
					goto ExitLoop;
				}
			}

		ExitLoop:

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Print("Done analysing all record interdependencies.");
		}

		public AnalyseAllRecordInterdependenciesMenu()
		{
			Buf = gEngine.Buf;

			SkipFieldNameList = new List<string>();

			AnalyseMenus = new IAnalyseRecordInterdependenciesMenu01<IGameBase>[]
			{
				gEngine.CreateInstance<IAnalyseArtifactRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}),
				gEngine.CreateInstance<IAnalyseEffectRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}),
				gEngine.CreateInstance<IAnalyseHintRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}),
				gEngine.CreateInstance<IAnalyseModuleRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}),
				gEngine.CreateInstance<IAnalyseMonsterRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}),
				gEngine.CreateInstance<IAnalyseRoomRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				})
			};

		}
	}
}
