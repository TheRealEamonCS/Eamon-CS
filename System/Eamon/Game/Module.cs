
// Module.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game
{
	[ClassMappings]
	public class Module : GameBase, IModule
	{
		#region Public Properties

		#region Interface IModule

		[FieldName(620)]
		public virtual string Author { get; set; }

		[FieldName(640)]
		public virtual string VolLabel { get; set; }

		[FieldName(660)]
		public virtual string SerialNum { get; set; }

		[FieldName(680)]
		public virtual DateTime LastMod { get; set; }

		[FieldName(700)]
		public virtual long IntroStory { get; set; }

		[FieldName(720)]
		public virtual long NumDirs { get; set; }

		[FieldName(740)]
		public virtual long NumRooms { get; set; }

		[FieldName(760)]
		public virtual long NumArtifacts { get; set; }

		[FieldName(780)]
		public virtual long NumEffects { get; set; }

		[FieldName(800)]
		public virtual long NumMonsters { get; set; }

		[FieldName(820)]
		public virtual long NumHints { get; set; }

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
				gDatabase.FreeModuleUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IModule module)
		{
			return this.Uid.CompareTo(module.Uid);
		}

		#endregion

		#region Interface IModule

		public virtual void PrintInfo()
		{
			var buf = new StringBuilder(gEngine.BufSize);

			buf.AppendPrint("This is {0}, by {1}.",
				Name,
				Author);

			buf.AppendFormat("{0}Serial Number:  {1}{0}Volume  Label:  {2}{0}Last Modified:  {3}{0}{0}",
				Environment.NewLine,
				SerialNum,
				VolLabel,
				LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

			buf.AppendFormat("{0}{1}", Desc, Environment.NewLine);

			gOut.Write("{0}", buf);
		}

		#endregion

		#region Class Module

		public Module()
		{
			Author = "";

			VolLabel = "";

			SerialNum = "";

			LastMod = DateTime.Now;
		}

		#endregion

		#endregion
	}
}
