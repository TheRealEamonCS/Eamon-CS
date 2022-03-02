
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

					Debug.Assert(artifact != null);

					var room = artifact.GetInRoom(true);

					if (room == null)
					{
							room = artifact.GetEmbeddedInRoom(true);
					}

					if (room != null)
					{
						PrintBortVisitArtifact(room, artifact);

						gGameState.R2 = room.Uid;

						NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
						{
							x.MoveMonsters = false;
						});
					}
					else
					{
						PrintBortArtifactRoomInvalid(artifact);
					}

					break;
				}

				case "visitmonster":
				{
					var monster = RecordList[0] as IMonster;

					Debug.Assert(monster != null);

					var room = monster.GetInRoom();

					if (room != null)
					{
						PrintBortVisitMonster(room, monster);

						gGameState.R2 = room.Uid;

						NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
						{
							x.MoveMonsters = false;
						});
					}
					else
					{
						PrintBortMonsterRoomInvalid(monster);
					}

					break;
				}

				case "visitroom":
				{
					var room = RecordList[0] as IRoom;

					Debug.Assert(room != null);

					PrintBortVisitRoom(room);

					gGameState.R2 = room.Uid;

					NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
					{
						x.MoveMonsters = false;
					});

					break;
				}

				case "recallartifact":
				{
					PrintBortRecallArtifacts(RecordList);

					foreach (var record in RecordList)
					{
						var artifact = record as IArtifact;

						Debug.Assert(artifact != null);

						artifact.SetInRoom(ActorRoom);
					}

					break;
				}

				case "recallmonster":
				{
					PrintBortRecallMonsters(RecordList);

					foreach (var record in RecordList)
					{
						var monster = record as IMonster;

						Debug.Assert(monster != null);

						monster.SetInRoom(ActorRoom);
					}

					break;
				}

				default:
				{
					Debug.Assert(1 == 0);

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
