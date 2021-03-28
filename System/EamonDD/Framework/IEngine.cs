
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonDD.Framework
{
	/// <summary></summary>
	public interface IEngine : Eamon.Framework.IEngine
	{
		/// <summary></summary>
		/// <returns></returns>
		bool IsAdventureFilesetLoaded();

		/// <summary></summary>
		/// <param name="secondPass"></param>
		/// <param name="ddfnFlag"></param>
		/// <param name="nlFlag"></param>
		void DdProcessArgv(bool secondPass, ref bool ddfnFlag, ref bool nlFlag);
	};
}
