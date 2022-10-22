
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
using static EamonRT.Game.Plugin.Globals;

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

				if (gEngine.Database.GetFilesetCount() > 0)
				{
					gOut.Print("{0}", gEngine.LineSep);

					gEngine.PrintWelcomeBack();

					gEngine.PrintEnterSeeIntroStoryChoice();

					Buf.Clear();

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					gEngine.Thread.Sleep(150);

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

			gOut.Print("{0}", gEngine.LineSep);

			PrintOutputBeginnersPrelude();

			var i = 0L;       // weird disambiguation hack

			if (!gCharacter.GetWeapon(i).IsActive())
			{
				PrintOutputBeginnersNoWeapons();

				gEngine.MainLoop.ShouldExecute = false;

				gEngine.ExitType = ExitType.GoToMainHall;
			}
			else if (gCharacter.ArmorExpertise != 0 || gCharacter.GetWeaponAbility(Weapon.Axe) != 5 || gCharacter.GetWeaponAbility(Weapon.Club) != 20 || gCharacter.GetWeaponAbility(Weapon.Sword) != 0)
			{
				PrintOutputBeginnersNotABeginner();

				gEngine.MainLoop.ShouldExecute = false;

				gEngine.ExitType = ExitType.GoToMainHall;
			}
			else
			{
				if (gCharacter.GetWeapon(1).IsActive())
				{
					PrintOutputBeginnersTooManyWeapons();

					Buf.Clear();

					gCharacter.ListWeapons(Buf);

					gOut.WriteLine("{0}", Buf);

					gOut.Print("{0}", gEngine.LineSep);

					gEngine.PrintEnterWeaponNumberChoice();

					Buf.Clear();

					rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, IsCharWpnNum, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					gEngine.Thread.Sleep(150);

					gOut.Print("{0}", gEngine.LineSep);

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
			gOut.Print("{0}", gEngine.LineSep);

			var effect = gEDB[gEngine.Module.IntroStory];

			if (effect != null)
			{
				gEngine.PrintMacroReplacedPagedString(effect.Desc, Buf);
			}
			else
			{
				gEngine.PrintNoIntroStory();
			}
		}

		public IntroStory()
		{
			Buf = gEngine.Buf;

			StoryType = IntroStoryType.Default;
		}
	}
}
