
// LMKKP1.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Game.Attributes;
using static LandOfTheMountainKing.Game.Plugin.Globals;

namespace LandOfTheMountainKing.Game
{
	[ClassMappings]
	public class LMKKP1 : Framework.ILMKKP1
	{
		public virtual int Lampdir { get; set; }		// = 7;	SET IN MAINLOOP.CS
		public virtual int NecklaceTaken { get; set; }		// 0:not taken, 1:taken but not given, 2:given
		public virtual int SaidHello { get; set; }      // Have you already said hello to Lisa?
		public virtual int SwampMonsterKilled { get; set; }      //	SET IN MAINLOOP.CS

		public virtual long Hard { get; set; }			//	SET IN MAINLOOP.CS
		public virtual long Agil { get; set; }			//	SET IN MAINLOOP.CS
		public virtual long Axe { get; set; }			//	SET IN MAINLOOP.CS
		public virtual long Bow { get; set; }			//	SET IN MAINLOOP.CS
		public virtual long Club { get; set; }			//	SET IN MAINLOOP.CS
		public virtual long Spear { get; set; }			//	SET IN MAINLOOP.CS
		public virtual long Sword { get; set; }			//	SET IN MAINLOOP.CS
		public virtual long Armor { get; set; }			// = 0;	SET IN MAINLOOP.CS
		public virtual long blast { get; set; }			// = gCharacter.GetSpellAbility(1);	SET IN MAINLOOP.CS
		public virtual long heal { get; set; }			// = gCharacter.GetSpellAbility(2);	SET IN MAINLOOP.CS
		public virtual long speed { get; set; }			// = gCharacter.GetSpellAbility(3);	SET IN MAINLOOP.CS
		public virtual long power { get; set; }			// = gCharacter.GetSpellAbility(4);	SET IN MAINLOOP.CS
	}
}
