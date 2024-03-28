
// IHint.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace Eamon.Framework
{
	/// <summary></summary>
	/// <remarks></remarks>
	public interface IHint : IGameBase, IComparable<IHint>
	{
		#region Properties

		/// <summary>
		/// Gets or sets whether this <see cref="IHint">Hint</see> is available to the player.
		/// </summary>
		/// <remarks>
		/// <see cref="IHint">Hint</see>s can be active at all times or, if necessary, deactivated initially and later activated (or vice versa) when
		/// certain arbitrary conditions are met.
		/// </remarks>
		bool Active { get; set; }

		/// <summary>
		/// Gets or sets the Question answered or topic addressed by this <see cref="IHint">Hint</see>.
		/// </summary>
		string Question { get; set; }

		/// <summary>
		/// Gets or sets the number of Answers for the <see cref="IHint">Hint</see>'s Question.
		/// </summary>
		long NumAnswers { get; set; }

		/// <summary>
		/// Gets or sets an array of <see cref="IHint">Hint</see> Answers.
		/// </summary>
		/// <remarks>
		/// The array always contains the same number of elements. The first <see cref="NumAnswers">NumAnswers</see> elements
		/// contain increasingly explicit Answers to the <see cref="Question">Question</see>, while the remaining unused
		/// elements are set to an empty string. Avoid accessing array elements directly in favor of using Getter/Setter
		/// methods to ensure games can override when necessary.
		/// </remarks>
		/// <seealso cref="GetAnswer(long)"/>
		/// <seealso cref="SetAnswer(long, string)"/>
		string[] Answers { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetAnswer(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetAnswer(long index, string value);

		#endregion
	}
}
