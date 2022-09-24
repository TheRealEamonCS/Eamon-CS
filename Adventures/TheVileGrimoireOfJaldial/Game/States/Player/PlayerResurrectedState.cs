
// PlayerResurrectedState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class PlayerResurrectedState : EamonRT.Game.States.State, Framework.States.IPlayerResurrectedState
	{
		public override void Execute()
		{
			var charMonster = gCharMonster;

			Debug.Assert(charMonster != null);

			var room = charMonster.GetInRoom();

			Debug.Assert(room != null);

			gEngine.ClearActionLists();

			// gSentenceParser.PrintDiscardingCommands() not called for this abrupt reality shift

			gSentenceParser.Clear();

			gEngine.PrintEffectDesc(109);

			gGameState.Die = 0;

			charMonster.DmgTaken = 0;

			gEngine.ResetMonsterStats(charMonster);

			var artifactList = charMonster.GetContainedList();

			gOut.EnableOutput = false;

			foreach (var artifact in artifactList)
			{
				if (gEngine.RollDice(1, 100, 0) < 25)
				{
					if (artifact.IsWornByCharacter())
					{
						Globals.CurrState = Globals.CreateInstance<IRemoveCommand>(x =>
						{
							x.ActorMonster = charMonster;

							x.ActorRoom = room;

							x.Dobj = artifact;
						});

						Globals.CurrCommand.Execute();
					}

					Globals.CurrState = Globals.CreateInstance<IDropCommand>(x =>
					{
						x.ActorMonster = charMonster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					Globals.CurrCommand.Execute();

					Globals.CurrState = this;
				}
			}

			gOut.EnableOutput = true;

			gGameState.Ro = 19;

			gGameState.R2 = gGameState.Ro;

			NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
			{
				x.MoveMonsters = false;
			});

			Globals.NextState = NextState;
		}

		public PlayerResurrectedState()
		{
			Name = "PlayerResurrectedState";
		}
	}
}
