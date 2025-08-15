
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override bool IsPlayerEnabled
		{
			get
			{
				var result = base.IsPlayerEnabled;

				// Disable various Commands

				if (Command is IBlastCommand || Command is IHealCommand || Command is ISpeedCommand || Command is IPowerCommand || Command is ISmileCommand || Command is IWaveCommand || Command is ISayCommand)
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

		public override void PrintMustBeFreed(IArtifact artifact)
		{
			// Garbage monster

			if (artifact?.Uid == 23)
			{
				gOut.Print("The muffled voice must be freed.");
			}
			else
			{
				base.PrintMustBeFreed(artifact);
			}
		}

		public override void PrintWorn(IArtifact artifact)
		{
			// Uniform

			if (artifact?.Uid == 33)
			{
				gOut.Print("You are now wearing the uniform.");
			}
			else
			{
				base.PrintWorn(artifact);
			}
		}

		public override void PrintRemoved(IArtifact artifact)
		{
			// Uniform

			if (artifact?.Uid == 33)
			{
				gOut.Print("You are no longer in uniform.");
			}
			else
			{
				base.PrintRemoved(artifact);
			}
		}
	}
}
