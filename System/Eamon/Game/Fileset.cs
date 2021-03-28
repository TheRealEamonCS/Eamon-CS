
// Fileset.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Fileset : GameBase, IFileset
	{
		#region Public Properties

		#region Interface IFileset

		public virtual string WorkDir { get; set; }

		public virtual string PluginFileName { get; set; }

		public virtual string ConfigFileName { get; set; }

		public virtual string FilesetFileName { get; set; }

		public virtual string CharacterFileName { get; set; }

		public virtual string ModuleFileName { get; set; }

		public virtual string RoomFileName { get; set; }

		public virtual string ArtifactFileName { get; set; }

		public virtual string EffectFileName { get; set; }

		public virtual string MonsterFileName { get; set; }

		public virtual string HintFileName { get; set; }

		public virtual string TriggerFileName { get; set; }

		public virtual string ScriptFileName { get; set; }

		public virtual string GameStateFileName { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeFilesetUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IFileset fileset)
		{
			return this.Uid.CompareTo(fileset.Uid);
		}

		#endregion

		#region Interface IFileset

		public virtual RetCode DeleteFiles(string fieldName)
		{
			StringBuilder buf;
			string fileName;
			RetCode rc;

			rc = RetCode.Success;

			buf = new StringBuilder(Constants.BufSize);

			var propInfos = new List<PropertyInfo>();

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				propInfos.AddRange(GetType().GetProperties().Where(pi => pi.Name.EndsWith("FileName")));
			}
			else
			{
				propInfos.Add(GetType().GetProperty(fieldName));
			}

			foreach (var pi in propInfos)
			{
				buf.Clear();

				fileName = pi.GetValue(this, null) as string;

				if (!string.IsNullOrWhiteSpace(fileName) && !fileName.Equals("NONE", StringComparison.OrdinalIgnoreCase))
				{
					if (!string.IsNullOrWhiteSpace(WorkDir) && !WorkDir.Equals("NONE", StringComparison.OrdinalIgnoreCase))
					{
						buf.Append(Globals.Path.Combine(WorkDir, fileName));
					}
					else
					{
						buf.Append(fileName);
					}

					try
					{
						Globals.File.Delete(buf.ToString());
					}
					catch (Exception ex)
					{
						if (ex != null)
						{
							// do nothing
						}
					}
				}
			}

			return rc;
		}

		#endregion

		#region Class Fileset

		public Fileset()
		{
			WorkDir = "NONE";

			PluginFileName = "";

			ConfigFileName = "";

			FilesetFileName = "";

			CharacterFileName = "";

			ModuleFileName = "";

			RoomFileName = "";

			ArtifactFileName = "";

			EffectFileName = "";

			MonsterFileName = "";

			HintFileName = "";

			TriggerFileName = "";

			ScriptFileName = "";

			GameStateFileName = "";
		}

		#endregion

		#endregion
	}
}
