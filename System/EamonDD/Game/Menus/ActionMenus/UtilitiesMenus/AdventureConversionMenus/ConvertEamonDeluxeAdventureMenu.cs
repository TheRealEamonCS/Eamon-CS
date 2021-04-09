
// ConvertEamonDeluxeAdventureMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using EamonDD.Game.Converters.EamonDeluxe;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ConvertEamonDeluxeAdventureMenu : Menu, IConvertEamonDeluxeAdventureMenu
	{
		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle("CONVERT EAMON DELUXE ADVENTURE", true);

			Debug.Assert(gEngine.IsAdventureFilesetLoaded());

			var edxac = new EDXAdventureConverter();

			gOut.Print("Converting an Eamon Deluxe adventure requires you to enter several key pieces of data.  This operation clears the in-memory database contents before loading converted records; if data already exists, you may want to abort this process and save the data before continuing.");

			gOut.Write("{0}Enter the full (absolute) path of the Eamon Deluxe adventure folder: ", Environment.NewLine);

			Buf.Clear();

			gOut.WordWrap = false;

			rc = Globals.In.ReadField(Buf, 256, null, '_', '\0', false, null, null, null, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.WordWrap = true;

			edxac.AdventureFolderPath = Buf.Trim().ToString().Replace('/', '\\');

			gOut.Print("{0}", Globals.LineSep);

			gOut.WriteLine();

			gOut.Write("Loading Eamon Deluxe adventure data... ");

			if (!edxac.LoadAdventureList() || !edxac.ConvertAdventures())
			{
				gOut.WriteLine("failed");

				gOut.Print(edxac.ErrorMessage);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			gOut.WriteLine("succeeded");

			gOut.Write("Loading Eamon Deluxe hint data... ");

			if (!edxac.LoadHintList() || !edxac.ConvertHints())
			{
				gOut.WriteLine("failed");

				gOut.Print(edxac.ErrorMessage);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			gOut.WriteLine("succeeded");

			Debug.Assert(edxac.AdventureList.Count > 0);

			gOut.Print("{0}", Globals.LineSep);

			gOut.WriteLine();

			for (var i = 0; i < edxac.AdventureList.Count; i++)
			{
				gOut.WriteLine("{0}. {1}", i + 1, edxac.AdventureList[i].Name);
			}

			gOut.Write("{0}Enter the adventure number to convert: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', false, null, null, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			var edxAdvNum = Convert.ToInt64(Buf.Trim().ToString());

			if (edxAdvNum < 1 || edxAdvNum > edxac.AdventureList.Count)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			var edxAdv = edxac.AdventureList[(int)edxAdvNum - 1];

			var edxHintList = new List<EDXHint>();

			if (edxac.HintList.Count > 0)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.WriteLine();

				for (var i = 0; i < edxac.HintList.Count; i++)
				{
					gOut.WriteLine("{0}. {1}", i + 1, edxac.HintList[i].Question);
				}

				gOut.Print("You can specify a comma-separated list of hints to convert; use an empty list for all hints.");

				gOut.Write("{0}Enter the hint numbers to include: ", Environment.NewLine);

				Buf.Clear();

				gOut.WordWrap = false;

				rc = Globals.In.ReadField(Buf, 256, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				var tokens = Buf.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

				var edxHintNums = Array.ConvertAll(tokens, t => long.TryParse(t, out var x) ? x : -1).Where(y => y != -1).ToArray();

				for (var i = 0; i < edxac.HintList.Count; i++)
				{
					if (edxHintNums == null || edxHintNums.Length <= 0 || edxHintNums.Contains(i + 1))
					{
						edxHintList.Add(edxac.HintList[i]);
					}
				}
			}

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("Eamon Deluxe adventure:");

			gOut.Print(edxAdv.Name);

			if (edxHintList.Count > 0)
			{
				gOut.Print("Eamon Deluxe hints:");

				gOut.WriteLine();

				foreach (var hint in edxHintList)
				{
					gOut.WriteLine(hint.Question);
				}
			}

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}Would you like to convert this adventure for use in Eamon CS (Y/N): ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			Globals.Module = null;

			Globals.Database.FreeModules();

			var module = Globals.CreateInstance<IModule>(x =>
			{
				x.Uid = Globals.Database.GetModuleUid();

				x.Name = edxAdv.Name.Trim().Truncate(Constants.ModNameLen);

				x.Desc = "TODO";

				x.Author = "TODO";

				x.VolLabel = "TODO";

				x.SerialNum = "000";

				x.LastMod = DateTime.Now;

				x.NumDirs = edxAdv._nd != 6 ? 12 : 6;

				x.NumRooms = edxAdv._nr;

				x.NumArtifacts = edxAdv._na;

				x.NumEffects = edxAdv._ne;

				x.NumMonsters = edxAdv._nm;

				x.NumHints = edxHintList.Count;
			});

			Globals.Database.AddModule(module);

			Globals.ModulesModified = true;

			Globals.Module = module;

			Globals.Database.FreeRooms();

			for (var i = 0; i < edxAdv._nr; i++)
			{
				var edxRoom = edxAdv.RoomList[i];

				Debug.Assert(edxRoom != null);

				edxRoom._rname = edxRoom._rname.Trim().ToTitleCase();

				if (edxRoom._rname.StartsWith("You Are", StringComparison.OrdinalIgnoreCase))
				{
					edxRoom._rname = edxRoom._rname.Substring(8);
				}
				else if (edxRoom._rname.StartsWith("You Stand", StringComparison.OrdinalIgnoreCase))
				{
					edxRoom._rname = edxRoom._rname.Substring(10);
				}

				edxRoom._rname = Regex.Replace(edxRoom._rname, @"[ .!?]*(\(.*\))?[ .!?]*$", "");

				if (edxRoom._rname.Length <= 0)
				{
					edxRoom._rname = "UNUSED";
				}

				edxRoom._rdesc = edxRoom._rdesc.Trim();

				if (edxRoom._rdesc.Length <= 0)
				{
					edxRoom._rdesc = "UNUSED";
				}

				var room = Globals.CreateInstance<IRoom>(x =>
				{
					x.Uid = Globals.Database.GetRoomUid();

					x.Name = edxRoom._rname.Truncate(Constants.RmNameLen);

					x.Desc = edxRoom._rdesc.Truncate(Constants.RmDescLen);

					x.LightLvl = edxRoom._rlight != 0 ? LightLevel.Light : LightLevel.Dark;

					x.Zone = 1;

					x.SetDirs(Direction.North, edxRoom._rd1);

					x.SetDirs(Direction.South, edxRoom._rd2);

					x.SetDirs(Direction.East, edxRoom._rd3);

					x.SetDirs(Direction.West, edxRoom._rd4);

					x.SetDirs(Direction.Up, edxRoom._rd5);

					x.SetDirs(Direction.Down, edxRoom._rd6);

					if (edxAdv._nd != 6)
					{
						x.SetDirs(Direction.Northeast, edxRoom._rd7);

						x.SetDirs(Direction.Northwest, edxRoom._rd8);

						x.SetDirs(Direction.Southeast, edxRoom._rd9);

						x.SetDirs(Direction.Southwest, edxRoom._rd10);
					}
				});

				Globals.Database.AddRoom(room);
			}

			Globals.RoomsModified = true;

			Globals.Database.FreeArtifacts();

			Globals.ArtifactsModified = true;

			Globals.Database.FreeEffects();

			for (var i = 0; i < edxAdv._ne; i++)
			{
				var edxEffect = edxAdv.EffectList[i];

				Debug.Assert(edxEffect != null);

				edxEffect._text = edxEffect._text.Trim();

				if (edxEffect._text.Length <= 0)
				{
					edxEffect._text = "UNUSED";
				}

				var effect = Globals.CreateInstance<IEffect>(x =>
				{
					x.Uid = Globals.Database.GetEffectUid();

					x.Desc = edxEffect._text.Truncate(Constants.EffDescLen);
				});

				Globals.Database.AddEffect(effect);
			}

			Globals.EffectsModified = true;

			Globals.Database.FreeMonsters();

			for (var i = 0; i < edxAdv._nm; i++)
			{
				var edxMonster = edxAdv.MonsterList[i];

				Debug.Assert(edxMonster != null);

				edxMonster._mname = edxMonster._mname.Trim();

				if (edxMonster._mname.Length <= 0)
				{
					edxMonster._mname = "UNUSED";
				}

				edxMonster._mdesc = edxMonster._mdesc.Trim();

				if (edxMonster._mdesc.Length <= 0)
				{
					edxMonster._mdesc = "UNUSED";
				}

				var monster = Globals.CreateInstance<IMonster>(x =>
				{
					x.Uid = Globals.Database.GetMonsterUid();

					x.Name = edxMonster._mname.Truncate(Constants.MonNameLen);

					x.Desc = edxMonster._mdesc.Truncate(Constants.MonDescLen);

					x.IsListed = true;

					x.Hardiness = edxMonster._md1;

					x.Agility = edxMonster._md2;

					x.GroupCount = edxMonster._md3;

					x.AttackCount = 1;

					x.Courage = edxMonster._md4;

					x.Location = edxMonster._md5;

					x.CombatCode = (CombatCode)edxMonster._md6;

					x.Armor = edxMonster._md7;

					x.Weapon = edxMonster._md8;

					x.NwDice = edxMonster._md9;

					x.NwSides = edxMonster._md10;

					x.Friendliness = (Friendliness)edxMonster._md11;

					x.Gender = Gender.Neutral;

					x.Field1 = edxMonster._md12;

					x.Field2 = edxMonster._md13;
				});

				// TODO: verify monster.Name contains no illegal tokens

				Globals.Database.AddMonster(monster);
			}

			Globals.MonstersModified = true;

			Globals.Database.FreeHints();

			for (var i = 0; i < edxHintList.Count; i++)
			{
				var edxHint = edxHintList[i];

				Debug.Assert(edxHint != null);

				edxHint.Question = edxHint.Question.Trim();

				if (edxHint.Question.Length <= 0)
				{
					edxHint.Question = "UNUSED";
				}

				var hint = Globals.CreateInstance<IHint>(x =>
				{
					x.Uid = Globals.Database.GetHintUid();

					x.Active = true;

					x.Question = edxHint.Question.Truncate(Constants.HntQuestionLen);

					x.NumAnswers = edxHint._nh;

					for (var j = 0; j < edxHint._nh; j++)
					{
						var edxAnswer = edxHint.AnswerList[j];

						Debug.Assert(edxAnswer != null);

						edxAnswer._text = edxAnswer._text.Trim();

						x.SetAnswers(j, edxAnswer._text.Truncate(Constants.HntAnswerLen));
					}
				});

				Globals.Database.AddHint(hint);
			}

			Globals.HintsModified = true;

			Globals.Database.FreeTriggers();

			Globals.TriggersModified = true;

			Globals.Database.FreeScripts();

			Globals.ScriptsModified = true;

		Cleanup:

			;
		}

		public ConvertEamonDeluxeAdventureMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
