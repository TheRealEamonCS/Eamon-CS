
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override bool IsPlayerEnabled 
		{
			get
			{
				return base.IsPlayerEnabled || Command is IFreeCommand || Command is ICloseCommand || Command is IDrinkCommand || Command is IEatCommand || Command is ILightCommand || Command is IOpenCommand || Command is IPutCommand || Command is IReadCommand || Command is IRemoveCommand || Command is IUseCommand || Command is IWearCommand || Command is IStatusCommand || Command is IGoCommand;
			}

			set
			{
				base.IsPlayerEnabled = value;
			}
		}

		public override bool IsMonsterEnabled
		{
			get
			{
				return base.IsMonsterEnabled || Command is IMonsterRemoveCommand;
			}

			set
			{
				base.IsMonsterEnabled = value;
			}
		}
	}
}
