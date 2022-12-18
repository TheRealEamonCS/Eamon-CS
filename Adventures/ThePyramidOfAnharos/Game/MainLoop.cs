
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			long effectUid = 0;

			var omarMonster = gMDB[1];

			Debug.Assert(omarMonster != null);

			var aliMonster = gMDB[2];

			Debug.Assert(aliMonster != null);

			var faroukMonster = gMDB[6];

			Debug.Assert(faroukMonster != null);

			gOut.Print("{0}", gEngine.LineSep);

			if (gGameState.KV > 0)
			{
				if (gGameState.KV == 1 && gGameState.KF != 1)
				{
					if (omarMonster.IsInRoomUid(gGameState.Ro) || aliMonster.IsInRoomUid(gGameState.Ro) || faroukMonster.IsInRoomUid(gGameState.Ro))
					{
						if (faroukMonster.IsInRoomUid(gGameState.Ro))
						{
							gEngine.PrintEffectDesc(38);

							gCharacter.HeldGold += 2000;

							effectUid = 37;
						}
						else
						{
							gCharacter.HeldGold += 1000;
						}

						if (aliMonster.IsInRoomUid(gGameState.Ro))
						{
							gCharacter.HeldGold -= 500;

							effectUid = 36;
						}
						else if (omarMonster.IsInRoomUid(gGameState.Ro))
						{
							effectUid = 35;
						}

						gEngine.PrintEffectDesc(effectUid);
					}
					else
					{
						gEngine.PrintEffectDesc(51);
					}
				}
				else
				{
					gEngine.PrintEffectDesc(48);

					gEngine.TaxLevied = true;

					if (gCharacter.HeldGold > 0)
					{
						gCharacter.HeldGold = 0;
					}

					if (gCharacter.BankGold > 0)
					{
						gCharacter.BankGold = 0;
					}
				}
			}
			else
			{
				gEngine.PrintEffectDesc(47);

				gEngine.TaxLevied = true;

				if (gCharacter.HeldGold > 0)
				{
					gCharacter.HeldGold = 0;
				}

				if (gCharacter.BankGold > 0)
				{
					gCharacter.BankGold = 0;
				}
			}

			gEngine.In.KeyPress(gEngine.Buf);

			base.Shutdown();
		}
	}
}
