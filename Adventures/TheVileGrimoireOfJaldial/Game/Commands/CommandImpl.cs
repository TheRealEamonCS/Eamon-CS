
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override bool IsPlayerEnabled 
		{
			get
			{
				return base.IsPlayerEnabled || Command is IFreeCommand || Command is IRequestCommand || Command is ICloseCommand || Command is IDrinkCommand || Command is IEatCommand || Command is ILightCommand || Command is IOpenCommand || Command is IPutCommand || Command is IReadCommand || Command is IRemoveCommand || Command is IUseCommand || Command is IWearCommand || Command is IStatusCommand || Command is IGoCommand;
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

		public override void PrintLightObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 1)
			{
				gOut.Print("A magical flame bursts from {0}.", artifact.GetTheName());
			}
			else
			{
				base.PrintLightObj(artifact);
			}
		}

		public override void PrintLightExtinguished(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 1)
			{
				gOut.Print("The fire is violently extinguished.");
			}
			else
			{
				base.PrintLightExtinguished(artifact);
			}
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			return !gGameState.ParalyzedTargets.ContainsKey(gGameState.Cm) && base.ShouldShowUnseenArtifacts(room, artifact);
		}
	}
}
