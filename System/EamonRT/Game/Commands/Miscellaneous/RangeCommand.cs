
// RangeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class RangeCommand : Command, IRangeCommand
	{
		/// <summary></summary>
		public virtual IList<IGameBase> RangeRecordList { get; set; }

		/// <summary></summary>
		public virtual long OptimalRange { get; set; }

		public override void ExecuteForPlayer()
		{
			var isEnemyPresent = gGameState.GetNBTL(Friendliness.Enemy) > 0;

			var weaponList = ActorMonster.GetCarriedList().Where(x => x.IsReadyableByMonster(ActorMonster)).ToList();

			GetRangeRecords();

			if (RangeRecordList.Count <= 0)
			{
				PrintNothingOfInterestNearby();

				goto Cleanup;
			}

			var recordList = gEngine.BuildSortedRangeBandList(RangeRecordList, ActorMonster, gGameState.ShowRangeBands);

			foreach (var (record, rangeBand, range) in recordList)
			{
				var isMonster = record is IMonster;

				var isDeadBody = false;

				var isBreakableContainer = false;

				if (record is IArtifact a)
				{
					isDeadBody = a.DeadBody != null;

					isBreakableContainer = a.GeneralContainer != null && a.GeneralContainer.IsBreakable();
				}

				var rangeStr = range > 0
					? $" at {range} varn{(range != 1 ? "s" : "")}"
					: "";

				var prefix = "";

				var isAre = "";

				if (record is IArtifact art)
				{
					prefix = " - ";         // " * ";

					isAre = art.EvalPlural("is", "are");
				}
				else if (record is IMonster mon)
				{
					prefix = " + ";         // mon.EvalReaction(" - ", " o ", " + ");

					isAre = mon.EvalPlural("is", "are");
				}
				else
				{
					prefix = "  ";

					isAre = "is";
				}

				gOut.WriteLine();

				gOut.Write("{0}{1} {2} {3}{4}.",
					prefix,
					record.GetTheName(true),
					isAre,
					gEngine.GetRangeBandString(rangeBand),
					rangeStr);

				gOut.WriteLine();

				if (isMonster || (isEnemyPresent && (isDeadBody || isBreakableContainer)))
				{
					foreach (var weapon in weaponList)
					{
						Debug.Assert(weapon.GeneralWeapon != null);

						var isReadied = ActorMonster.Weapon == weapon.Uid;

						var suffix = isReadied ? " (r)" : " (c)";

						var rangeResult = gEngine.CheckWeaponRange(weapon, range);

						if (rangeResult == RangeResult.InRange)
						{
							gOut.WriteLine();

							gOut.Write("   {0}{1}:  Optimal range.",
								weapon.GetNoneName(true, false),
								suffix);
						}
						else
						{
							string status;

							string direction;

							switch (rangeResult)
							{
								case RangeResult.TooClose:

									status = "Too close";
									
									direction = "retreat";
									
									break;

								case RangeResult.SubOptimalClose:

									status = "Suboptimal";

									direction = "retreat";

									break;

								case RangeResult.SubOptimalFar:

									status = "Suboptimal";

									direction = "approach";

									break;

								case RangeResult.OutOfRange:

									status = "Out of range";

									direction = "approach";

									break;

								default:

									continue;
							}

							OptimalRange = range < weapon.GeneralWeapon.Field12
								? weapon.GeneralWeapon.Field12 - range
								: range - weapon.GeneralWeapon.Field13;

							var optimalStr = $"{OptimalRange} varn{(OptimalRange != 1 ? "s" : "")}";

							gOut.WriteLine();

							gOut.Write("   {0}{1}:  {2}, {3} {4}.",
								weapon.GetNoneName(true, false),
								suffix,
								status,
								direction,
								optimalStr);
						}
					}
				}

				gOut.WriteLine();
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>();
			}
		}

		public virtual void GetRangeRecords()
		{
			Debug.Assert(RangeRecordList != null);

			if (Dobj != null)
			{
				RangeRecordList.Add(Dobj);
			}
			else
			{
				RangeRecordList.AddRange(gEngine.GetArtifactList(a => a.IsInRoom(ActorRoom)));

				RangeRecordList.AddRange(gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && !m.IsCharacterMonster()));
			}
		}

		public RangeCommand()
		{
			Synonyms = new string[] { "distance" };

			SortOrder = 345;

			IsSentenceParserEnabled = false;

			IsPlayerEnabled = false;

			IsMonsterEnabled = false;

			Name = "RangeCommand";

			Verb = "range";

			Type = CommandType.Miscellaneous;

			RangeRecordList = new List<IGameBase>();
		}
	}
}
