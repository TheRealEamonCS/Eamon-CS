
// AfterPrintPlayerRoomEventState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using TheWayfarersInn.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.States
{
	[ClassMappings]
	public class AfterPrintPlayerRoomEventState : EamonRT.Game.States.AfterPrintPlayerRoomEventState, IAfterPrintPlayerRoomEventState
	{
		public virtual void UnseenApparitionEvent(object obj)
		{
			//long rl;

			var unseenApparitionMonster = gMDB[2];

			Debug.Assert(unseenApparitionMonster != null);

			var eventState = gGameState.GetEventState(EventState.UnseenApparition);

			if (unseenApparitionMonster.IsInLimbo() || (!gCharRoom.IsWayfarersInnRoom() && !gCharRoom.IsWayfarersInnClearingRoom()))
			{
				eventState = 0;
			}

			switch (eventState)
			{
				case 1:

					// TODO: implement

					break;
			}

			gGameState.SetEventState(EventState.UnseenApparition, eventState);

			if (eventState > 0)
			{
				gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "UnseenApparition", 0, null);
			}
		}

		public virtual void BlueBandedCentipedesEvent(object obj)
		{
			long rl;

			var blueBandedCentipedesMonster = gMDB[3];

			Debug.Assert(blueBandedCentipedesMonster != null);

			var eventState = gGameState.GetEventState(EventState.BlueBandedCentipedes);

			if (blueBandedCentipedesMonster.IsInLimbo() && eventState > 9)
			{
				eventState = 0;
			}

			switch (eventState)
			{
				case 1:

					eventState++;

					break;

				case 2:

					if (gCharRoom.Uid == 27)
					{
						gEngine.PrintEffectDesc(67);
					}

					if (gGameState.TotalCentipedeCounter < 8)
					{
						eventState = 9;
					}
					else
					{
						eventState++;
					}

					break;

				case 3:

					eventState++;

					break;

				case 4:

					if (gCharRoom.Uid == 27)
					{
						gEngine.PrintEffectDesc(68);
					}

					if (gGameState.TotalCentipedeCounter < 17)
					{
						eventState = 9;
					}
					else
					{
						eventState++;
					}

					break;

				case 5:

					eventState++;

					break;

				case 6:

					if (gCharRoom.Uid == 27)
					{
						gEngine.PrintEffectDesc(69);
					}

					if (gGameState.TotalCentipedeCounter < 33)
					{
						eventState = 9;
					}
					else
					{
						eventState++;
					}

					break;

				case 7:

					eventState++;

					break;

				case 8:

					if (gCharRoom.Uid == 27)
					{
						gEngine.PrintEffectDesc(70);
					}

					eventState++;

					break;

				case 11:

					if (gCharRoom.Uid == 27 && gGameState.AttackingCentipedeCounter < blueBandedCentipedesMonster.CurrGroupCount)
					{
						rl = Math.Min(gEngine.RollDice(1, 5, 0), blueBandedCentipedesMonster.CurrGroupCount - gGameState.AttackingCentipedeCounter);

						gGameState.AttackingCentipedeCounter += rl;

						var origCurrGroupCount = blueBandedCentipedesMonster.CurrGroupCount;

						blueBandedCentipedesMonster.CurrGroupCount = rl;

						gOut.Print(gGameState.AttackingCentipedeCounter > rl ? "{0} join{1} the attacking swarm!" : "{0} swarm{1} in to attack!", blueBandedCentipedesMonster.GetArticleName(true), rl > 1 ? "" : "s");

						blueBandedCentipedesMonster.CurrGroupCount = origCurrGroupCount;
					}

					eventState = 10;

					break;

				case 12:

					if (gCharRoom.Uid == 27 && gGameState.AttackingCentipedeCounter > 0)
					{
						rl = Math.Min(gEngine.RollDice(1, 3, 2), gGameState.AttackingCentipedeCounter);

						gGameState.AttackingCentipedeCounter -= rl;

						var origCurrGroupCount = blueBandedCentipedesMonster.CurrGroupCount;

						blueBandedCentipedesMonster.CurrGroupCount = rl;

						gOut.Print("{0} abandon{1} the attacking swarm!", blueBandedCentipedesMonster.GetArticleName(true), rl > 1 ? "" : "s");

						blueBandedCentipedesMonster.CurrGroupCount = origCurrGroupCount;
					}

					eventState = 10;

					break;
			}

			gGameState.SetEventState(EventState.BlueBandedCentipedes, eventState);

			if (eventState == 9 || eventState == 10 || eventState == 13 || eventState == 14)
			{
				gGameState.BeforePrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "BlueBandedCentipedes", 0, null);
			}
			else if (eventState > 0)
			{
				gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "BlueBandedCentipedes", 0, null);
			}
		}

		public virtual void ChildsApparitionEvent(object obj)
		{
			long rl;

			var childsApparitionMonster = gMDB[4];

			Debug.Assert(childsApparitionMonster != null);

			var hearthwatcherMonster = gMDB[23];

			Debug.Assert(hearthwatcherMonster != null);

			var giantWoodenStatueArtifact = gADB[28];

			Debug.Assert(giantWoodenStatueArtifact != null);

			var eventState = gGameState.GetEventState(EventState.ChildsApparition);

			if (childsApparitionMonster.IsInLimbo() || !gCharRoom.IsWayfarersInnRoom())
			{
				eventState = 0;
			}

			switch (eventState)
			{
				case 1:

					if (childsApparitionMonster.IsInRoom(gCharRoom) && gCharRoom.IsLit())
					{
						rl = gEngine.RollDice(1, 100, 0);

						if (!gGameState.CharlotteMet)
						{
							if (rl < 50)
							{
								eventState = 4;
							}
						}
						else
						{
							if (!gGameState.CharlotteBlackthornStory)
							{
								if (rl < 50)
								{
									eventState = 5;
								}
							}
							else if (!gGameState.CharlotteArtisansStory)
							{
								if (rl < 50)
								{
									eventState = 6;
								}
							}
							else
							{
								if (rl < 10)
								{
									eventState = 7;
								}
							}

							if (eventState == 1)
							{
								rl = gEngine.RollDice(1, 100, 0);

								if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
								{
									if (rl < 10)
									{
										eventState = 8;
									}
								}
							}

							if (eventState == 1)
							{
								rl = gEngine.RollDice(1, 100, 0);

								if (hearthwatcherMonster.IsInRoom(gCharRoom))
								{
									if (rl < 50)
									{
										eventState = 9;
									}
								}
							}
						}
					}

					break;

				case 2:

					if (childsApparitionMonster.IsInRoom(gCharRoom))
					{
						gOut.Print("{0} cries profusely.", gCharRoom.EvalLightLevel("An unseen entity", childsApparitionMonster.GetTheName(true)));
					}

					rl = gEngine.RollDice(1, 100, 0);

					if (rl < 50)
					{
						eventState++;
					}
					
					break;

				case 3:

					if (childsApparitionMonster.IsInRoom(gCharRoom))
					{
						gOut.Print("{0} sniffles{1}.", gCharRoom.EvalLightLevel("The entity", childsApparitionMonster.GetTheName(true)), gCharRoom.EvalLightLevel("", ", wiping tears from her eyes"));
					}

					rl = gEngine.RollDice(1, 100, 0);

					if (rl < 50)
					{
						eventState = 1;
					}

					break;

				case 4:

					if (childsApparitionMonster.IsInRoom(gCharRoom) && gCharRoom.IsLit())
					{
						gEngine.PrintEffectDesc(142);

						childsApparitionMonster.Name = "Charlotte";

						childsApparitionMonster.ArticleType = ArticleType.None;

						gGameState.CharlotteMet = true;
					}

					eventState = 1;

					break;

				case 5:

					if (childsApparitionMonster.IsInRoom(gCharRoom) && gCharRoom.IsLit())
					{
						gEngine.PrintEffectDesc(143);

						gGameState.CharlotteBlackthornStory = true;
					}

					eventState = 1;

					break;

				case 6:

					if (childsApparitionMonster.IsInRoom(gCharRoom) && gCharRoom.IsLit())
					{
						gEngine.PrintEffectDesc(144);

						gGameState.CharlotteArtisansStory = true;
					}

					eventState = 1;

					break;

				case 7:

					if (childsApparitionMonster.IsInRoom(gCharRoom) && gCharRoom.IsLit())
					{
						var eventFunc = gEngine.ChildsApparitionEventFuncList[0];

						gEngine.ChildsApparitionEventFuncList.RemoveAt(0);

						Debug.Assert(eventFunc != null);

						eventFunc(gCharRoom, childsApparitionMonster);

						gEngine.ChildsApparitionEventFuncList.Add(eventFunc);
					}

					eventState = 1;

					break;

				case 8:

					if (childsApparitionMonster.IsInRoom(gCharRoom) && gGameState.GetNBTL(Friendliness.Enemy) > 0 && gCharRoom.IsLit())
					{
						gOut.Print("{0} looks bored as you fight for your life!", childsApparitionMonster.GetTheName(true));
					}

					eventState = 1;

					break;

				case 9:

					if (childsApparitionMonster.IsInRoom(gCharRoom) && hearthwatcherMonster.IsInRoom(gCharRoom) && gCharRoom.IsLit())
					{
						gEngine.PrintEffectDesc(139);

						gEngine.PrintEffectDesc(gCharRoom.Uid == 24 ? 140 : 141);

						hearthwatcherMonster.DmgTaken = 0;

						hearthwatcherMonster.SetInLimbo();

						giantWoodenStatueArtifact.SetInRoomUid(24);
					}

					eventState = 1;

					break;
			}

			gGameState.SetEventState(EventState.ChildsApparition, eventState);

			if (eventState > 0)
			{
				gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "ChildsApparition", 0, null);
			}
		}

		public virtual void NolanEvent(object obj)
		{
			//long rl;

			var nolanMonster = gMDB[24];

			Debug.Assert(nolanMonster != null);

			var eventState = gGameState.GetEventState(EventState.Nolan);

			if (nolanMonster.IsInLimbo())
			{
				eventState = 0;
			}

			switch (eventState)
			{
				case 1:

					// TODO: implement

					break;
			}

			gGameState.SetEventState(EventState.Nolan, eventState);

			if (eventState > 0)
			{
				gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "Nolan", 0, null);
			}
		}

		public virtual void ForestEvent(object obj)
		{
			long rl;

			var eventState = gGameState.GetEventState(EventState.Forest);

			if (!gCharRoom.IsForestRoom())
			{
				eventState = 0;
			}

			switch (eventState)
			{
				case 1:

					rl = gEngine.RollDice(1, 100, 0);

					if (rl < 5)
					{
						var eventFunc = gEngine.ForestEventFuncList[0];

						gEngine.ForestEventFuncList.RemoveAt(0);

						Debug.Assert(eventFunc != null);

						eventFunc(gCharRoom);

						gEngine.ForestEventFuncList.Add(eventFunc);
					}

					break;
			}

			gGameState.SetEventState(EventState.Forest, eventState);

			if (eventState > 0)
			{
				gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "Forest", 0, null);
			}
		}

		public virtual void RiverEvent(object obj)
		{
			long rl;

			var eventState = gGameState.GetEventState(EventState.River);

			if (!gCharRoom.IsRiverRoom())
			{
				eventState = 0;
			}

			switch(eventState)
			{
				case 1:

					rl = gEngine.RollDice(1, 100, 0);

					if (rl < 5)
					{
						var eventFunc = gEngine.RiverEventFuncList[0];

						gEngine.RiverEventFuncList.RemoveAt(0);

						Debug.Assert(eventFunc != null);

						eventFunc(gCharRoom);

						gEngine.RiverEventFuncList.Add(eventFunc);
					}

					break;
			}

			gGameState.SetEventState(EventState.River, eventState);

			if (eventState > 0)
			{
				gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "River", 0, null);
			}
		}
	}
}
