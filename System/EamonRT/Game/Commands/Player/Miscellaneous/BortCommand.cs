
// BortCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus;
using EamonDD.Framework.Menus.HierarchicalMenus;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class BortCommand : Command, IBortCommand
	{
		public virtual IGameBase Record { get; set; }

		public virtual string Action { get; set; }

		/// <summary></summary>
		public virtual IArtifact BortArtifact { get; set; }

		/// <summary></summary>
		public virtual IMonster BortMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom BortRoom { get; set; }

		/// <summary></summary>
		public virtual IMainMenu BortMenu { get; set; }

		/// <summary></summary>
		public virtual IConfig Config { get; set; }

		/// <summary></summary>
		public virtual bool SuppressNewLines { get; set; }

		public override void Execute()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(Action));

			Globals.BortCommand = true;

			switch(Action)
			{
				case "visitartifact":

					BortArtifact = Record as IArtifact;

					Debug.Assert(BortArtifact != null);

					BortRoom = BortArtifact.GetInRoom(true);

					if (BortRoom == null)
					{
							BortRoom = BortArtifact.GetEmbeddedInRoom(true);
					}

					if (BortRoom != null)
					{
						PrintBortVisitArtifact(BortRoom, BortArtifact);

						gGameState.R2 = BortRoom.Uid;

						NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
						{
							x.MoveMonsters = false;
						});
					}
					else
					{
						PrintBortArtifactRoomInvalid(BortArtifact);
					}

					break;

				case "visitmonster":

					BortMonster = Record as IMonster;

					Debug.Assert(BortMonster != null);

					BortRoom = BortMonster.GetInRoom();

					if (BortRoom != null)
					{
						PrintBortVisitMonster(BortRoom, BortMonster);

						gGameState.R2 = BortRoom.Uid;

						NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
						{
							x.MoveMonsters = false;
						});
					}
					else
					{
						PrintBortMonsterRoomInvalid(BortMonster);
					}

					break;

				case "visitroom":

					BortRoom = Record as IRoom;

					Debug.Assert(BortRoom != null);

					PrintBortVisitRoom(BortRoom);

					gGameState.R2 = BortRoom.Uid;

					NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
					{
						x.MoveMonsters = false;
					});

					break;

				case "recallartifact":

					BortArtifact = Record as IArtifact;

					Debug.Assert(BortArtifact != null);

					PrintBortRecallArtifact(ActorRoom, BortArtifact);

					Globals.RevealContentCounter--;

					BortArtifact.SetInRoom(ActorRoom);

					Globals.RevealContentCounter++;

					break;

				case "recallmonster":

					BortMonster = Record as IMonster;

					Debug.Assert(BortMonster != null);

					PrintBortRecallMonster(ActorRoom, BortMonster);

					BortMonster.SetInRoom(ActorRoom);

					break;

				case "rungameeditor":

					Config = Globals.CloneInstance(Globals.Config);

					Debug.Assert(Config != null);

					Globals.Config.DdEditingModules = true;

					Globals.Config.DdEditingRooms = true;

					Globals.Config.DdEditingArtifacts = true;

					Globals.Config.DdEditingEffects = true;

					Globals.Config.DdEditingMonsters = true;

					Globals.Config.DdEditingHints = true;

					Globals.Config.DdEditingTriggers = true;

					Globals.Config.DdEditingScripts = true;

					SuppressNewLines = gOut.SuppressNewLines;

					gOut.SuppressNewLines = false;

					Globals.DdMenu = Globals.CreateInstance<IDdMenu>();

					BortMenu = Globals.CreateInstance<IMainMenu>();

					BortMenu.Execute();

					BortMenu = null;

					Globals.DdMenu = null;

					gOut.SuppressNewLines = SuppressNewLines;

					Globals.Config = Config;

					Globals.Module = gEngine.GetModule();

					Debug.Assert(Globals.Module != null && Globals.Module.Uid > 0);

					Debug.Assert(gRDB[gGameState.Ro] != null);

					break;

				default:

					Debug.Assert(1 == 0);

					break;
			}

			Globals.BortCommand = false;

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
		}
	}
}
