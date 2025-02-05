
// IRoom.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	/// <remarks></remarks>
	public interface IRoom : IGameBase, IComparable<IRoom>
	{
		#region Properties

		/// <summary>
		/// Gets or sets a value indicating the amount of light in this <see cref="IRoom">Room</see>.
		/// </summary>
		LightLevel LightLvl { get; set; }

		/// <summary>
		/// Gets or sets a value indicating this <see cref="IRoom">Room</see>'s general type.
		/// </summary>
		RoomType Type { get; set; }

		/// <summary>
		/// Gets or sets a value that is unused by Eamon CS, but indicates the zone of this <see cref="IRoom">Room</see>.
		/// </summary>
		/// <remarks>
		/// This value is provided to the game developer to allow sets of <see cref="IRoom">Room</see>s to be arbitrarily grouped.
		/// Then the groups of Rooms can be processed further by the game code if necessary.
		/// </remarks>
		long Zone { get; set; }

		/// <summary>
		/// Gets or sets an array of <see cref="IRoom">Room</see> exit links, each element corresponding to one <see cref="Direction">Direction</see>.
		/// </summary>
		/// <remarks>
		/// The array always contains the same number of elements, and the 0'th element is unused. Exit links can be <see cref="IRoom">Room</see>
		/// <see cref="IGameBase.Uid"> Uid</see>s or special codes representing things like game exits, doors, blocked pathways or other more exotic
		/// game-specific portals. Avoid accessing array elements directly in favor of using Getter/Setter methods to ensure games can override when
		/// necessary.
		/// </remarks>
		/// <seealso cref="GetDir(long)"/>
		/// <seealso cref="GetDir(Direction)"/>
		/// <seealso cref="SetDir(long,long)"/>
		/// <seealso cref="SetDir(Direction,long)"/>
		long[] Dirs { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetDir(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		long GetDir(Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetDir(long index, long value);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <param name="value"></param>
		void SetDir(Direction dir, long value);

		/// <summary>
		/// Indicates whether this <see cref="IRoom">Room</see> is lit.
		/// </summary>
		/// <returns>If <see cref="LightLvl"/> == <see cref="LightLevel.Light"/> then <c>true</c>, otherwise <c>false</c>.</returns>
		bool IsLit();

		/// <summary></summary>
		/// <returns></returns>
		bool IsViewable();

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionInvalid(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionInvalid(Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionRoom(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionRoom(Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionRoom01(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionRoom01(Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionExit(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionExit(Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionDoor(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionDoor(Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="includeExit"></param>
		/// <returns></returns>
		bool IsDirectionSpecial(long index, bool includeExit = true);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <param name="includeExit"></param>
		/// <returns></returns>
		bool IsDirectionSpecial(Direction dir, bool includeExit = true);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionInObviousExitsList(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionInObviousExitsList(Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetDirectionDoorUid(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		long GetDirectionDoorUid(Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IArtifact GetDirectionDoor(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		IArtifact GetDirectionDoor(Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		void SetDirectionExit(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		void SetDirectionExit(Direction dir);
	
		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="artifactUid"></param>
		void SetDirectionDoorUid(long index, long artifactUid);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <param name="artifactUid"></param>
		void SetDirectionDoorUid(Direction dir, long artifactUid);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="artifact"></param>
		void SetDirectionDoor(long index, IArtifact artifact);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <param name="artifact"></param>
		void SetDirectionDoor(Direction dir, IArtifact artifact);

		/// <summary></summary>
		/// <param name="showDesc"></param>
		/// <param name="monsterList"></param>
		/// <param name="artifactList"></param>
		/// <param name="recordList"></param>
		/// <returns></returns>
		string GetYouAlsoSee(bool showDesc, IList<IMonster> monsterList, IList<IArtifact> artifactList, IList<IGameBase> recordList);

		/// <summary>
		/// Gets this <see cref="IRoom">Room</see>'s obvious exits prefix string, based on its <see cref="Type">Type</see>.
		/// </summary>
		/// <returns></returns>
		string GetObviousExits();

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		bool IsMonsterListedInRoom(IMonster monster);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		bool IsArtifactListedInRoom(IArtifact artifact);

		/// <summary>
		/// Evaluates this <see cref="IRoom">Room</see>'s <see cref="LightLvl">LightLvl</see>, returning a value of type T.
		/// </summary>
		/// <param name="darkValue"></param>
		/// <param name="lightValue"></param>
		/// <returns></returns>
		T EvalLightLevel<T>(T darkValue, T lightValue);

		/// <summary></summary>
		/// <param name="nonviewableValue"></param>
		/// <param name="viewableValue"></param>
		/// <returns></returns>
		T EvalViewability<T>(T nonviewableValue, T viewableValue);

		/// <summary>
		/// Evaluates this <see cref="IRoom">Room</see>'s <see cref="Type">Type</see>, returning a value of type T.
		/// </summary>
		/// <param name="indoorsValue"></param>
		/// <param name="outdoorsValue"></param>
		/// <returns></returns>
		T EvalRoomType<T>(T indoorsValue, T outdoorsValue);

		/// <summary></summary>
		/// <param name="roomFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetTakeableList(Func<IArtifact, bool> roomFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="roomFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetEmbeddedList(Func<IArtifact, bool> roomFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="roomFindFunc"></param>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IGameBase> GetContainedList(Func<IGameBase, bool> roomFindFunc = null, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="modFunc"></param>
		/// <param name="useNames"></param>
		/// <returns></returns>
		RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="verboseRoomDesc"></param>
		/// <param name="verboseMonsterDesc"></param>
		/// <param name="verboseArtifactDesc"></param>
		/// <param name="verboseNames"></param>
		/// <param name="recordNameListArgs"></param>
		/// <returns></returns>
		RetCode BuildPrintedFullDesc(StringBuilder buf, Func<IMonster, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool verboseRoomDesc = false, bool verboseMonsterDesc = false, bool verboseArtifactDesc = false, bool verboseNames = false, IRecordNameListArgs recordNameListArgs = null);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <returns></returns>
		RetCode BuildPrintedTooDarkToSeeDesc(StringBuilder buf);

		#endregion
	}
}
