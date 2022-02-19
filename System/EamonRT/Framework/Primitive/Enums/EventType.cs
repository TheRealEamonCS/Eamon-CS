
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
		/// An event that fires at the end of the current round, after all processing is done.
		/// </summary>
		AfterEndRound = 0,

		/// <summary>
		/// An event that fires at the start of a new round, before any processing is done.
		/// </summary>
		BeforeStartRound,

		/// <summary>
		/// An event that fires after the player moves to a new <see cref="IRoom">Room</see>, and any carried light
		/// source is extinguished (if necessary).
		/// </summary>
		AfterExtinguishLightSourceCheck,

		/// <summary>
		/// An event that fires after the player moves to a new <see cref="IRoom">Room</see>, and any <see cref="IMonster">Monster</see>s
		/// in the exited Room (friendly or hostile) have followed.
		/// </summary>
		AfterMoveMonsters,

		/// <summary>
		/// An event that fires after the player's destination <see cref="IRoom">Room</see> <see cref="IGameBase.Uid"> Uid</see>
		/// is calculated and stored.
		/// </summary>
		AfterSetDestinationRoom,

		/// <summary>
		/// An event that fires before the player's command prompt is printed.
		/// </summary>
		BeforePrintCommandPrompt,

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
		/// An event that fires before the player's <see cref="IRoom">Room</see> is printed.
		/// </summary>
		BeforePrintPlayerRoom,

		/// <summary>
		/// An event that fires after the player's command is processed (but not executed)
		/// and the <see cref="IPluginGlobals.LastCommandList">LastCommandList</see> cleared.
		/// </summary>
		AfterClearLastCommandList,

		/// <summary>
		/// An event that fires after a <see cref="IMonster">Monster</see> targeted by an attack
		/// gets aggravated.
		/// </summary>
		AfterAggravateMonster,

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
		/// An event that fires after checking if a readied weapon is given away.
		/// </summary>
		AfterGiveReadiedWeaponCheck,

		/// <summary>
		/// An event that fires before a <see cref="IMonster">Monster</see> takes the gold offered by the player.
		/// </summary>
		BeforeTakePlayerGold,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see> is closed.
		/// </summary>
		AfterCloseArtifact,

		/// <summary>
		/// An event that fires before checking if an <see cref="IArtifact">Artifact</see> is fully drunk.
		/// </summary>
		BeforeNowEmptyArtifactCheck,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see> is drunk.
		/// </summary>
		AfterDrinkArtifact,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see> is eaten.
		/// </summary>
		AfterEatArtifact,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see>'s full description is printed (but before
		/// units are listed for drinkables/edibles).
		/// </summary>
		AfterPrintArtifactFullDesc,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see>'s container contents are printed.
		/// </summary>
		AfterPrintArtifactContents,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see> is opened but before the open message is printed.
		/// </summary>
		BeforePrintArtifactOpen,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see> is opened.
		/// </summary>
		AfterOpenArtifact,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see> is put into a container.
		/// </summary>
		AfterPutArtifact,

		/// <summary>
		/// An event that fires before an <see cref="IArtifact">Artifact</see>'s read text is printed.
		/// </summary>
		BeforePrintArtifactReadText,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see> is read.
		/// </summary>
		AfterReadArtifact,

		/// <summary>
		/// An event that fires after a worn <see cref="IArtifact">Artifact</see> is removed.
		/// </summary>
		AfterRemoveWornArtifact,

		/// <summary>
		/// An event that fires before an <see cref="IArtifact">Artifact</see> is used.
		/// </summary>
		BeforeUseArtifact,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see> is worn.
		/// </summary>
		AfterWearArtifact,

		/// <summary>
		/// An event that fires before spoken text is printed.
		/// </summary>
		BeforePrintSayText,

		/// <summary>
		/// An event that fires after spoken text is printed.
		/// </summary>
		AfterPrintSayText,

		/// <summary>
		/// An event that fires after the player's status text is printed.
		/// </summary>
		AfterPrintPlayerStatus,

		/// <summary>
		/// An event that fires after checking if exits are available for fleeing, and it resolves that there are.
		/// </summary>
		AfterNumberOfExitsCheck,

		/// <summary>
		/// An event that fires after an <see cref="IArtifact">Artifact</see> is readied.
		/// </summary>
		AfterReadyArtifact,

		/// <summary>
		/// An event that fires before a <see cref="IMonster">Monster</see> is attacked or blasted.
		/// </summary>
		BeforeAttackMonster,

		/// <summary>
		/// An event that fires before an <see cref="IArtifact">Artifact</see> is attacked or blasted.
		/// </summary>
		BeforeAttackArtifact,

		/// <summary>
		/// An event that fires before a <see cref="IMonster">Monster</see>'s health status is examined.
		/// </summary>
		BeforeExamineMonsterHealthStatus,

		/// <summary>
		/// An event that fires after a <see cref="IMonster">Monster</see>'s health status is examined.
		/// </summary>
		AfterExamineMonsterHealthStatus,

		/// <summary>
		/// An event that fires before a <see cref="IMonster">Monster</see>'s health status is inventoried.
		/// </summary>
		BeforeInventoryMonsterHealthStatus,

		/// <summary>
		/// An event that fires after a <see cref="IMonster">Monster</see>'s health status is inventoried.
		/// </summary>
		AfterInventoryMonsterHealthStatus,

		/// <summary>
		/// An event that fires after checking if a non-enemy <see cref="IMonster">Monster</see> should be attacked.
		/// </summary>
		AfterAttackNonEnemyCheck,
	}
}
