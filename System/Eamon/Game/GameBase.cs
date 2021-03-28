
// GameBase.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	public abstract class GameBase : IGameBase
	{
		#region Public Properties

		#region Interface IGameBase

		public virtual long Uid { get; set; }

		public virtual bool IsUidRecycled { get; set; }

		public virtual string Name { get; set; }

		public virtual string Desc { get; set; }

		public virtual string[] Synonyms { get; set; }

		public virtual bool Seen { get; set; }

		public virtual ArticleType ArticleType { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public abstract void Dispose(bool disposing);

		public void Dispose()      // virtual intentionally omitted
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		#endregion

		#region Interface IGameBase

		public virtual void SetParentReferences()
		{
			// do nothing
		}

		public virtual string GetPluralName(string fieldName, StringBuilder buf = null)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			if (buf == null)
			{
				buf = Globals.Buf;
			}

			buf.Clear();

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetPluralName01(StringBuilder buf = null)
		{
			return GetPluralName("Name", buf);
		}

		public virtual string GetDecoratedName(string fieldName, ArticleType articleType, bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool groupCountOne = false, StringBuilder buf = null)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			if (buf == null)
			{
				buf = Globals.Buf;
			}

			buf.Clear();

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetNoneName(bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool groupCountOne = false, StringBuilder buf = null)
		{
			return GetDecoratedName("Name", ArticleType.None, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetArticleName(bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool groupCountOne = false, StringBuilder buf = null)
		{
			return GetDecoratedName("Name", ArticleType, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetTheName(bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool groupCountOne = false, StringBuilder buf = null)
		{
			return GetDecoratedName("Name", ArticleType.The, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf.Clear();

		Cleanup:

			return rc;
		}

		#endregion

		#region Class GameBase

		public GameBase()
		{
			IsUidRecycled = true;

			Name = "";

			Desc = "";
		}

		#endregion

		#endregion
	}
}
