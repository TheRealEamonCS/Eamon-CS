﻿
// DeleteAdventureMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteAdventureMenu : AdventureSupportMenu01, IDeleteAdventureMenu
	{
		public override void Execute()
		{
			gOut.WriteLine();

			gEngine.PrintTitle("DELETE ADVENTURE", true);

			Debug.Assert(!gEngine.IsAdventureFilesetLoaded());

			GotoCleanup = false;

			var workDir = gEngine.Directory.GetCurrentDirectory();

			GetAdventureName();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			SelectAdvDbDataFiles();

			QueryToDeleteAdventure();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (IsAdventureNameValid())
			{
				if (gEngine.File.Exists(gEngine.AdventuresDir + @"\" + AdventureName + @"\" + AdventureName + @".csproj"))
				{
					if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					{
						CheckForPrerequisites();

						if (GotoCleanup)
						{
							goto Cleanup;
						}
					}

					DeleteAdvBinaryFiles();

					if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					{
						RemoveProjectFromSolution();
					}
				}

				UpdateAdvDbDataFiles();
			}

			DeleteAdventureFolder();

			DeleteQuickLaunchFiles();

			PrintAdventureDeleted();

		Cleanup:

			if (GotoCleanup)
			{
				// TODO: rollback adventure delete if possible
			}

			gEngine.Directory.SetCurrentDirectory(workDir);
		}

		/// <summary></summary>
		public virtual void QueryToDeleteAdventure()
		{
			RetCode rc;

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Print("WARNING:  you are about to delete this adventure and all associated datafiles from storage.  If you have any doubts, you should select 'N' and backup your Eamon CS repository before proceeding.  This action is PERMANENT!");

			gOut.Write("{0}Would you like to delete this adventure from Eamon CS (Y/N): ", Environment.NewLine);

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("The adventure was not deleted.");

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		public virtual void RemoveProjectFromSolution()
		{
			var result = RetCode.Failure;

			gOut.Print("{0}", gEngine.LineSep);

			gOut.WriteLine();

			if (IsAdventureNameValid())
			{
				var projName = gEngine.Path.GetFullPath(gEngine.Path.Combine(gEngine.AdventuresDir + @"\" + AdventureName, AdventureName + ".csproj"));

				Debug.Assert(!string.IsNullOrWhiteSpace(projName));

				try
				{
					using (var process = new Process())
					{
						process.StartInfo.RedirectStandardOutput = true;
						process.StartInfo.RedirectStandardError = true;
						process.StartInfo.UseShellExecute = false;
						process.StartInfo.CreateNoWindow = true;

						process.StartInfo.FileName = "dotnet";
						process.StartInfo.Arguments = string.Format("sln Eamon-CS.sln remove \"{0}\"", projName);
						process.StartInfo.WorkingDirectory = string.Format("..{0}..", gEngine.Path.DirectorySeparatorChar);

						gOut.Write("Removing {0} project ... ", gEngine.Path.GetFileNameWithoutExtension(projName));

						process.Start();

						result = process.WaitForExit(120000) && process.ExitCode == 0 ? RetCode.Success : RetCode.Failure;

						try { process.Kill(); } catch (Exception) { }

						if (result == RetCode.Success)
						{
							gOut.WriteLine("succeeded.");
						}
						else
						{
							gOut.WriteLine("failed.");
						}
					}
				}
				catch (Exception ex)
				{
					gOut.WriteLine(ex.ToString());

					result = RetCode.Failure;
				}
			}

			if (result == RetCode.Failure)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("The adventure project was not removed.");

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		public virtual void DeleteAdventureFolder()
		{
			if (IsAdventureNameValid())
			{
				if (gEngine.Directory.Exists(gEngine.AdventuresDir + @"\" + AdventureName))
				{
					gEngine.Directory.Delete(gEngine.AdventuresDir + @"\" + AdventureName, true);
				}

				if (gEngine.Directory.Exists(gEngine.AndroidAdventuresDir + @"\" + AdventureName))
				{
					gEngine.Directory.Delete(gEngine.AndroidAdventuresDir + @"\" + AdventureName, true);
				}
			}
		}

		/// <summary></summary>
		public virtual void DeleteQuickLaunchFiles()
		{
			if (IsAdventureNameValid())
			{
				var scriptFilePath = gEngine.Path.Combine(".", "EAMONPM_SCRIPTS.TXT");

				var editFilePrefix = string.Format("EamonDD|Edit{0}", AdventureName);

				var resumeFilePrefix = string.Format("EamonRT|Resume{0}", AdventureName);

				// TODO: use try/catch ???

				var linesList = gEngine.File.ReadAllLines(scriptFilePath).ToList();

				Debug.Assert(linesList != null);

				linesList = linesList.Where(line => !line.StartsWith(editFilePrefix, StringComparison.OrdinalIgnoreCase) && !line.StartsWith(resumeFilePrefix, StringComparison.OrdinalIgnoreCase)).ToList();

				Debug.Assert(linesList != null);

				gEngine.File.WriteAllLines(scriptFilePath, linesList.ToArray(), new ASCIIEncoding());
			}
		}

		/// <summary></summary>
		public virtual void PrintAdventureDeleted()
		{
			gOut.Print("{0}", gEngine.LineSep);

			gOut.Print("The adventure was successfully deleted.");
		}

		public DeleteAdventureMenu()
		{

		}
	}
}
