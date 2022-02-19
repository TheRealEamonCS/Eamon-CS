
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

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
				Globals.MonsterCurses = true;
			}

			base.ExecuteAttack();

			Globals.MonsterCurses = false;
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

				Globals.Buf.Clear();

				deadBodyArtifact.BuildPrintedFullDesc(Globals.Buf, false);

				gOut.Write("{0}", Globals.Buf);

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
