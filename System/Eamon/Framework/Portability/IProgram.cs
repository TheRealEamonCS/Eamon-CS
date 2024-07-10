
// IProgram.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public interface IProgram
	{
		/// <summary></summary>
		bool EnableStdio { get; set; }

		/// <summary></summary>
		bool LineWrapUserInput { get; set; }

		/// <summary></summary>
		Action<IDictionary<Type, Type>> LoadPortabilityClassMappings { get; set; }

		/// <summary></summary>
		/// <param name="args"></param>
		void Main(string[] args);
	}
}
