
// EventType.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.Primitive.Enums
{
	/// <summary></summary>
	public enum EventType : long
	{
		/// <summary>
		/// An event that fires at the end of the current round, after all processing has been done.
		/// </summary>
		AfterRoundEnd = 0,

		/// <summary>
		/// An event that fires at the start of a new round, before any processing has been done.
		/// </summary>
		BeforeRoundStart,

		/// <summary>
		/// An event that fires after the player has moved to a new <see cref="IRoom">Room</see>, and any carried light
		/// source has been extinguished (if necessary).
		/// </summary>
		AfterExtinguishLightSourceCheck,

		/// <summary>
		/// An event that fires after the player has moved to a new <see cref="IRoom">Room</see>, and any <see cref="IMonster">Monster</see>s
		/// in the exited Room (friendly or hostile) have followed.
		/// </summary>
		AfterMoveMonsters,

		/// <summary>
		/// An event that fires after the player's destination <see cref="IRoom">Room</see> <see cref="IGameBase.Uid"> Uid</see>
		/// is calculated and stored.
		/// </summary>
		AfterDestinationRoomSet,

		/// <summary>
		/// An event that fires before the player's command prompt is printed.
		/// </summary>
		BeforeCommandPromptPrint,

		/// <summary>
		/// An event that fires before it is known whether the player can move to a <see cref="IRoom">Room</see>.
		/// </summary>
		BeforeCanMoveToRoomCheck,

		/// <summary>
		/// An event that fires after it is known whether a blocking <see cref="IArtifact">Artifact</see> (for example,
		/// a door) prevents the player's movement.
		/// </summary>
		AfterBlockingArtifactCheck,

		/// <summary>
		/// An event that fires before the player's <see cref="IRoom">Room</see> has been printed.
		/// </summary>
		BeforePlayerRoomPrint,

		/// <summary>
		/// An event that fires after the player's command has been processed (but not executed)
		/// and the <see cref="IPluginGlobals.LastCommandList">LastCommandList</see> cleared.
		/// </summary>
		AfterLastCommandListClear,

		/// <summary>
		/// An event that fires after the player's spell cast attempt has resolved as successful.
		/// </summary>
		AfterPlayerSpellCastCheck,

		/// <summary>
		/// An event that fires after the <see cref="IMonster">Monster</see> targeted by the <see cref="Spell.Blast">Blast</see>
		/// spell gets aggravated.
		/// </summary>
		AfterMonsterGetsAggravated,

		/// <summary>
		/// An event that fires before a guard <see cref="IMonster">Monster</see> prevents a bound Monster from being freed.
		/// </summary>
		BeforeGuardMonsterCheck,

		/// <summary>
		/// An event that fires before a key <see cref="IArtifact">Artifact</see> prevents a bound Monster from being freed.
		/// </summary>
		BeforeKeyArtifactCheck,

		/// <summary>
		/// An event that fires after limits are enforced on the weight a <see cref="IMonster">Monster</see> can carry.
		/// </summary>
		AfterEnforceMonsterWeightLimitsCheck,

		/// <summary>
		/// An event that fires after checking whether the player is giving away a readied weapon.
		/// </summary>
		AfterPlayerGivesReadiedWeaponCheck,

		/// <summary>
		/// An event that fires before a <see cref="IMonster">Monster</see> takes the gold offered by the player.
		/// </summary>
		BeforeMonsterTakesGold,

		/// <summary>
		/// An event that fires after the player closes an <see cref="IArtifact">Artifact</see>.
		/// </summary>
		AfterArtifactClose,

		/// <summary>
		/// An event that fires before checking whether an <see cref="IArtifact">Artifact</see> has been fully drunk.
		/// </summary>
		BeforeArtifactNowEmptyCheck,

		/// <summary>
		/// An event that fires after the player drinks an <see cref="IArtifact">Artifact</see>.
		/// </summary>
		AfterArtifactDrink,

		/// <summary>
		/// An event that fires after the player eats an <see cref="IArtifact">Artifact</see>.
		/// </summary>
		AfterArtifactEat,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see>'s full description has been printed (but before
		/// units are listed for drinkables/edibles).
		/// </summary>
		AfterArtifactFullDescPrint,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see>'s container contents are printed.
		/// </summary>
		AfterArtifactContentsPrint,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see>'s open message has been printed (but before
		/// inventory is listed for containers).
		/// </summary>
		AfterArtifactOpenPrint,

		/// <summary>
		/// An event that fires after the player opens an <see cref="IArtifact">Artifact</see>.
		/// </summary>
		AfterArtifactOpen,

		/// <summary>
		/// An event that fires after the player puts an <see cref="IArtifact">Artifact</see> into a container.
		/// </summary>
		AfterArtifactPut,

		/// <summary>
		/// An event that fires before an <see cref="IArtifact">Artifact</see>'s read text is printed.
		/// </summary>
		BeforeArtifactReadTextPrint,

		/// <summary>
		/// An event that fires after the player reads an <see cref="IArtifact">Artifact</see>.
		/// </summary>
		AfterArtifactRead,

		/// <summary></summary>
		AfterWornArtifactRemove,

		/// <summary>
		/// An event that fires before the player uses an <see cref="IArtifact">Artifact</see>.
		/// </summary>
		BeforeArtifactUse,

		/// <summary>
		/// An event that fires after the player wears an <see cref="IArtifact">Artifact</see>.
		/// </summary>
		AfterArtifactWear,

		/// <summary>
		/// An event that fires before the player's spoken text is printed.
		/// </summary>
		BeforePlayerSayTextPrint,

		/// <summary>
		/// An event that fires after the player says something.
		/// </summary>
		AfterPlayerSay,

		/// <summary>
		/// An event that fires after the player's status text has been printed.
		/// </summary>
		AfterPlayerStatus,

		/// <summary>
		/// An event that fires after checking whether exits are available for fleeing, and it resolves that there are.
		/// </summary>
		AfterNumberOfExitsCheck,
	}
}
