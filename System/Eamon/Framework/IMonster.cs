
// IMonster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	/// <remarks></remarks>
	public interface IMonster : IGameBase, IComparable<IMonster>
	{
		#region Properties

		/// <summary>
		/// Gets or sets a description shown after this <see cref="IMonster">Monster</see>'s <see cref="IGameBase.Name">Name</see> in
		/// various lists that indicates its state.
		/// </summary>
		string StateDesc { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IMonster">Monster</see> should be displayed in
		/// various lists.
		/// </summary>
		bool IsListed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating how this <see cref="IMonster">Monster</see>'s singular <see cref="IGameBase.Name">Name</see> is
		/// modified to produce its plural Name.
		/// </summary>
		PluralType PluralType { get; set; }

		/// <summary></summary>
		long Hardiness { get; set; }

		/// <summary></summary>
		long Agility { get; set; }

		/// <summary>
		/// Gets or sets the original number of members in this <see cref="IMonster">Monster</see>'s group at the beginning of the game (will
		/// be 1 for individuals).
		/// </summary>
		long GroupCount { get; set; }

		/// <summary>
		/// Gets or sets the number of attacks each member in this <see cref="IMonster">Monster</see>'s group can make every combat round.
		/// </summary>
		long AttackCount { get; set; }

		/// <summary></summary>
		long Courage { get; set; }

		/// <summary>
		/// Gets or sets the location of this <see cref="IMonster">Monster</see> in the game (typically a <see cref="IRoom">Room</see>
		/// <see cref="IGameBase.Uid">Uid</see>).
		/// </summary>
		long Location { get; set; }

		/// <summary>
		/// Gets or sets a value indicating this <see cref="IMonster">Monster</see>'s behavior when in combat.
		/// </summary>
		CombatCode CombatCode { get; set; }

		/// <summary></summary>
		long Armor { get; set; }

		/// <summary></summary>
		long Weapon { get; set; }

		/// <summary>
		/// Gets or sets the hit dice done per attack, for <see cref="IMonster">Monster</see>s that can use natural attacks (based
		/// on <see cref="CombatCode">CombatCode</see>).
		/// </summary>
		/// <remarks>
		/// In the XdY nomenclature used by roleplaying games, this is X.
		/// </remarks>
		long NwDice { get; set; }

		/// <summary>
		/// Gets or sets the hit dice sides done per attack, for <see cref="IMonster">Monster</see>s that can use natural attacks (based
		/// on <see cref="CombatCode">CombatCode</see>).
		/// </summary>
		/// <remarks>
		/// In the XdY nomenclature used by roleplaying games, this is Y.
		/// </remarks>
		long NwSides { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="IArtifact">Artifact</see><see cref="IGameBase.Uid"> Uid</see> for this <see cref="IMonster">Monster</see>'s
		/// dead body (may be 0 if none exists).
		/// </summary>
		long DeadBody { get; set; }

		/// <summary></summary>
		Friendliness Friendliness { get; set; }

		/// <summary>
		/// Gets or sets this <see cref="IMonster">Monster</see>'s gender.
		/// </summary>
		Gender Gender { get; set; }

		/// <summary>
		/// Gets or sets the initial number of members in this <see cref="IMonster">Monster</see>'s group at the beginning of the current
		/// combat round (will be 1 for individuals).
		/// </summary>
		long InitGroupCount { get; set; }

		/// <summary>
		/// Gets or sets the current number of members in this <see cref="IMonster">Monster</see>'s group (will be 1 for individuals).
		/// </summary>
		long CurrGroupCount { get; set; }

		/// <summary></summary>
		Friendliness Reaction { get; set; }

		/// <summary>
		/// Gets or sets the number of hit points of damage taken by this <see cref="IMonster">Monster</see>, either in combat or otherwise.
		/// </summary>
		long DmgTaken { get; set; }

		/// <summary>
		/// Gets or sets a value that is unused by Eamon CS, but provided for game developer usage.
		/// </summary>
		long Field1 { get; set; }

		/// <summary>
		/// Gets or sets a value that is unused by Eamon CS, but provided for game developer usage.
		/// </summary>
		long Field2 { get; set; }

		/// <summary>
		/// Gets or sets an array of <see cref="IMonsterSpell">MonsterSpell</see> objects that define
		/// this <see cref="IMonster">Monster</see>'s spellcasting ability (NPC only).
		/// </summary>
		IMonsterSpell[] Spells { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Indicates whether this <see cref="IMonster">Monster</see> is dead.
		/// </summary>
		/// <remarks>
		/// By default, to determine dead status the system compares <see cref="IMonster.DmgTaken">DmgTaken</see> with
		/// <see cref="IMonster.Hardiness">Hardiness</see>.
		/// </remarks>
		/// <returns>If so then <c>true</c>, otherwise <c>false</c>.</returns>
		bool IsDead();

		/// <summary></summary>
		/// <returns></returns>
		bool IsCarryingWeapon();

		/// <summary></summary>
		/// <param name="includeWeaponFumble"></param>
		/// <returns></returns>
		bool IsWeaponless(bool includeWeaponFumble);

		/// <summary></summary>
		/// <returns></returns>
		bool HasDeadBody();

		/// <summary></summary>
		/// <returns></returns>
		bool HasWornInventory();

		/// <summary></summary>
		/// <returns></returns>
		bool HasCarriedInventory();

		/// <summary></summary>
		/// <returns></returns>
		bool HasHumanNaturalAttackDescs();

		/// <summary></summary>
		/// <param name="oldLocation"></param>
		/// <param name="newLocation"></param>
		/// <returns></returns>
		bool HasMoved(long oldLocation, long newLocation);

		/// <summary></summary>
		/// <returns></returns>
		bool IsInRoom();

		/// <summary></summary>
		/// <returns></returns>
		bool IsInLimbo();

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <returns></returns>
		bool IsInRoomUid(long roomUid);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <returns></returns>
		bool IsInRoom(IRoom room);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		bool IsAttackable(IMonster monster);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <param name="fleeing"></param>
		/// <returns></returns>
		bool CanMoveToRoomUid(long roomUid, bool fleeing);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="fleeing"></param>
		/// <returns></returns>
		bool CanMoveToRoom(IRoom room, bool fleeing);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <param name="fleeing"></param>
		/// <returns></returns>
		bool CanMoveInDirection(Direction dir, bool fleeing);

		/// <summary>
		/// Indicates whether this <see cref="IMonster">Monster</see> is allowed to attack with multiple <see cref="IArtifact">Artifact</see> weapons
		/// in a single combat round, if certain other conditions are met.
		/// </summary>
		/// <remarks>
		/// This method allows support for <see cref="IMonster">Monster</see>s that can multi-attack and also multi-wield.  The conditions that need to
		/// be met to trigger this behavior include:  the Monster is an individual, it is armed and carrying more than one <see cref="IArtifact">Artifact</see>
		/// weapon and this method must return <c>true</c>.  By default, it returns <c>false</c>.
		/// </remarks>
		/// <returns>If so then <c>true</c>, otherwise <c>false</c>.</returns>
		bool CanAttackWithMultipleWeapons();

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		bool CanCarryArtifactWeight(IArtifact artifact);

		/// <summary></summary>
		/// <returns></returns>
		long GetCarryingWeaponUid();

		/// <summary></summary>
		/// <returns></returns>
		long GetDeadBodyUid();

		/// <summary></summary>
		/// <returns></returns>
		long GetInRoomUid();

		/// <summary></summary>
		/// <returns></returns>
		IRoom GetInRoom();

		/// <summary></summary>
		/// <param name="roomUid"></param>
		void SetInRoomUid(long roomUid);

		/// <summary></summary>
		void SetInLimbo();

		/// <summary></summary>
		/// <param name="room"></param>
		void SetInRoom(IRoom room);

		/// <summary></summary>
		/// <returns></returns>
		bool IsInRoomLit();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldFleeRoom();

		/// <summary></summary>
		/// <param name="spellCast"></param>
		/// <param name="spellTarget"></param>
		/// <returns></returns>
		bool ShouldCastSpell(ref Enums.Spell spellCast, ref IGameBase spellTarget);

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldReadyWeapon();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldShowContentsWhenExamined();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldShowHealthStatusWhenExamined();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldShowHealthStatusWhenInventoried();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldShowVerboseNameStateDesc();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldProcessInGameLoop();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldRefuseToAcceptGold();

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		bool ShouldRefuseToAcceptGift(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		bool ShouldRefuseToAcceptDeadBody(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		bool ShouldPreferNaturalWeaponsToWeakerWeapon(IArtifact artifact);

		/// <summary></summary>
		/// <returns></returns>
		bool CheckNBTLHostility();

		/// <summary></summary>
		/// <returns></returns>
		bool CheckCourage();

		/// <summary>
		/// Evaluates this <see cref="IMonster">Monster</see>'s <see cref="Reaction">Reaction</see>, returning a value of type T.
		/// </summary>
		/// <param name="enemyValue"></param>
		/// <param name="neutralValue"></param>
		/// <param name="friendValue"></param>
		/// <returns></returns>
		T EvalReaction<T>(T enemyValue, T neutralValue, T friendValue);

		/// <summary>
		/// Evaluates this <see cref="IMonster">Monster</see>'s <see cref="Gender">Gender</see>, returning a value of type T.
		/// </summary>
		/// <param name="maleValue"></param>
		/// <param name="femaleValue"></param>
		/// <param name="neutralValue"></param>
		/// <returns></returns>
		T EvalGender<T>(T maleValue, T femaleValue, T neutralValue);

		/// <summary>
		/// Evaluates this <see cref="IMonster">Monster</see>'s <see cref="CurrGroupCount">CurrGroupCount</see>, returning a value of type T.
		/// </summary>
		/// <param name="singularValue"></param>
		/// <param name="pluralValue"></param>
		/// <returns></returns>
		T EvalPlural<T>(T singularValue, T pluralValue);

		/// <summary>
		/// Evaluates the <see cref="IRoom.LightLvl">LightLvl</see> of this <see cref="IMonster">Monster</see>'s <see cref="IRoom">Room</see> (as
		/// determined by its <see cref="Location">Location</see>), returning a value of type T.
		/// </summary>
		/// <param name="darkValue"></param>
		/// <param name="lightValue"></param>
		/// <returns></returns>
		T EvalInRoomLightLevel<T>(T darkValue, T lightValue);

		/// <summary></summary>
		/// <param name="charisma"></param>
		void ResolveReaction(long charisma);

		/// <summary></summary>
		/// <param name="character"></param>
		void ResolveReaction(ICharacter character);

		/// <summary></summary>
		/// <param name="value"></param>
		/// <param name="isArtifactValue"></param>
		void CalculateGiftFriendliness(long value, bool isArtifactValue);

		/// <summary>
		/// Indicates whether this <see cref="IMonster">Monster</see> represents the player character.
		/// </summary>
		/// <returns>If so then <c>true</c>, otherwise <c>false</c>.</returns>
		bool IsCharacterMonster();

		/// <summary></summary>
		/// <returns></returns>
		bool IsStateDescSideNotes();

		/// <summary></summary>
		/// <returns></returns>
		long GetWeightCarryableGronds();

		/// <summary></summary>
		/// <returns></returns>
		long GetFleeingMemberCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetMaxMemberActionCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetMaxMemberAttackCount();

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <returns></returns>
		IMonsterSpell GetMonsterSpell(Enums.Spell spell);

		/// <summary></summary>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetCarriedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetWornList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetContainedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		RetCode EnforceFullInventoryWeightLimits(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="weight"></param>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		RetCode GetFullInventoryWeight(ref long weight, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="appendNewLine"></param>
		void AddHealthStatus(StringBuilder buf, bool appendNewLine = true);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		string[] GetWeaponAttackDescs(IArtifact artifact);

		/// <summary></summary>
		/// <returns></returns>
		string[] GetHumanAttackDescs();

		/// <summary></summary>
		/// <returns></returns>
		string[] GetNaturalAttackDescs();

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		/// <returns></returns>
		string GetAttackDescString(IRoom room, IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		string[] GetWeaponMissDescs(IArtifact artifact);

		/// <summary></summary>
		/// <returns></returns>
		string[] GetNaturalMissDescs();

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		string GetMissDescString(IArtifact artifact);

		/// <summary></summary>
		/// <returns></returns>
		string GetArmorDescString();

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monsterName"></param>
		/// <param name="isPlural"></param>
		/// <param name="fleeing"></param>
		/// <returns></returns>
		string GetCantFindExitDescString(IRoom room, string monsterName, bool isPlural, bool fleeing);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monsterName"></param>
		/// <param name="isPlural"></param>
		/// <param name="fleeing"></param>
		/// <returns></returns>
		string GetMembersExitRoomDescString(IRoom room, string monsterName, bool isPlural, bool fleeing);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monsterName"></param>
		/// <param name="isPlural"></param>
		/// <param name="fleeing"></param>
		/// <param name="exitDirection"></param>
		/// <returns></returns>
		string GetExitRoomDescString(IRoom room, string monsterName, bool isPlural, bool fleeing, Direction exitDirection);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monsterName"></param>
		/// <param name="isPlural"></param>
		/// <param name="fleeing"></param>
		/// <param name="enterDirection"></param>
		/// <returns></returns>
		string GetEnterRoomDescString(IRoom room, string monsterName, bool isPlural, bool fleeing, Direction enterDirection);

		/// <summary></summary>
		/// <param name="youString"></param>
		/// <param name="maleString"></param>
		/// <param name="femaleString"></param>
		/// <param name="neutralString"></param>
		/// <param name="groupString"></param>
		/// <returns></returns>
		string GetPovString(string youString, string maleString, string femaleString, string neutralString, string groupString);

		#endregion
	}
}
