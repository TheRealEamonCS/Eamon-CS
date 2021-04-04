
// EDXHint.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonDD.Game.Converters.EamonDeluxe
{
	/// <summary>
	/// </summary>
	public class EDXHint
	{
		public long _nh;

		public long _hptr;

		public virtual IList<EDXDesc> AnswerList { get; set; }

		public virtual string Question { get; set; }

		public EDXHint()
		{
			AnswerList = new List<EDXDesc>();
		}
	}
}
