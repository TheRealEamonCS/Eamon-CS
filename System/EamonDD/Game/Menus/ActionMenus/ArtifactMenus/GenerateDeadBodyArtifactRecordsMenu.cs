
// GenerateDeadBodyArtifactRecordsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class GenerateDeadBodyArtifactRecordsMenu : Menu, IGenerateDeadBodyArtifactRecordsMenu
	{
		public override void Execute()
		{
			IArtifact artifact;
			RetCode rc;

			var artUids = new long[2];

			var monUids = new long[2];

			var nlFlag = false;

			var exited = false;

			gOut.WriteLine();

			gEngine.PrintTitle("GENERATE DEAD BODY ARTIFACT RECORDS", true);

			var maxMonUid = gDatabase.GetMonsterUid(false);

			gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(43, '\0', 0, "Enter the starting Monster Uid", "1"));

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			monUids[0] = Convert.ToInt64(Buf.Trim().ToString());

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(43, '\0', 0, "Enter the ending Monster Uid", maxMonUid > 0 ? maxMonUid.ToString() : "1"));

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, maxMonUid > 0 ? maxMonUid.ToString() : "1", null, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			monUids[1] = Convert.ToInt64(Buf.Trim().ToString());

			var monsterList = gEngine.GetMonsterList(m => m.Uid >= monUids[0] && m.Uid <= monUids[1]);

			var k = monsterList.Count();

			if (k > 0)
			{
				gOut.Print("{0}", gEngine.LineSep);
			}

			var helper = gEngine.CreateInstance<IMonsterHelper>();

			var j = 0;

			foreach (var monster in monsterList)
			{
				if (!exited)
				{
					helper.Record = monster;

					helper.ListRecord(false, false, false, false, false, false);

					nlFlag = true;

					if (j != 0 && ((j % (gEngine.NumRows - 8)) == 0 || j == k - 1))
					{
						nlFlag = false;

						gOut.WriteLine();

						gOut.Print("{0}", gEngine.LineSep);

						gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

						Buf.Clear();

						rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

						Debug.Assert(gEngine.IsSuccess(rc));

						if (Buf.Length > 0 && Buf[0] == 'X')
						{
							exited = true;
						}
						else if (j < k - 1)
						{
							gOut.Print("{0}", gEngine.LineSep);
						}
					}
				}

				j++;
			}

			if (nlFlag)
			{
				gOut.WriteLine();
			}

			if (j > 0)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Would you like to generate {1} (Y/N): ", Environment.NewLine, j > 1 ? "dead body Artifact records" : "a dead body Artifact record");

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				for (var i = 0; i < j; i++)
				{
					var monster = monsterList.ElementAt(i);

					Debug.Assert(monster != null);

					var lastChar = monster.Name.Length > 0 ? monster.Name[monster.Name.Length - 1] : '\0';

					artifact = gEngine.CreateInstance<IArtifact>(x =>
					{
						x.Uid = gDatabase.GetArtifactUid();
						x.Name = string.Format("{0}{1} body", monster.Name, char.ToUpper(lastChar) != 'S' ? "'s" : "'");
						x.Desc = string.Format("You see {0}.", x.Name);
						x.IsListed = true;
						x.Weight = 150;
						x.GetCategory(0).Type = ArtifactType.DeadBody;
						x.SetArtifactCategoryCount(1);
					});

					if (i == 0)
					{
						artUids[0] = artifact.Uid;
					}

					if (i == j - 1)
					{
						artUids[1] = artifact.Uid;
					}

					rc = gDatabase.AddArtifact(artifact);

					Debug.Assert(gEngine.IsSuccess(rc));

					gEngine.ArtifactsModified = true;

					if (gEngine.Module != null)
					{
						gEngine.Module.NumArtifacts++;

						gEngine.ModulesModified = true;
					}
				}

				gOut.Print("{0}", gEngine.LineSep);

				Buf.SetFormat(j > 1 ? "Generated dead body Artifacts with Uids between {0} and {1}, inclusive." : "Generated a dead body Artifact with Uid {0}.", artUids[0], artUids[1]);

				gOut.Print("{0}", Buf);
			}

		Cleanup:

			;
		}

		public GenerateDeadBodyArtifactRecordsMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
