
// Effect.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Effect : GameBase, IEffect
	{
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
				Globals.Database.FreeEffectUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IEffect effect)
		{
			return this.Uid.CompareTo(effect.Uid);
		}

		#endregion

		#region Interface IEffect

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (!string.IsNullOrWhiteSpace(Desc))
			{
				buf.AppendPrint("{0}", Desc);
			}

		Cleanup:

			return rc;
		}

		#endregion

		#region Class Effect

		public Effect()
		{
			
		}

		#endregion

		#endregion
	}
}
