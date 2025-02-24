
// PlayerResurrectedState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.States
{
	[ClassMappings]
	public class PlayerResurrectedState : EamonRT.Game.States.State, Framework.States.IPlayerResurrectedState
	{
		public override void Execute()
		{
			var charMonster = gCharMonster;

			Debug.Assert(charMonster != null);

			var room = charMonster.GetInRoom();

			Debug.Assert(room != null);

			var ringArtifact = gADB[22];

			Debug.Assert(ringArtifact != null);

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Write("{0}Press any key to continue: ", Environment.NewLine);

			gEngine.Buf.Clear();

			var rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNull, null, gEngine.IsCharAny);

			Debug.Assert(gEngine.IsSuccess(rc));

			gEngine.Thread.Sleep(150);

			gOut.Print("{0}", gEngine.LineSep);

			// gSentenceParser.PrintDiscardingCommands() not called for this abrupt reality shift

			gEngine.ResetProperties(PropertyResetCode.SwitchContext);

			gEngine.PrintEffectDesc(3);

			gGameState.Die = 0;

			charMonster.Parry = charMonster.InitParry;

			charMonster.DmgTaken = 0;

			gEngine.ResetMonsterStats(charMonster);

			gEngine.MagicRingLowersMonsterStats(charMonster);

			var weaponArtifact = charMonster.Weapon > 0 ? gADB[charMonster.Weapon] : null;

			if (weaponArtifact != null)
			{
				gOut.EnableOutput = false;

				var dropCommand = gEngine.CreateInstance<IDropCommand>(x =>
				{
					x.ActorMonster = charMonster;

					x.ActorRoom = room;

					x.Dobj = weaponArtifact;
				});

				dropCommand.Execute();

				gOut.EnableOutput = true;
			}

			ringArtifact.SetInLimbo();

			room = gRDB[1];

			Debug.Assert(room != null);

			room.Seen = false;

			gEngine.EnforceCharacterWeightLimits02(room);		// Needed because player stats have changed

			gGameState.Ro = 1;

			gGameState.R2 = gGameState.Ro;

			NextState = gEngine.CreateInstance<IAfterPlayerMoveState>(x =>
			{
				x.MoveMonsters = false;
			});

			gEngine.NextState = NextState;
		}

		public PlayerResurrectedState()
		{
			Name = "PlayerResurrectedState";
		}
	}
}
