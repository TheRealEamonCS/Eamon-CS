
// PluralType.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of Plural Types.
	/// </summary>
	/// <remarks>
	/// These plural types are applied to <see cref="IArtifact">Artifact</see>s and <see cref="IMonster">Monster</see>s and
	/// improve the aesthetics of the output, making it more natural to read.  You can modify any record at runtime to change its
	/// plural type, if desired.  The plural type is only applicable to plural Artifacts and group Monsters, although if possible
	/// you should still assign the correct one even when it won't be used.  If you have a special situation where none of the
	/// following values applies (eg, "Rings of Xylo"), customized plural names can be created using <see cref="IEffect">Effect</see>s.
	/// </remarks>
	public enum PluralType : long
	{
		/// <summary>
		/// No plural type is ever applied.  For example, "aircraft" or "moose".
		/// </summary>
		None = 0,

		/// <summary>
		/// The name is followed by "s".  For example, "flashlights" or "wolverines".
		/// </summary>
		S,
		
		/// <summary>
		/// The name is followed by "es".  For example, "crucifixes" or "finches".
		/// </summary>
		Es,
		
		/// <summary>
		/// The name ends with "y" - which is dropped - and is followed by "ies".  For example, "factories" or "canaries".
		/// </summary>
		YIes
	}
}
