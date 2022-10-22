
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.Globals;

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

					var command = gEngine.CreateInstance<IExamineCommand>();

					CopyCommandData(command);

					NextState = command;

					break;

				case 115:

					// Student paper

					if (!gGameState.PaperRead)
					{
						var spell = gEngine.GetSpell(Spell.Speed);

						Debug.Assert(spell != null);

						gCharacter.ModSpellAbility(Spell.Speed, 25);

						if (gCharacter.GetSpellAbility(Spell.Speed) > spell.MaxValue)
						{
							gCharacter.SetSpellAbility(Spell.Speed, spell.MaxValue);
						}

						gEngine.PrintEffectDesc(76);

						gOut.Print("Your ability to cast {0} increases!", spell.Name);

						gGameState.PaperRead = true;
					}
					else
					{
						gOut.Print("Nothing happens.");
					}

					NextState = gEngine.CreateInstance<IMonsterStartState>();

					break;

				default:

					base.Execute();

					break;
			}
		}
	}
}
