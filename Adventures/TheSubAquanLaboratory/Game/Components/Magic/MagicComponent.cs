
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.Components
{
	[ClassMappings]
	public class MagicComponent : EamonRT.Game.Components.MagicComponent, IMagicComponent
	{
		public override void PrintSonicBoom(IRoom room)
		{
			Debug.Assert(room != null);

			gEngine.PrintEffectDesc(80 + (IsRoomInLab(room) ? 1 : 0));
		}

		public override bool ShouldShowBlastSpellAttack()
		{
			var artUids = new long[] { 83, 84, 85 };

			return (DobjMonster != null || !artUids.Contains(DobjArtifact.Uid)) && base.ShouldShowBlastSpellAttack();
		}

		public override void CheckAfterCastPower()
		{
			var rl = gEngine.RollDice(1, 100, 0);

			if (rl < 11)
			{
				// 10% Chance of raising the dead

				var found = gEngine.ResurrectDeadBodies(ActorRoom);

				if (found)
				{
					MagicState = MagicState.EndMagic;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 21)
			{
				// 10% Chance of stuff vanishing

				var found = gEngine.MakeArtifactsVanish(ActorRoom, a => a.IsInRoom(ActorRoom) && !a.IsUnmovable() && a.Uid != 82 && a.Uid != 89);

				if (found)
				{
					MagicState = MagicState.EndMagic;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 31)
			{
				// 10% Chance of cracking dome

				if (IsRoomInLab(ActorRoom))
				{
					gEngine.PrintEffectDesc(44);

					gGameState.Die = 1;

					SetNextStateFunc(gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					}));

					MagicState = MagicState.EndMagic;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 41)
			{
				// 10% Chance of insta-heal (tm)

				if (ActorMonster.DmgTaken > 0)
				{
					ActorMonster.DmgTaken = 0;

					gOut.Print("All of your wounds are suddenly healed!");

					gEngine.Buf.SetFormat("{0}You are ", Environment.NewLine);

					ActorMonster.AddHealthStatus(gEngine.Buf);

					gOut.Write("{0}", gEngine.Buf);

					MagicState = MagicState.EndMagic;

					goto Cleanup;
				}
				else
				{
					rl = 100;
				}
			}

			if (rl < 101)
			{
				// 60% Chance of boom over lake/in lab or fortune cookie

				base.CheckAfterCastPower();
			}

		Cleanup:

			;
		}

		public virtual bool IsRoomInLab(IRoom room)
		{
			Debug.Assert(room != null);

			return room.Uid == 18 || room.Zone == 2;
		}
	}
}
