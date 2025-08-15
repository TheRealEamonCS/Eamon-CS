
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Data.Common;
using System.Text;
using Eamon.Framework;

namespace Firestorm.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		/// <summary></summary>
		new StringBuilder Buf01 { get; set; }

		long[] GU { get; set; }

		string[] RM { get; set; }

		long[] RN { get; set; }

		long SecretBonus { get; }

		bool RestoreGame { get; set; }

		void PrintPebblesLeft(IArtifact artifact);

		void PrintHealingHerbsLeft(IArtifact artifact);

		long GetGU(long index);

		string GetRM(long index);

		long GetRN(long index);
	}
}
