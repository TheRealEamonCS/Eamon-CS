
// TransferProtocol.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
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

			var argsList = new List<string>() { "-pfn", "EamonMH.dll", "-wd", "." };

			if (!string.IsNullOrWhiteSpace(filePrefix))
			{
				argsList.Add("-fp");

				argsList.Add(filePrefix);
			}

			argsList.Add("-fsfn");

			argsList.Add(filesetFileName);

			argsList.Add("-chrfn");

			argsList.Add(characterFileName);

			argsList.Add("-efn");

			argsList.Add(effectFileName);

			argsList.Add("-chrnm");

			argsList.Add(characterName);

			if (gEngine.IgnoreMutex)
			{
				argsList.Add("-im");
			}

			if (gEngine.DisableValidation)
			{
				argsList.Add("-dv");
			}

			if (gEngine.RepaintWindow)
			{
				argsList.Add("-rw");
			}
			
			if (gEngine.EnableScreenReaderMode)
			{
				argsList.Add("-esrm");
			}

			Program.NextArgs = argsList.ToArray();
		}

		public virtual void SendCharacterOnAdventure(string workDir, string filePrefix, string pluginFileName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(workDir));

			Debug.Assert(!string.IsNullOrWhiteSpace(pluginFileName));

			var argsList = new List<string>() { "-pfn", pluginFileName, "-wd", NormalizePath(workDir) };

			if (!string.IsNullOrWhiteSpace(filePrefix))
			{
				argsList.Add("-fp");

				argsList.Add(filePrefix);
			}

			argsList.Add("-cfgfn");

			argsList.Add("EAMONCFG.DAT");

			if (gEngine.IgnoreMutex)
			{
				argsList.Add("-im");
			}

			if (gEngine.DisableValidation)
			{
				argsList.Add("-dv");
			}

			if (gEngine.RepaintWindow)
			{
				argsList.Add("-rw");
			}
			
			if (gEngine.EnableScreenReaderMode)
			{
				argsList.Add("-esrm");
			}

			Program.NextArgs = argsList.ToArray();
		}

		public virtual void RecallCharacterFromAdventure(string workDir, string filePrefix, string pluginFileName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(workDir));

			Debug.Assert(!string.IsNullOrWhiteSpace(pluginFileName));

			var dir = gEngine.Directory.GetCurrentDirectory();

			string[] args = null;

			var argsList = new List<string>() { "-pfn", pluginFileName, "-wd", NormalizePath(workDir) };

			if (!string.IsNullOrWhiteSpace(filePrefix))
			{
				argsList.Add("-fp");

				argsList.Add(filePrefix);
			}

			argsList.Add("-dgs");

			argsList.Add("-im");

			if (gEngine.DisableValidation)
			{
				argsList.Add("-dv");
			}

			if (gEngine.RepaintWindow)
			{
				argsList.Add("-rw");
			}
			
			if (gEngine.EnableScreenReaderMode)
			{
				argsList.Add("-esrm");
			}

			args = argsList.ToArray();

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
