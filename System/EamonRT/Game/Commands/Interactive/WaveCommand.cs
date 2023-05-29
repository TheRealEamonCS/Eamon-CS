
// WaveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class WaveCommand : Command, IWaveCommand
	{
		/// <summary></summary>
		public virtual IList<IMonster> WavingMonsterList { get; set; }

		public override void ExecuteForPlayer()
		{
			WavingMonsterList = gEngine.GetEmotingMonsterList(ActorRoom, ActorMonster, false);

			if (WavingMonsterList.Count <= 0)
			{
				PrintOkay();

				goto Cleanup;
			}

			foreach (var monster in WavingMonsterList)
			{
				gEngine.PrintMonsterEmotes(monster, false);
			}

			gOut.WriteLine();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public WaveCommand()
		{
			SortOrder = 315;

			Name = "WaveCommand";

			Verb = "wave";

			Type = CommandType.Interactive;
		}
	}
}
