
// ISentenceParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Text;

namespace EamonRT.Framework.Parsing
{
	/// <summary></summary>
	public interface ISentenceParser
	{
		/// <summary></summary>
		StringBuilder InputBuf { get; set; }

		/// <summary></summary>
		string LastInputStr { get; set; }

		/// <summary></summary>
		IList<string> ParserInputStrList { get; set; }

		/// <summary></summary>
		string ParserInputStr { get; }

		/// <summary></summary>
		void PrintDiscardingCommands();

		/// <summary></summary>
		void Clear();

		/// <summary></summary>
		void ReplacePronounsAndProcessDobjNameList();

		/// <summary></summary>
		void Execute();
	}
}
