
// GameBase.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Polenter.Serialization;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game
{
	public abstract class GameBase : IGameBase
	{
		#region Public Properties

		#region Interface IGameBase

		[FieldName(100)]
		public virtual long Uid { get; set; }

		[FieldName(300)]
		public virtual string Name { get; set; }

		[FieldName(400)]
		public virtual string Desc { get; set; }

		public virtual string[] Synonyms { get; set; }

		[FieldName(500)]
		public virtual bool Seen { get; set; }

		[FieldName(510)]
		public virtual bool Moved { get; set; }

		[FieldName(600)]
		public virtual ArticleType ArticleType { get; set; }

		[ExcludeFromSerialization]
		[ExcludeFromDeepCopy]
		public virtual string ParserMatchName { get; set; }

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
			// Do nothing
		}

		public virtual string GetPluralName(string fieldName)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			var buf = new StringBuilder(gEngine.BufSize);

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetPluralName01()
		{
			return GetPluralName("Name");
		}

		public virtual string GetDecoratedName(string fieldName, ArticleType articleType, bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool showContents = false, bool groupCountOne = false)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			var buf = new StringBuilder(gEngine.BufSize);

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetNoneName(bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool showContents = false, bool groupCountOne = false)
		{
			return GetDecoratedName("Name", ArticleType.None, upshift, showCharOwned, showStateDesc, showContents, groupCountOne);
		}

		public virtual string GetArticleName(bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool showContents = false, bool groupCountOne = false)
		{
			return GetDecoratedName("Name", ArticleType, upshift, showCharOwned, showStateDesc, showContents, groupCountOne);
		}

		public virtual string GetTheName(bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool showContents = false, bool groupCountOne = false)
		{
			return GetDecoratedName("Name", ArticleType.The, upshift, showCharOwned, showStateDesc, showContents, groupCountOne);
		}

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName, bool showVerboseName)
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
			Name = "";

			Desc = "";
		}

		#endregion

		#endregion
	}
}
