
// ArticleType.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of Article Types.
	/// </summary>
	/// <remarks>
	/// These article types are applied to the <see cref="IGameBase.Name">Name</see>s of <see cref="IArtifact">Artifact</see>s and
	/// <see cref="IMonster">Monster</see>s to improve the aesthetics of the output, making it more natural to read.  You can modify
	/// any record at runtime to change its article type if desired.
	/// </remarks>
	public enum ArticleType : long
	{
		/// <summary>
		/// No article is ever applied.  For example, "Trollsfire" or "Heinrich".
		/// </summary>
		None = 0,

		/// <summary>
		/// The name is preceded by "a".  For example, "a flashlight" or "a wolverine".
		/// </summary>
		A,

		/// <summary>
		/// The name is preceded by "an".  For example, "an axe" or "an orc".
		/// </summary>
		An,

		/// <summary>
		/// The name is preceded by "some".  For example, "some leather armor", "some silver cups" or "some green slime".
		/// </summary>
		/// <remarks>
		/// For <see cref="IArtifact">Artifact</see>s, it is important to distinguish between singular Artifacts (like the leather armor) and
		/// plural Artifacts (the silver cups).  Plural Artifacts should be named as singular; this article type combined with the right plural
		/// type will produce the correct name.  The situation for <see cref="IMonster">Monster</see>s is similar; it is important to distinguish
		/// between singular Monsters (like the green slime) and group Monsters.  Group Monsters have their own plural syntax (eg, "seven kobolds").
		/// Group Monsters should be named as singular, but with a singular article type (eg, "a kobold"), and the right plural type.
		/// </remarks>
		Some,

		/// <summary>
		/// The name is preceded by "the".  For example, "the Rings of Xylo" or "the Emerald Warrior".
		/// </summary>
		/// <remarks>
		/// Typically only applied to unique <see cref="IArtifact">Artifact</see>s or <see cref="IMonster">Monster</see>s, or those with special
		/// importance in the game.
		/// </remarks>
		The
	}
}
