
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class ReadyCommand : EamonRT.Game.Commands.ReadyCommand, IReadyCommand
	{
		public ReadyCommand()
		{
			ArtTypes = new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon, ArtifactType.Wearable };
		}
	}
}
