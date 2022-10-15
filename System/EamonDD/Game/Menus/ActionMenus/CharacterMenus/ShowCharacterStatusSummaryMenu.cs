
// ShowCharacterStatusSummaryMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ShowCharacterStatusSummaryMenu : Menu, IShowCharacterStatusSummaryMenu
	{
		public class AdventuringCharacter
		{
			public virtual IModule Module { get; set; }

			public virtual ICharacter Character { get; set; }
		}

		public virtual void PrintPostShowLineSep()
		{
			gOut.WriteLine();

			gOut.Print("{0}", Globals.LineSep);
		}

		public override void Execute()
		{
			RetCode rc;

			var nlFlag = false;

			gOut.WriteLine();

			gEngine.PrintTitle("SHOW CHARACTER STATUS SUMMARY", true);

			var advCharList = new List<AdventuringCharacter>();

			var adventureDirs = Globals.Directory.GetDirectories(Constants.AdventuresDir);

			var j = (long)adventureDirs.Length;

			var i = 0;

			while (i < j)
			{
				var chrfn = Globals.Path.Combine(adventureDirs[i], "FRESHMEAT.DAT");

				if (Globals.File.Exists(chrfn))
				{
					try
					{
						var fileName = Globals.Path.GetFullPath(@".\" + Globals.Path.GetFileNameWithoutExtension(adventureDirs[i]) + ".dll");

						if (Globals.File.Exists(fileName))
						{
							Assembly.LoadFrom(fileName);
						}
					}
					catch (Exception)
					{
						// do nothing
					}

					var modfn = Globals.Path.Combine(adventureDirs[i], "MODULE.DAT");

					rc = Globals.PushDatabase();

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = Globals.Database.LoadCharacters(chrfn, printOutput: false);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = Globals.Database.LoadModules(modfn, printOutput: false);

					Debug.Assert(gEngine.IsSuccess(rc));

					var character = Globals.Database.CharacterTable.Records.FirstOrDefault();

					Debug.Assert(character != null);

					var module = Globals.Database.ModuleTable.Records.FirstOrDefault();

					Debug.Assert(module != null);

					advCharList.Add(new AdventuringCharacter()
					{
						Character = Globals.CloneInstance(character),

						Module = Globals.CloneInstance(module)
					});

					rc = Globals.PopDatabase();

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				i++;
			}

			var characterTable = Globals.Database.CharacterTable;

			j = characterTable.GetRecordCount();

			i = 0;

			foreach (var character in characterTable.Records)
			{
				if (character.Status == Status.Adventuring)
				{
					var advChar = advCharList.FirstOrDefault(ac => ac.Character.Uid == character.Uid);

					Buf.SetFormat("{0,3}. {1}: {2} ({3})", character.Uid, gEngine.Capitalize(character.Name), character.Status, advChar != null ? gEngine.Capitalize(advChar.Module.Name) : "???");

					if (Buf.Length > Constants.RightMargin)
					{
						Buf.Length = (int)(Constants.RightMargin - 4);

						Buf.Append("...)");
					}
				}
				else
				{
					Buf.SetFormat("{0,3}. {1}: {2}", character.Uid, gEngine.Capitalize(character.Name), character.Status);
				}

				gOut.Write("{0}{1}", Environment.NewLine, Buf.ToString());

				nlFlag = true;

				if ((i != 0 && (i % (Constants.NumRows - 8)) == 0) || i == j - 1)
				{
					nlFlag = false;

					PrintPostShowLineSep();

					gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

					Debug.Assert(gEngine.IsSuccess(rc));

					gOut.Print("{0}", Globals.LineSep);

					if (Buf.Length > 0 && Buf[0] == 'X')
					{
						break;
					}
				}

				i++;
			}

			if (nlFlag)
			{
				gOut.WriteLine();
			}

			gOut.Print("Done showing Character status summary.");
		}

		public ShowCharacterStatusSummaryMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
