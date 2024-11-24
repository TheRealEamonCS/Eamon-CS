
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override bool IsPlayerEnabled
		{
			get
			{
				var result = base.IsPlayerEnabled;

				// Enable various Commands disabled by default for Ruleset Version 5 (DDD5)

				if (Command is ICloseCommand || Command is IDrinkCommand || Command is IEatCommand || Command is IOpenCommand || Command is IReadCommand || Command is IRemoveCommand || Command is IWearCommand)
				{
					result = true;
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

			// Large chest / Chest / Gold coins (sacks)

			if (Command is IOpenCommand && obj is IArtifact artifact && (artifact.Uid == 17 || artifact.Uid == 22 || artifact.Uid == 25))
			{
				gOut.Print("You would only make a mess. Wait until you leave.");        // Borrowed from Beginner's Cave II
			}
			else
			{
				base.PrintCantVerbObj(obj);
			}
		}

		public override void PrintNoneLeft(IArtifact artifact)
		{
			Debug.Assert(artifact != null);
			
			// Golden cup

			if (artifact.Uid == 18)
			{
				gOut.Print("{0} {1} empty.", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));
			}
			else
			{
				base.PrintNoneLeft(artifact);
			}
		}

		public override void PrintObjAmountLeft(IArtifact artifact, long objAmount, bool objEdible)
		{
			Debug.Assert(artifact != null);

			// Golden cup

			if (artifact.Uid == 18)
			{
				gOut.Print("{0} {1} empty.", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));
			}
			else
			{
				base.PrintObjAmountLeft(artifact, objAmount, objEdible);
			}
		}
	}
}
