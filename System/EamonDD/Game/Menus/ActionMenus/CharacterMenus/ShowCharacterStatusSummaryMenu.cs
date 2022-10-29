
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
using static EamonDD.Game.Plugin.Globals;

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

			gOut.Print("{0}", gEngine.LineSep);
		}

		public override void Execute()
		{
			RetCode rc;

			var nlFlag = false;

			gOut.WriteLine();

			gEngine.PrintTitle("SHOW CHARACTER STATUS SUMMARY", true);

			var advCharList = new List<AdventuringCharacter>();

			var adventureDirs = gEngine.Directory.GetDirectories(gEngine.AdventuresDir);

			var j = (long)adventureDirs.Length;

			var i = 0;

			while (i < j)
			{
				var chrfn = gEngine.Path.Combine(adventureDirs[i], "FRESHMEAT.DAT");

				if (gEngine.File.Exists(chrfn))
				{
					try
					{
						var fileName = gEngine.Path.GetFullPath(@".\" + gEngine.Path.GetFileNameWithoutExtension(adventureDirs[i]) + ".dll");

						if (gEngine.File.Exists(fileName))
						{
							Assembly.LoadFrom(fileName);
						}
					}
					catch (Exception)
					{
						// do nothing
					}

					var modfn = gEngine.Path.Combine(adventureDirs[i], "MODULE.DAT");

					rc = gEngine.PushDatabase();

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = gEngine.Database.LoadCharacters(chrfn, printOutput: false);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = gEngine.Database.LoadModules(modfn, printOutput: false);

					Debug.Assert(gEngine.IsSuccess(rc));

					var character = gEngine.Database.CharacterTable.Records.FirstOrDefault();

					Debug.Assert(character != null);

					var module = gEngine.Database.ModuleTable.Records.FirstOrDefault();

					Debug.Assert(module != null);

					advCharList.Add(new AdventuringCharacter()
					{
						Character = gEngine.CloneInstance(character),

						Module = gEngine.CloneInstance(module)
					});

					rc = gEngine.PopDatabase();

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				i++;
			}

			var characterTable = gEngine.Database.CharacterTable;

			j = characterTable.GetRecordCount();

			i = 0;

			foreach (var character in characterTable.Records)
			{
				if (character.Status == Status.Adventuring)
				{
					var advChar = advCharList.FirstOrDefault(ac => ac.Character.Uid == character.Uid);

					Buf.SetFormat("{0,3}. {1}: {2} ({3})", character.Uid, gEngine.Capitalize(character.Name), character.Status, advChar != null ? gEngine.Capitalize(advChar.Module.Name) : "???");

					if (Buf.Length > gEngine.RightMargin)
					{
						Buf.Length = (int)(gEngine.RightMargin - 4);

						Buf.Append("...)");
					}
				}
				else
				{
					Buf.SetFormat("{0,3}. {1}: {2}", character.Uid, gEngine.Capitalize(character.Name), character.Status);
				}

				gOut.Write("{0}{1}", Environment.NewLine, Buf.ToString());

				nlFlag = true;

				if ((i != 0 && (i % (gEngine.NumRows - 8)) == 0) || i == j - 1)
				{
					nlFlag = false;

					PrintPostShowLineSep();

					gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

					Buf.Clear();

					rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

					Debug.Assert(gEngine.IsSuccess(rc));

					gOut.Print("{0}", gEngine.LineSep);

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
			Buf = gEngine.Buf;
		}
	}
}
