
// HokasTokasMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class HokasTokasMenu : Menu, IHokasTokasMenu
	{
		/// <summary></summary>
		public virtual double? Rtio { get; set; }

		public override void Execute()
		{
			ISpell spell;
			RetCode rc;
			long i;

			gOut.Print("{0}", Globals.LineSep);

			/* 
				Full Credit:  Derived wholly from Donald Brown's Classic Eamon

				File: MAIN HALL
				Line: 3010
			*/

			if (Rtio == null)
			{
				var c2 = Globals.Character.GetMerchantAdjustedCharisma();

				Rtio = gEngine.GetMerchantRtio(c2);
			}

			Buf.Clear();

			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				i = (long)sv;

				spell = gEngine.GetSpells(sv);

				Debug.Assert(spell != null);

				Buf.AppendFormat("{0}{1}{2} for {3}",
					i == (long)spellValues[0] ? "" : i == (long)spellValues[spellValues.Count - 1] ? " and " : ", ",
					gEngine.GetMerchantAskPrice(spell.HokasPrice, (double)Rtio),
					i == (long)spellValues[0] ? " gold pieces" : "",
					spell.HokasName != null ? spell.HokasName : spell.Name);
			}

			var spellStr = Buf.ToString();

			gOut.Write("{0}After a few minutes of diligent searching, you find Hokas Tokas, the old mage.  He looks at you and says, \"So you want old Tokey to teach you some magic, heh heh?  Well, it'll cost you.{0}{0}Today my fees are {1}.{0}{0}Well, which will it be?\"{0}", Environment.NewLine, spellStr);

			gOut.Print("{0}", Globals.LineSep);

			Buf.Clear();

			foreach (var sv in spellValues)
			{
				i = (long)sv;

				spell = gEngine.GetSpells(sv);

				Debug.Assert(spell != null);

				Buf.AppendFormat("{0}{1}{2}={3}{4}",
					i == (long)spellValues[0] ? Environment.NewLine : "",
					i != (long)spellValues[0] ? ", " : "",
					i,
					spell.HokasName != null ? spell.HokasName : spell.Name,
					i == (long)spellValues[spellValues.Count - 1] ? ", X=Exit: " : "");
			}

			gOut.Write("{0}", Buf);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharSpellTypeOrX, gEngine.IsCharSpellTypeOrX);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			gOut.Print("{0}", Globals.LineSep);

			if (Buf.Length == 0 || Buf[0] == 'X')
			{
				gOut.Print("As you leave, you hear Hokas muttering about cheapskate adventurers always wanting something for nothing.");

				Globals.In.KeyPress(Buf);

				goto Cleanup;
			}

			i = Convert.ToInt64(Buf.Trim().ToString());

			if (Globals.Character.GetSpellAbilities(i) > 0)
			{
				gOut.Write("{0}Hokas says, \"I ought to take your gold anyway, but haven't you forgotten something?  I already taught you that spell!\"{0}{0}Shaking his head sadly, he returns to the bar.{0}", Environment.NewLine);

				Globals.In.KeyPress(Buf);

				goto Cleanup;
			}

			spell = gEngine.GetSpells((Spell)i);

			Debug.Assert(spell != null);

			var ap = gEngine.GetMerchantAskPrice(spell.HokasPrice, (double)Rtio);

			if (ap > Globals.Character.HeldGold)
			{
				gOut.Print("When Hokas sees that you don't have enough to pay him, he stalks to the bar, muttering about youngsters who should be turned into frogs.");

				Globals.In.KeyPress(Buf);

				goto Cleanup;
			}

			Globals.Character.HeldGold -= ap;

			var sa = gEngine.RollDice(1, 51, 24);

			sa += (long)Math.Round((51.0 / 100.0) * (double)Globals.Character.GetIntellectBonusPct());

			Debug.Assert(sa >= 1 && sa <= 100);

			Globals.Character.SetSpellAbilities(i, sa);

			Globals.CharactersModified = true;

			gOut.Print("Hokas teaches you your spell, takes his fee, and returns to his stool at the bar.  As you walk away you hear him order a double dragon blomb.");

			Globals.In.KeyPress(Buf);

		Cleanup:

			;
		}

		public HokasTokasMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
