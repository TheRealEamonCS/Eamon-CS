
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Helpers
{
	[ClassMappings]
	public class GameStateHelper : Eamon.Game.Helpers.GameStateHelper, IGameStateHelper
	{
		public virtual new Framework.IGameState Record
		{
			get
			{
				return (Framework.IGameState)base.Record;
			}

			set
			{
				if (base.Record != value)
				{
					base.Record = value;
				}
			}
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWoodenBridgeUseCounter()
		{
			return Record.WoodenBridgeUseCounter >= 0 && Record.WoodenBridgeUseCounter <= 3;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateTotalCentipedeCounter()
		{
			return Record.TotalCentipedeCounter >= 0 && Record.TotalCentipedeCounter <= 50;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateAttackingCentipedeCounter()
		{
			return Record.AttackingCentipedeCounter >= 0 && Record.AttackingCentipedeCounter <= Record.TotalCentipedeCounter;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateBedroomTurnCounter()
		{
			return Record.BedroomTurnCounter >= 0 && Record.BedroomTurnCounter <= 15;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateKitchenRiddleState()
		{
			return Record.KitchenRiddleState >= 0 && Record.KitchenRiddleState <= 4;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNolanPuddingAttackOdds()
		{
			return Record.NolanPuddingAttackOdds >= 0 && Record.NolanPuddingAttackOdds <= 100;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNolanRustMonsterAttackOdds()
		{
			return Record.NolanRustMonsterAttackOdds >= 0 && Record.NolanRustMonsterAttackOdds <= 100;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateMetalArmorArtifactUid()
		{
			return Record.MetalArmorArtifactUid >= 0 /* && Record.MetalArmorArtifactUid <= gEngine.Module.NumArtifacts */;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateMonsterTotalDmgTaken()
		{
			var result = true;

			for (var i = 0; i < Record.MonsterTotalDmgTaken.Length; i++)
			{
				if (Record.MonsterTotalDmgTaken[i] < 0)
				{
					result = false;

					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateEventStates()
		{
			var result = true;

			for (var i = 0; i < Record.EventStates.Length; i++)
			{
				if (Record.EventStates[i] < 0)
				{
					result = false;

					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateGuestRoomDictionary()
		{
			return Record.GuestRoomDictionary != null && Record.GuestRoomDictionary.Count == 13;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateOpenWindowRoomUids()
		{
			return Record.OpenWindowRoomUids != null && Record.OpenWindowRoomUids.Count >= 0 && Record.OpenWindowRoomUids.Count <= 17;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateForgedArtifactUids()
		{
			return Record.ForgedArtifactUids != null && Record.ForgedArtifactUids.Count >= 0;
		}

		public GameStateHelper()
		{

		}
	}
}
