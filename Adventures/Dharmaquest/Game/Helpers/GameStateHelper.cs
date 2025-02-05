
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.Helpers
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
		public virtual bool ValidateBlackWizardName()
		{
			return !string.IsNullOrWhiteSpace(Record.BlackWizardName);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateKarma()
		{
			return Record.Karma >= 0 && Record.Karma <= 100;
		}

		public GameStateHelper()
		{

		}
	}
}
