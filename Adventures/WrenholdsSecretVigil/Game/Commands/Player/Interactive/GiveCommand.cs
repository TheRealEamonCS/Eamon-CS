
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterEnforceMonsterWeightLimitsCheck)
			{
				if (IobjMonster.Uid == 1)
				{
					// Give death dog the dead rabbit

					if (DobjArtifact.Uid == 15)
					{
						DobjArtifact.SetInLimbo();

						IobjMonster.Friendliness = (Friendliness)150;

						IobjMonster.ResolveReaction(gCharacter);

						PrintGiveObjToActor(DobjArtifact, IobjMonster);

						gEngine.PrintEffectDesc(13);

						if (IobjMonster.Reaction == Friendliness.Friend)
						{
							gOut.Print("{0} barks once and wags its tail!", IobjMonster.GetTheName(true));
						}
					}
					else
					{
						gEngine.PrintMonsterEmotes(IobjMonster);

						gOut.WriteLine();
					}

					GotoCleanup = true;
				}

				// Further disable bribing

				else if (gIobjMonster(this).ShouldRefuseToAcceptGift01(DobjArtifact))
				{
					gEngine.PrintMonsterEmotes(IobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}
			}
			else if (eventType == EventType.BeforeTakePlayerGold)
			{
				// Disable bribing

				if (IobjMonster.Uid == 1 || IobjMonster.Reaction < Friendliness.Friend)
				{
					gEngine.PrintMonsterEmotes(IobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}
			}
		}
	}
}
