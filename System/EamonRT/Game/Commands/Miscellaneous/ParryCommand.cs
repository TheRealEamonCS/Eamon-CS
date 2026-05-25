
// ParryCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class ParryCommand : Command, IParryCommand
	{
		public virtual long Parry { get; set; }

		public virtual bool PrintCombatStanceChanged { get; set; }

		/// <summary></summary>
		public virtual long OldParry { get; set; }

		public override void ExecuteForPlayer()
		{
			if (Parry >= 0 && Parry <= 100)
			{
				ProcessEvents(EventType.BeforeAdjustParry);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				OldParry = ActorMonster.Parry;

				ActorMonster.Parry = Parry;

				if (PrintCombatStanceChanged)
				{
					PrintTakeCombatStance(ActorMonster);
				}

				if (ActorMonster.ShouldCombatStanceChangedConsumeTurn(OldParry, Parry))
				{
					if (ActorMonster.CheckNBTLHostility())
					{
						gEngine.PauseCombat();
					}
				} 
				else
				{ 
					NextState = gEngine.CreateInstance<IStartState>();
				}

				ProcessEvents(EventType.AfterAdjustParry);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}
			else
			{
				gEngine.ShouldPreTurnProcess = false;

				ProcessEvents(EventType.BeforeCheckParry);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				PrintCombatStance(DobjMonster != null ? DobjMonster : ActorMonster);

				NextState = gEngine.CreateInstance<IStartState>();

				ProcessEvents(EventType.AfterCheckParry);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public override void ExecuteForMonster()
		{
			if (Parry >= 0 && Parry <= 100)
			{
				OldParry = ActorMonster.Parry;
	
				ActorMonster.Parry = Parry;

				Debug.Assert(gCharMonster != null);

				if (PrintCombatStanceChanged && gCharMonster.IsInRoom(ActorRoom))
				{
					if (ActorRoom.IsViewable())
					{
						PrintTakeCombatStance(ActorMonster);
					}
					else
					{
						PrintTakeCombatStance01(ActorMonster);
					}
				}
				
				if (ActorMonster.ShouldCombatStanceChangedConsumeTurn(OldParry, Parry))
				{
					if (ActorMonster.CheckNBTLHostility())
					{
						gEngine.PauseCombat();
					}

					GotoCleanup = true;
				}
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public ParryCommand()
		{
			SortOrder = 343;

			IsDarkEnabled = true;

			IsPlayerEnabled = false;

			IsMonsterEnabled = false;

			Name = "ParryCommand";

			Verb = "parry";

			Type = CommandType.Miscellaneous;

			Parry = -1;

			PrintCombatStanceChanged = true;
		}
	}
}
