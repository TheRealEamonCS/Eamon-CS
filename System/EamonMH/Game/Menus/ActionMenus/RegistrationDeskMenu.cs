
// RegistrationDeskMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus.ActionMenus;
using EamonMH.Framework.Menus.HierarchicalMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class RegistrationDeskMenu : Menu, IRegistrationDeskMenu
	{
		/// <summary></summary>
		public virtual double? Rtio { get; set; }

		/// <summary></summary>
		public virtual string AdventureName { get; set; }

		/// <summary></summary>
		public virtual long NumChances { get; set; }

		public override void Execute()
		{
			SelectCharacter();
		}

		/// <summary></summary>
		public virtual void BadCharacterName()
		{
			gOut.Print("{0}", Globals.LineSep);

			NumChances--;

			gOut.Print("{0}",
				NumChances == 12 ? "He pulls out a sword and begins to sharpen it, saying \"Ye'd best be givin' me yer name laddie, if ye know wots good fer ye!!!\"" :
				NumChances == 11 ? "\"I've 'bout had me fill o' yer sick sensa 'umor!!  Now gimme yer name!!\"" :
				NumChances == 10 ? string.Format("The man cuts one of your fingers off!!  He then eats it!!!!{0}{0}Then he says, \"Are ye ready t' talk now?\"", Environment.NewLine) :
				NumChances > 0 ? "The man cuts off another finger!!!  He eats this one too!!" :
				string.Format("The man starts slowly, \"Well, ye be outta fingers!\"{0}{0}The man then spins around and runs you through with a speed you have never seen before!  (and never will again.)", Environment.NewLine));
		}

		/// <summary></summary>
		public virtual void RecallCharacterFromAdventure()
		{
			RetCode rc;

			var filesets = Globals.Database.FilesetTable.Records;

			foreach (var fileset in filesets)
			{
				rc = Globals.PushDatabase();

				Debug.Assert(gEngine.IsSuccess(rc));

				if (!string.IsNullOrWhiteSpace(fileset.FilesetFileName) && !fileset.FilesetFileName.Equals("NONE", StringComparison.OrdinalIgnoreCase))
				{
					var fsfn = Globals.Path.Combine(fileset.WorkDir, fileset.FilesetFileName);

					rc = Globals.Database.LoadFilesets(fsfn, printOutput: false);

					Debug.Assert(gEngine.IsSuccess(rc));

					RecallCharacterFromAdventure();
				}
				else
				{
					var chrfn = Globals.Path.Combine(fileset.WorkDir, "FRESHMEAT.DAT");

					if (Globals.File.Exists(chrfn))
					{
						rc = Globals.Database.LoadCharacters(chrfn, printOutput: false);

						Debug.Assert(gEngine.IsSuccess(rc));

						var character = Globals.Database.CharacterTable.Records.FirstOrDefault();

						if (character != null && character.Uid == gCharacter.Uid && character.Name.Equals(gCharacter.Name, StringComparison.OrdinalIgnoreCase))
						{
							AdventureName = fileset.Name;

							gCharacter.Status = Status.Alive;

							Globals.CharactersModified = true;

							Globals.TransferProtocol.RecallCharacterFromAdventure(fileset.WorkDir, Globals.FilePrefix, fileset.PluginFileName);
						}
					}
				}

				rc = Globals.PopDatabase();

				Debug.Assert(gEngine.IsSuccess(rc));

				if (gCharacter.Status != Status.Adventuring)
				{
					break;
				}
			}
		}

		/// <summary></summary>
		/// <param name="character"></param>
		public virtual void CreateCharacter(ICharacter character)
		{
			RetCode rc;

			Debug.Assert(character != null);

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("He hits his forehead and says, \"Ah, ye must be new here!  Well, wait just a minute and I'll bring someone out to take care of ye.\"");

			gOut.Print("The Irishman says, \"First I must know whether ye be male or female.  Which are ye?\"");

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}You give him your gender ({1}=Male, {2}=Female): ",
				Environment.NewLine,
				(long)Gender.Male,
				(long)Gender.Female);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, null, gEngine.IsChar0Or1, gEngine.IsChar0Or1);

			Debug.Assert(gEngine.IsSuccess(rc));

			character.Gender = (Gender)Convert.ToInt64(Buf.Trim().ToString());

			var helper = Globals.CreateInstance<ICharacterHelper>(x =>
			{
				x.Record = character;
			});

			Debug.Assert(helper.ValidateField("Gender"));

			Globals.Thread.Sleep(150);

			gOut.Print("{0}", Globals.LineSep);

			character.Uid = Globals.Database.GetCharacterUid();

			character.IsUidRecycled = true;

			character.Status = Status.Alive;

			var weaponValues = EnumUtil.GetValues<Weapon>();

			for (var i = 0; i < weaponValues.Count; i++)
			{
				var wv = weaponValues[i];

				var weapon = gEngine.GetWeapons(wv);

				Debug.Assert(weapon != null);

				character.SetWeaponAbilities(wv, Convert.ToInt64(weapon.EmptyVal));
			}

			character.HeldGold = 200;

			gOut.Write("{0}The Irishman walks away and in walks a tall man of possibly Elvish descent.{0}{0}He studies you for a moment and says, \"Here is a booklet of instructions for you to read, and your prime attributes are--{0}", Environment.NewLine);

			var statValues = EnumUtil.GetValues<Stat>();

			while (true)
			{
				for (var i = 0; i < statValues.Count; i++)
				{
					var sv = statValues[i];

					var stat = gEngine.GetStats(sv);

					Debug.Assert(stat != null);

					character.Stats[(long)sv] = gEngine.RollDice(3, 8, 0);

					gOut.Write("{0}{1,27}{2}{3}",
						Environment.NewLine,
						string.Format("{0}: ", stat.Name),
						character.GetStats(sv),
						i == statValues.Count - 1 ? string.Format("\"{0}", Environment.NewLine) : "");
				}

				if (character.GetStats(Stat.Intellect) + character.GetStats(Stat.Hardiness) + character.GetStats(Stat.Agility) < 39 || character.Stats.Sum() < 52)
				{
					gOut.Print("\"You are such a poor excuse for an adventurer that we will allow you to commit suicide.\"");

					gOut.Print("{0}", Globals.LineSep);

					gOut.Write("{0}Press Y to commit suicide or N to continue: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					if (Buf.Length > 0 && Buf[0] == 'Y')
					{
						gOut.Print("{0}", Globals.LineSep);

						gOut.Print("\"We resurrect you again and your prime attributes are--");
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}
			}

			rc = Globals.Database.AddCharacter(character);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.CharactersModified = true;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}Press R to read instructions or T to give them back: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharROrT, gEngine.IsCharROrT);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			gOut.Print("{0}", Globals.LineSep);

			if (Buf.Length > 0 && Buf[0] == 'R')
			{
				gOut.WriteLine("{0}You read the instructions and they say--{0}", Environment.NewLine);

				gEngine.PrintTitle("INFORMATION ABOUT THE WORLD OF EAMON", false);

				gOut.Write("{0}You will have to buy a weapon.  Your chance to hit with it will be determined by the weapon complexity, your ability in that weapon class, how heavy your armor is, and the difference in agility between you and your enemy.{0}{0}The five classes of weapons (and your current abilities with each) are--{0}", Environment.NewLine);

				gOut.Write("{0}{1,28}", Environment.NewLine, "Club/Mace......20%");

				gOut.Write("{0}{1,28}", Environment.NewLine, "Spear..........10%");

				gOut.Write("{0}{1,28}", Environment.NewLine, "Axe.............5%");

				gOut.Write("{0}{1,28}", Environment.NewLine, "Sword...........0%");

				gOut.Print("{0,28}", "Bow...........-10%");

				gOut.Print("Every time you score a hit in battle, your ability in the weapon class may go up by 2%, if a random number from 1-100 is less than your chance to miss!");

				Globals.In.KeyPress(Buf);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("There are four armor types and you may also carry a shield if you do not use a two-handed weapon.  These protections will absorb hits placed upon you (almost always!) but they lower your chance to hit.  The protections are--");

				gOut.Write("{0}{1,61}", Environment.NewLine, "Armor          Hits Absorbed        Odds Adjustment");

				gOut.Write("{0}{1,61}", Environment.NewLine, "---------------------------------------------------");

				gOut.Write("{0}{1,56}", Environment.NewLine, "None .................0 ....................0%");

				gOut.Write("{0}{1,56}", Environment.NewLine, "Leather ..............1 ..................-10%");

				gOut.Write("{0}{1,56}", Environment.NewLine, "Chain ................2 ..................-20%");

				gOut.Write("{0}{1,56}", Environment.NewLine, "Plate ................5 ..................-60%");

				gOut.Print("{0,56}", "Shield ...............1 ...................-5%");

				gOut.Print("You will develop an Armor Expertise, which will go up when you hit a blow wearing armor and your expertise is less than the armor you are wearing.  No matter how high your Armor Expertise is, however, the net effect of armor will never increase your chance to hit.");

				Globals.In.KeyPress(Buf);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}You can carry weight up to ten times your hardiness, or, {1} Gronds.  (A measure of weight, one Grond = 10 Dos.){0}{0}Additionally, your hardiness tells how many points of damage you can survive.  Therefore, you can be hit with {2} 1-point blows before you die.{0}{0}You will not be told how many blows you have taken.  You will be merely told such things as--{0}{0}   \"Wow!  That one hurt!\"{0}or \"You don't feel very well.\"{0}{0}Your charisma ({3}) affects how the citizens of Eamon react to you.  You affect a monster's friendliness rating by your charisma less ten, difference times two ({4}%).{0}{0}You start off with 200 gold pieces, which you will want to spend on supplies for your first adventure.  You will get a lower price for items if your charisma is high.{0}",
					Environment.NewLine,
					character.GetWeightCarryableGronds(),
					character.GetStats(Stat.Hardiness),
					character.GetStats(Stat.Charisma),
					character.GetCharmMonsterPct());

				Globals.In.KeyPress(Buf);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}After you begin to accumulate wealth, you may want to put some of your money into the bank, where it cannot be stolen.  However it is a good idea to always carry some gold with you for use in bargaining and ransom situations.{0}{0}You should also hire a Wizard to teach you some magic spells.  Your intellect ({1}) affects your ability to learn both skills and spells.  There are four spells you can learn--{0}{0}Blast: Throw a magical blast at your enemies to inflict damage.{0}Heal : Remove damage from your body.{0}Speed: Double your agility for a short time.{0}Power: This unpredictable spell is different in each adventure.{0}{0}Other types of spells may be available in various adventures, and items may have special properties.  However, these will only work in the adventure where they were found.  Thus it is best (and you have no choice but to) sell all items found in adventures except for weapons and armor.{0}",
					Environment.NewLine,
					character.GetStats(Stat.Intellect));

				Globals.In.KeyPress(Buf);

				gOut.Print("{0}", Globals.LineSep);
			}

			gOut.Write("{0}The man behind the desk takes back the instructions and says, \"It is now time for you to start your life.\"  He makes an odd sign with his hand and says, \"Live long and prosper.\"{0}{0}You now wander into the Main Hall...{0}", Environment.NewLine);

			Globals.In.KeyPress(Buf);

			Globals.Character = character;
		}

		/// <summary></summary>
		public virtual void SelectCharacter()
		{
			RetCode rc;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("You are greeted there by a burly Irishman who looks at you with a scowl and asks you, \"What's yer name?\"");

			var character = Globals.CreateInstance<ICharacter>();

			var menu = Globals.CreateInstance<IMainHallMenu>();

			var effect = null as IEffect;

			while (true)
			{
				Rtio = null;

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}You give him your name (type it in now): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.CharNameLen, null, ' ', '\0', false, null, null, gEngine.IsCharAnyButDquoteCommaColon, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Buf.SetFormat("{0}", Regex.Replace(Buf.ToString(), @"\s+", " ").Trim());

				character.Name = Buf.ToString();

				var helper = Globals.CreateInstance<ICharacterHelper>(x =>
				{
					x.Record = character;
				});

				Globals.Thread.Sleep(150);

				if (helper.ValidateField("Name"))
				{
					gOut.Print("{0}", Globals.LineSep);

					if (effect == null)
					{
						var effectUid = gEngine.RollDice(1, Globals.Database.GetEffectsCount(), 0);

						effect = gEDB[effectUid];
					}

					gOut.Print("He starts looking through his book, while muttering something about {0}", effect != null ? effect.Desc : "not having enough snappy comments.");

					Globals.Character = Globals.Database.CharacterTable.Records.FirstOrDefault(c => c.Name.Equals(character.Name, StringComparison.OrdinalIgnoreCase));

					if (gCharacter == null)
					{
						gOut.Print("He eventually looks at you and says, \"Yer name's na in here.  Have ye given it to me aright?\"");

						gOut.Print("{0}", Globals.LineSep);

						gOut.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

						Buf.Clear();

						rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

						Debug.Assert(gEngine.IsSuccess(rc));

						Globals.Thread.Sleep(150);

						if (Buf.Length > 0 && Buf[0] == 'Y')
						{
							character.Name = gEngine.Capitalize(character.Name);

							Globals.CharacterName = character.Name;

							CreateCharacter(character);

							menu.Execute();

							goto Cleanup;
						}
						else
						{
							BadCharacterName();
						}
					}
					else
					{
						/* 
							Full Credit:  Derived wholly from Donald Brown's Classic Eamon

							File: MAIN HALL
							Line: 2020
						*/

						if (Rtio == null)
						{
							var c2 = gCharacter.GetMerchantAdjustedCharisma();

							Rtio = gEngine.GetMerchantRtio(c2);
						}

						Globals.CharacterName = gCharacter.Name;

						if (gCharacter.Status == Status.Alive)
						{
							gOut.Print("Finally he looks up and says, \"Ah, here ye be.  Well, go and have fun in the hall.\"");

							Globals.In.KeyPress(Buf);

							menu.Execute();

							goto Cleanup;
						}
						else if (gCharacter.Status == Status.Adventuring)
						{
							gOut.Write("{0}The burly Irishman stares at you intently and says, \"If ye really be {1}, it means ye've been recalled from yer adventure with the help of a local wizard, and fer a fee.  Is this true?\"{0}{0}(Warning:  Any saved games for that adventure will be deleted!){0}",
								Environment.NewLine,
								gCharacter.Name);

							gOut.Print("{0}", Globals.LineSep);

							gOut.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

							Buf.Clear();

							rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

							Debug.Assert(gEngine.IsSuccess(rc));

							Globals.Thread.Sleep(150);

							if (Buf.Length > 0 && Buf[0] == 'Y')
							{
								gOut.Print("{0}", Globals.LineSep);

								AdventureName = "";

								RecallCharacterFromAdventure();

								if (gCharacter.Status != Status.Adventuring)
								{
									var ap = gEngine.GetMerchantAskPrice(Constants.RecallPrice, (double)Rtio);

									gOut.Print("Pointing to an entry in his registry, the Irishman exclaims, \"Says 'ere the wizard found ye in {0} and charged {1} gold coin{2} for his services{3}!\"",
										AdventureName.Length > 0 ? AdventureName : gEngine.UnknownName,
										ap,
										ap != 1 ? "s" : "",
										gCharacter.HeldGold + gCharacter.BankGold < ap ? ", but ye didn't have enough to pay the fee, even after he put a lien on yer bank account!  Uh-oh, he said he'd get even soon" :
										gCharacter.HeldGold < ap ? "!  He even put a lien on yer bank account to ensure full payment" :
										"");

									gCharacter.HeldGold -= ap;

									Globals.CharactersModified = true;

									gOut.Print("Finally he looks up and says, \"Welcome back from yer adventure.  Now go and have fun in the hall.\"");

									Globals.In.KeyPress(Buf);

									menu.Execute();

									goto Cleanup;
								}
								else
								{
									gOut.Write("{0}Pointing to an entry in his registry, the Irishman exclaims, \"Says 'ere the wizard couldn't locate {1} in any known adventure!\"{0}{0}(You will have to manually change {2} status using EamonDD.){0}",
										Environment.NewLine,
										gCharacter.Name,
										gCharacter.EvalGender("his", "her", "its"));
								}
							}
						}
						else
						{
							Debug.Assert(gCharacter.Status == Status.Dead || gCharacter.Status == Status.Unknown);

							gOut.Print("The burly Irishman gets a {0} look in his eyes and says, \"Ye can't be {1}, {2} be {3}.  Now who'r ye again?\"",
								gCharacter.Status == Status.Dead ? "sad" : "puzzled",
								gCharacter.Name,
								gCharacter.EvalGender("he", "she", "it"),
								gCharacter.Status == Status.Dead ? "dead" : "missing");
						}
					}
				}
				else
				{
					BadCharacterName();
				}

				if (NumChances == 0)
				{
					Globals.In.KeyPress(Buf);

					goto Cleanup;
				}
			}

		Cleanup:

			;
		}

		public RegistrationDeskMenu()
		{
			Buf = Globals.Buf;

			AdventureName = "";

			NumChances = 13;
		}
	}
}
