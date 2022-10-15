
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override long Location
		{
			get
			{
				var result = base.Location;

				if (Globals.EnableMutateProperties)
				{
					switch (Uid)
					{
						case 5:     // Iron gate

							if (gGameState.Ro == 10)
							{
								result = 10;

								DoorGate.Field1 = 9;
							}
							else
							{
								DoorGate.Field1 = 10;
							}

							break;

						case 4:     // Iron fence
						case 6:     // Brass bell

							if (gGameState.Ro == 10)
							{
								result = 10;
							}

							break;

						case 14:    // Wooden ladder

							if (gGameState.Ro == 35)
							{
								result = 35;

								DoorGate.Field1 = 23;
							}
							else
							{
								DoorGate.Field1 = 35;
							}

							break;

						case 16:    // Wooden cart

							if (gGameState.WinchCounter == 0)
							{
								result = 84;
							}

							break;

						case 17:    // Iron anvil
						{
							var roomUids = new long[] { 124, 125, 126 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;
						}

						case 21:    // Placid lake

							if (gEngine.BeachRoomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;

						case 39:    // Support beams

							if (gEngine.SupportBeamsRoomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;

						case 40:    // Pipes

							if (gEngine.TracksRoomUids.Contains(gGameState.Ro) || gGameState.Ro == 57)
							{
								result = gGameState.Ro;
							}

							break;

						case 44:    // Tracks

							if (gEngine.TracksRoomUids.Contains(gGameState.Ro) && gGameState.Ro != gGameState.OreCartTracksRoomUid)
							{
								result = gGameState.Ro;
							}

							break;

						case 51:    // Conveyor belt
						{
							var roomUids = new long[] { 82, 83, 85 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;
						}

						case 55:    // Lava river

							if (gEngine.LavaRiverRoomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;

						case 57:    // River
						{
							var sluiceGateArtifact = gADB[25];

							Debug.Assert(sluiceGateArtifact != null);

							if (sluiceGateArtifact.DoorGate.IsOpen())
							{
								result = 39;
							}

							break;
						}

						case 58:    // Underground river
						{
							var roomUids = new long[] { 72, 73, 74 };

							var sluiceGateArtifact = gADB[25];

							Debug.Assert(sluiceGateArtifact != null);

							if (roomUids.Contains(gGameState.Ro) && sluiceGateArtifact.DoorGate.IsOpen())
							{
								result = gGameState.Ro;
							}

							break;
						}

						case 59:    // Waterfall
						{
							var sluiceGateArtifact = gADB[25];

							Debug.Assert(sluiceGateArtifact != null);

							var waterDiversionGateArtifact = gADB[61];

							Debug.Assert(waterDiversionGateArtifact != null);

							if (sluiceGateArtifact.DoorGate.IsOpen() && waterDiversionGateArtifact.DoorGate.IsOpen())
							{
								result = 57;
							}

							break;
						}

						case 69:    // Rope

							// Note: using hardcoded Location to avoid infinite recursion

							if (gGameState.Ro == 37 && result == 2070)
							{
								result = gGameState.Ro;
							}

							break;

						case 71:    // Lever
						{
							// Note: using hardcoded Location for simplicity

							if (gGameState.Ro == 82)
							{
								result = 2050;		// Debris sifter
							}
							else if (gGameState.Ro == 83)
							{
								result = 2049;		// Rock grinder
							}
							else if (gGameState.Ro == 85)
							{
								result = 2048;		// Rock crusher
							}

							break;
						}

						case 78:     // Animal path

							if (gGameState.Ro == 123)
							{
								result = 123;

								DoorGate.Field1 = 142;
							}
							else
							{
								DoorGate.Field1 = 114;
							}

							break;

						default:

							// do nothing

							break;
					}
				}

				return result;
			}

			set
			{
				base.Location = value;
			}
		}

		public override bool IsAttackable(ref IArtifactCategory ac)
		{
			var artifactUids = new long[] { 15, 18, 39 };

			// Iron lever / Hand winch / Support beams are attackable

			if (artifactUids.Contains(Uid))
			{
				ac = GetCategory(0);

				return true;
			}
			else
			{
				return base.IsAttackable(ref ac);
			}
		}

		public override bool IsInContainerOpenedFromTop()
		{
			var artifactUids = new long[] { 29, 81, 83, 84 };

			// Iron steam turbine / Desk / Fireplace / Stove opened from side

			return !artifactUids.Contains(Uid) ? base.IsInContainerOpenedFromTop() : false;
		}

		public override bool ShouldExposeContentsToRoom(ContainerType containerType = ContainerType.In)
		{
			// Pack animals exposed in corral, even when it's closed

			return Uid != 13 || containerType != ContainerType.In ? base.ShouldExposeContentsToRoom(containerType) : true;
		}

		public override bool ShouldExposeInContentsWhenClosed()
		{
			// Pack animals exposed in corral, even when it's closed

			return Uid != 13 ? base.ShouldExposeInContentsWhenClosed() : true;
		}
	}
}
