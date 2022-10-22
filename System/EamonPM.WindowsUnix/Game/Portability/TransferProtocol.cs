﻿
// TransferProtocol.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Portability;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Portability
{
	public class TransferProtocol : ITransferProtocol
	{
		public virtual void SendCharacterToMainHall(string filePrefix, string filesetFileName, string characterFileName, string effectFileName, string characterName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(filesetFileName));

			Debug.Assert(!string.IsNullOrWhiteSpace(characterFileName));

			Debug.Assert(!string.IsNullOrWhiteSpace(effectFileName));

			Debug.Assert(!string.IsNullOrWhiteSpace(characterName));

			if (!string.IsNullOrWhiteSpace(filePrefix))
			{
				Program.NextArgs = new string[] { "-pfn", "EamonMH.dll", "-wd", ".", "-fp", filePrefix, "-fsfn", filesetFileName, "-chrfn", characterFileName, "-efn", effectFileName, "-chrnm", characterName };
			}
			else
			{
				Program.NextArgs = new string[] { "-pfn", "EamonMH.dll", "-wd", ".", "-fsfn", filesetFileName, "-chrfn", characterFileName, "-efn", effectFileName, "-chrnm", characterName };
			}
		}

		public virtual void SendCharacterOnAdventure(string workDir, string filePrefix, string pluginFileName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(workDir));

			Debug.Assert(!string.IsNullOrWhiteSpace(pluginFileName));

			if (!string.IsNullOrWhiteSpace(filePrefix))
			{
				Program.NextArgs = new string[] { "-pfn", pluginFileName, "-wd", NormalizePath(workDir), "-fp", filePrefix, "-cfgfn", "EAMONCFG.DAT" };
			}
			else
			{
				Program.NextArgs = new string[] { "-pfn", pluginFileName, "-wd", NormalizePath(workDir), "-cfgfn", "EAMONCFG.DAT" };
			}
		}

		public virtual void RecallCharacterFromAdventure(string workDir, string filePrefix, string pluginFileName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(workDir));

			Debug.Assert(!string.IsNullOrWhiteSpace(pluginFileName));

			var dir = gEngine.Directory.GetCurrentDirectory();

			string[] args = null;

			if (!string.IsNullOrWhiteSpace(filePrefix))
			{
				args = new string[] { "-pfn", pluginFileName, "-wd", NormalizePath(workDir), "-fp", filePrefix, "-dgs", "-im" };
			}
			else
			{
				args = new string[] { "-pfn", pluginFileName, "-wd", NormalizePath(workDir), "-dgs", "-im" };
			}

			Program.ExecutePlugin(args, false);

			gEngine.Directory.SetCurrentDirectory(dir);
		}

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public virtual string NormalizePath(string path)
		{
			return path != null ? path.Replace(gEngine.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', gEngine.Path.DirectorySeparatorChar) : null;
		}
	}
}
