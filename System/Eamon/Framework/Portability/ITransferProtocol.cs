
// ITransferProtocol.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public interface ITransferProtocol
	{
		/// <summary></summary>
		/// <param name="filePrefix"></param>
		/// <param name="filesetFileName"></param>
		/// <param name="characterFileName"></param>
		/// <param name="effectFileName"></param>
		/// <param name="characterName"></param>
		void SendCharacterToMainHall(string filePrefix, string filesetFileName, string characterFileName, string effectFileName, string characterName);

		/// <summary></summary>
		/// <param name="workDir"></param>
		/// <param name="filePrefix"></param>
		/// <param name="pluginFileName"></param>
		void SendCharacterOnAdventure(string workDir, string filePrefix, string pluginFileName);

		/// <summary></summary>
		/// <param name="workDir"></param>
		/// <param name="filePrefix"></param>
		/// <param name="pluginFileName"></param>
		void RecallCharacterFromAdventure(string workDir, string filePrefix, string pluginFileName);
	}
}
