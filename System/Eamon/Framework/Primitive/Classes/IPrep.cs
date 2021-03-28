
// IPrep.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Primitive.Classes
{
	/// <summary></summary>
	public interface IPrep
	{
		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		ContainerType ContainerType { get; set; }
	}
}
