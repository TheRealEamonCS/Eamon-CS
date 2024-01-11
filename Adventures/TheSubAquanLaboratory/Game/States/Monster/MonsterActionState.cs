
// MonsterActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class MonsterActionState : EamonRT.Game.States.MonsterActionState, IMonsterActionState
	{
		public override void Execute()
		{
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			// Friendly monsters attack the back wall or glass walls

			if (gEngine.AttackingWall && LoopMonster.IsInRoomUid(gGameState.Ro) && LoopMonster.Reaction == Friendliness.Friend && ((LoopMonster.Weapon > -1 && LoopMonster.Weapon <= gDatabase.GetArtifactCount()) || LoopMonster.CombatCode == CombatCode.NaturalWeapons) && gGameState.GetNBTL(Friendliness.Enemy) <= 0)
			{
				gEngine.ProcessWallAttack(LoopMonsterRoom, LoopMonster, gEngine.WallArtifact, false);

				NextState = gEngine.CreateInstance<IMonsterLoopIncrementState>();

				gEngine.NextState = NextState;
			}
			else
			{
				base.Execute();
			}
		}
	}
}

