
// IIntroStory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using EamonRT.Framework.Primitive.Enums;

namespace EamonRT.Framework
{
	/// <summary></summary>
	public interface IIntroStory
	{
		/// <summary></summary>
		StringBuilder Buf { get; set; }

		/// <summary></summary>
		IntroStoryType StoryType { get; set; }

		/// <summary></summary>
		bool ShouldPrintOutput { get; set; }

		/// <summary></summary>
		void PrintOutput();
	}
}
