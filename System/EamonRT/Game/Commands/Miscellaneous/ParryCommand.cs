
// ParryCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
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
				OldParry = ActorMonster.Parry;

				ActorMonster.Parry = Parry;

				if (PrintCombatStanceChanged)
				{
					PrintAssumeCombatStance(ActorMonster);
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
			}
			else
			{
				PrintCombatStance(DobjMonster != null ? DobjMonster : ActorMonster);

				NextState = gEngine.CreateInstance<IStartState>();
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public override void ExecuteForMonster()
		{
			if (Parry >= 0 && Parry <= 100)
			{
				ActorMonster.Parry = Parry;

				Debug.Assert(gCharMonster != null);

				if (PrintCombatStanceChanged && gCharMonster.IsInRoom(ActorRoom))
				{
					if (ActorRoom.IsViewable())
					{
						PrintAssumeCombatStance(ActorMonster);
					}
					else
					{
						PrintAssumeCombatStance01(ActorMonster);
					}
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
			SortOrder = 345;

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
