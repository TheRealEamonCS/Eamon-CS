
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace ARuncibleCargo.Framework
{
	/// <summary></summary>
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary></summary>
		long DreamCounter { get; set; }

		/// <summary></summary>
		long SwarmyCounter { get; set; }

		/// <summary></summary>
		long CargoOpenCounter { get; set; }

		/// <summary></summary>
		long CargoInRoom { get; set; }

		/// <summary></summary>
		long GiveAmazonMoney { get; set; }

		/// <summary></summary>
		bool[] PookaMet { get; set; }

		/// <summary></summary>
		bool AmazonMet { get; set; }

		/// <summary></summary>
		bool BillAndAmazonMeet { get; set; }

		/// <summary></summary>
		bool PrinceMet { get; set; }

		/// <summary></summary>
		bool AmazonLilWarning { get; set; }

		/// <summary></summary>
		bool BillLilWarning { get; set; }

		/// <summary></summary>
		bool FireEscaped { get; set; }

		/// <summary></summary>
		bool CampEntered { get; set; }

		/// <summary></summary>
		bool PaperRead { get; set; }

		/// <summary></summary>
		bool Explosive { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool GetPookaMet(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetPookaMet(long index, bool value);

		#endregion
	}
}
