
// MapCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class MapCommand : Command, IMapCommand
	{
		/// <summary></summary>
		public virtual IList<IGameBase> RangeRecordList { get; set; }

		/// <summary></summary>
		public virtual IList<(char Symbol, string Label)> LegendEntries { get; set; }

		/// <summary></summary>
		public virtual IList<(long Cell, char Symbol)> EntityCells { get; set; }

		/// <summary></summary>
		public virtual IList<IList<(long Cell, char Symbol)>> Rows { get; set; }

		/// <summary></summary>
		public virtual IList<string> TruncationNotices { get; set; }

		/// <summary></summary>
		public virtual long[] TickIntervals { get; set; }

		/// <summary></summary>
		public virtual char[] LabelChars { get; set; }

		/// <summary></summary>
		public virtual char[] RulerChars { get; set; }

		/// <summary></summary>
		public virtual long InteriorWidth { get; set; } = 76;

		/// <summary></summary>
		public virtual long MinClearance { get; set; } = 2;

		/// <summary></summary>
		public virtual long MinCellsPerTick { get; set; } = 5;

		/// <summary></summary>
		public virtual long MaxCoord { get; set; }

		/// <summary></summary>
		public virtual long ArtifactCount { get; set; }

		/// <summary></summary>
		public virtual long MonsterCount { get; set; }

		/// <summary></summary>
		public virtual long ArtifactNumber { get; set; }

		/// <summary></summary>
		public virtual char MonsterLetter { get; set; }

		/// <summary></summary>
		public virtual bool ArtifactsTruncated { get; set; }

		/// <summary></summary>
		public virtual bool MonstersTruncated { get; set; }

		public override void ExecuteForPlayer()
		{
			gEngine.ShouldPreTurnProcess = false;

			MaxCoord = ActorRoom.MaxCoord > 0 ? ActorRoom.MaxCoord : 1;

			var orderedRangeBands = EnumUtil.GetValues<RangeBand>();

			RangeRecordList.AddRange(gEngine.GetArtifactList(a => a.IsInRoom(ActorRoom)));

			RangeRecordList.AddRange(gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && !m.IsCharacterMonster()));

			var recordList = gEngine.BuildSortedRangeBandList(RangeRecordList, ActorMonster, gGameState.ShowRangeBands);

			ArtifactCount = recordList.Count(x => x.record is IArtifact);

			MonsterCount = recordList.Count(x => x.record is IMonster);

			ArtifactNumber = 1;

			ArtifactsTruncated = false;

			MonsterLetter = 'A';

			MonstersTruncated = false;

			LegendEntries.Add(('@', "Player"));

			EntityCells.Add((CoordToCell(ActorMonster.Coord, MaxCoord), '@'));

			foreach (var (record, rangeBand, range) in recordList)
			{
				if (record is IArtifact a)
				{
					char sym;

					if (ArtifactNumber <= 9)
					{
						sym = (char)('0' + ArtifactNumber);
					}
					else if (ArtifactNumber <= 35)
					{
						sym = (char)('a' + ArtifactNumber - 10);
					}
					else
					{
						ArtifactsTruncated = true;

						break;
					}

					ArtifactNumber++;

					var artName = string.Format("{0}{1}", a.GetNoneName(true), range > 0 ? string.Format(" ({0})", range) : "");

					LegendEntries.Add((sym, artName));

					EntityCells.Add((CoordToCell(a.Coord, MaxCoord), sym));
				}
				else if (record is IMonster m)
				{
					if (MonsterLetter > 'Z')
					{
						MonstersTruncated = true;

						break;
					}

					char sym = MonsterLetter++;

					var monName = string.Format("{0}{1}", m.GetNoneName(true), range > 0 ? string.Format(" ({0})", range) : "");

					LegendEntries.Add((sym, monName));

					EntityCells.Add((CoordToCell(m.Coord, MaxCoord), sym));
				}
			}

			if (MonstersTruncated)
			{
				TruncationNotices.Add($" * {MonsterCount - 26} Monster(s) not shown (exceeds A–Z limit).");
			}

			if (ArtifactsTruncated)
			{
				TruncationNotices.Add($" * {ArtifactCount - 35} Artifact(s) not shown (exceeds 1–9, a–z limit).");
			}

			gOut.WriteLine();

			gEngine.PrintTitle("--- Legend ---", false);

			gOut.WriteLine();

			var leftWidth = (int)(InteriorWidth / 2) - 2;
			
			var rightWidth = (int)InteriorWidth - leftWidth - 1;

			int half = (LegendEntries.Count + 1) / 2;

			for (var i = 0; i < half; i++)
			{
				var (symL, labelL) = LegendEntries[i];

				string left = $" {symL}={labelL}";

				if (left.Length > leftWidth)
				{
					left = left.Substring(0, leftWidth);
				}

				if (i + half < LegendEntries.Count)
				{
					var (symR, labelR) = LegendEntries[i + half];

					string right = $" {symR}={labelR}";

					if (right.Length > rightWidth)
					{
						right = right.Substring(0, rightWidth);
					}

					gOut.WriteLine($"{left,-36}{right}");       // Note: -leftWidth hardcoded is -36
				}
				else
				{
					gOut.WriteLine(left);
				}
			}

			if (TruncationNotices.Count > 0)
			{
				gOut.WriteLine();
			}

			foreach (var notice in TruncationNotices)
			{
				gOut.WriteLine(notice);
			}

			long tickInterval = TickIntervals[TickIntervals.Length - 1];

			foreach (var candidate in TickIntervals)
			{
				double cellsPerTick = (double)candidate / MaxCoord * InteriorWidth;

				if (cellsPerTick >= MinCellsPerTick)
				{
					tickInterval = candidate;

					break;
				}
			}

			for (long varn = 0; varn <= MaxCoord; varn += tickInterval)
			{
				long cell = CoordToCell(varn, MaxCoord);

				if (cell > 0 && cell < InteriorWidth - 1)
				{
					RulerChars[cell] = '+';
				}

				string label = varn.ToString();

				for (int k = 0; k < label.Length && cell + k < InteriorWidth; k++)
				{
					if (LabelChars[cell + k] == ' ')
					{
						LabelChars[cell + k] = label[k];
					}
				}
			}

			string maxLabel = MaxCoord.ToString();

			int maxLabelStart = (int)InteriorWidth - maxLabel.Length;

			for (int k = 0; k < maxLabel.Length; k++)
			{
				LabelChars[maxLabelStart + k] = maxLabel[k];
			}

			gOut.WriteLine();

			gOut.WriteLine(" " + new string(LabelChars));

			gOut.WriteLine("|" + new string(RulerChars) + "|");

			foreach (var entity in EntityCells.OrderBy(e => e.Cell))
			{
				bool placed = false;

				foreach (var row in Rows)
				{
					long lastCell = row[row.Count - 1].Cell;

					if (entity.Cell >= lastCell + MinClearance)
					{
						row.Add(entity);

						placed = true;

						break;
					}
				}

				if (!placed)
				{
					Rows.Add(new List<(long Cell, char Symbol)> { entity });
				}
			}

			foreach (var row in Rows)
			{
				var line = new string(' ', (int)InteriorWidth).ToCharArray();

				foreach (var (cell, sym) in row)
				{
					line[cell] = sym;
				}

				gOut.WriteLine(" " + new string(line));
			}

			gOut.WriteLine();

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>();
			}
		}

		public virtual long CoordToCell(long coord, long maxCoord)
		{
			Debug.Assert(coord <= maxCoord);

			return (long)Math.Round((double)coord / maxCoord * (InteriorWidth - 1));
		}

		public MapCommand()
		{
			SortOrder = 347;

			IsSentenceParserEnabled = false;

			IsPlayerEnabled = false;

			IsMonsterEnabled = false;

			Name = "MapCommand";

			Verb = "map";

			Type = CommandType.Miscellaneous;

			RangeRecordList = new List<IGameBase>();

			LegendEntries = new List<(char Symbol, string Label)>();

			EntityCells = new List<(long Cell, char Symbol)>();

			Rows = new List<IList<(long Cell, char Symbol)>>();

			TruncationNotices = new List<string>();

			TickIntervals = new long[] { 5, 10, 20, 25, 50, 100, 200, 250, 500, 1000 };

			LabelChars = new string(' ', (int)InteriorWidth).ToCharArray();

			RulerChars = new string('-', (int)InteriorWidth).ToCharArray();
		}
	}
}
