
// GenerateDummyArtifactRecordsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class GenerateDummyArtifactRecordsMenu : Menu, IGenerateDummyArtifactRecordsMenu
	{
		public override void Execute()
		{
			IArtifact artifact;
			RetCode rc;

			var artUids = new long[2];

			gOut.WriteLine();

			gEngine.PrintTitle("GENERATE DUMMY ARTIFACT RECORDS", true);

			gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(43, '\0', 0, "Enter the number to generate", "0"));

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			var j = Convert.ToInt64(Buf.Trim().ToString());

			for (var i = 0; i < j; i++)
			{
				artifact = gEngine.CreateInstance<IArtifact>(x =>
				{
					x.Uid = gEngine.Database.GetArtifactUid();
					x.Name = string.Format("artifact {0}", x.Uid);
					x.Desc = string.Format("You see artifact {0}.", x.Uid);
					x.IsListed = true;
					x.GetCategory(0).Type = ArtifactType.Treasure;
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

				rc = gEngine.Database.AddArtifact(artifact);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.ArtifactsModified = true;

				if (gEngine.Module != null)
				{
					gEngine.Module.NumArtifacts++;

					gEngine.ModulesModified = true;
				}
			}

			if (j > 0)
			{
				gOut.Print("{0}", gEngine.LineSep);

				Buf.SetFormat(j > 1 ? "Generated dummy Artifacts with Uids between {0} and {1}, inclusive." : "Generated a dummy Artifact with Uid {0}.", artUids[0], artUids[1]);

				gOut.Print("{0}", Buf);
			}
		}

		public GenerateDummyArtifactRecordsMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
