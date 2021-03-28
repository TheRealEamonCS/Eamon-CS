
// IntroStory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using EamonRT.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game
{
	[ClassMappings]
	public class IntroStory : IIntroStory
	{
		public virtual StringBuilder Buf { get; set; }

		public virtual IntroStoryType StoryType { get; set; }

		public virtual bool ShouldPrintOutput
		{
			get
			{
				var result = true;

				if (Globals.Database.GetFilesetsCount() > 0)
				{
					gOut.Print("{0}", Globals.LineSep);

					gOut.Print("Welcome back to {0}!", Globals.Module.Name);

					gOut.Write("{0}Would you like to see the introduction story again (Y/N) [N]: ", Environment.NewLine);

					Buf.Clear();

					var rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					if (Buf.Length > 0 && Buf[0] != 'Y')
					{
						result = false;
					}
				}

				return result;
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public virtual void PrintOutput()
		{
			switch (StoryType)
			{
				case IntroStoryType.Beginners:

					PrintOutputBeginners();

					break;

				default:

					PrintOutputDefault();

					break;
			}
		}

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		public virtual bool IsCharWpnNum(char ch)
		{
			var i = 0L;

			var rc = gCharacter.GetWeaponCount(ref i);

			Debug.Assert(gEngine.IsSuccess(rc));

			return ch >= '1' && ch <= ('1' + (i - 1));
		}

		/// <summary></summary>
		public virtual void PrintOutputBeginnersPrelude() 
		{ 
		
		}

		/// <summary></summary>
		public virtual void PrintOutputBeginnersTooManyWeapons() 
		{ 
		
		}

		/// <summary></summary>
		public virtual void PrintOutputBeginnersNoWeapons() 
		{ 
		
		}

		/// <summary></summary>
		public virtual void PrintOutputBeginnersNotABeginner() 
		{ 
		
		}

		/// <summary></summary>
		public virtual void PrintOutputBeginnersMayNowProceed() 
		{ 
		
		}

		/// <summary></summary>
		public virtual void PrintOutputBeginners()
		{
			RetCode rc;

			gOut.Print("{0}", Globals.LineSep);

			PrintOutputBeginnersPrelude();

			var i = 0L;       // weird disambiguation hack

			if (!gCharacter.GetWeapons(i).IsActive())
			{
				PrintOutputBeginnersNoWeapons();

				Globals.MainLoop.ShouldExecute = false;

				Globals.ExitType = ExitType.GoToMainHall;
			}
			else if (gCharacter.ArmorExpertise != 0 || gCharacter.GetWeaponAbilities(Weapon.Axe) != 5 || gCharacter.GetWeaponAbilities(Weapon.Club) != 20 || gCharacter.GetWeaponAbilities(Weapon.Sword) != 0)
			{
				PrintOutputBeginnersNotABeginner();

				Globals.MainLoop.ShouldExecute = false;

				Globals.ExitType = ExitType.GoToMainHall;
			}
			else
			{
				if (gCharacter.GetWeapons(1).IsActive())
				{
					PrintOutputBeginnersTooManyWeapons();

					Buf.Clear();

					gCharacter.ListWeapons(Buf);

					gOut.WriteLine("{0}", Buf);

					gOut.Print("{0}", Globals.LineSep);

					gOut.Write("{0}Press the number of the weapon to select: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, IsCharWpnNum, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					gOut.Print("{0}", Globals.LineSep);

					Debug.Assert(gGameState != null);

					gGameState.UsedWpnIdx = Convert.ToInt64(Buf.Trim().ToString());

					gGameState.UsedWpnIdx--;
				}

				PrintOutputBeginnersMayNowProceed();
			}
		}

		/// <summary></summary>
		public virtual void PrintOutputDefault()
		{
			gOut.Print("{0}", Globals.LineSep);

			var effect = gEDB[Globals.Module.IntroStory];

			if (effect != null)
			{
				gEngine.PrintMacroReplacedPagedString(effect.Desc, Buf);
			}
			else
			{
				gOut.Print("There is no introduction story for this adventure.");
			}
		}

		public IntroStory()
		{
			Buf = Globals.Buf;

			StoryType = IntroStoryType.Default;
		}
	}
}
