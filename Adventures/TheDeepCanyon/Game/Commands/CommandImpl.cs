
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
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
				return base.IsMonsterEnabled || Command is IRemoveCommand;
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

		public override void PrintDontHaveItNotHere()
		{
			gEngine.PushRulesetVersion(0);

			base.PrintDontHaveItNotHere();

			gEngine.PopRulesetVersion();
		}

		public override void PrintDontHaveIt()
		{
			gEngine.PushRulesetVersion(0);

			base.PrintDontHaveIt();

			gEngine.PopRulesetVersion();
		}

		public override bool ShouldAllowRedirectToGetCommand()
		{
			return true;
		}
	}
}
