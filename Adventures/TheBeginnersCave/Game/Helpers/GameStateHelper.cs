
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace TheBeginnersCave.Game.Helpers
{
	[ClassMappings]
	public class GameStateHelper : Eamon.Game.Helpers.GameStateHelper, IGameStateHelper
	{
		public virtual new Framework.IGameState Record
		{
			get
			{
				return (Framework.IGameState)base.Record;
			}

			set
			{
				if (base.Record != value)
				{
					base.Record = value;
				}
			}
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateTrollsfire()
		{
			return Record.Trollsfire >= 0 && Record.Trollsfire <= 1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateBookWarning()
		{
			return Record.BookWarning >= 0 && Record.BookWarning <= 1;
		}

		public GameStateHelper()
		{
			FieldNameList.AddRange(new List<string>()
			{
				"Trollsfire",
				"BookWarning",
			});
		}
	}
}
