
// Hint.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Hint : GameBase, IHint
	{
		#region Public Properties

		#region Interface IHint

		[FieldName(620)]
		public virtual bool Active { get; set; }

		[FieldName(640)]
		public virtual string Question { get; set; }

		[FieldName(660)]
		public virtual long NumAnswers { get; set; }

		[FieldName(680)]
		public virtual string[] Answers { get; set; }

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
				Globals.Database.FreeHintUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IHint hint)
		{
			return this.Uid.CompareTo(hint.Uid);
		}

		#endregion

		#region Interface IHint

		public virtual string GetAnswer(long index)
		{
			return Answers[index];
		}

		public virtual void SetAnswer(long index, string value)
		{
			Answers[index] = value;
		}

		#endregion

		#region Class Hint

		public Hint()
		{
			Question = "";

			Answers = new string[]
			{
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				""
			};
		}

		#endregion

		#endregion
	}
}
