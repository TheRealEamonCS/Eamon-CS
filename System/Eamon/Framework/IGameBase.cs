
// IGameBase.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Text;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IGameBase : IDisposable
	{
		#region Properties

		/// <summary>
		/// Gets or sets a unique Id that distinguishes this <see cref="IGameBase">Record</see> from others of its type.
		/// </summary>
		/// <remarks>
		/// Every foundational Record Type in Eamon CS (e.g., <see cref="IRoom">Room</see>, <see cref="IArtifact">Artifact</see>, etc.)
		/// inherits this unique Id.  This allows the game designer to find and manipulate specific <see cref="IGameBase">Record</see>s,
		/// either using tools like EamonDD or during gameplay.  The unique Id is a discrete sequence number starting at 1 for each
		/// inheriting Record Type, which means you can get (for example) both a Room and Artifact with the same unique Id.
		/// </remarks>
		long Uid { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IGameBase">Record</see>'s <see cref="Uid">Uid</see> should be
		/// recycled (cached for reuse) when the Dispose method is called.
		/// </summary>
		bool IsUidRecycled { get; set; }

		/// <summary>
		/// Gets or sets the name of this <see cref="IGameBase">Record</see> as shown in various lists or the Record's full description.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the detailed description of this <see cref="IGameBase">Record</see> shown when the Record is 
		/// first <see cref="Seen">Seen</see> or later examined.
		/// </summary>
		string Desc { get; set; }

		/// <summary>
		/// Gets or sets a string array containing <see cref="Name">Name</see> synonyms for this <see cref="IGameBase">Record</see>
		/// (may be <c>null</c>).
		/// </summary>
		/// <remarks>
		/// In the case of <see cref="IArtifact">Artifact</see>s and <see cref="IMonster">Monster</see>s, the game uses synonyms
		/// during parsing.
		/// </remarks>
		string[] Synonyms { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IGameBase">Record</see> has been seen by the player character.
		/// </summary>
		bool Seen { get; set; }

		/// <summary>
		/// Gets or sets a value indicating how this <see cref="IGameBase">Record</see>'s <see cref="IGameBase.Name">Name</see> is
		/// prefixed to produce its listed Name.
		/// </summary>
		ArticleType ArticleType { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		void SetParentReferences();

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetPluralName(string fieldName, StringBuilder buf = null);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetPluralName01(StringBuilder buf = null);

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <param name="articleType"></param>
		/// <param name="upshift"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="showStateDesc"></param>
		/// <param name="groupCountOne"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetDecoratedName(string fieldName, ArticleType articleType, bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool groupCountOne = false, StringBuilder buf = null);

		/// <summary></summary>
		/// <param name="upshift"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="showStateDesc"></param>
		/// <param name="groupCountOne"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetNoneName(bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool groupCountOne = false, StringBuilder buf = null);

		/// <summary></summary>
		/// <param name="upshift"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="showStateDesc"></param>
		/// <param name="groupCountOne"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetArticleName(bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool groupCountOne = false, StringBuilder buf = null);

		/// <summary></summary>
		/// <param name="upshift"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="showStateDesc"></param>
		/// <param name="groupCountOne"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetTheName(bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool groupCountOne = false, StringBuilder buf = null);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="showName"></param>
		/// <returns></returns>
		RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName);

		#endregion
	}
}
