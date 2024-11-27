
// ConvertApple2EamonAdventureMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
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
using EamonDD.Game.Converters.Apple2Eamon;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ConvertApple2EamonAdventureMenu : Menu, IConvertApple2EamonAdventureMenu
	{
		public virtual ArtifactType[] ArtifactTypeMappings { get; set; }

		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle("CONVERT APPLE II EAMON ADVENTURE", true);

			Debug.Assert(gEngine.IsAdventureFilesetLoaded());

			var a2eac = new A2EAdventureConverter();

			gOut.Print("Converting an Apple II Eamon adventure requires you to enter several key pieces of data.  This operation clears the in-memory database contents before loading converted records; if data already exists, you may want to abort this process and save the data before continuing.");

			gOut.Print("The Apple II .dsk file containing the adventure should be extracted to a unique Windows folder using the Cider Press utility.  You should always configure to preserve Apple II formats before extracting the disk image; Eamon CS only recognizes the original binary format.");

			gOut.Print("After converting the adventure, you can modify its data to your liking and then save it by exiting EamonDD from the main menu.  Select 'Y' when asked whether you would like to keep it.");

			gOut.Print("The conversion process minimally validates an adventure's data; upon reloading into any Eamon CS program, a thorough validation occurs.  You may find that the data needs manual repairs to get it to load successfully.");

			gOut.Write("{0}Enter the full (absolute) path of the Apple II Eamon adventure folder: ", Environment.NewLine);

			Buf.Clear();

			gOut.WordWrap = false;

			rc = gEngine.In.ReadField(Buf, 256, null, '_', '\0', false, null, null, null, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.WordWrap = true;

			a2eac.AdventureFolderPath = Buf.Trim().ToString().Replace(gEngine.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', gEngine.Path.DirectorySeparatorChar);

			gOut.Print("{0}", gEngine.LineSep);

			gOut.WriteLine();

			gOut.Write("Loading Apple II Eamon adventure data ... ");

			if (!a2eac.LoadAdventure() || !a2eac.ConvertAdventure())
			{
				gOut.WriteLine("failed.");

				gOut.Print(a2eac.ErrorMessage);

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			gOut.WriteLine("succeeded.");

			var a2eAdv = a2eac.Adventure;

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Print("Apple II Eamon adventure:");

			gOut.Print(a2eAdv.Name);

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

				if (!a2eAdv.Name.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					x.Name = a2eAdv.Name;

					if (!x.Name.Any(char.IsLower))
					{
						x.Name = x.Name.ToLower();
					}

					x.Name = x.Name.Trim(new char[] { ' ', '\"' }).ToTitleCase().Truncate(gEngine.ModNameLen);
				}
				else
				{
					x.Name = a2eAdv.Name;
				}

				x.Desc = "TODO";

				x.Author = "TODO";

				x.VolLabel = "TODO";

				x.SerialNum = "000";

				x.LastMod = DateTime.Now;

				x.NumDirs = a2eAdv._nd != 6 ? 12 : 6;

				x.NumRooms = a2eAdv._nr;

				x.NumArtifacts = a2eAdv._na;

				x.NumEffects = a2eAdv._ne;

				x.NumMonsters = a2eAdv._nm;

				x.NumHints = gDatabase.GetHintCount();
			});

			gDatabase.AddModule(module);

			gEngine.ModulesModified = true;

			gEngine.Module = module;

			gDatabase.FreeRooms();

			for (var i = 0; i < a2eAdv._nr; i++)
			{
				var a2eRoom = a2eAdv.RoomList[i];

				Debug.Assert(a2eRoom != null);

				if (!a2eRoom._rname.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					if (!a2eRoom._rname.Any(char.IsLower))
					{
						a2eRoom._rname = a2eRoom._rname.ToLower();
					}

					a2eRoom._rname = a2eRoom._rname.Trim(new char[] { ' ', '\"' }).ToTitleCase();
				}

				a2eRoom._rname = Regex.Replace(a2eRoom._rname, @"[ .!?]*(\(.*\))?[ .!?]*$", "");

				if (a2eRoom._rname.Length <= 0)
				{
					a2eRoom._rname = "UNUSED";
				}

				if (!a2eRoom._rdesc.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					if (!a2eRoom._rdesc.Any(char.IsLower))
					{
						a2eRoom._rdesc = a2eRoom._rdesc.ToLower();
					}

					a2eRoom._rdesc = a2eRoom._rdesc.Trim(new char[] { ' ', '\"' });

					if (a2eRoom._rdesc.Length > 0 && !Regex.IsMatch(a2eRoom._rdesc, @".*\p{P}$"))
					{
						a2eRoom._rdesc += ".";
					}
				}

				a2eRoom._rdesc = Regex.Replace(a2eRoom._rdesc, @"\s+", " ");

				var regex = new Regex(@"(^[a-z])|[?!.]\s+(.)", RegexOptions.ExplicitCapture);

				a2eRoom._rdesc = regex.Replace(a2eRoom._rdesc, s => s.Value.ToUpper());

				a2eRoom._rdesc = a2eRoom._rdesc.Replace(" i ", " I ");

				if (a2eRoom._rdesc.Length <= 0)
				{
					a2eRoom._rdesc = "UNUSED";
				}

				var room = gEngine.CreateInstance<IRoom>(x =>
				{
					x.Uid = gDatabase.GetRoomUid();

					x.Name = a2eRoom._rname.Truncate(gEngine.RmNameLen);

					x.Desc = a2eRoom._rdesc.Truncate(gEngine.RmDescLen);

					x.LightLvl = a2eRoom._rlight != 0 ? LightLevel.Light : LightLevel.Dark;

					x.Zone = 1;

					if (a2eRoom._rd1 > 500)
					{
						x.SetDirectionDoorUid(Direction.North, a2eRoom._rd1 - 500);
					}
					else if (a2eRoom._rd1 == -99)
					{
						x.SetDirectionExit(Direction.North);
					}
					else
					{
						x.SetDir(Direction.North, a2eRoom._rd1);
					}

					if (a2eRoom._rd2 > 500)
					{
						x.SetDirectionDoorUid(Direction.South, a2eRoom._rd2 - 500);
					}
					else if (a2eRoom._rd2 == -99)
					{
						x.SetDirectionExit(Direction.South);
					}
					else
					{
						x.SetDir(Direction.South, a2eRoom._rd2);
					}

					if (a2eRoom._rd3 > 500)
					{
						x.SetDirectionDoorUid(Direction.East, a2eRoom._rd3 - 500);
					}
					else if (a2eRoom._rd3 == -99)
					{
						x.SetDirectionExit(Direction.East);
					}
					else
					{
						x.SetDir(Direction.East, a2eRoom._rd3);
					}

					if (a2eRoom._rd4 > 500)
					{
						x.SetDirectionDoorUid(Direction.West, a2eRoom._rd4 - 500);
					}
					else if (a2eRoom._rd4 == -99)
					{
						x.SetDirectionExit(Direction.West);
					}
					else
					{
						x.SetDir(Direction.West, a2eRoom._rd4);
					}

					if (a2eRoom._rd5 > 500)
					{
						x.SetDirectionDoorUid(Direction.Up, a2eRoom._rd5 - 500);
					}
					else if (a2eRoom._rd5 == -99)
					{
						x.SetDirectionExit(Direction.Up);
					}
					else
					{
						x.SetDir(Direction.Up, a2eRoom._rd5);
					}

					if (a2eRoom._rd6 > 500)
					{
						x.SetDirectionDoorUid(Direction.Down, a2eRoom._rd6 - 500);
					}
					else if (a2eRoom._rd6 == -99)
					{
						x.SetDirectionExit(Direction.Down);
					}
					else
					{
						x.SetDir(Direction.Down, a2eRoom._rd6);
					}

					if (a2eAdv._nd != 6)
					{
						if (a2eRoom._rd7 > 500)
						{
							x.SetDirectionDoorUid(Direction.Northeast, a2eRoom._rd7 - 500);
						}
						else if (a2eRoom._rd7 == -99)
						{
							x.SetDirectionExit(Direction.Northeast);
						}
						else
						{
							x.SetDir(Direction.Northeast, a2eRoom._rd7);
						}

						if (a2eRoom._rd8 > 500)
						{
							x.SetDirectionDoorUid(Direction.Northwest, a2eRoom._rd8 - 500);
						}
						else if (a2eRoom._rd8 == -99)
						{
							x.SetDirectionExit(Direction.Northwest);
						}
						else
						{
							x.SetDir(Direction.Northwest, a2eRoom._rd8);
						}

						if (a2eRoom._rd9 > 500)
						{
							x.SetDirectionDoorUid(Direction.Southeast, a2eRoom._rd9 - 500);
						}
						else if (a2eRoom._rd9 == -99)
						{
							x.SetDirectionExit(Direction.Southeast);
						}
						else
						{
							x.SetDir(Direction.Southeast, a2eRoom._rd9);
						}

						if (a2eRoom._rd10 > 500)
						{
							x.SetDirectionDoorUid(Direction.Southwest, a2eRoom._rd10 - 500);
						}
						else if (a2eRoom._rd10 == -99)
						{
							x.SetDirectionExit(Direction.Southwest);
						}
						else
						{
							x.SetDir(Direction.Southwest, a2eRoom._rd10);
						}
					}
				});

				gDatabase.AddRoom(room);
			}

			gEngine.RoomsModified = true;

			gDatabase.FreeArtifacts();

			var artifactHelper = gEngine.CreateInstance<IArtifactHelper>();

			Debug.Assert(artifactHelper != null);

			for (var i = 0; i < a2eAdv._na; i++)
			{
				var a2eArtifact = a2eAdv.ArtifactList[i];

				Debug.Assert(a2eArtifact != null);

				if (!a2eArtifact._artname.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					if (!a2eArtifact._artname.Any(char.IsLower))
					{
						a2eArtifact._artname = a2eArtifact._artname.ToLower();
					}

					a2eArtifact._artname = a2eArtifact._artname.Trim(new char[] { ' ', '\"' });
				}

				if (a2eArtifact._artname.Length <= 0)
				{
					a2eArtifact._artname = "UNUSED";
				}

				if (!a2eArtifact._artdesc.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					if (!a2eArtifact._artdesc.Any(char.IsLower))
					{
						a2eArtifact._artdesc = a2eArtifact._artdesc.ToLower();
					}

					a2eArtifact._artdesc = a2eArtifact._artdesc.Trim(new char[] { ' ', '\"' });

					if (a2eArtifact._artdesc.Length > 0 && !Regex.IsMatch(a2eArtifact._artdesc, @".*\p{P}$"))
					{
						a2eArtifact._artdesc += ".";
					}
				}

				a2eArtifact._artdesc = Regex.Replace(a2eArtifact._artdesc, @"\s+", " ");

				var regex = new Regex(@"(^[a-z])|[?!.]\s+(.)", RegexOptions.ExplicitCapture);

				a2eArtifact._artdesc = regex.Replace(a2eArtifact._artdesc, s => s.Value.ToUpper());

				a2eArtifact._artdesc = a2eArtifact._artdesc.Replace(" i ", " I ");

				if (a2eArtifact._artdesc.Length <= 0)
				{
					a2eArtifact._artdesc = "UNUSED";
				}

				var embedArtifactInRoom = false;

				var artifact = gEngine.CreateInstance<IArtifact>(x =>
				{
					x.Uid = gDatabase.GetArtifactUid();

					x.Name = a2eArtifact._artname.Truncate(gEngine.ArtNameLen);

					x.Desc = a2eArtifact._artdesc.Truncate(gEngine.ArtDescLen);

					x.IsListed = true;

					x.Value = a2eArtifact._ad1;

					x.Weight = a2eArtifact._ad3;

					if (a2eArtifact._ad4 == -1)
					{
						x.SetCarriedByMonsterUid(long.MaxValue);
					}
					else if (a2eArtifact._ad4 == -999)
					{
						x.SetWornByMonsterUid(long.MaxValue);
					}
					else if (a2eArtifact._ad4 < -1 && a2eArtifact._ad4 > -501)
					{
						x.SetCarriedByMonsterUid(Math.Abs(a2eArtifact._ad4) - 1);
					}
					else if (a2eArtifact._ad4 == 0)
					{
						x.SetInLimbo();
					}
					else if (a2eArtifact._ad4 > 0 && a2eArtifact._ad4 < 201)
					{
						x.SetInRoomUid(a2eArtifact._ad4);
					}
					else if (a2eArtifact._ad4 > 500 && a2eArtifact._ad4 < 1001)
					{
						x.SetCarriedByContainerUid(a2eArtifact._ad4 - 500);
					}
					else if (a2eArtifact._ad4 > 200 && a2eArtifact._ad4 < 501)
					{
						x.SetEmbeddedInRoomUid(a2eArtifact._ad4 - 200);
					}
					else
					{
						x.Location = a2eArtifact._ad4;
					}

					try
					{
						x.Type = ArtifactTypeMappings[a2eArtifact._ad2];
					}
					catch (Exception)
					{
						x.Type = a2eArtifact._ad2 > 11 ? ArtifactType.User1 + (a2eArtifact._ad2 - 12) : (ArtifactType)a2eArtifact._ad2;
					}

					switch(a2eArtifact._ad2)
					{
						case 2:
						case 3:

							x.Field1 = a2eArtifact._ad5;

							x.Field2 = a2eArtifact._ad6;

							x.Field3 = a2eArtifact._ad7;

							x.Field4 = a2eArtifact._ad8;

							x.Field5 = a2eArtifact._ad6 == 2 ? 2 : 1;

							break;

						case 4:

							x.Field1 = a2eArtifact._ad5;

							x.Field2 = a2eArtifact._ad6 > 0 ? 1000 + a2eArtifact._ad6 : a2eArtifact._ad7;

							break;

						case 5:

							x.Field1 = a2eArtifact._ad5;

							break;

						case 6:
						case 7:
						case 10:

							x.Field1 = a2eArtifact._ad5;

							x.Field2 = a2eArtifact._ad6;

							x.Field3 = a2eArtifact._ad7;

							break;

						case 8:

							x.Field1 = a2eArtifact._ad5;

							x.Field2 = a2eArtifact._ad6;

							x.Field3 = a2eArtifact._ad7 > 0 ? 1000 + a2eArtifact._ad7 : a2eArtifact._ad6 != 0 || a2eArtifact._ad8 == 1 ? 1 : 0;

							x.Field4 = a2eArtifact._ad8;

							embedArtifactInRoom = a2eArtifact._ad8 == 1;

							break;

						case 11:

							x.Field1 = a2eArtifact._ad5;

							x.Field2 = a2eArtifact._ad6;

							break;

						default:

							// Do nothing

							break;
					}
				});

				artifact.SetArtifactCategoryCount(1);

				artifactHelper.Record = artifact;

				if (!artifactHelper.ValidateField("Name"))
				{
					artifact.Name = "TODO";
				}

				if (embedArtifactInRoom && !artifact.IsEmbeddedInRoom())
				{
					var roomUid = artifact.GetInRoomUid(true);

					if (roomUid > 0)
					{
						artifact.SetEmbeddedInRoomUid(roomUid);
					}
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

				container.Field4 = containedList.Count;
			}

			gEngine.ArtifactsModified = true;

			gDatabase.FreeEffects();

			for (var i = 0; i < a2eAdv._ne; i++)
			{
				var a2eEffect = a2eAdv.EffectList[i];

				Debug.Assert(a2eEffect != null);

				if (!a2eEffect._text.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					if (!a2eEffect._text.Any(char.IsLower))
					{
						a2eEffect._text = a2eEffect._text.ToLower();
					}

					a2eEffect._text = a2eEffect._text.Trim(new char[] { ' ', '\"' });

					if (a2eEffect._text.Length > 0 && !Regex.IsMatch(a2eEffect._text, @".*\p{P}$"))
					{
						a2eEffect._text += ".";
					}
				}

				a2eEffect._text = Regex.Replace(a2eEffect._text, @"\s+", " ");

				var regex = new Regex(@"(^[a-z])|[?!.]\s+(.)", RegexOptions.ExplicitCapture);

				a2eEffect._text = regex.Replace(a2eEffect._text, s => s.Value.ToUpper());

				a2eEffect._text = a2eEffect._text.Replace(" i ", " I ");

				if (a2eEffect._text.Length <= 0)
				{
					a2eEffect._text = "UNUSED";
				}

				var effect = gEngine.CreateInstance<IEffect>(x =>
				{
					x.Uid = gDatabase.GetEffectUid();

					x.Desc = a2eEffect._text.Truncate(gEngine.EffDescLen);
				});

				gDatabase.AddEffect(effect);
			}

			gEngine.EffectsModified = true;

			gDatabase.FreeMonsters();

			var monsterHelper = gEngine.CreateInstance<IMonsterHelper>();

			Debug.Assert(monsterHelper != null);

			for (var i = 0; i < a2eAdv._nm; i++)
			{
				var a2eMonster = a2eAdv.MonsterList[i];

				Debug.Assert(a2eMonster != null);

				if (!a2eMonster._mname.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					if (!a2eMonster._mname.Any(char.IsLower))
					{
						a2eMonster._mname = a2eMonster._mname.ToLower();
					}

					a2eMonster._mname = a2eMonster._mname.Trim(new char[] { ' ', '\"' });
				}

				if (a2eMonster._mname.Length <= 0)
				{
					a2eMonster._mname = "UNUSED";
				}

				if (!a2eMonster._mdesc.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					if (!a2eMonster._mdesc.Any(char.IsLower))
					{
						a2eMonster._mdesc = a2eMonster._mdesc.ToLower();
					}

					a2eMonster._mdesc = a2eMonster._mdesc.Trim(new char[] { ' ', '\"' });

					if (a2eMonster._mdesc.Length > 0 && !Regex.IsMatch(a2eMonster._mdesc, @".*\p{P}$"))
					{
						a2eMonster._mdesc += ".";
					}
				}

				a2eMonster._mdesc = Regex.Replace(a2eMonster._mdesc, @"\s+", " ");

				var regex = new Regex(@"(^[a-z])|[?!.]\s+(.)", RegexOptions.ExplicitCapture);

				a2eMonster._mdesc = regex.Replace(a2eMonster._mdesc, s => s.Value.ToUpper());

				a2eMonster._mdesc = a2eMonster._mdesc.Replace(" i ", " I ");

				if (a2eMonster._mdesc.Length <= 0)
				{
					a2eMonster._mdesc = "UNUSED";
				}

				var monster = gEngine.CreateInstance<IMonster>(x =>
				{
					x.Uid = gDatabase.GetMonsterUid();

					x.Name = a2eMonster._mname.Truncate(gEngine.MonNameLen);

					x.Desc = a2eMonster._mdesc.Truncate(gEngine.MonDescLen);

					x.IsListed = true;

					x.Hardiness = a2eMonster._md1;

					x.Agility = a2eMonster._md2;

					x.GroupCount = !string.IsNullOrWhiteSpace(a2eAdv._ver) && a2eAdv._ver.StartsWith("7") ? a2eMonster._md3 : 1;

					x.AttackCount = 1;

					if (!string.IsNullOrWhiteSpace(a2eAdv._ver) && a2eAdv._ver.StartsWith("7"))
					{
						x.Courage = a2eMonster._md4;
					}
					else
					{
						x.Courage = a2eMonster._md4 > 100 ? 200 : a2eMonster._md4 == 100 ? 105 : a2eMonster._md4;				// Another possible formula: a2eMonster._md4 >= 100 ? 200 : a2eMonster._md4;
					}

					x.Location = a2eMonster._md5;

					x.CombatCode = CombatCode.Weapons;

					if (!string.IsNullOrWhiteSpace(a2eAdv._ver) && a2eAdv._ver.StartsWith("7"))
					{
						x.Armor = a2eMonster._md7;

						x.Weapon = a2eMonster._md8;

						x.NwDice = a2eMonster._md9;

						x.NwSides = a2eMonster._md10;

						x.Friendliness = (Friendliness)a2eMonster._md11;
					}
					else
					{
						x.Armor = a2eMonster._md8;

						x.Weapon = a2eMonster._md9;

						x.NwDice = a2eMonster._md11;

						x.NwSides = a2eMonster._md12;

						x.Friendliness = (Friendliness)(100 + a2eMonster._md3);
					}

					x.Gender = Gender.Neutral;

					if (string.IsNullOrWhiteSpace(a2eAdv._ver) || !a2eAdv._ver.StartsWith("7"))
					{
						x.Field1 = a2eMonster._md7;

						x.Field2 = a2eMonster._md10;
					}
					else
					{
						x.Field1 = 0;

						x.Field2 = 0;
					}
				});

				monsterHelper.Record = monster;

				if (!monsterHelper.ValidateField("Name"))
				{
					monster.Name = "TODO";
				}

				gDatabase.AddMonster(monster);
			}

			var monsterList = gMDB.Records.Where(m => m.Weapon > 0).ToList();

			foreach (var monster in monsterList)
			{
				var weaponArtifact = gADB[monster.Weapon];

				if (weaponArtifact != null && weaponArtifact.IsInLimbo())
				{
					weaponArtifact.SetCarriedByMonsterUid(monster.Uid);
				}
			}

			gEngine.MonstersModified = true;

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Print("The adventure was successfully converted.");

		Cleanup:

			;
		}

		public ConvertApple2EamonAdventureMenu()
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
				ArtifactType.Treasure,
				ArtifactType.BoundMonster,
				ArtifactType.Wearable
			};
		}
	}
}
