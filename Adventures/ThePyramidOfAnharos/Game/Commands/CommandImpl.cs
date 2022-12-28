
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override bool IsDarkEnabled 
		{
			get
			{
				var result = base.IsDarkEnabled;

				// LookCommand / ExamineCommand enabled in Black room

				if ((Command is ILookCommand || Command is IExamineCommand) && ActorRoom.Uid == 39)
				{
					result = true;
				}

				return result;
			}

			set
			{
				base.IsDarkEnabled = value;
			}
		}

		public override void PrintYouSeeNothingSpecial()
		{
			// Suppress message in Black room

			if (ActorRoom.Uid != 39)
			{
				base.PrintYouSeeNothingSpecial();
			}
		}
	}
}
