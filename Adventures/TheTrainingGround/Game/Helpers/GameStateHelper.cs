
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace TheTrainingGround.Game.Helpers
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
		public virtual bool ValidateGenderChangeCounter()
		{
			return Record.GenderChangeCounter >= 0 && Record.GenderChangeCounter <= 2;
		}

		public GameStateHelper()
		{
			FieldNameList.AddRange(new List<string>()
			{
				"GenderChangeCounter",
			});
		}
	}
}
