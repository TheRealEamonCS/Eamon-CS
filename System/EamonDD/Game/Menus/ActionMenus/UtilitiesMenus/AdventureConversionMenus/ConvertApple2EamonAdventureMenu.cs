
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
using static EamonDD.Game.Plugin.PluginContext;

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

			rc = Globals.In.ReadField(Buf, 256, null, '_', '\0', false, null, null, null, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.WordWrap = true;

			a2eac.AdventureFolderPath = Buf.Trim().ToString().Replace('/', '\\');

			gOut.Print("{0}", Globals.LineSep);

			gOut.WriteLine();

			gOut.Write("Loading Apple II Eamon adventure data... ");

			if (!a2eac.LoadAdventure() || !a2eac.ConvertAdventure())
			{
				gOut.WriteLine("failed");

				gOut.Print(a2eac.ErrorMessage);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not converted.");

				goto Cleanup;
			}

			gOut.WriteLine("succeeded");

			var a2eAdv = a2eac.Adventure;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("Apple II Eamon adventure:");

			gOut.Print(a2eAdv.Name);

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

				if (!a2eAdv.Name.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					x.Name = a2eAdv.Name.ToLower().Trim(new char[] { ' ', '\"' }).ToTitleCase().Truncate(Constants.ModNameLen);
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
			});

			Globals.Database.AddModule(module);

			Globals.ModulesModified = true;

			Globals.Module = module;

			Globals.Database.FreeRooms();

			for (var i = 0; i < a2eAdv._nr; i++)
			{
				var a2eRoom = a2eAdv.RoomList[i];

				Debug.Assert(a2eRoom != null);

				if (!a2eRoom._rname.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					a2eRoom._rname = a2eRoom._rname.ToLower().Trim(new char[] { ' ', '\"' }).ToTitleCase();
				}

				a2eRoom._rname = Regex.Replace(a2eRoom._rname, @"[ .!?]*(\(.*\))?[ .!?]*$", "");

				if (a2eRoom._rname.Length <= 0)
				{
					a2eRoom._rname = "UNUSED";
				}

				if (!a2eRoom._rdesc.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					a2eRoom._rdesc = a2eRoom._rdesc.ToLower().Trim(new char[] { ' ', '\"' });
				}

				a2eRoom._rdesc = Regex.Replace(a2eRoom._rdesc, @"\s+", " ");

				var regex = new Regex(@"(^[a-z])|[?!.]\s+(.)", RegexOptions.ExplicitCapture);

				a2eRoom._rdesc = regex.Replace(a2eRoom._rdesc, s => s.Value.ToUpper());

				a2eRoom._rdesc = a2eRoom._rdesc.Replace(" i ", " I ");

				if (a2eRoom._rdesc.Length <= 0)
				{
					a2eRoom._rdesc = "UNUSED";
				}

				var room = Globals.CreateInstance<IRoom>(x =>
				{
					x.Uid = Globals.Database.GetRoomUid();

					x.Name = a2eRoom._rname.Truncate(Constants.RmNameLen);

					x.Desc = a2eRoom._rdesc.Truncate(Constants.RmDescLen);

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
						x.SetDirs(Direction.North, a2eRoom._rd1);
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
						x.SetDirs(Direction.South, a2eRoom._rd2);
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
						x.SetDirs(Direction.East, a2eRoom._rd3);
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
						x.SetDirs(Direction.West, a2eRoom._rd4);
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
						x.SetDirs(Direction.Up, a2eRoom._rd5);
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
						x.SetDirs(Direction.Down, a2eRoom._rd6);
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
							x.SetDirs(Direction.Northeast, a2eRoom._rd7);
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
							x.SetDirs(Direction.Northwest, a2eRoom._rd8);
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
							x.SetDirs(Direction.Southeast, a2eRoom._rd9);
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
							x.SetDirs(Direction.Southwest, a2eRoom._rd10);
						}
					}
				});

				Globals.Database.AddRoom(room);
			}

			Globals.RoomsModified = true;

			Globals.Database.FreeArtifacts();

			var artifactHelper = Globals.CreateInstance<IArtifactHelper>();

			Debug.Assert(artifactHelper != null);

			for (var i = 0; i < a2eAdv._na; i++)
			{
				var a2eArtifact = a2eAdv.ArtifactList[i];

				Debug.Assert(a2eArtifact != null);

				if (!a2eArtifact._artname.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					a2eArtifact._artname = a2eArtifact._artname.ToLower().Trim(new char[] { ' ', '\"' });
				}

				if (a2eArtifact._artname.Length <= 0)
				{
					a2eArtifact._artname = "UNUSED";
				}

				if (!a2eArtifact._artdesc.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					a2eArtifact._artdesc = a2eArtifact._artdesc.ToLower().Trim(new char[] { ' ', '\"' });
				}

				a2eArtifact._artdesc = Regex.Replace(a2eArtifact._artdesc, @"\s+", " ");

				var regex = new Regex(@"(^[a-z])|[?!.]\s+(.)", RegexOptions.ExplicitCapture);

				a2eArtifact._artdesc = regex.Replace(a2eArtifact._artdesc, s => s.Value.ToUpper());

				a2eArtifact._artdesc = a2eArtifact._artdesc.Replace(" i ", " I ");

				if (a2eArtifact._artdesc.Length <= 0)
				{
					a2eArtifact._artdesc = "UNUSED";
				}

				var artifact = Globals.CreateInstance<IArtifact>(x =>
				{
					x.Uid = Globals.Database.GetArtifactUid();

					x.Name = a2eArtifact._artname.Truncate(Constants.ArtNameLen);

					x.Desc = a2eArtifact._artdesc.Truncate(Constants.ArtDescLen);

					x.IsListed = true;

					x.Value = a2eArtifact._ad1;

					x.Weight = a2eArtifact._ad3;

					if (a2eArtifact._ad4 == -1)
					{
						x.SetCarriedByCharacter();
					}
					else if (a2eArtifact._ad4 == -999)
					{
						x.SetWornByCharacter();
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

					x.Type = ArtifactTypeMappings[a2eArtifact._ad2];


					// TODO


				});

				artifact.SetArtifactCategoryCount(1);

				artifactHelper.Record = artifact;

				if (!artifactHelper.ValidateField("Name"))
				{
					artifact.Name = "TODO";
				}

				Globals.Database.AddArtifact(artifact);
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

			Globals.ArtifactsModified = true;

			Globals.Database.FreeEffects();

			for (var i = 0; i < a2eAdv._ne; i++)
			{
				var a2eEffect = a2eAdv.EffectList[i];

				Debug.Assert(a2eEffect != null);

				if (!a2eEffect._text.Equals("TODO", StringComparison.OrdinalIgnoreCase))
				{
					a2eEffect._text = a2eEffect._text.ToLower().Trim(new char[] { ' ', '\"' });
				}

				a2eEffect._text = Regex.Replace(a2eEffect._text, @"\s+", " ");

				var regex = new Regex(@"(^[a-z])|[?!.]\s+(.)", RegexOptions.ExplicitCapture);

				a2eEffect._text = regex.Replace(a2eEffect._text, s => s.Value.ToUpper());

				a2eEffect._text = a2eEffect._text.Replace(" i ", " I ");

				if (a2eEffect._text.Length <= 0)
				{
					a2eEffect._text = "UNUSED";
				}

				var effect = Globals.CreateInstance<IEffect>(x =>
				{
					x.Uid = Globals.Database.GetEffectUid();

					x.Desc = a2eEffect._text.Truncate(Constants.EffDescLen);
				});

				Globals.Database.AddEffect(effect);
			}

			Globals.EffectsModified = true;



			var monsterList = gMDB.Records.Where(m => m.Weapon > 0).ToList();

			foreach (var monster in monsterList)
			{
				var weaponArtifact = gADB[monster.Weapon];

				if (weaponArtifact != null && weaponArtifact.IsInLimbo())
				{
					weaponArtifact.SetCarriedByMonsterUid(monster.Uid);
				}
			}



			Globals.Database.FreeTriggers();

			Globals.TriggersModified = true;

			Globals.Database.FreeScripts();

			Globals.ScriptsModified = true;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("The adventure was successfully converted.");

		Cleanup:

			;
		}

		public ConvertApple2EamonAdventureMenu()
		{
			Buf = Globals.Buf;

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
