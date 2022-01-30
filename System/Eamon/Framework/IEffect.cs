
// IEffect.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Text;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IEffect : IGameBase, IComparable<IEffect>
	{
		#region Methods

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <returns></returns>
		RetCode BuildPrintedFullDesc(StringBuilder buf);

		#endregion
	}
}
