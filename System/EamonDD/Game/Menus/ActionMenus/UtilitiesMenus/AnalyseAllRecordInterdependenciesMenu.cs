
// AnalyseAllRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseAllRecordInterdependenciesMenu : Menu, IAnalyseAllRecordInterdependenciesMenu
	{
		public virtual IList<IAnalyseRecordInterdependenciesMenu01<IGameBase>> AnalyseMenuList { get; set; }

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

				foreach (var menu in AnalyseMenuList)
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

						gEngine.DdSuppressPostInputSleep = true;

						rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

						Debug.Assert(gEngine.IsSuccess(rc));

						gEngine.DdSuppressPostInputSleep = false;

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

			AnalyseMenuList = new List<IAnalyseRecordInterdependenciesMenu01<IGameBase>>();

			if (gDatabase.ArtifactTableType == ArtifactTableType.CharArt)
			{
				AnalyseMenuList.Add(gEngine.CreateInstance<IAnalyseCharacterRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}));
			}

			AnalyseMenuList.Add(gEngine.CreateInstance<IAnalyseArtifactRecordInterdependenciesMenu01>(x =>
			{
				x.SkipFieldNameList = SkipFieldNameList;
				x.ClearSkipFieldNameList = false;
			}));

			if (gDatabase.ArtifactTableType == ArtifactTableType.Default)
			{
				AnalyseMenuList.Add(gEngine.CreateInstance<IAnalyseEffectRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}));

				AnalyseMenuList.Add(gEngine.CreateInstance<IAnalyseHintRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}));

				AnalyseMenuList.Add(gEngine.CreateInstance<IAnalyseModuleRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}));

				AnalyseMenuList.Add(gEngine.CreateInstance<IAnalyseMonsterRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}));

				AnalyseMenuList.Add(gEngine.CreateInstance<IAnalyseRoomRecordInterdependenciesMenu01>(x =>
				{
					x.SkipFieldNameList = SkipFieldNameList;
					x.ClearSkipFieldNameList = false;
				}));
			}
		}
	}
}
