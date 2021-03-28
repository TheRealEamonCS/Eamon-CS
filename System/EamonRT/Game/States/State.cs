
// State.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	public abstract class State : IState
	{
		#region Private Properties

		private IStateImpl StateImpl { get; set; }

		#endregion

		#region Public Properties

		#region Interface IStateSignatures

		public virtual long Uid
		{
			get
			{
				return StateImpl.Uid;
			}

			set
			{
				StateImpl.Uid = value;
			}
		}

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

		public virtual void PrintEnemiesNearby()
		{
			StateImpl.PrintEnemiesNearby();
		}

		public virtual void ProcessEvents(EventType eventType)
		{
			StateImpl.ProcessEvents(eventType);
		}

		public virtual void ProcessRevealContentArtifactList(bool printOutput = true)
		{
			StateImpl.ProcessRevealContentArtifactList(printOutput);
		}

		public virtual string GetDarkName(IGameBase target, ArticleType articleType, string nameType, bool upshift, bool groupCountOne)
		{
			return StateImpl.GetDarkName(target, articleType, nameType, upshift, groupCountOne);
		}

		public virtual bool ShouldPreTurnProcess()
		{
			return StateImpl.ShouldPreTurnProcess();
		}

		public virtual void Execute()
		{
			StateImpl.Execute();
		}

		public virtual void PreExecute()
		{
			StateImpl.PreExecute();
		}

		#endregion

		#region Class State

		public State()
		{
			StateImpl = Globals.CreateInstance<IStateImpl>(x =>
			{
				x.State = this;
			});
		}

		#endregion

		#endregion
	}
}
