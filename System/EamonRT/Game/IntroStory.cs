
// IntroStory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
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

				if (gDatabase.GetFilesetCount() > 0)
				{
					gOut.Print("{0}", gEngine.LineSep);

					gEngine.PrintWelcomeBack();

					gEngine.PrintEnterSeeIntroStoryChoice();

					Buf.Clear();

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

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
			var artifactList = gCharacter.GetCarriedList().Where(a => a.GeneralWeapon != null).ToList();

			return ch >= '1' && ch <= ('1' + (artifactList.Count - 1));
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

			var artifactList = gCharacter.GetCarriedList().Where(a => a.GeneralWeapon != null).ToList();

			if (artifactList.Count <= 0)
			{
				PrintOutputBeginnersNoWeapons();

				gEngine.MainLoop.ShouldStartup = false;

				gEngine.MainLoop.ShouldExecute = false;

				gEngine.MainLoop.ShouldShutdown = false;

				gEngine.ExitType = ExitType.GoToMainHall;
			}
			else if (gCharacter.ArmorExpertise != 0 || gCharacter.GetWeaponAbility(Weapon.Axe) != 5 || gCharacter.GetWeaponAbility(Weapon.Club) != 20 || gCharacter.GetWeaponAbility(Weapon.Sword) != 0)
			{
				PrintOutputBeginnersNotABeginner();

				gEngine.MainLoop.ShouldStartup = false;

				gEngine.MainLoop.ShouldExecute = false;

				gEngine.MainLoop.ShouldShutdown = false;

				gEngine.ExitType = ExitType.GoToMainHall;
			}
			else
			{
				if (artifactList.Count > 1)
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
