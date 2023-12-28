
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game
{
	[ClassMappings(typeof(IArtifact))]
	public class Artifact : Eamon.Game.Artifact, Framework.IArtifact
	{
		public override string[] Synonyms
		{
			get
			{
				var result = base.Synonyms;

				if (gEngine.EnableMutateProperties)
				{ 
					switch (Uid)
					{
						case 129:   // Jagged breach

							result = gGameState.Ro == 42 || gGameState.Ro == 59 ?
								new string[] { "north wall breach", "wall breach", "breach" } :
								new string[] { "south wall breach", "wall breach", "breach" };

							break;

						case 137:   // Window
						{
							var roomUids = new long[] { 46 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = new string[] { "broken window" };
							}
							
							break;
						}
					}
				}

				return result;
			}

			set
			{
				base.Synonyms = value;
			}
		}

		public override long Location
		{
			get
			{
				var result = base.Location;

				if (gEngine.EnableMutateProperties)
				{
					switch (Uid)
					{
						case 1:     // Wooden bridge

							if (gGameState.Ro == 7)
							{
								result = 7;

								DoorGate.Field1 = gGameState.WoodenBridgeUseCounter > 2 ? -59 : 6;
							}
							else
							{
								DoorGate.Field1 = 7;
							}

							break;

						case 3:		// Wayfarers Inn west wing
						case 4:		// Wayfarers Inn east wing
						{
							var roomUids = new long[] { 10, 11, 12 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;
						}

						case 73:		// North door (Innkeeper's Quarters)
						{
							if (gGameState.Ro == 38)
							{
								result = 38;

								Name = "door";

								DoorGate.Field1 = 37;
							}
							else
							{
								Name = "north door";

								DoorGate.Field1 = 38;
							}

							break;
						}

						case 96:     // Wayfarers Inn east wing
						case 97:     // Courtyard
						{
							var roomUids = new long[] { 50, 51 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;

								if (Uid == 97)
								{
									DoorGate.Field1 = result == 50 ? 30 : 31;
								}
							}

							break;
						}

						case 112:   // Ladder
						{
							var room20 = gRDB[20];

							Debug.Assert(room20 != null);

							var room67 = gRDB[67];

							Debug.Assert(room67 != null);

							if (gGameState.LadderUsed)
							{
								var roomUids = new long[] { 20, 67 };

								if (roomUids.Contains(gGameState.Ro))
								{
									result = gGameState.Ro;

									StateDesc = result == 20 ? " (leading up to the loft)" : " (leading down to the ground)";

									Type = ArtifactType.DoorGate;

									Field1 = result == 20 ? 67 : 20;

									Field2 = -1;

									Field3 = 0;

									Field4 = 0;

									Field5 = 0;

									room20.SetDirectionDoorUid(Direction.Up, 112);

									room67.SetDirectionDoorUid(Direction.Down, 112);
								}
							}
							else if (Type == ArtifactType.DoorGate)
							{
								StateDesc = "";

								Type = ArtifactType.Treasure;

								Field1 = 0;

								Field2 = 0;

								Field3 = 0;

								Field4 = 0;

								Field5 = 0;

								room20.SetDir(Direction.Up, 0);

								room67.SetDir(Direction.Down, 0);
							}

							break;
						}

						case 129:   // Jagged breach
						{
							var roomUids = new long[] { 42, 45, 57, 59 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;

								DoorGate.Field1 = gGameState.Ro == 42 ? 11 :
										gGameState.Ro == 45 ? 9 :
										gGameState.Ro == 57 ? 10 :
										12;
							}

							break;
						}

						case 137:   // Window

							if (gEngine.NorthWindowRoomUids.Contains(gGameState.Ro) || gEngine.SouthWindowRoomUids.Contains(gGameState.Ro) || gEngine.WestWindowRoomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;

						case 143:   // Forest
						{
							var roomUids = new long[] { 1, 2, 3, 8, 14, 15 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;
						}

						case 144:   // Rock
						{
							var roomUids = new long[] { 3, 4, 5, 6, 7 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;
						}

						case 145:   // Gorge
						case 146:	// River
						{
							var roomUids = new long[] { 4, 5, 6, 7 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;
						}

						case 147:   // Forest clearing
						case 148:   // Birds
						{
							var roomUids = new long[] { 9, 10, 11, 12, 16 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;
						}

						case 149:   // Insects
						case 153:	// Windows
						{
							var roomUids = new long[] { 9, 10, 11, 12 };

							if (roomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
							}

							break;
						}

						case 154:   // Furniture set

							if (gEngine.GuestRoomUids.Contains(gGameState.Ro))
							{
								result = gGameState.Ro;
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
				var origLocation = base.Location;

				base.Location = value;

				if (gEngine.EnableMutateProperties)
				{
					// Schedule drop carried artisan body part event

					if (origLocation != value && IsCarriedByMonster(gCharMonster) && IsArtisanBodyPartArtifact())
					{
						gGameState.BeforePrintPlayerRoomEventHeap.Remove((k, v) =>
						{
							long uid = 0;

							return v.EventName == "DropArtisanBodyPartArtifact" && long.TryParse(v.EventParam.ToString(), out uid) && Uid == uid;
						});

						gGameState.BeforePrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 6, "DropArtisanBodyPartArtifact", Uid);
					}

					// Water disappears when location changed

					else if (Uid == 60 && !IsCarriedByContainerUid(47) && !IsCarriedByContainerUid(24))
					{
						base.Location = gEngine.LimboLocation;
					}

					// Ladder no longer used when location changed

					else if (Uid == 112)
					{
						gGameState.LadderUsed = false;
					}
				}
			}
		}

		public override bool HasMoved(long oldLocation, long newLocation)
		{
			var artifactUids = new long[] { 117, 123, 127, 167, 174, 175 };

			// Dire wolf pups / Glass jar / Horse harnesses / Forgecraft Codex / Plates / Utensils

			return !artifactUids.Contains(Uid) ? base.HasMoved(oldLocation, newLocation) : false;
		}

		public override bool IsAttackable(ref IArtifactCategory ac)
		{
			// Treeline gap / Small graveyard / Child's skeleton / Courtyard

			if (Uid == 16 || Uid == 21 || Uid == 54 || Uid == 97)
			{
				ac = null;

				return false;
			}
			else
			{
				return base.IsAttackable(ref ac);
			}
		}

		public override bool IsRequestable()
		{
			var artifactUids = new long[] { 177, 178, 189 };

			// Leather armor / Boots / Ghostly teddy bear

			return !artifactUids.Contains(Uid) ? base.IsRequestable() : false;
		}

		public override bool IsInContainerOpenedFromTop()
		{
			// Registration desk

			return Uid != 26 ? base.IsInContainerOpenedFromTop() : false;
		}

		public override bool ShouldExposeContentsToMonster(MonsterType monsterType = MonsterType.NonCharMonster, ContainerType containerType = ContainerType.In)
		{
			// Bucket exposes contents to character

			if (Uid == 47 && monsterType == MonsterType.CharMonster)
			{
				return true;
			}
			else
			{
				return base.ShouldExposeContentsToMonster(monsterType, containerType);
			}
		}

		public override bool ShouldExposeContentsToRoom(ContainerType containerType = ContainerType.In)
		{
			// Bucket / Giant spiderwebs / Shelf expose contents to room

			if (Uid == 47 || Uid == 50 || Uid == 173)
			{
				return true;
			}
			else
			{
				return base.ShouldExposeContentsToRoom(containerType);
			}
		}

		public override bool ShouldAddContents(IArtifact artifact, ContainerType containerType = ContainerType.In)
		{
			Debug.Assert(artifact != null);

			// Mannequin can only contain wearables

			return Uid == 183 ? artifact.Wearable != null : base.ShouldAddContents(artifact, containerType);
		}

		public override bool ShouldRevealContentsWhenMovedIntoLimbo(ContainerType containerType = ContainerType.In)
		{
			// Pile of body parts reveals contents when hacked to pieces

			if (Uid == 51)
			{
				return true;
			}
			else
			{
				return base.ShouldRevealContentsWhenMovedIntoLimbo(containerType);
			}
		}

		public override string GetContainerSomethingDesc()
		{
			// Canvas covers

			return Uid != 124 ? base.GetContainerSomethingDesc() : "large objects";
		}

		public virtual bool IsArtisanBodyPartArtifact()
		{
			var artifactUids = new long[] { 43, 52, 87, 93, 102, 110 };

			return artifactUids.Contains(Uid);
		}
	}
}
