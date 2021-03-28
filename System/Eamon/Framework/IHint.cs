
// IHint.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IHint : IGameBase, IComparable<IHint>
	{
		#region Properties

		/// <summary></summary>
		bool Active { get; set; }

		/// <summary></summary>
		string Question { get; set; }

		/// <summary></summary>
		long NumAnswers { get; set; }

		/// <summary></summary>
		string[] Answers { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetAnswers(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetAnswers(long index, string value);

		#endregion
	}
}
