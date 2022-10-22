
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void ExecuteAttack()
		{
			// Allow cursing if defender is enemy

			if (DobjMonster != null && DobjMonster.Reaction == Friendliness.Enemy)
			{
				gEngine.MonsterCurses = true;
			}

			base.ExecuteAttack();

			gEngine.MonsterCurses = false;
		}

		public override void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell)
		{
			Debug.Assert(room != null && dobjMonster != null);

			var monsterDies = dobjMonster.IsDead();

			var deadBodyArtifact = dobjMonster.DeadBody > 0 ? gADB[dobjMonster.DeadBody] : null;

			// Desc of dead body; set flag as seen

			if (dobjMonster.CurrGroupCount == 1 && deadBodyArtifact != null && monsterDies)
			{
				if (!blastSpell)
				{
					gOut.WriteLine();
				}

				gEngine.Buf.Clear();

				deadBodyArtifact.BuildPrintedFullDesc(gEngine.Buf, false, false);

				gOut.Write("{0}", gEngine.Buf);

				deadBodyArtifact.Seen = true;
			}

			base.PrintHealthStatus(room, actorMonster, dobjMonster, blastSpell);
		}

		public override void PrintAlreadyBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Swallower shark

			if (artifact.Uid == 31)
			{
				gOut.Print("You already mangled {0}!", artifact.EvalPlural("it", "them"));
			}
			else
			{
				base.PrintAlreadyBrokeIt(artifact);
			}
		}
	}
}
