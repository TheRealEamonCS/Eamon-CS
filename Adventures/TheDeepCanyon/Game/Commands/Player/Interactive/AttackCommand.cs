
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// ATTACK elephants

			if (eventType == EventType.BeforeAttackMonster && DobjMonster.Uid == 24)
			{
				gOut.Print("As you try to attack, one of the elephants takes your weapon.");

				var weaponArtifact = gADB[ActorMonster.Weapon];

				Debug.Assert(weaponArtifact != null);

				gOut.EnableOutput = false;

				var dropCommand = Globals.CreateInstance<IDropCommand>(x =>
				{
					x.ActorMonster = ActorMonster;

					x.ActorRoom = ActorRoom;

					x.Dobj = weaponArtifact;
				});

				dropCommand.Execute();

				gOut.EnableOutput = true;

				weaponArtifact.SetInLimbo();

				GotoCleanup = true;
			}
		}
	}
}
