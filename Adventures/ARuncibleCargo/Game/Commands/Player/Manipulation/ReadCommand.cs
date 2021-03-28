
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			switch (DobjArtifact.Uid)
			{
				case 16:

					// Read sign on Sam's door () () ()

					var command = Globals.CreateInstance<IExamineCommand>();

					CopyCommandData(command);

					NextState = command;

					break;

				case 115:

					// Student paper

					if (!gGameState.PaperRead)
					{
						var spell = gEngine.GetSpells(Spell.Speed);

						Debug.Assert(spell != null);

						gCharacter.ModSpellAbilities(Spell.Speed, 25);

						if (gCharacter.GetSpellAbilities(Spell.Speed) > spell.MaxValue)
						{
							gCharacter.SetSpellAbilities(Spell.Speed, spell.MaxValue);
						}

						gEngine.PrintEffectDesc(76);

						gOut.Print("Your ability to cast {0} just increased!", spell.Name);

						gGameState.PaperRead = true;
					}
					else
					{
						gOut.Print("Nothing happens.");
					}

					NextState = Globals.CreateInstance<IMonsterStartState>();

					break;

				default:

					base.Execute();

					break;
			}
		}
	}
}
