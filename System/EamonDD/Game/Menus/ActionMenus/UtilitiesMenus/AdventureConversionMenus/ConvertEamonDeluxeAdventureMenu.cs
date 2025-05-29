
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
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using EamonDD.Game.Converters.EamonDeluxe;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ConvertEamonDeluxeAdventureMenu : Menu, IConvertEamonDeluxeAdventureMenu
	{
		public virtual ArtifactType[] ArtifactTypeMappings { get; set; }

		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle("CONVERT EAMON DELUXE ADVENTURE", true);

			Debug.Assert(gEngine.IsAdventureFilesetLoaded());

			var edxac = new EDXAdventureConverter();

			gOut.Print("Converting an Eamon Deluxe adventure requires you to enter several key pieces of data.  This operation clears the in-memory database contents before loading converted records; if data already exists, you may want to abort this process and save the data before continuing.");

			gOut.Print("After converting the adventure, you can modify its data to your liking and then save it by exiting EamonDD from the main menu.  Select 'Y' when asked whether you would like to keep it.");

			gOut.Print("The conversion process minimally validates an adventure's data; upon reloading into any Eamon CS program, a thorough validation occurs.  You may find that the data needs manual repairs to get it to load successfully.");

			gOut.Write("{0}Enter the full (absolute) path of the Eamon Deluxe adventure folder: ", Environment.NewLine);

			Buf.Clear();

			gOut.WordWrap = false;

			rc = gEngine.In.ReadField(Buf, 256, null, '_', '\0', false, null, null, null, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.WordWrap = true;

			edxac.AdventureFolderPath = Buf.Trim().ToString().Replace(gEngine.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', gEngine.Path.DirectorySeparatorChar);

			gOut.Print("{0}", gEngine.LineSep);

			gOut.WriteLine();

			gOut.Write("Loading Eamon Deluxe adventure data ... ");

			if (!edxac.LoadAdventureList() || !edxac.ConvertAdventures())
			{
				gOut.WriteLine("failed.");

				gOut.Print(edxac.ErrorMessage);

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			gOut.WriteLine("succeeded.");

			gOut.Write("Loading Eamon Deluxe hint data ... ");

			if (!edxac.LoadHintList() || !edxac.ConvertHints())
			{
				gOut.WriteLine("failed.");

				gOut.Print(edxac.ErrorMessage);

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			gOut.WriteLine("succeeded.");

			Debug.Assert(edxac.AdventureList.Count > 0);

			gOut.Print("{0}", gEngine.LineSep);

			gOut.WriteLine();

			for (var i = 0; i < edxac.AdventureList.Count; i++)
			{
				gOut.WriteLine("{0}. {1}", i + 1, edxac.AdventureList[i].Name);
			}

			gOut.Write("{0}Enter the adventure number to convert: ", Environment.NewLine);

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', false, null, null, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			var edxAdvNum = Convert.ToInt64(Buf.Trim().ToString());

			if (edxAdvNum < 1 || edxAdvNum > edxac.AdventureList.Count)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			var edxAdv = edxac.AdventureList[(int)edxAdvNum - 1];

			var edxHintList = new List<EDXHint>();

			if (edxac.HintList.Count > 0)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.WriteLine();

				for (var i = 0; i < edxac.HintList.Count; i++)
				{
					gOut.WriteLine("{0}. {1}", i + 1, edxac.HintList[i].Question);
				}

				gOut.Print("You can specify a comma-separated list of hints to convert; use an empty list for all hints.");

				gOut.Write("{0}Enter the hint numbers to include: ", Environment.NewLine);

				Buf.Clear();

				gOut.WordWrap = false;

				rc = gEngine.In.ReadField(Buf, 256, null, '_', '\0', true, null, null, null, null);

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

			gOut.Print("{0}", gEngine.LineSep);

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

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Write("{0}Would you like to convert this adventure for use in Eamon CS (Y/N): ", Environment.NewLine);

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			gEngine.Module = null;

			gDatabase.FreeModules();

			var module = gEngine.CreateInstance<IModule>(x =>
			{
				x.Uid = gDatabase.GetModuleUid();

				x.Name = edxAdv.Name.Trim().Truncate(gEngine.ModNameLen);

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

			gDatabase.AddModule(module);

			gEngine.ModulesModified = true;

			gEngine.Module = module;

			gDatabase.FreeRooms();

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

				if (edxRoom._rdesc.Length > 0 && !Regex.IsMatch(edxRoom._rdesc, @".*\p{P}$"))
				{
					edxRoom._rdesc += ".";
				}

				if (edxRoom._rdesc.Length <= 0)
				{
					edxRoom._rdesc = "UNUSED";
				}

				var room = gEngine.CreateInstance<IRoom>(x =>
				{
					x.Uid = gDatabase.GetRoomUid();

					x.Name = edxRoom._rname.Truncate(gEngine.RmNameLen);

					x.Desc = edxRoom._rdesc.Truncate(gEngine.RmDescLen);

					x.LightLvl = edxRoom._rlight != 0 ? LightLevel.Light : LightLevel.Dark;

					x.Zone = 1;

					x.SetDir(Direction.North, edxRoom._rd1);

					x.SetDir(Direction.South, edxRoom._rd2);

					x.SetDir(Direction.East, edxRoom._rd3);

					x.SetDir(Direction.West, edxRoom._rd4);

					x.SetDir(Direction.Up, edxRoom._rd5);

					x.SetDir(Direction.Down, edxRoom._rd6);

					if (edxAdv._nd != 6)
					{
						x.SetDir(Direction.Northeast, edxRoom._rd7);

						x.SetDir(Direction.Northwest, edxRoom._rd8);

						x.SetDir(Direction.Southeast, edxRoom._rd9);

						x.SetDir(Direction.Southwest, edxRoom._rd10);
					}
				});

				gDatabase.AddRoom(room);
			}

			gEngine.RoomsModified = true;

			gDatabase.FreeArtifacts();

			var artifactHelper = gEngine.CreateInstance<IArtifactHelper>();

			Debug.Assert(artifactHelper != null);
			
			artifactHelper.RecordTable = gDatabase.ArtifactTable;

			for (var i = 0; i < edxAdv._na; i++)
			{
				var edxArtifact = edxAdv.ArtifactList[i];

				Debug.Assert(edxArtifact != null);

				edxArtifact._artname = edxArtifact._artname.Trim();

				if (edxArtifact._artname.Length <= 0)
				{
					edxArtifact._artname = "UNUSED";
				}

				edxArtifact._artdesc = edxArtifact._artdesc.Trim();

				if (edxArtifact._artdesc.Length > 0 && !Regex.IsMatch(edxArtifact._artdesc, @".*\p{P}$"))
				{
					edxArtifact._artdesc += ".";
				}

				if (edxArtifact._artdesc.Length <= 0)
				{
					edxArtifact._artdesc = "UNUSED";
				}

				var artifact = gEngine.CreateInstance<IArtifact>(x =>
				{
					x.Uid = gDatabase.GetArtifactUid();

					x.Name = edxArtifact._artname.Truncate(gEngine.ArtNameLen);

					x.Desc = edxArtifact._artdesc.Truncate(gEngine.ArtDescLen);

					x.IsListed = true;

					x.Value = edxArtifact._ad1;

					x.Weight = edxArtifact._ad3;

					if (edxArtifact._ad4 == -1)
					{
						x.SetCarriedByMonsterUid(long.MaxValue);
					}
					else if (edxArtifact._ad4 == -999)
					{
						x.SetWornByMonsterUid(long.MaxValue);
					}
					else if (edxArtifact._ad4 < -1 && edxArtifact._ad4 > -1001)
					{
						x.SetCarriedByMonsterUid(Math.Abs(edxArtifact._ad4) - 1);
					}
					else if (edxArtifact._ad4 < -1000 && edxArtifact._ad4 > -2000)
					{
						x.SetWornByMonsterUid(Math.Abs(edxArtifact._ad4) - 1000);
					}
					else if (edxArtifact._ad4 == 0)
					{
						x.SetInLimbo();
					}
					else if (edxArtifact._ad4 > 0 && edxArtifact._ad4 < 1000)
					{
						x.SetInRoomUid(edxArtifact._ad4);
					}
					else if (edxArtifact._ad4 > 1000 && edxArtifact._ad4 < 2000)
					{
						x.SetCarriedByContainerUid(edxArtifact._ad4 - 1000);
					}
					else if (edxArtifact._ad4 > 2000 && edxArtifact._ad4 < 3000)
					{
						x.SetEmbeddedInRoomUid(edxArtifact._ad4 - 2000);
					}
					else
					{
						x.Location = edxArtifact._ad4;
					}

					x.Type = ArtifactTypeMappings[edxArtifact._ad2];

					x.Field1 = edxArtifact._ad5;

					x.Field2 = edxArtifact._ad6;

					if (edxArtifact._ad2 != 4)
					{
						x.Field3 = edxArtifact._ad7;
					}
					else
					{
						x.Field3 = 0;
					}

					x.Field4 = edxArtifact._ad8;

					if (edxArtifact._ad2 == 2 || edxArtifact._ad2 == 3)
					{
						x.Field5 = edxArtifact._ad6 == 2 ? 2 : 1;
					}
					else
					{
						x.Field5 = 0;
					}
					
					x.SetFieldsValue(6, gEngine.NumArtifactCategoryFields, 0);
				});

				artifact.SetArtifactCategoryCount(1);

				artifactHelper.Record = artifact;

				if (!artifactHelper.ValidateField("Name"))
				{
					artifact.Name = "TODO";
				}

				gDatabase.AddArtifact(artifact);
			}

			var containerList = gADB.Records.Where(a => a.Type == ArtifactType.InContainer).ToList();

			foreach (var container in containerList)
			{
				var containedList = container.GetContainedList();

				var containedWeight = 0L;

				foreach (var containedArtifact in containedList)
				{
					if (!containedArtifact.IsUnmovable())
					{
						containedWeight += containedArtifact.Weight;
					}
				}

				container.Field3 = containedWeight;
			}

			Debug.Assert(gDatabase.ArtifactTableType == ArtifactTableType.Default);

			gEngine.ArtifactsModified = true;

			gDatabase.FreeEffects();

			for (var i = 0; i < edxAdv._ne; i++)
			{
				var edxEffect = edxAdv.EffectList[i];

				Debug.Assert(edxEffect != null);

				edxEffect._text = edxEffect._text.Trim();

				if (edxEffect._text.Length > 0 && !Regex.IsMatch(edxEffect._text, @".*\p{P}$"))
				{
					edxEffect._text += ".";
				}

				if (edxEffect._text.Length <= 0)
				{
					edxEffect._text = "UNUSED";
				}

				var effect = gEngine.CreateInstance<IEffect>(x =>
				{
					x.Uid = gDatabase.GetEffectUid();

					x.Desc = edxEffect._text.Truncate(gEngine.EffDescLen);
				});

				gDatabase.AddEffect(effect);
			}

			gEngine.EffectsModified = true;

			gDatabase.FreeMonsters();

			var monsterHelper = gEngine.CreateInstance<IMonsterHelper>();

			Debug.Assert(monsterHelper != null);
			
			monsterHelper.RecordTable = gDatabase.MonsterTable;

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

				if (edxMonster._mdesc.Length > 0 && !Regex.IsMatch(edxMonster._mdesc, @".*\p{P}$"))
				{
					edxMonster._mdesc += ".";
				}

				if (edxMonster._mdesc.Length <= 0)
				{
					edxMonster._mdesc = "UNUSED";
				}

				var monster = gEngine.CreateInstance<IMonster>(x =>
				{
					x.Uid = gDatabase.GetMonsterUid();

					x.Name = edxMonster._mname.Truncate(gEngine.MonNameLen);

					x.Desc = edxMonster._mdesc.Truncate(gEngine.MonDescLen);

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

					x.InitParry = gEngine.EnableEnhancedCombat ? 50 : 0;

					x.Field1 = edxMonster._md12;

					x.Field2 = edxMonster._md13;
				});

				monsterHelper.Record = monster;

				if (!monsterHelper.ValidateField("Name"))
				{
					monster.Name = "TODO";
				}

				gDatabase.AddMonster(monster);
			}

			gEngine.MonstersModified = true;

			gDatabase.FreeHints();

			for (var i = 0; i < edxHintList.Count; i++)
			{
				var edxHint = edxHintList[i];

				Debug.Assert(edxHint != null);

				edxHint.Question = edxHint.Question.Trim();

				if (edxHint.Question.Length <= 0)
				{
					edxHint.Question = "UNUSED";
				}

				var hint = gEngine.CreateInstance<IHint>(x =>
				{
					x.Uid = gDatabase.GetHintUid();

					x.Active = true;

					x.Question = edxHint.Question.Truncate(gEngine.HntQuestionLen);

					if (edxHint._nh > x.Answers.Length)
					{
						x.Answers = new string[edxHint._nh];

						for (var j = 0; j < x.Answers.Length; j++)
						{
							x.Answers[j] = "";
						}
					}

					x.NumAnswers = edxHint._nh;

					for (var j = 0; j < edxHint._nh; j++)
					{
						var edxAnswer = edxHint.AnswerList[j];

						Debug.Assert(edxAnswer != null);

						edxAnswer._text = edxAnswer._text.Trim();

						x.SetAnswer(j, edxAnswer._text.Truncate(gEngine.HntAnswerLen));
					}
				});

				gDatabase.AddHint(hint);
			}

			gEngine.HintsModified = true;

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Print("The adventure was successfully converted.");

		Cleanup:

			;
		}

		public ConvertEamonDeluxeAdventureMenu()
		{
			Buf = gEngine.Buf;

			ArtifactTypeMappings = new ArtifactType[]
			{
				ArtifactType.Gold,
				ArtifactType.Treasure,
				ArtifactType.Weapon,
				ArtifactType.MagicWeapon,
				ArtifactType.InContainer,
				ArtifactType.LightSource,
				ArtifactType.Drinkable,
				ArtifactType.Readable,
				ArtifactType.DoorGate,
				ArtifactType.Edible,
				ArtifactType.BoundMonster,
				ArtifactType.Wearable,
				ArtifactType.DisguisedMonster,
				ArtifactType.DeadBody,
				ArtifactType.User1,
				ArtifactType.User2,
				ArtifactType.User3
			};
		}
	}
}
