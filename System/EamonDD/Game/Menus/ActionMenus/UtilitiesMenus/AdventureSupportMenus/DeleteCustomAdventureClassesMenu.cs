
// DeleteCustomAdventureClassesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteCustomAdventureClassesMenu : AdventureSupportMenu01, IDeleteCustomAdventureClassesMenu
	{
		public override void Execute()
		{
			gOut.WriteLine();

			gEngine.PrintTitle("DELETE CUSTOM ADVENTURE CLASSES", true);

			Debug.Assert(!gEngine.IsAdventureFilesetLoaded());

			GotoCleanup = false;

			var workDir = gEngine.Directory.GetCurrentDirectory();

			CheckForPrerequisites();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			GetAdventureName();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			GetAuthorName();

			GetAuthorInitials();

			if (IsAdventureNameValid())
			{
				gEngine.Directory.SetCurrentDirectory(gEngine.AdventuresDir + @"\" + AdventureName);

				SelectClassFilesToDelete();

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				QueryToProcessAdventure();

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				DeleteSelectedClassFiles();

				UpdateDatFileClasses();

				gEngine.Directory.SetCurrentDirectory(workDir);
			}

			DeleteAdvBinaryFiles();

			RebuildProject();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			PrintAdventureProcessed();

		Cleanup:

			if (GotoCleanup)
			{
				// TODO: rollback classes delete if possible
			}

			gEngine.Directory.SetCurrentDirectory(workDir);
		}

		/// <summary></summary>
		public virtual void SelectClassFilesToDelete()
		{
			var invalidClassFileNames = new string[] { "Program.cs", "IEngine.cs", "Engine.cs", "Globals.cs" };

			SelectedClassFileList = new List<string>();

			var classFileName = string.Empty;

			gEngine.DdSuppressPostInputSleep = true;

			while (true)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Enter file name of interface/class: ", Environment.NewLine);

				Buf.Clear();

				gOut.WordWrap = false;

				var rc = gEngine.In.ReadField(Buf, 120, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				classFileName = Buf.Trim().ToString().Replace('/', '\\');

				if (classFileName.Length == 0)
				{
					goto Cleanup;
				}

				if (!classFileName.StartsWith(@".\"))
				{
					classFileName = string.Empty;
				}
				else if (!classFileName.Contains(@"\Game\") && !classFileName.Contains(@"\Framework\"))
				{
					classFileName = string.Empty;
				}
				else if (!classFileName.EndsWith(".cs") || classFileName.Contains(@"\.") || invalidClassFileNames.FirstOrDefault(fn => fn.Equals(gEngine.Path.GetFileName(classFileName), StringComparison.OrdinalIgnoreCase)) != null || SelectedClassFileList.FirstOrDefault(fn => fn.Equals(classFileName, StringComparison.OrdinalIgnoreCase)) != null || !gEngine.File.Exists(classFileName))
				{
					classFileName = string.Empty;
				}

				gOut.Print("{0}", gEngine.LineSep);

				if (classFileName.Length > 0)
				{
					SelectedClassFileList.Add(classFileName);

					gOut.Print("The file name path was added to the selected class files list.");
				}
				else
				{
					gOut.Print("The file name path was invalid or the file was not found.");
				}
			}

		Cleanup:

			gEngine.DdSuppressPostInputSleep = false;

			if (SelectedClassFileList.Count == 0)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("The adventure was not processed.");

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		public virtual void DeleteSelectedClassFiles()
		{
			gOut.Print("{0}", gEngine.LineSep);

			gOut.WriteLine();

			foreach (var selectedClassFile in SelectedClassFileList)
			{
				if (gEngine.File.Exists(selectedClassFile))
				{
					gEngine.File.Delete(selectedClassFile);
				}

				if (selectedClassFile.Contains(@"\Game\"))
				{
					var selectedInterfaceFile = gEngine.Path.GetDirectoryName(selectedClassFile.Replace(@"\Game\", @"\Framework\")) + @"\I" + gEngine.Path.GetFileName(selectedClassFile);

					if (gEngine.File.Exists(selectedInterfaceFile))
					{
						gEngine.File.Delete(selectedInterfaceFile);
					}
				}
			}

			if (IsAdventureNameValid())
			{
				gEngine.Directory.DeleteEmptySubdirectories(@"..\" + AdventureName, true);
			}
		}

		public DeleteCustomAdventureClassesMenu()
		{

		}
	}
}
