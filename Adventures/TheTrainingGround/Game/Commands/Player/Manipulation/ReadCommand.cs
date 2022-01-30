
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Plain scroll increases BLAST ability

			if (eventType == EventType.AfterReadArtifact && DobjArtifact.Uid == 29)
			{
				var spell = gEngine.GetSpells(Spell.Blast);

				Debug.Assert(spell != null);

				DobjArtifact.SetInLimbo();

				gCharacter.ModSpellAbilities(Spell.Blast, 10);

				if (gCharacter.GetSpellAbilities(Spell.Blast) > spell.MaxValue)
				{
					gCharacter.SetSpellAbilities(Spell.Blast, spell.MaxValue);
				}

				gGameState.ModSa(Spell.Blast, 250);

				if (gGameState.GetSa(Spell.Blast) > spell.MaxValue)
				{
					gGameState.SetSa(Spell.Blast, spell.MaxValue);
				}
			}
		}
	}
}
