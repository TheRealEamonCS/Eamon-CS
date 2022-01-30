
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheWayfarersInn.Game.Plugin.PluginContext;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override bool IsPlayerEnabled 
		{
			get
			{
				var result = base.IsPlayerEnabled;

				// Disable (nearly) all Commands if MatureContent setting is false; player must opt in

				if (!(Command is IQuitCommand || Command is IRestoreCommand || Command is ISettingsCommand) && !gGameState.MatureContent)
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
	}
}
