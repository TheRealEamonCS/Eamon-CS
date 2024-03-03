
// Fileset.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game
{
	[ClassMappings]
	public class Fileset : GameBase, IFileset
	{
		#region Public Properties

		#region Interface IFileset

		[FieldName(620)]
		public virtual string WorkDir { get; set; }

		[FieldName(640)]
		public virtual string PluginFileName { get; set; }

		[FieldName(660)]
		public virtual string ConfigFileName { get; set; }

		[FieldName(680)]
		public virtual string FilesetFileName { get; set; }

		[FieldName(700)]
		public virtual string CharacterFileName { get; set; }

		[FieldName(720)]
		public virtual string ModuleFileName { get; set; }

		[FieldName(740)]
		public virtual string RoomFileName { get; set; }

		[FieldName(760)]
		public virtual string ArtifactFileName { get; set; }

		[FieldName(780)]
		public virtual string EffectFileName { get; set; }

		[FieldName(800)]
		public virtual string MonsterFileName { get; set; }

		[FieldName(820)]
		public virtual string HintFileName { get; set; }

		[FieldName(880)]
		public virtual string GameStateFileName { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				gDatabase.FreeFilesetUid(Uid);

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

			buf = new StringBuilder(gEngine.BufSize);

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
						buf.Append(gEngine.Path.Combine(WorkDir, fileName));
					}
					else
					{
						buf.Append(fileName);
					}

					try
					{
						gEngine.File.Delete(buf.ToString());
					}
					catch (Exception ex)
					{
						if (ex != null)
						{
							// Do nothing
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

			GameStateFileName = "";
		}

		#endregion

		#endregion
	}
}
