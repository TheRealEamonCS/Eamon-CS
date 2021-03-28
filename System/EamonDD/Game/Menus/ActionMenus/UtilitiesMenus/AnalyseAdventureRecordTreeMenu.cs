
// AnalyseAdventureRecordTreeMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseAdventureRecordTreeMenu : Menu, IAnalyseAdventureRecordTreeMenu
	{
		public virtual IList<string> RecordTreeStringList { get; set; }

		public override void Execute()
		{
			RetCode rc;

			var nlFlag = false;

			gOut.WriteLine();

			gEngine.PrintTitle("ANALYSE ADVENTURE RECORD TREE", true);

			Debug.Assert(gEngine.IsAdventureFilesetLoaded());

			RecordTreeStringList.Clear();

			RecordTreeStringList.Add(string.Format("{0}[{1}",
				Environment.NewLine,
				Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName));

			var roomList = Globals.Database.RoomTable.Records;

			foreach (var room in roomList)
			{
				AnalyseRoomRecordTree(room, "R", 1);
			}

			var monsterList = gEngine.GetMonsterList(m => !m.IsInRoom());

			foreach (var monster in monsterList)
			{
				AnalyseMonsterRecordTree(monster, "M", 1);
			}

			var artifactList = gEngine.GetArtifactList(a => !a.IsInRoom() && !a.IsEmbeddedInRoom() && !a.IsCarriedByMonster() && !a.IsWornByMonster() && !a.IsCarriedByContainer());

			foreach (var artifact in artifactList)
			{
				AnalyseArtifactRecordTree(artifact, "A", 1);
			}

			RecordTreeStringList.Add(string.Format("{0}]", roomList.Count > 0 || monsterList.Count > 0 || artifactList.Count > 0 ? Environment.NewLine : ""));

			gOut.Write("{0}Would you like to use page breaks (Y/N) [Y]: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, "Y", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Print("{0}", Globals.LineSep);

			gOut.WriteLine();
			gEngine.PrintTitle("--- Legend ---", false);
			gOut.WriteLine();

			gOut.WriteLine("{0}{1}", "R#  = Room Uid #".PadTRight(40, ' '), "M#  = Monster Uid #");
			gOut.WriteLine("{0}{1}", "A#  = Artifact Uid #".PadTRight(40, ' '), "EA# = Embedded Artifact Uid #");
			gOut.WriteLine("{0}{1}", "CA# = Carried Artifact Uid #".PadTRight(40, ' '), "WA# = Worn Artifact Uid #");
			gOut.WriteLine("{0}{1}", "IA# = In Container Artifact Uid #".PadTRight(40, ' '), "OA# = On Container Artifact Uid #");
			gOut.WriteLine("{0}{1}", "UA# = Under Container Artifact Uid #".PadTRight(40, ' '), "BA# = Behind Container Artifact Uid #");

			Globals.In.KeyPress(new StringBuilder(Constants.BufSize));

			gOut.Print("{0}", Globals.LineSep);

			if (Buf.Length == 0 || Buf[0] != 'N')
			{
				var i = 0;

				var j = 0;

				var k = RecordTreeStringList.Count;

				while (i < k)
				{
					gOut.Write(RecordTreeStringList[i]);

					if (RecordTreeStringList[i].Contains(Environment.NewLine))
					{
						j++;
					}

					nlFlag = true;

					if (i == k - 1 || (j >= Constants.NumRows - 10 && (RecordTreeStringList[i + 1].StartsWith(Environment.NewLine + "\t[R") || RecordTreeStringList[i + 1].StartsWith(Environment.NewLine + "\t[M") || RecordTreeStringList[i + 1].StartsWith(Environment.NewLine + "\t[A"))))
					{
						nlFlag = false;

						gOut.WriteLine();

						gOut.Print("{0}", Globals.LineSep);

						gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

						Buf.Clear();

						rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

						Debug.Assert(gEngine.IsSuccess(rc));

						gOut.Print("{0}", Globals.LineSep);

						if (Buf.Length > 0 && Buf[0] == 'X')
						{
							break;
						}

						j = 0;
					}

					i++;
				}
			}
			else
			{
				for (var i = 0; i < RecordTreeStringList.Count; i++)
				{
					gOut.Write(RecordTreeStringList[i]);
				}

				gOut.WriteLine();

				Globals.In.KeyPress(Buf);

				gOut.Print("{0}", Globals.LineSep);
			}

			if (nlFlag)
			{
				gOut.WriteLine();
			}

			gOut.Print("Done analysing adventure record tree.");
		}

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="tag"></param>
		/// <param name="indentLevel"></param>
		public virtual void AnalyseArtifactRecordTree(IArtifact artifact, string tag, long indentLevel)
		{
			Debug.Assert(artifact != null && tag != null && indentLevel > 0);

			var indentString = new string('\t', (int)indentLevel);

			RecordTreeStringList.Add(string.Format("{0}{1}[{2}{3}: {4}", Environment.NewLine, indentString, tag, artifact.Uid, artifact.GetArticleName(true, buf: Buf)));

			var containedList = artifact.GetContainedList(containerType: (ContainerType)(-1));

			foreach (var containedArtifact in containedList)
			{
				AnalyseArtifactRecordTree(containedArtifact,
					containedArtifact.GetCarriedByContainerContainerType() == ContainerType.On ? "OA" :
					containedArtifact.GetCarriedByContainerContainerType() == ContainerType.Under ? "UA" :
					containedArtifact.GetCarriedByContainerContainerType() == ContainerType.Behind ? "BA" :
					"IA",
					indentLevel + 1);
			}

			RecordTreeStringList.Add(string.Format("{0}]", containedList.Count > 0 ? Environment.NewLine + indentString : ""));
		}

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="tag"></param>
		/// <param name="indentLevel"></param>
		public virtual void AnalyseMonsterRecordTree(IMonster monster, string tag, long indentLevel)
		{
			Debug.Assert(monster != null && tag != null && indentLevel > 0);

			var indentString = new string('\t', (int)indentLevel);

			RecordTreeStringList.Add(string.Format("{0}{1}[{2}{3}: {4}", Environment.NewLine, indentString, tag, monster.Uid, monster.GetArticleName(true, buf: Buf)));

			var wornList = monster.GetWornList();

			foreach (var wornArtifact in wornList)
			{
				AnalyseArtifactRecordTree(wornArtifact, "WA", indentLevel + 1);
			}

			var carriedList = monster.GetCarriedList();

			foreach (var carriedArtifact in carriedList)
			{
				AnalyseArtifactRecordTree(carriedArtifact, "CA", indentLevel + 1);
			}

			RecordTreeStringList.Add(string.Format("{0}]", wornList.Count > 0 || carriedList.Count > 0 ? Environment.NewLine + indentString : ""));
		}

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="tag"></param>
		/// <param name="indentLevel"></param>
		public virtual void AnalyseRoomRecordTree(IRoom room, string tag, long indentLevel)
		{
			Debug.Assert(room != null && tag != null && indentLevel > 0);

			var indentString = new string('\t', (int)indentLevel);

			RecordTreeStringList.Add(string.Format("{0}{1}[{2}{3}: {4}", Environment.NewLine, indentString, tag, room.Uid, room.Name));

			var monsterList = gEngine.GetMonsterList(m => m.IsInRoom(room));

			foreach (var monster in monsterList)
			{
				AnalyseMonsterRecordTree(monster, "M", indentLevel + 1);
			}

			var artifactList = gEngine.GetArtifactList(a => a.IsInRoom(room) || a.IsEmbeddedInRoom(room));

			foreach (var artifact in artifactList)
			{
				AnalyseArtifactRecordTree(artifact, artifact.IsEmbeddedInRoom() ? "EA" : "A", indentLevel + 1);
			}

			RecordTreeStringList.Add(string.Format("{0}]", monsterList.Count > 0 || artifactList.Count > 0 ? Environment.NewLine + indentString : ""));
		}

		public AnalyseAdventureRecordTreeMenu()
		{
			Buf = Globals.Buf;

			RecordTreeStringList = new List<string>();
		}
	}
}
