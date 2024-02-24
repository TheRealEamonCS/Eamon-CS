
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework.Primitive.Enums;
using Eamon.Framework.Utilities;

namespace Eamon.Framework
{
	/// <summary>
	/// Represents the base properties that comprise the saved state of a game. These properties are stored when the
	/// game is saved and restored when the game is resumed.
	/// </summary>
	/// <remarks>
	/// Game designers can extend this interface and add new properties needed for the game state of their
	/// new game, implementing those properties in the game's underlying derived GameState class. Provided the
	/// SharpSerializer library can handle the new properties, the save and restore of the game's new GameState is
	/// automatically supported by Eamon CS. Luckily, this library is very powerful and only exotic things like cyclic
	/// graphs aren't supported. Be careful to distinguish between game state and scratch variables used only during
	/// gameplay, which are best stored elsewhere (perhaps in <see cref="Plugin.IEngine">IEngine</see>).
	/// </remarks>
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

		/// <summary>
		/// Gets or sets a value indicating the player character death status (-1=Quitting Game; 0=Alive; 1=Dead).
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
		/// Gets or sets a value indicating whether names for <see cref="IMonster">Monster</see>s and
		/// <see cref="IArtifact">Artifact</see>s should always be printed verbosely.
		/// </summary>
		bool Vn { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether mature content should be allowed during gameplay.
		/// </summary>
		bool MatureContent { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the game is being played as traditional Interactive
		/// Fiction or an Eamon adventure.
		/// </summary>
		bool InteractiveFiction { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the game is using an enhanced parser or traditional
		/// Eamon parser.
		/// </summary>
		bool EnhancedParser { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether pronouns attach to the most recently used indirect
		/// or direct object (enhanced parser only).
		/// </summary>
		bool IobjPronounAffinity { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the parser emits a message when it changes the object
		/// referred to by a pronoun. (enhanced parser only).
		/// </summary>
		bool ShowPronounChanges { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the parser emits a message when it executes each
		/// command in a series (enhanced parser only).
		/// </summary>
		bool ShowFulfillMessages { get; set; }

		/// <summary>
		/// Gets or sets the current turn in the game; this is incremented after player command parsing but before execution.
		/// </summary>
		long CurrTurn { get; set; }

		/// <summary>
		/// Gets or sets the number of milliseconds to pause between actions during a combat round.
		/// </summary>
		long PauseCombatMs { get; set; }

		/// <summary>
		/// Gets or sets the number of actions to pause between during a combat round.
		/// </summary>
		long PauseCombatActions { get; set; }

		/// <summary>
		/// Gets or sets a value that indexes <see cref="ImportedArtUids">ImportedArtUids</see> and indicates the next
		/// available element to store an imported <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see>.
		/// </summary>
		long ImportedArtUidsIdx { get; set; }

		/// <summary>
		/// Gets or sets a value that indexes <see cref="HeldWpnUids">HeldWpnUids</see> and indicates which weapon in that
		/// array is being used by the player in the game (Beginner's adventures only).
		/// </summary>
		long UsedWpnIdx { get; set; }

		/// <summary>
		/// Gets or sets an array of player character <see cref="Spell">Spell</see> abilities that is updated during gameplay.
		/// </summary>
		/// <remarks>
		/// This array is initialized and synchronized with the player character's permanent
		/// <see cref="ICharacter.SpellAbilities">SpellAbilities</see> before and during the game. Avoid accessing array elements
		/// directly in favor of using Getter/Setter/Mutator methods to ensure games can override when necessary.
		/// </remarks>
		/// <seealso cref="GetSa(long)"/>
		/// <seealso cref="GetSa(Spell)"/>
		/// <seealso cref="SetSa(long, long)"/>
		/// <seealso cref="SetSa(Spell, long)"/>
		/// <seealso cref="ModSa(long, long)"/>
		/// <seealso cref="ModSa(Spell, long)"/>
		long[] Sa { get; set; }

		/// <summary>
		/// Gets or sets an array of <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see>s corresponding
		/// to the Artifacts brought into the game by the player (armor, shield and weapons).
		/// </summary>
		/// <remarks>
		/// This array is loaded during game startup. The <see cref="ImportedArtUidsIdx">ImportedArtUidsIdx</see> property
		/// indicates the number of <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see>s stored in the
		/// array (and also the next element available to store another). Avoid accessing array elements directly in favor
		/// of using Getter/Setter methods to ensure games can override when necessary.
		/// </remarks>
		/// <seealso cref="GetImportedArtUid(long)"/>
		/// <seealso cref="SetImportedArtUid(long, long)"/>
		long[] ImportedArtUids { get; set; }

		/// <summary>
		/// Gets or sets an array of <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see>s corresponding
		/// to the weapons brought into the game by the player (Beginner's adventures only).
		/// </summary>
		/// <remarks>
		/// All but one of these weapons is put in limbo at game startup; the <see cref="UsedWpnIdx">UsedWpnIdx</see> property
		/// indicates which weapon is being used by the player in the game.  All weapons are returned to the player when the
		/// game is exited. Avoid accessing array elements directly in favor of using Getter/Setter methods to ensure games
		/// can override when necessary.
		/// </remarks>
		/// <seealso cref="GetHeldWpnUid(long)"/>
		/// <seealso cref="SetHeldWpnUid(long, long)"/>
		long[] HeldWpnUids { get; set; }

		/// <summary>
		/// Gets or sets the data structure that holds events to be fired before the player character's current
		/// <see cref="IRoom">Room</see> is printed.</summary>
		/// <remarks></remarks>
		IEventHeap BeforePrintPlayerRoomEventHeap { get; set; }

		/// <summary>
		/// Gets or sets the data structure that holds events to be fired after the player character's current
		/// <see cref="IRoom">Room</see> is printed, but before the command prompt is displayed.
		/// </summary>
		/// <remarks></remarks>
		IEventHeap AfterPrintPlayerRoomEventHeap { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Gets the total <see cref="Stat.Hardiness">Hardiness</see> of <see cref="IMonster">Monsters</see> with a specified
		/// <see cref="Friendliness"/> in a particular <see cref="IRoom">Room</see>.
		/// </summary>
		/// <param name="index">The Friendliness value expressed as a long datatype.</param>
		/// <param name="roomUid">The Room <see cref="IGameBase.Uid"> Uid</see>, or 0 for the player's current Room. (Optional)</param>
		/// <remarks>
		/// For group Monsters, the value added to the sum is equal to (Hardiness * <see cref="IMonster.CurrGroupCount">CurrGroupCount</see>).
		/// When the <paramref name="roomUid"/> is omitted the player's current Room is used. This method can be overridden in games when
		/// necessary.
		/// </remarks>
		/// <returns>A single summed Hardiness value for all affected Monsters.</returns>
		/// <seealso cref="GetNBTL(Friendliness, long)"/>
		long GetNBTL(long index, long roomUid = 0);

		/// <summary>
		/// Gets the total <see cref="Stat.Hardiness">Hardiness</see> of <see cref="IMonster">Monsters</see> with a specified
		/// <see cref="Friendliness"/> in a particular <see cref="IRoom">Room</see>.
		/// </summary>
		/// <param name="friendliness">The Friendliness value.</param>
		/// <param name="roomUid">The Room <see cref="IGameBase.Uid"> Uid</see>, or 0 for the player's current Room. (Optional)</param>
		/// <remarks>
		/// This convenience wrapper should typically never be overridden in games. The other base method remarks are also applicable here.
		/// </remarks>
		/// <returns>A single summed Hardiness value for all affected Monsters.</returns>
		/// <seealso cref="GetNBTL(long, long)"/>
		long GetNBTL(Friendliness friendliness, long roomUid = 0);

		/// <summary>
		/// Gets the total <see cref="IMonster.DmgTaken">DmgTaken</see> of <see cref="IMonster">Monsters</see> with a specified
		/// <see cref="Friendliness"/> in a particular <see cref="IRoom">Room</see>.
		/// </summary>
		/// <param name="index">The Friendliness value expressed as a long datatype.</param>
		/// <param name="roomUid">The Room <see cref="IGameBase.Uid"> Uid</see>, or 0 for the player's current Room. (Optional)</param>
		/// <remarks>
		/// This method returns the summed damage taken for games using Ruleset Version 5 or 62; for others it returns 0. For group
		/// Monsters, the value added to the sum is the damage taken by the "current" group member. When the <paramref name="roomUid"/>
		/// is omitted the player's current Room is used. This method can be overridden in games when necessary.
		/// </remarks>
		/// <returns>A single summed DmgTaken value for all affected Monsters.</returns>
		/// <seealso cref="GetDTTL(Friendliness, long)"/>
		long GetDTTL(long index, long roomUid = 0);

		/// <summary>
		/// Gets the total <see cref="IMonster.DmgTaken">DmgTaken</see> of <see cref="IMonster">Monsters</see> with a specified
		/// <see cref="Friendliness"/> in a particular <see cref="IRoom">Room</see>.
		/// </summary>
		/// <param name="friendliness">The Friendliness value.</param>
		/// <param name="roomUid">The Room <see cref="IGameBase.Uid"> Uid</see>, or 0 for the player's current Room. (Optional)</param>
		/// <remarks>
		/// This convenience wrapper should typically never be overridden in games. The other base method remarks are also applicable here.
		/// </remarks>
		/// <returns>A single summed DmgTaken value for all affected Monsters.</returns>
		/// <seealso cref="GetDTTL(long, long)"/>
		long GetDTTL(Friendliness friendliness, long roomUid = 0);

		/// <summary>Gets the player's current ability for a given <see cref="Spell"/>.</summary>
		/// <param name="index">The Spell value expressed as a long datatype.</param>
		/// <remarks>
		/// The value changes during gameplay based on Spell usage and regeneration each round. This method can be overridden in games
		/// when necessary.
		/// </remarks>
		/// <returns>The current ability value for the Spell.</returns>
		/// <seealso cref="GetSa(Spell)"/>
		long GetSa(long index);

		/// <summary>Gets the player's current ability for a given <see cref="Spell"/>.</summary>
		/// <param name="spell">The Spell value.</param>
		/// <remarks>
		/// This convenience wrapper should typically never be overridden in games. The other base method remarks are also applicable here.
		/// </remarks>
		/// <returns>The current ability value for the Spell.</returns>
		/// <seealso cref="GetSa(long)"/>
		long GetSa(Spell spell);

		/// <summary>Gets the imported <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see> at a given index.</summary>
		/// <param name="index">The array index.</param>
		/// <remarks>The <see cref="ImportedArtUids">ImportedArtUids</see> array stores Artifacts imported by the player into the game. This
		/// method can be overridden in games when necessary.</remarks>
		/// <returns>The imported Artifact Uid.</returns>
		long GetImportedArtUid(long index);

		/// <summary>Gets the held weapon <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see> at a given index.</summary>
		/// <param name="index">The array index.</param>
		/// <remarks>The <see cref="HeldWpnUids">HeldWpnUids</see> array stores all weapon Artifacts brought into a Beginner's adventure. This
		/// method can be overridden in games when necessary.</remarks>
		/// <returns>The held weapon Artifact Uid.</returns>
		long GetHeldWpnUid(long index);

		/// <summary>Sets the player's current ability for a given <see cref="Spell"/>.</summary>
		/// <param name="index">The Spell value expressed as a long datatype.</param>
		/// <param name="value">The new current ability value.</param>
		/// <remarks>
		/// The current ability value can be changed during gameplay for any reason. This method can be overridden in games
		/// when necessary.
		/// </remarks>
		/// <seealso cref="SetSa(Spell, long)"/>
		void SetSa(long index, long value);

		/// <summary>Sets the player's current ability for a given <see cref="Spell"/>.</summary>
		/// <param name="spell">The Spell value.</param>
		/// <param name="value">The new current ability value.</param>
		/// <remarks>
		/// This convenience wrapper should typically never be overridden in games. The other base method remarks are also applicable here.
		/// </remarks>
		/// <seealso cref="SetSa(long, long)"/>
		void SetSa(Spell spell, long value);

		/// <summary>
		/// Sets the imported <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see> at a
		/// specified index to a given value.
		/// </summary>
		/// <param name="index">The array index.</param>
		/// <param name="value">The new imported Artifact Uid.</param>
		/// <remarks>
		/// The imported Artifact Uid is stored in the <see cref="ImportedArtUids">ImportedArtUids</see>
		/// array. This method can be overridden in games when necessary.
		/// </remarks>
		void SetImportedArtUid(long index, long value);

		/// <summary>
		/// Sets the held weapon <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see> at a
		/// specified index to a given value.
		/// </summary>
		/// <param name="index">The array index.</param>
		/// <param name="value">The new held weapon Artifact Uid.</param>
		/// <remarks>
		/// The held weapon Artifact Uid is stored in the <see cref="HeldWpnUids">HeldWpnUids</see> array.
		/// This method can be overridden in games when necessary.
		/// </remarks>
		void SetHeldWpnUid(long index, long value);

		/// <summary>Modifies the player's current ability for a given <see cref="Spell"/>.</summary>
		/// <param name="index">The Spell value expressed as a long datatype.</param>
		/// <param name="value">The modifier to the current ability value.</param>
		/// <remarks>
		/// The current ability value can be incremented or decremented during gameplay for any reason, based on the
		/// <paramref name="value"/> being positive or negative. This method can be overridden in games when necessary.
		/// </remarks>
		/// <seealso cref="ModSa(Spell, long)"/>
		void ModSa(long index, long value);

		/// <summary>Modifies the player's current ability for a given <see cref="Spell"/>.</summary>
		/// <param name="spell">The Spell value.</param>
		/// <param name="value">The modifier to the current ability value.</param>
		/// <remarks>
		/// This convenience wrapper should typically never be overridden in games. The other base method remarks are also applicable here.
		/// </remarks>
		/// <seealso cref="ModSa(long, long)"/>
		void ModSa(Spell spell, long value);

		#endregion
	}
}
