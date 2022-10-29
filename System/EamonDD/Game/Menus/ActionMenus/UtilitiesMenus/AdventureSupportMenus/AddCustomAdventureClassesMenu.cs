
// AddCustomAdventureClassesMenu.cs

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
	public class AddCustomAdventureClassesMenu : AdventureSupportMenu01, IAddCustomAdventureClassesMenu
	{
		/// <summary></summary>
		public virtual IList<bool> IncludeInterfaceList { get; set; }

		public override void Execute()
		{
			gOut.WriteLine();

			gEngine.PrintTitle("ADD CUSTOM ADVENTURE CLASSES", true);

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

			gEngine.Directory.SetCurrentDirectory("..");

			SelectClassFilesToAdd();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			gEngine.Directory.SetCurrentDirectory(workDir);

			QueryToProcessAdventure();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			AddSelectedClassFiles();

			gEngine.Directory.SetCurrentDirectory(gEngine.AdventuresDir + @"\" + AdventureName);

			UpdateDatFileClasses();

			gEngine.Directory.SetCurrentDirectory(workDir);

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
		public virtual void SelectClassFilesToAdd()
		{
			var invalidClassFileNames = new string[] { "Program.cs", "IEngine.cs", "Engine.cs", "Globals.cs" };

			SelectedClassFileList = new List<string>();

			IncludeInterfaceList = new List<bool>();

			var classFileName = string.Empty;

			var includeInterface = false;

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

				includeInterface = false;

				if (!classFileName.StartsWith(@".\Eamon\") && !classFileName.StartsWith(@".\EamonDD\") && !classFileName.StartsWith(@".\EamonRT\"))
				{
					classFileName = string.Empty;
				}
				else if (!classFileName.Contains(@"\Game\") && !classFileName.Contains(@"\Framework\"))
				{
					classFileName = string.Empty;
				}
				else
				{
					var destClassFileName = classFileName.Replace(classFileName.StartsWith(@".\Eamon\") ? @".\Eamon\" : classFileName.StartsWith(@".\EamonDD\") ? @".\EamonDD\" : @".\EamonRT\", gEngine.AdventuresDir + @"\" + AdventureName + @"\").Replace(@"..\..\", @"..\");

					if (!classFileName.EndsWith(".cs") || classFileName.Contains(@"\.") || invalidClassFileNames.FirstOrDefault(fn => fn.Equals(gEngine.Path.GetFileName(classFileName), StringComparison.OrdinalIgnoreCase)) != null || SelectedClassFileList.FirstOrDefault(fn => fn.Equals(classFileName, StringComparison.OrdinalIgnoreCase)) != null || gEngine.File.Exists(destClassFileName))
					{
						classFileName = string.Empty;
					}

					if (!gEngine.File.Exists(classFileName))
					{
						if (classFileName.StartsWith(@".\EamonRT\Game\States\") || classFileName.StartsWith(@".\EamonRT\Game\Commands\") || classFileName.StartsWith(@".\EamonRT\Framework\States\") || classFileName.StartsWith(@".\EamonRT\Framework\Commands\"))
						{
							gOut.Print("{0}", gEngine.LineSep);

							gOut.Write("{0}Would you like to derive directly from {1} (Y/N) [N]: ", Environment.NewLine,
								classFileName.Contains(@"\Game\States\") ? "State" :
								classFileName.Contains(@"\Game\Commands\") ? "Command" :
								classFileName.Contains(@"\Framework\States\") ? "IState" :
								"ICommand");

							Buf.Clear();

							rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

							Debug.Assert(gEngine.IsSuccess(rc));

							if (Buf.Length > 0 && Buf[0] == 'Y')
							{
								if (classFileName.Contains(@"\Game\"))
								{
									includeInterface = true;
								}
							}
							else
							{
								classFileName = string.Empty;
							}
						}
						else
						{
							classFileName = string.Empty;
						}
					}
				}

				gOut.Print("{0}", gEngine.LineSep);

				if (classFileName.Length > 0)
				{
					SelectedClassFileList.Add(classFileName);

					if (!includeInterface && classFileName.Contains(@"\Game\"))
					{
						gOut.Write("{0}Would you like to add a custom interface for this class (Y/N) [N]: ", Environment.NewLine);

						Buf.Clear();

						rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

						Debug.Assert(gEngine.IsSuccess(rc));

						if (Buf.Length > 0 && Buf[0] == 'Y')
						{
							includeInterface = true;
						}

						gOut.Print("{0}", gEngine.LineSep);
					}

					IncludeInterfaceList.Add(includeInterface);

					gOut.Print("The file name path was added to the selected class files list.");
				}
				else
				{
					gOut.Print("The file name path was invalid or the source/destination file was not found, or already exists.");
				}
			}

		Cleanup:

			if (SelectedClassFileList.Count == 0)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("The adventure was not processed.");

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		public virtual void AddSelectedClassFiles()
		{
			gOut.Print("{0}", gEngine.LineSep);

			gOut.WriteLine();

			for (var i = 0; i < SelectedClassFileList.Count; i++)
			{
				ParentClassFileName = SelectedClassFileList[i].Replace(@".\", @"..\");

				IncludeInterface = IncludeInterfaceList[i];

				CreateCustomClassFile();
			}
		}

		public AddCustomAdventureClassesMenu()
		{

		}
	}
}
