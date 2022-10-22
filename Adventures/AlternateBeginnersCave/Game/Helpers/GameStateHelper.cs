
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Game.Helpers
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
		public virtual bool ValidateMagicDaggerCounter()
		{
			return Record.MagicDaggerCounter >= -1 && Record.MagicDaggerCounter <= 35;
		}

		public GameStateHelper()
		{

		}
	}
}
