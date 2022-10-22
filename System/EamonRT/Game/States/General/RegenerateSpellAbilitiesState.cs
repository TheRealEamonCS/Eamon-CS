
// RegenerateSpellAbilitiesState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class RegenerateSpellAbilitiesState : State, IRegenerateSpellAbilitiesState
	{
		/// <summary></summary>
		public virtual IList<Spell> SpellValueList { get; set; }

		/// <summary></summary>
		public virtual long SpellIncrement { get; set; }

		public override void Execute()
		{
			if (gEngine.CommandPromptSeen && !gEngine.ShouldPreTurnProcess)
			{
				goto Cleanup;
			}

			SpellValueList = EnumUtil.GetValues<Spell>();

			foreach (var sv in SpellValueList)
			{
				if (gGameState.GetSa(sv) < gCharacter.GetSpellAbility(sv))
				{
					SpellIncrement = (long)((double)gGameState.GetSa(sv) * 1.1) - gGameState.GetSa(sv);

					if (gGameState.GetSa(sv) > 0 && SpellIncrement < 1)
					{
						SpellIncrement = 1;
					}

					gGameState.ModSa(sv, SpellIncrement);

					if (gGameState.GetSa(sv) > gCharacter.GetSpellAbility(sv))
					{
						gGameState.SetSa(sv, gCharacter.GetSpellAbility(sv));
					}
				}
			}
				
		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IBeforePrintPlayerRoomEventState>();
			}

			gEngine.NextState = NextState;
		}

		public RegenerateSpellAbilitiesState()
		{
			Name = "RegenerateSpellAbilitiesState";
		}
	}
}
