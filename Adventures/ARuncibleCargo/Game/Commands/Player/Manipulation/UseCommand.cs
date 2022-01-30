
// UseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeUseArtifact)
			{
				switch (DobjArtifact.Uid)
				{
					case 34:

						// Telescope

						gEngine.PrintEffectDesc(121);

						GotoCleanup = true;

						break;

					case 129:

						// Cargo

						gEngine.PrintEffectDesc(128);

						GotoCleanup = true;

						break;

					case 45:

						// Detonator

						var cargoArtifact = gADB[129];

						Debug.Assert(cargoArtifact != null);

						var explosiveDeviceArtifact = gADB[43];

						Debug.Assert(explosiveDeviceArtifact != null);

						var princeMonster = gMDB[38];

						Debug.Assert(princeMonster != null);

						if (explosiveDeviceArtifact.IsCarriedByContainer(cargoArtifact) && cargoArtifact.IsCarriedByMonster(princeMonster))
						{
							var gatesArtifact = gADB[137];

							Debug.Assert(gatesArtifact != null);

							var ac = gatesArtifact.DoorGate;

							Debug.Assert(ac != null);

							if (!ac.IsOpen())
							{
								// Blow up bandits with explosive-rigged Cargo

								gOut.Print("You activate the detonator...");

								gOut.Print("{0}", Globals.LineSep);

								gEngine.PrintEffectDesc(138);

								Globals.In.KeyPress(Globals.Buf);

								gGameState.Die = 0;

								Globals.ExitType = ExitType.FinishAdventure;

								Globals.MainLoop.ShouldShutdown = true;

								NextState = Globals.CreateInstance<IStartState>();

								GotoCleanup = true;
							}
							else
							{
								gEngine.PrintEffectDesc(137);

								NextState = Globals.CreateInstance<IStartState>();

								GotoCleanup = true;
							}
						}
						else
						{
							gEngine.PrintEffectDesc(136);

							NextState = Globals.CreateInstance<IStartState>();

							GotoCleanup = true;
						}

						break;
				}
			}
		}
	}
}
