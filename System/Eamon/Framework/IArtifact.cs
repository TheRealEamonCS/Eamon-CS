
// IArtifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IArtifact : IGameBase, IComparable<IArtifact>
	{
		#region Properties

		/// <summary>
		/// Gets or sets a description shown after this <see cref="IArtifact">Artifact</see>'s <see cref="IGameBase.Name">Name</see> in
		/// various lists that indicates its state.
		/// </summary>
		string StateDesc { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IArtifact">Artifact</see> is owned by the player character.
		/// </summary>
		bool IsCharOwned { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IArtifact">Artifact</see> represents a group of objects.
		/// </summary>
		bool IsPlural { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IArtifact">Artifact</see> should be displayed in
		/// various lists.
		/// </summary>
		bool IsListed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating how this <see cref="IArtifact">Artifact</see>'s singular <see cref="IGameBase.Name">Name</see> is
		/// modified to produce its plural Name.
		/// </summary>
		PluralType PluralType { get; set; }

		/// <summary>
		/// Gets or sets the base value of this <see cref="IArtifact">Artifact</see> in gold pieces.
		/// </summary>
		long Value { get; set; }

		/// <summary>
		/// Gets or sets the weight of this <see cref="IArtifact">Artifact</see> in Gronds.
		/// </summary>
		long Weight { get; set; }

		/// <summary>
		/// Gets the recursive weight of this <see cref="IArtifact">Artifact</see> in Gronds.
		/// </summary>
		long RecursiveWeight { get; }

		/// <summary>
		/// Gets or sets the location of this <see cref="IArtifact">Artifact</see> in the game (typically a <see cref="IRoom">Room</see>
		/// <see cref="IGameBase.Uid">Uid</see> or a special code).
		/// </summary>
		long Location { get; set; }

		/// <summary>
		/// Gets or sets the primary <see cref="IArtifactCategory.Type">Type</see> of this <see cref="IArtifact">Artifact</see>, 
		/// a convenience to emulate Eamon Deluxe.
		/// </summary>
		ArtifactType Type { get; set; }

		/// <summary>
		/// Gets or sets the primary <see cref="IArtifactCategory.Field1">Field1</see> of this <see cref="IArtifact">Artifact</see>,
		/// a convenience to emulate Eamon Deluxe.
		/// </summary>
		long Field1 { get; set; }

		/// <summary>
		/// Gets or sets the primary <see cref="IArtifactCategory.Field2">Field2</see> of this <see cref="IArtifact">Artifact</see>,
		/// a convenience to emulate Eamon Deluxe.
		/// </summary>
		long Field2 { get; set; }

		/// <summary>
		/// Gets or sets the primary <see cref="IArtifactCategory.Field3">Field3</see> of this <see cref="IArtifact">Artifact</see>,
		/// a convenience to emulate Eamon Deluxe.
		/// </summary>
		long Field3 { get; set; }

		/// <summary>
		/// Gets or sets the primary <see cref="IArtifactCategory.Field4">Field4</see> of this <see cref="IArtifact">Artifact</see>,
		/// a convenience to emulate Eamon Deluxe.
		/// </summary>
		long Field4 { get; set; }

		/// <summary>
		/// Gets or sets the primary <see cref="IArtifactCategory.Field5">Field5</see> of this <see cref="IArtifact">Artifact</see>,
		/// a convenience to emulate Eamon Deluxe.
		/// </summary>
		long Field5 { get; set; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.Gold">Gold</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory Gold { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.Treasure">Treasure</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory Treasure { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.Weapon">Weapon</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory Weapon { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.MagicWeapon">MagicWeapon</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory MagicWeapon { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding to 
		/// either <see cref="ArtifactType.Weapon">Weapon</see> or <see cref="ArtifactType.MagicWeapon">MagicWeapon</see>; intended as a
		/// convenience.
		/// </summary>
		IArtifactCategory GeneralWeapon { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.InContainer">InContainer</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory InContainer { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.OnContainer">OnContainer</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory OnContainer { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.UnderContainer">UnderContainer</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory UnderContainer { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.BehindContainer">BehindContainer</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory BehindContainer { get; }

		/// <summary>
		/// Gets the first found <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see>
		/// corresponding to any of <see cref="ArtifactType.InContainer">InContainer</see>, <see cref="ArtifactType.OnContainer">OnContainer</see>, 
		/// <see cref="ArtifactType.UnderContainer">UnderContainer</see> or <see cref="ArtifactType.BehindContainer">BehindContainer</see>; 
		/// intended as a convenience.
		/// </summary>
		IArtifactCategory GeneralContainer { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.LightSource">LightSource</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory LightSource { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.Drinkable">Drinkable</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory Drinkable { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.Readable">Readable</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory Readable { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.DoorGate">DoorGate</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory DoorGate { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.Edible">Edible</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory Edible { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.BoundMonster">BoundMonster</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory BoundMonster { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.Wearable">Wearable</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory Wearable { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.DisguisedMonster">DisguisedMonster</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory DisguisedMonster { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.DeadBody">DeadBody</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory DeadBody { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.User1">User1</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory User1 { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.User2">User2</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory User2 { get; }

		/// <summary>
		/// Gets the <see cref="IArtifactCategory">ArtifactCategory</see> for this <see cref="IArtifact">Artifact</see> corresponding
		/// to <see cref="ArtifactType.User3">User3</see>; intended as a convenience.
		/// </summary>
		IArtifactCategory User3 { get; }

		/// <summary>
		/// Gets or sets an array of <see cref="IArtifactCategory">ArtifactCategory</see> objects that define
		/// this <see cref="IArtifact">Artifact</see>'s behavior in the game.
		/// </summary>
		IArtifactCategory[] Categories { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IArtifactCategory GetCategory(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetSynonym(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetCategory(long index, IArtifactCategory value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetSynonym(long index, string value);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByCharacter(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByMonster(bool recurse = false);

		/// <summary></summary>
		/// <returns></returns>
		bool IsCarriedByContainer();

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsWornByCharacter(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsWornByMonster(bool recurse = false);

		/// <summary></summary>
		/// <returns></returns>
		bool IsReadyableByCharacter();

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsInRoom(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsEmbeddedInRoom(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByContainerContainerTypeExposedToMonster(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByContainerContainerTypeExposedToRoom(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsInLimbo(bool recurse = false);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByMonsterUid(long monsterUid, bool recurse = false);

		/// <summary></summary>
		/// <param name="containerUid"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByContainerUid(long containerUid, bool recurse = false);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsWornByMonsterUid(long monsterUid, bool recurse = false);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <returns></returns>
		bool IsReadyableByMonsterUid(long monsterUid);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsInRoomUid(long roomUid, bool recurse = false);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsEmbeddedInRoomUid(long roomUid, bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByContainerContainerTypeExposedToCharacter(bool recurse = false);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByContainerContainerTypeExposedToMonsterUid(long monsterUid, bool recurse = false);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByContainerContainerTypeExposedToRoomUid(long roomUid, bool recurse = false);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByMonster(IMonster monster, bool recurse = false);

		/// <summary></summary>
		/// <param name="container"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByContainer(IArtifact container, bool recurse = false);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsWornByMonster(IMonster monster, bool recurse = false);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		bool IsReadyableByMonster(IMonster monster);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsInRoom(IRoom room, bool recurse = false);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsEmbeddedInRoom(IRoom room, bool recurse = false);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByContainerContainerTypeExposedToMonster(IMonster monster, bool recurse = false);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		bool IsCarriedByContainerContainerTypeExposedToRoom(IRoom room, bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		long GetCarriedByMonsterUid(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		long GetCarriedByContainerUid(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		long GetWornByMonsterUid(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		long GetInRoomUid(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		long GetEmbeddedInRoomUid(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IMonster GetCarriedByMonster(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IArtifact GetCarriedByContainer(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IMonster GetWornByMonster(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IRoom GetInRoom(bool recurse = false);

		/// <summary></summary>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IRoom GetEmbeddedInRoom(bool recurse = false);

		/// <summary></summary>
		/// <returns></returns>
		ContainerType GetCarriedByContainerContainerType();

		/// <summary></summary>
		void SetCarriedByCharacter();

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		void SetCarriedByMonsterUid(long monsterUid);

		/// <summary></summary>
		/// <param name="containerUid"></param>
		/// <param name="containerType"></param>
		void SetCarriedByContainerUid(long containerUid, ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		void SetWornByCharacter();

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		void SetWornByMonsterUid(long monsterUid);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		void SetInRoomUid(long roomUid);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		void SetEmbeddedInRoomUid(long roomUid);

		/// <summary></summary>
		void SetInLimbo();

		/// <summary></summary>
		/// <param name="monster"></param>
		void SetCarriedByMonster(IMonster monster);

		/// <summary></summary>
		/// <param name="container"></param>
		/// <param name="containerType"></param>
		void SetCarriedByContainer(IArtifact container, ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <param name="monster"></param>
		void SetWornByMonster(IMonster monster);

		/// <summary></summary>
		/// <param name="room"></param>
		void SetInRoom(IRoom room);

		/// <summary></summary>
		/// <param name="room"></param>
		void SetEmbeddedInRoom(IRoom room);

		/// <summary></summary>
		/// <returns></returns>
		bool IsInRoomLit();

		/// <summary></summary>
		/// <returns></returns>
		bool IsEmbeddedInRoomLit();

		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool IsFieldStrength(long value);

		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		long GetFieldStrength(long value);

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <returns></returns>
		bool IsWeapon(Weapon weapon);

		/// <summary></summary>
		/// <param name="ac"></param>
		/// <returns></returns>
		bool IsAttackable(ref IArtifactCategory ac);

		/// <summary></summary>
		/// <returns></returns>
		bool IsRequestable();

		/// <summary></summary>
		/// <returns></returns>
		bool IsUnmovable();

		/// <summary></summary>
		/// <returns></returns>
		bool IsUnmovable01();

		/// <summary></summary>
		/// <returns></returns>
		bool IsArmor();

		/// <summary></summary>
		/// <returns></returns>
		bool IsShield();

		/// <summary></summary>
		/// <returns></returns>
		bool IsDisguisedMonster();

		/// <summary></summary>
		/// <returns></returns>
		bool IsStateDescSideNotes();

		/// <summary></summary>
		/// <returns></returns>
		bool IsInContainerOpenedFromTop();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldAllowBlastSkillGains();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldExposeInContentsWhenClosed();

		/// <summary></summary>
		/// <param name="containerType"></param>
		/// <returns></returns>
		bool ShouldExposeContentsToCharacter(ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <param name="containerType"></param>
		/// <returns></returns>
		bool ShouldExposeContentsToMonster(ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <param name="containerType"></param>
		/// <returns></returns>
		bool ShouldExposeContentsToRoom(ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <param name="containerType"></param>
		/// <returns></returns>
		bool ShouldAddContentsWhenCarried(ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <param name="containerType"></param>
		/// <returns></returns>
		bool ShouldAddContentsWhenWorn(ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="containerType"></param>
		/// <returns></returns>
		bool ShouldAddContents(IArtifact artifact, ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldRevealContentsWhenMoved(ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldRevealContentsWhenMovedIntoLimbo(ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldShowContentsWhenExamined();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldShowContentsWhenOpened();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldShowVerboseNameContentsNameList();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldShowVerboseNameStateDesc();

		/// <summary></summary>
		/// <param name="containerType"></param>
		/// <returns></returns>
		long GetMaxContentsNameListCount(ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <returns></returns>
		string GetDoorGateFleeDesc();

		/// <summary></summary>
		/// <returns></returns>
		string GetProvidingLightDesc();

		/// <summary></summary>
		/// <returns></returns>
		string GetReadyWeaponDesc();

		/// <summary></summary>
		/// <returns></returns>
		string GetBrokenDesc();

		/// <summary></summary>
		/// <returns></returns>
		string GetEmptyDesc();

		/// <summary></summary>
		/// <param name="singularValue"></param>
		/// <param name="pluralValue"></param>
		/// <returns></returns>
		T EvalPlural<T>(T singularValue, T pluralValue);

		/// <summary></summary>
		/// <param name="darkValue"></param>
		/// <param name="lightValue"></param>
		/// <returns></returns>
		T EvalInRoomLightLevel<T>(T darkValue, T lightValue);

		/// <summary></summary>
		/// <param name="darkValue"></param>
		/// <param name="lightValue"></param>
		/// <returns></returns>
		T EvalEmbeddedInRoomLightLevel<T>(T darkValue, T lightValue);

		/// <summary></summary>
		/// <param name="artifactType"></param>
		/// <returns></returns>
		IArtifactCategory GetArtifactCategory(ArtifactType artifactType);

		/// <summary></summary>
		/// <param name="artifactTypes"></param>
		/// <param name="categoryArrayPrecedence"></param>
		/// <returns></returns>
		IArtifactCategory GetArtifactCategory(ArtifactType[] artifactTypes, bool categoryArrayPrecedence = true);

		/// <summary></summary>
		/// <param name="artifactTypes"></param>
		/// <returns></returns>
		IList<IArtifactCategory> GetArtifactCategoryList(ArtifactType[] artifactTypes);

		/// <summary></summary>
		/// <param name="count"></param>
		/// <returns></returns>
		RetCode SetArtifactCategoryCount(long count);

		/// <summary></summary>
		/// <param name="stateDesc"></param>
		/// <param name="dupAllowed"></param>
		/// <returns></returns>
		RetCode AddStateDesc(string stateDesc, bool dupAllowed = false);

		/// <summary></summary>
		/// <param name="stateDesc"></param>
		/// <returns></returns>
		RetCode RemoveStateDesc(string stateDesc);

		/// <summary></summary>
		/// <param name="artifactFindFunc"></param>
		/// <param name="containerType"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetContainedList(Func<IArtifact, bool> artifactFindFunc = null, ContainerType containerType = ContainerType.In, bool recurse = false);

		/// <summary></summary>
		/// <param name="count"></param>
		/// <param name="weight"></param>
		/// <param name="containerType"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		RetCode GetContainerInfo(ref long count, ref long weight, ContainerType containerType = ContainerType.In, bool recurse = false);

		#endregion
	}
}
