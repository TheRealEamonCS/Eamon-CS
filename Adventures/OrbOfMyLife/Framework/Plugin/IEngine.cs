
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace OrbOfMyLife.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		bool VerboseRoomDescOrNotSeen { get; set; }

		bool RestoreGame { get; set; }

		void BuildRandomRoomExits(Eamon.Framework.IRoom room);

		void BuildRandomMonster(Eamon.Framework.IRoom room, Eamon.Framework.IMonster monster, Eamon.Framework.IArtifact deadBodyArtifact);

		void MonstersGetUnnerved(bool prependNewLine = false);

		void CrystalBallShatters(Eamon.Framework.IRoom room, Eamon.Framework.IArtifact artifact);
	}
}
