
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using EamonRT.Framework.Commands;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game
{
	[ClassMappings]
	public class MainLoop : IMainLoop
	{
		public virtual StringBuilder Buf { get; set; }

		public virtual bool ShouldStartup { get; set; }

		public virtual bool ShouldShutdown { get; set; }

		public virtual bool ShouldExecute { get; set; }

		public virtual void Startup()
		{
			gEngine.EnforceCharacterWeightLimits();

			var monster = gEngine.ConvertCharacterToMonster();

			Debug.Assert(monster != null);

			gGameState.Cm = monster.Uid;

			Debug.Assert(gGameState.Cm > 0);

			gEngine.NormalizeArtifactValuesAndWeights();

			gEngine.AddUniqueCharsToArtifactAndMonsterNames();

			gEngine.AddMissingDescs();

			gGameState.Ro = gEngine.StartRoom;

			gGameState.R2 = gEngine.StartRoom;

			gGameState.R3 = gEngine.StartRoom;

			gEngine.InitSaArray();

			gEngine.CreateCommands();

			gEngine.InitRooms();

			gEngine.InitArtifacts();

			gEngine.InitMonsters();

			gEngine.Module.NumArtifacts = gDatabase.GetArtifactCount();

			gEngine.Module.NumMonsters = gDatabase.GetMonsterCount();

			gEngine.CreateInitialState(false);
		}

		public virtual void Shutdown()
		{
			var weaponList = new List<IArtifact>();

			gEngine.SetArmorClass();

			gEngine.ConvertToCarriedInventory(weaponList);

			gEngine.SellExcessWeapons(weaponList);

			Debug.Assert(gCharMonster != null);

			gEngine.ConvertMonsterToCharacter(gCharMonster, weaponList);

			gEngine.SellInventoryToMerchant();
		}

		public virtual void Execute()
		{
			gOut.WriteLine("{0}{1}{0}", Environment.NewLine, gEngine.LineSep);

			gEngine.PrintWakingUpMonsters();

			gEngine.Thread.Sleep(3000);

			gOut.WriteLine("{0}{1}{0}", Environment.NewLine, gEngine.LineSep);

			gEngine.PrintBaseProgramVersion();

			gEngine.PrintWelcomeToEamonCS();

			gOut.Print("{0}", gEngine.LineSep);

			while (gEngine.GameRunning)
			{
				Debug.Assert(gEngine.CurrState != null);

				var command = gEngine.CurrState as ICommand;

				if (command != null && command.ActorMonster.IsCharacterMonster())
				{
					gEngine.LastCommandList.Add(command);
				}

				gEngine.CurrState.Stage();

				if (--gEngine.ActionListCounter < 0)
				{
					gEngine.CheckToProcessActionLists();

					gEngine.ActionListCounter = 0;
				}

				gEngine.CurrState = gEngine.NextState;

				gEngine.NextState = null;
			}
		}

		public MainLoop()
		{
			Buf = gEngine.Buf;

			ShouldStartup = true;

			ShouldShutdown = true;

			ShouldExecute = true;
		}
	}
}
