
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override bool IsPlayerEnabled
		{
			get
			{
				var result = base.IsPlayerEnabled;

				// Enable various Commands disabled by default for Ruleset Version 62 (DDD 6.2)

				if (Command is ICloseCommand || Command is IRemoveCommand || Command is IWearCommand)
				{
					result = true;
				}

				// Disable various Commands enabled by default for Ruleset Version 62 (DDD 6.2)

				if (Command is IDrinkCommand || Command is ILightCommand)
				{
					result = false;
				}

				return result;
			}

			set
			{
				base.IsPlayerEnabled = value;
			}
		}

		public override void PrintCantVerbObj(IGameBase obj)
		{
			Debug.Assert(obj != null);

			// Small treasure chest

			if (Command is IOpenCommand && obj is IArtifact artifact && artifact.Uid == 44)
			{
				gOut.Print("You would only make a mess. Wait until you leave.");
			}

			// Non-wearable

			else if (Command is IWearCommand && obj is IArtifact artifact02 && artifact02.Wearable == null)
			{
				gOut.Print("You can't wear that and stay respectable.");
			}
			else
			{
				base.PrintCantVerbObj(obj);
			}
		}
	}
}
