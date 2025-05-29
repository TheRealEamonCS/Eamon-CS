
// AnalyseCharacterRecordTreeMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseCharacterRecordTreeMenu : Menu, IAnalyseCharacterRecordTreeMenu
	{
		public virtual IList<string> RecordTreeStringList { get; set; }

		public override void Execute()
		{
			RetCode rc;

			var nlFlag = false;

			gOut.WriteLine();

			gEngine.PrintTitle("ANALYSE CHARACTER RECORD TREE", true);

			Debug.Assert(gEngine.IsCharacterInventoryLoaded());

			RecordTreeStringList.Clear();

			RecordTreeStringList.Add(string.Format("{0}[Characters", Environment.NewLine));

			var characterList = gDatabase.CharacterTable.Records;

			foreach (var character in characterList)
			{
				AnalyseCharacterRecordTree(character, "CH", 1);
			}

			var artifactList = gEngine.GetArtifactList(a => (!a.IsCarriedByCharacter() && !a.IsWornByCharacter()) || (a.IsCarriedByCharacter() && gDatabase.FindCharacter(a.GetCarriedByCharacterUid()) == null) || (a.IsWornByCharacter() && gDatabase.FindCharacter(a.GetWornByCharacterUid()) == null));

			foreach (var artifact in artifactList)
			{
				AnalyseArtifactRecordTree(artifact, "A", 1);
			}

			RecordTreeStringList.Add(string.Format("{0}]", characterList.Count > 0 || artifactList.Count > 0 ? Environment.NewLine : ""));

			gOut.Write("{0}Would you like to use page breaks (Y/N) [{1}Y]: ", Environment.NewLine, gEngine.EnableScreenReaderMode ? "Default " : "");

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, "Y", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Print("{0}", gEngine.LineSep);

			gOut.WriteLine();
			gEngine.PrintTitle("--- Legend ---", false);
			gOut.WriteLine();

			gOut.WriteLine("{0}{1}", "CH# = Character Uid #".PadTRight(40, ' '), "A#  = Artifact Uid #");
			gOut.WriteLine("{0}{1}", "CA# = Carried Artifact Uid #".PadTRight(40, ' '), "WA# = Worn Artifact Uid #");

			gEngine.In.KeyPress(new StringBuilder(gEngine.BufSize));

			gOut.Print("{0}", gEngine.LineSep);

			gEngine.DdSuppressPostInputSleep = true;

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

					if (i == k - 1 || (j >= gEngine.NumRows - 10 && (RecordTreeStringList[i + 1].StartsWith(Environment.NewLine + "\t[CH") || RecordTreeStringList[i + 1].StartsWith(Environment.NewLine + "\t[A"))))
					{
						nlFlag = false;

						gOut.WriteLine();

						gOut.Print("{0}", gEngine.LineSep);

						gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

						Buf.Clear();

						rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

						Debug.Assert(gEngine.IsSuccess(rc));

						gOut.Print("{0}", gEngine.LineSep);

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

				gEngine.In.KeyPress(Buf);

				gOut.Print("{0}", gEngine.LineSep);
			}

			gEngine.DdSuppressPostInputSleep = false;

			if (nlFlag)
			{
				gOut.WriteLine();
			}

			gOut.Print("Done analysing Character record tree.");
		}

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="tag"></param>
		/// <param name="indentLevel"></param>
		public virtual void AnalyseArtifactRecordTree(IArtifact artifact, string tag, long indentLevel)
		{
			Debug.Assert(artifact != null && tag != null && indentLevel > 0);

			var indentString = new string('\t', (int)indentLevel);

			RecordTreeStringList.Add(string.Format("{0}{1}[{2}{3}: {4}]", Environment.NewLine, indentString, tag, artifact.Uid, artifact.GetArticleName(true, false)));
		}

		/// <summary></summary>
		/// <param name="character"></param>
		/// <param name="tag"></param>
		/// <param name="indentLevel"></param>
		public virtual void AnalyseCharacterRecordTree(ICharacter character, string tag, long indentLevel)
		{
			Debug.Assert(character != null && tag != null && indentLevel > 0);

			var indentString = new string('\t', (int)indentLevel);

			RecordTreeStringList.Add(string.Format("{0}{1}[{2}{3}: {4}", Environment.NewLine, indentString, tag, character.Uid, character.Name));

			var wornList = character.GetWornList();

			foreach (var wornArtifact in wornList)
			{
				AnalyseArtifactRecordTree(wornArtifact, "WA", indentLevel + 1);
			}

			var carriedList = character.GetCarriedList();

			foreach (var carriedArtifact in carriedList)
			{
				AnalyseArtifactRecordTree(carriedArtifact, "CA", indentLevel + 1);
			}

			RecordTreeStringList.Add(string.Format("{0}]", wornList.Count > 0 || carriedList.Count > 0 ? Environment.NewLine + indentString : ""));
		}

		public AnalyseCharacterRecordTreeMenu()
		{
			Buf = gEngine.Buf;

			RecordTreeStringList = new List<string>();
		}
	}
}
