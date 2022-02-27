
// BortCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class BortCommand : Command, IBortCommand
	{
		public virtual IList<IGameBase> RecordList { get; set; }

		public virtual string Action { get; set; }

		public override void Execute()
		{
			Debug.Assert(RecordList.Count > 0 && !string.IsNullOrWhiteSpace(Action));

			switch(Action)
			{
				case "visitartifact":
				{
					var artifact = RecordList[0] as IArtifact;

					if (artifact != null)
					{
						var room = artifact.GetInRoom(true);

						if (room != null)
						{
							gOut.Print("Using Bort to visit artifact.");

							gGameState.R2 = room.Uid;

							NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
							{
								x.MoveMonsters = false;
							});
						}
						else
						{
							gOut.Print("The Bort artifact room is invalid.");
						}
					}
					else
					{
						gOut.Print("The Bort artifact is invalid.");
					}

					break;
				}

				case "visitmonster":
				{
					var monster = RecordList[0] as IMonster;

					if (monster != null)
					{
						var room = monster.GetInRoom();

						if (room != null)
						{
							gOut.Print("Using Bort to visit monster.");

							gGameState.R2 = room.Uid;

							NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
							{
								x.MoveMonsters = false;
							});
						}
						else
						{
							gOut.Print("The Bort monster room is invalid.");
						}
					}
					else
					{
						gOut.Print("The Bort monster is invalid.");
					}

					break;
				}

				case "visitroom":
				{
					var room = RecordList[0] as IRoom;

					if (room != null)
					{
						gOut.Print("Using Bort to visit room.");

						gGameState.R2 = room.Uid;

						NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
						{
							x.MoveMonsters = false;
						});
					}
					else
					{
						gOut.Print("The Bort room is invalid.");
					}

					break;
				}

				case "recallartifact":
				{
					gOut.Print("Using Bort to recall artifact{0}.", RecordList.Count != 1 ? "s" : "");

					foreach (var record in RecordList)
					{
						var artifact = record as IArtifact;

						if (artifact != null)
						{
							artifact.SetInRoom(ActorRoom);
						}
					}

					break;
				}

				case "recallmonster":
				{
					gOut.Print("Using Bort to recall monster{0}.", RecordList.Count != 1 ? "s" : "");

					foreach (var record in RecordList)
					{
						var monster = record as IMonster;

						if (monster != null)
						{
							monster.SetInRoom(ActorRoom);
						}
					}

					break;
				}

				default:
				{
					gOut.Print("The Bort Action is invalid.");

					break;
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public BortCommand()
		{
			SortOrder = 435;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

#if !DEBUG
			IsPlayerEnabled = false;
#endif

			Uid = 99;

			Name = "BortCommand";

			Verb = "bort";

			Type = CommandType.Miscellaneous;

			RecordList = new List<IGameBase>();
		}
	}
}
