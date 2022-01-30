
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Utilities;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IGameState : IGameBase, IComparable<IGameState>
	{
		#region Properties

		/// <summary>
		/// Gets or sets the <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see> for the armor being worn by
		/// the player character (may be 0 if none exists).
		/// </summary>
		long Ar { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="IMonster">Monster</see><see cref="IGameBase.Uid"> Uid</see> being used to represent the
		/// player character.
		/// </summary>
		long Cm { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see> for the currently active
		/// <see cref="ArtifactType.LightSource">LightSource</see> (may be 0 if none exists).
		/// </summary>
		/// <remarks>
		/// Here are some basics about the <see cref="ArtifactType.LightSource">LightSource </see><see cref="ArtifactType">ArtifactType</see>
		/// in Eamon CS.  Like Eamon Deluxe, it directly supports only one light source active at a time, and it must be in the <see cref="IRoom">Room</see>
		/// with the player.  When the LightSource is active, its <see cref="IGameBase.Uid">Uid</see> is stored in <see cref="Ls">Ls</see>; if
		/// no LightSource is active, this property will be 0.  Furthermore, if the LightSource is portable, it must be carried by the player
		/// when activated as well (the game automatically causes the player character to pick it up).  If a portable LightSource is dropped,
		/// it will go out; if the LightSource is not portable, it will go out when the player leaves the room.  If one LightSource is lit when
		/// another is active, the currently lit LightSource will go out.
		/// </remarks>
		long Ls { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="IRoom">Room</see><see cref="IGameBase.Uid"> Uid</see> for the player character's current
		/// location (may be 0 if in limbo).
		/// </summary>
		long Ro { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="IRoom">Room</see><see cref="IGameBase.Uid"> Uid</see> for the location the player character
		/// is entering (during a movement operation).
		/// </summary>
		long R2 { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="IRoom">Room</see><see cref="IGameBase.Uid"> Uid</see> for the location the player character
		/// is exiting (during a movement operation).
		/// </summary>
		long R3 { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see> for the shield being worn by
		/// the player character (may be 0 if none exists).
		/// </summary>
		long Sh { get; set; }

		/// <summary></summary>
		long Af { get; set; }

		/// <summary>
		/// Gets or sets a value indicating the player character death status (-1=Quitting Game, 0=Alive, 1=Dead).
		/// </summary>
		long Die { get; set; }

		/// <summary>
		/// Gets or sets the number of rounds remaining before the player character's <see cref="Spell.Speed">Speed</see> spell expires.
		/// </summary>
		long Speed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="IRoom">Room</see>s should always be printed verbosely.
		/// </summary>
		bool Vr { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="IMonster">Monster</see>s should always be printed verbosely.
		/// </summary>
		bool Vm { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="IArtifact">Artifact</see>s should always be printed verbosely.
		/// </summary>
		bool Va { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether mature content should be allowed during gameplay.
		/// </summary>
		bool MatureContent { get; set; }

		/// <summary></summary>
		bool EnhancedParser { get; set; }

		/// <summary></summary>
		bool ShowPronounChanges { get; set; }

		/// <summary></summary>
		bool ShowFulfillMessages { get; set; }

		/// <summary>
		/// Gets or sets the current turn in the game; this is incremented after player command parsing but before execution.
		/// </summary>
		long CurrTurn { get; set; }

		/// <summary>
		/// Gets or sets the number of milliseconds to pause between attacks during a combat round.
		/// </summary>
		long PauseCombatMs { get; set; }

		/// <summary>
		/// Gets or sets a value that indexes <see cref="ImportedArtUids">ImportedArtUids</see> and indicates the next
		/// available element to store an imported <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see>.
		/// </summary>
		long ImportedArtUidsIdx { get; set; }

		/// <summary>
		/// Gets or sets a value that indexes <see cref="HeldWpnUids">HeldWpnUids</see> and indicates which weapon in that
		/// array is being used by the player in the game.  (Only used in the beginner's adventures).
		/// </summary>
		long UsedWpnIdx { get; set; }

		/// <summary>
		/// Gets or sets an array of player character <see cref="Spell">Spell</see> abilities that is updated during gameplay.
		/// </summary>
		long[] Sa { get; set; }

		/// <summary>
		/// Gets or sets an array of <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see>s corresponding
		/// to the Artifacts brought into the game by the player (armor, shield and weapons).
		/// </summary>
		/// <remarks>
		/// The <see cref="ImportedArtUidsIdx">ImportedArtUidsIdx</see> property indicates the number of
		/// <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see>s stored in the array (and also the next
		/// element available to store another).
		/// </remarks>
		long[] ImportedArtUids { get; set; }

		/// <summary>
		/// Gets or sets an array of <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see>s corresponding
		/// to the weapons brought into the game by the player.  (Only used in the beginner's adventures).
		/// </summary>
		/// <remarks>
		/// All but one of these weapons is put in limbo; the <see cref="UsedWpnIdx">UsedWpnIdx</see> property indicates which
		/// weapon is being used by the player in the game.  All weapons are returned to the player when the game is exited.
		/// </remarks>
		long[] HeldWpnUids { get; set; }

		/// <summary></summary>
		/// <remarks></remarks>
		EventHeap BeforePrintPlayerRoomEventHeap { get; set; }

		/// <summary></summary>
		/// <remarks></remarks>
		EventHeap AfterPrintPlayerRoomEventHeap { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetNBTL(long index);

		/// <summary></summary>
		/// <param name="friendliness"></param>
		/// <returns></returns>
		long GetNBTL(Friendliness friendliness);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetDTTL(long index);

		/// <summary></summary>
		/// <param name="friendliness"></param>
		/// <returns></returns>
		long GetDTTL(Friendliness friendliness);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetSa(long index);

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <returns></returns>
		long GetSa(Spell spell);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetImportedArtUids(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetHeldWpnUids(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetSa(long index, long value);

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <param name="value"></param>
		void SetSa(Spell spell, long value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetImportedArtUids(long index, long value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetHeldWpnUids(long index, long value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void ModSa(long index, long value);

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <param name="value"></param>
		void ModSa(Spell spell, long value);

		#endregion
	}
}
