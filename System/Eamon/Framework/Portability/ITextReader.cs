
// ITextReader.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Text;

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public interface ITextReader
	{
		/// <summary></summary>
		bool EnableInput { get; set; }

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="bufSize"></param>
		/// <param name="boxChars"></param>
		/// <param name="fillChar"></param>
		/// <param name="maskChar"></param>
		/// <param name="emptyAllowed"></param>
		/// <param name="emptyVal"></param>
		/// <param name="modifyCharFunc"></param>
		/// <param name="validCharFunc"></param>
		/// <param name="termCharFunc"></param>
		/// <returns></returns>
		RetCode ReadField(StringBuilder buf, long bufSize, char[] boxChars, char fillChar, char maskChar, bool emptyAllowed, string emptyVal, Func<char, char> modifyCharFunc, Func<char, bool> validCharFunc, Func<char, bool> termCharFunc);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="bufSize"></param>
		/// <param name="boxChars"></param>
		/// <param name="emptyVal"></param>
		/// <param name="validCharFunc"></param>
		/// <returns></returns>
		RetCode ReadField(StringBuilder buf, long bufSize, char[] boxChars, string emptyVal, Func<char, bool> validCharFunc);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="bufSize"></param>
		/// <param name="boxChars"></param>
		/// <param name="emptyVal"></param>
		/// <returns></returns>
		RetCode ReadField(StringBuilder buf, long bufSize, char[] boxChars, string emptyVal);

		/// <summary></summary>
		/// <returns></returns>
		string ReadLine();

		/// <summary></summary>
		/// <param name="intercept"></param>
		/// <returns></returns>
		char ReadKey(bool intercept);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="initialNewLine"></param>
		void KeyPress(StringBuilder buf, bool initialNewLine = true);
	}
}
