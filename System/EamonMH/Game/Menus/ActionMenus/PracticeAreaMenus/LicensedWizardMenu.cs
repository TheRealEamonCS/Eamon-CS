
// LicensedWizardMenu.cs

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
	public class LicensedWizardMenu : Menu, ILicensedWizardMenu
	{
		/// <summary></summary>
		public virtual double? Rtio { get; set; }

		public override void Execute()
		{
			ISpell spell;
			RetCode rc;
			long ap = 0;
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

			gOut.Print("\"Ah, so.  Welcome to my shop, oh {0} Adventurer.  May the blessings of the gods be yours.\"", Globals.Character.EvalGender("Mighty", "Fair", "Androgynous"));

			gOut.Print("\"And what mystical prowess do you wish this humble one to impart to one of your magnificence?\"");

			gOut.Print("{0}", Globals.LineSep);

			Buf.Clear();

			var spellValues = EnumUtil.GetValues<Spell>();

			for (i = 0; i < spellValues.Count; i++)
			{
				spell = gEngine.GetSpells(spellValues[(int)i]);

				Debug.Assert(spell != null);

				Buf.AppendFormat("{0}{1}{2}={3}{4}",
					i == 0 ? Environment.NewLine : "",
					i != 0 ? ", " : "",
					(long)spellValues[(int)i],
					spell.HokasName ?? spell.Name,
					i == spellValues.Count - 1 ? ": " : "");
			}

			gOut.Write("{0}", Buf);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, null, gEngine.IsCharSpellType, gEngine.IsCharSpellType);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			gOut.Print("{0}", Globals.LineSep);

			Debug.Assert(Buf.Length > 0);

			i = Convert.ToInt64(Buf.Trim().ToString());

			spell = gEngine.GetSpells((Spell)i);

			Debug.Assert(spell != null);

			if (Globals.Character.GetSpellAbilities(i) == 0)
			{
				gOut.Print("\"You will have to first buy that spell from the mage in the Main Hall!\"");

				goto Cleanup;
			}

			ap = gEngine.GetMerchantAskPrice(Constants.SpellTrainingPrice, (double)Rtio);

			gOut.Print("\"So you wish to learn how to use your spell more effectively!  My fee is {0} gold piece{1} for every try.\"", ap, ap != 1 ? "s" : "");

			gOut.Print("The mystic teaches you to concentrate while casting spells and says, \"Now cast your spell while I offer suggestions from nearby.\"");

			while (true)
			{
				Globals.In.KeyPress(Buf);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("Ability: {0}        Gold: {1}", Globals.Character.GetSpellAbilities(i), Globals.Character.HeldGold);

				if (Globals.Character.HeldGold >= ap)
				{
					gOut.Write("{0}1=Cast, 2=Rest, X=Exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsChar1Or2OrX, gEngine.IsChar1Or2OrX);

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					gOut.Print("{0}", Globals.LineSep);

					if (Buf.Length == 0 || Buf[0] == 'X')
					{
						break;
					}
					else if (Buf[0] == '1')
					{
						var rl = gEngine.RollDice(1, 100, 0);

						if (rl > 90)
						{
							gOut.Print("\"Perfect cast!  Incredible!\"");

							Globals.Character.ModSpellAbilities(i, 4);
						}
						else if (rl > 50)
						{
							gOut.Print("\"Success!  A good cast!\"");

							Globals.Character.ModSpellAbilities(i, 2);
						}
						else
						{
							gOut.Print("\"Nothing?  Concentrate!\"");
						}

						if (Globals.Character.GetSpellAbilities(i) > spell.MaxValue)
						{
							Globals.Character.SetSpellAbilities(i, spell.MaxValue);
						}

						Globals.Character.HeldGold -= ap;

						Globals.CharactersModified = true;
					}
					else
					{
						gOut.Print("GASP ... MUMBLE ... GASP.  \"Breathe deeply!\"");
					}
				}
				else
				{
					gOut.Print("\"Sorry, but my fee exceeds your assets!\"");

					break;
				}
			}

		Cleanup:

			gOut.Print("\"My ancient ancestors thank you for your excellent patronage!  May your magicks be always successful!\"");

			Globals.In.KeyPress(Buf);
		}

		public LicensedWizardMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
