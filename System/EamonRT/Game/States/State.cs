
// State.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	public abstract class State : IState
	{
		#region Private Properties

		private IStateImpl StateImpl { get; set; }

		#endregion

		#region Public Properties

		#region Interface IStateSignatures

		public virtual bool GotoCleanup
		{
			get
			{
				return StateImpl.GotoCleanup;
			}

			set
			{
				StateImpl.GotoCleanup = value;
			}
		}

		public virtual string Name
		{
			get
			{
				return StateImpl.Name;
			}

			set
			{
				StateImpl.Name = value;
			}
		}

		public virtual IState NextState
		{
			get
			{
				return StateImpl.NextState;
			}

			set
			{
				StateImpl.NextState = value;
			}
		}

		public virtual bool PreserveNextState
		{
			get
			{
				return StateImpl.PreserveNextState;
			}

			set
			{
				StateImpl.PreserveNextState = value;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IStateSignatures

		public virtual void PrintObjBlocksTheWay(IArtifact artifact)
		{
			StateImpl.PrintObjBlocksTheWay(artifact);
		}

		public virtual void PrintLightAlmostOut(IArtifact artifact, long turnCounter)
		{
			StateImpl.PrintLightAlmostOut(artifact, turnCounter);
		}

		public virtual void PrintCommands(IList<ICommand> commandList, CommandType commandType, ref bool newSeen)
		{
			StateImpl.PrintCommands(commandList, commandType, ref newSeen);
		}

		public virtual void PrintNewCommandSeen()
		{
			StateImpl.PrintNewCommandSeen();
		}

		public virtual void PrintCommandPrompt()
		{
			StateImpl.PrintCommandPrompt();
		}

		public virtual void PrintFulfillMessage(string inputStr)
		{
			StateImpl.PrintFulfillMessage(inputStr);
		}

		public virtual void PrintDontFollowYou()
		{
			StateImpl.PrintDontFollowYou();
		}

		public virtual void PrintDontFollowYou02()
		{
			StateImpl.PrintDontFollowYou02();
		}

		public virtual void PrintCantGoThatWay()
		{
			StateImpl.PrintCantGoThatWay();
		}

		public virtual void PrintCantVerbThere(string verb)
		{
			StateImpl.PrintCantVerbThere(verb);
		}

		public virtual void PrintRideOffIntoSunset()
		{
			StateImpl.PrintRideOffIntoSunset();
		}

		public virtual void PrintLeaveAdventure()
		{
			StateImpl.PrintLeaveAdventure();
		}

		public virtual void PrintEnemiesNearby()
		{
			StateImpl.PrintEnemiesNearby();
		}

		public virtual void PrintSpeedSpellExpired()
		{
			StateImpl.PrintSpeedSpellExpired();
		}

		public virtual void BeforePrintCommands()
		{
			StateImpl.BeforePrintCommands();
		}

		public virtual void AfterPrintCommands()
		{
			StateImpl.AfterPrintCommands();
		}

		public virtual void ProcessEvents(EventType eventType)
		{
			StateImpl.ProcessEvents(eventType);
		}

		public virtual string GetDarkName(IGameBase target, ArticleType articleType, string nameType, bool upshift, bool groupCountOne)
		{
			return StateImpl.GetDarkName(target, articleType, nameType, upshift, groupCountOne);
		}

		public virtual IList<long> GetLoopMonsterUidList()
		{
			return StateImpl.GetLoopMonsterUidList();
		}

		public virtual void Stage()
		{
			StateImpl.Stage();
		}

		public virtual void Execute()
		{
			StateImpl.Execute();
		}

		#endregion

		#region Class State

		public State()
		{
			StateImpl = gEngine.CreateInstance<IStateImpl>(x =>
			{
				x.State = this;
			});
		}

		#endregion

		#endregion
	}
}
