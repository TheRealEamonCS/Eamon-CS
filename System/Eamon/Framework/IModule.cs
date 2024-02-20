
// IModule.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	/// <remarks></remarks>
	public interface IModule : IGameBase, IComparable<IModule>
	{
		#region Properties

		/// <summary>
		/// Gets or sets the name(s) of the author(s) for this adventure.
		/// </summary>
		string Author { get; set; }

		/// <summary>
		/// Gets or sets the volume label for this adventure; this is a code consisting of the author(s) initials and
		/// an author-specific sequence number, e.g. MP-001.
		/// </summary>
		string VolLabel { get; set; }

		/// <summary>
		/// Gets or sets the serial number for this adventure; this is a sequence number that spans authors and indicates
		/// the number of games in Eamon CS, e.g. 013.
		/// </summary>
		string SerialNum { get; set; }

		/// <summary>
		/// Gets or sets the last date this adventure was modified; this is updated by the EamonDD game editor.
		/// </summary>
		DateTime LastMod { get; set; }

		/// <summary>
		/// Gets or sets an <see cref="IEffect">Effect</see> <see cref="IGameBase.Uid"> Uid</see> that corresponds to the
		/// introduction story for this adventure.
		/// </summary>
		/// <remarks>
		/// The <see cref="IEffect">Effect</see> referenced by this value will frequently be the initial prose in a set of
		/// Events chained together using the typical Eamon CS mechanism.  You can look at any existing game for an example
		/// of this.  The value may also be zero (0), in which case the adventure has no introduction story.
		/// </remarks>
		long IntroStory { get; set; }

		/// <summary>
		/// Gets or sets the number of directional links for <see cref="IRoom">Room</see>s in this adventure.  Valid values
		/// are 6 or 12, corresponding to compass <see cref="Direction">Direction</see>s.
		/// </summary>
		long NumDirs { get; set; }

		/// <summary>
		/// Gets or sets the number of <see cref="IRoom">Room</see>s in this adventure.  This value is kept in sync with
		/// the corresponding ROOMS.DAT file.
		/// </summary>
		long NumRooms { get; set; }

		/// <summary>
		/// Gets or sets the number of <see cref="IArtifact">Artifact</see>s in this adventure.  This value is kept in sync with
		/// the corresponding ARTIFACTS.DAT file.
		/// </summary>
		long NumArtifacts { get; set; }

		/// <summary>
		/// Gets or sets the number of <see cref="IEffect">Effect</see>s in this adventure.  This value is kept in sync with
		/// the corresponding EFFECTS.DAT file.
		/// </summary>
		long NumEffects { get; set; }

		/// <summary>
		/// Gets or sets the number of <see cref="IMonster">Monster</see>s in this adventure.  This value is kept in sync with
		/// the corresponding MONSTERS.DAT file.
		/// </summary>
		long NumMonsters { get; set; }

		/// <summary>
		/// Gets or sets the number of <see cref="IHint">Hint</see>s in this adventure.  This value is kept in sync with
		/// the corresponding HINTS.DAT file.
		/// </summary>
		long NumHints { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		void PrintInfo();

		#endregion
	}
}
