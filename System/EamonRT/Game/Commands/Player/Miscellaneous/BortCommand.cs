
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
using static EamonRT.Game.Plugin.Globals;

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
		public virtual IConfig BortConfig { get; set; }

		/// <summary></summary>
		public virtual IMainMenu BortMenu { get; set; }

		/// <summary></summary>
		public virtual PunctSpaceCode PunctSpaceCode { get; set; }

		/// <summary></summary>
		public virtual bool SuppressNewLines { get; set; }

		public override void Execute()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(Action));

			try
			{
				gEngine.BortCommand = true;

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

							NextState = gEngine.CreateInstance<IAfterPlayerMoveState>(x =>
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

							NextState = gEngine.CreateInstance<IAfterPlayerMoveState>(x =>
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

						NextState = gEngine.CreateInstance<IAfterPlayerMoveState>(x =>
						{
							x.MoveMonsters = false;
						});

						break;

					case "recallartifact":

						BortArtifact = Record as IArtifact;

						Debug.Assert(BortArtifact != null);

						PrintBortRecallArtifact(ActorRoom, BortArtifact);

						try
						{
							gEngine.RevealContentCounter--;

							BortArtifact.SetInRoom(ActorRoom);
						}
						finally
						{
							gEngine.RevealContentCounter++;
						}

						break;

					case "recallmonster":

						BortMonster = Record as IMonster;

						Debug.Assert(BortMonster != null);

						PrintBortRecallMonster(ActorRoom, BortMonster);

						BortMonster.SetInRoom(ActorRoom);

						break;

					case "rungameeditor":

						BortConfig = gEngine.CloneInstance(gEngine.Config);

						Debug.Assert(BortConfig != null);

						gEngine.Config.DdEditingModules = true;

						gEngine.Config.DdEditingRooms = true;

						gEngine.Config.DdEditingArtifacts = true;

						gEngine.Config.DdEditingEffects = true;

						gEngine.Config.DdEditingMonsters = true;

						gEngine.Config.DdEditingHints = true;

						PunctSpaceCode = gOut.PunctSpaceCode;

						gOut.PunctSpaceCode = PunctSpaceCode.None;

						SuppressNewLines = gOut.SuppressNewLines;

						gOut.SuppressNewLines = false;

						gEngine.DdMenu = gEngine.CreateInstance<IDdMenu>();

						BortMenu = gEngine.CreateInstance<IMainMenu>();

						try
						{
							gEngine.RevealContentCounter--;

							BortMenu.Execute();
						}
						finally
						{
							gEngine.RevealContentCounter++;
						}

						BortMenu = null;

						gEngine.DdMenu = null;

						gOut.SuppressNewLines = SuppressNewLines;

						gOut.PunctSpaceCode = PunctSpaceCode;

						gEngine.Config = BortConfig;

						gEngine.Module = gEngine.GetModule();

						Debug.Assert(gEngine.Module != null && gEngine.Module.Uid > 0);

						Debug.Assert(gRDB[gGameState.Ro] != null);

						break;

					default:

						Debug.Assert(1 == 0);

						break;
				}
			}
			finally
			{
				gEngine.BortCommand = false;
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>();
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

			Name = "BortCommand";

			Verb = "bort";

			Type = CommandType.Miscellaneous;
		}
	}
}
