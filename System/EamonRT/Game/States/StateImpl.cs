
// StateImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class StateImpl : IStateImpl
	{
		public virtual IState State { get; set; }

		public virtual bool GotoCleanup { get; set; }

		public virtual string Name { get; set; }

		public virtual IState NextState { get; set; }

		public virtual bool PreserveNextState { get; set; }

		public virtual void PrintObjBlocksTheWay(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} block{1} the way!", artifact.GetTheName(true), artifact.EvalPlural("s", ""));
		}

		public virtual void PrintLightAlmostOut(IArtifact artifact, long turnCounter)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0}{1}", artifact.GetTheName(true), turnCounter <= 10 ? " is almost out!" : " grows dim!");
		}

		public virtual void PrintCommands(IList<ICommand> commandList, CommandType commandType, ref bool newSeen)
		{
			Debug.Assert(commandList != null && Enum.IsDefined(typeof(CommandType), commandType));

			switch(commandType)
			{
				case CommandType.Movement:

					gOut.Print("Movement Commands:");

					break;

				case CommandType.Manipulation:

					gOut.Print("Artifact Manipulation:");

					break;

				case CommandType.Interactive:

					gOut.Print("Interactive:");

					break;

				case CommandType.Miscellaneous:

					gOut.Print("Miscellaneous:");

					break;
			}

			gEngine.Buf.Clear();

			var rc = gEngine.BuildCommandList(commandList, commandType, gEngine.Buf, ref newSeen);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Write("{0}", gEngine.Buf);
		}

		public virtual void PrintNewCommandSeen()
		{
			gOut.Print("(*) New Command");
		}

		public virtual void PrintCommandPrompt()
		{
			gOut.Write("{0}{1}", Environment.NewLine, gEngine.CommandPrompt);
		}

		public virtual void PrintFulfillMessage(string inputStr)
		{
			Debug.Assert(inputStr != null);

			gOut.Print("{{Fulfilling:  \"{0}\"}}", inputStr);
		}

		public virtual void PrintDontFollowYou()
		{
			gOut.Print("I don't follow you.");
		}

		public virtual void PrintDontFollowYou02()
		{
			State.PrintDontFollowYou();
		}

		public virtual void PrintCantGoThatWay()
		{
			gOut.Print("You can't go that way!");
		}

		public virtual void PrintCantVerbThere(string verb)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(verb));

			gOut.Print("You can't {0} there.", verb);
		}

		public virtual void PrintRideOffIntoSunset()
		{
			gOut.Print("You successfully ride off into the sunset.");
		}

		public virtual void PrintLeaveAdventure()
		{
			gOut.Write("{0}Leave this adventure (Y/N): ", Environment.NewLine);
		}

		public virtual void PrintEnemiesNearby()
		{
			gOut.Print("You can't do that with unfriendlies about!");
		}

		public virtual void PrintSpeedSpellExpired()
		{
			gOut.Print("Your speed spell has{0} expired!", gEngine.IsRulesetVersion(5, 15, 25) ? " just" : "");
		}

		public virtual void ProcessEvents(EventType eventType)
		{

		}

		public virtual string GetDarkName(IGameBase target, ArticleType articleType, string nameType, bool upshift, bool groupCountOne)
		{
			string result = null;

			Debug.Assert(target != null);

			Debug.Assert(Enum.IsDefined(typeof(ArticleType), articleType));

			var artifact = target as IArtifact;

			if (artifact != null)
			{
				switch (articleType)
				{
					case ArticleType.None:

						result = artifact.EvalPlural("unseen object", "unseen objects");

						break;

					case ArticleType.A:
					case ArticleType.An:

						result = artifact.EvalPlural("an unseen object", "unseen objects");

						break;

					case ArticleType.Some:

						result = artifact.EvalPlural("an unseen object", "some unseen objects");

						break;

					case ArticleType.The:

						result = artifact.EvalPlural("the unseen object", "the unseen objects");

						break;
				}
			}
			else
			{
				var monster = target as IMonster;

				Debug.Assert(monster != null);

				switch (articleType)
				{
					case ArticleType.None:

						result = groupCountOne ? "unseen entity" : monster.EvalPlural("unseen entity", "unseen entities");

						break;

					case ArticleType.A:
					case ArticleType.An:

						result = groupCountOne ? "an unseen entity" : monster.EvalPlural("an unseen entity", "unseen entities");

						break;

					case ArticleType.Some:

						result = groupCountOne ? "an unseen entity" : monster.EvalPlural("an unseen entity", "some unseen entities");

						break;

					case ArticleType.The:

						result = groupCountOne ? "the unseen entity" : monster.EvalPlural("the unseen entity", "the unseen entities");

						break;
				}
			}

			if (upshift && result != null)
			{
				result = result.FirstCharToUpper();
			}

			return result;
		}

		public virtual IList<long> GetLoopMonsterUidList()
		{
			return gEngine.Database.MonsterTable.Records.OrderBy(m => m.Uid).Select(m01 => m01.Uid).ToList();
		}

		public virtual void Stage()
		{
			State.Execute();
		}

		public virtual void Execute()
		{

		}

		public StateImpl()
		{
			// Here we make an exception to the "always use State" rule

			Name = "";
		}
	}
}
