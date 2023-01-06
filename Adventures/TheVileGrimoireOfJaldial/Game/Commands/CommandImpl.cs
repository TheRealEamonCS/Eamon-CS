
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

		public override void PrintTooHeavy(IArtifact artifact, bool getAll = false)
		{
			gEngine.PushRulesetVersion(0);

			base.PrintTooHeavy(artifact, getAll);

			gEngine.PopRulesetVersion();
		}

		public override void PrintReceived(IArtifact artifact)
		{
			gEngine.PushRulesetVersion(0);

			base.PrintReceived(artifact);

			gEngine.PopRulesetVersion();
		}

		public override void PrintRetrieved(IArtifact artifact)
		{
			gEngine.PushRulesetVersion(0);

			base.PrintRetrieved(artifact);

			gEngine.PopRulesetVersion();
		}

		public override void PrintTaken(IArtifact artifact, bool getAll = false)
		{
			gEngine.PushRulesetVersion(0);

			base.PrintTaken(artifact, getAll);

			gEngine.PopRulesetVersion();
		}

		public override void PrintReadied(IArtifact artifact)
		{
			gEngine.PushRulesetVersion(0);

			base.PrintReadied(artifact);

			gEngine.PopRulesetVersion();
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

		public override void PrintNobodyHereByThatName()
		{
			gEngine.PushRulesetVersion(0);

			base.PrintNobodyHereByThatName();

			gEngine.PopRulesetVersion();
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			return !gGameState.ParalyzedTargets.ContainsKey(gGameState.Cm) && base.ShouldShowUnseenArtifacts(room, artifact);
		}
	}
}
