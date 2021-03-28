
// IAnalyseRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;

namespace EamonDD.Framework.Menus.ActionMenus
{
	/// <summary></summary>
	public interface IAnalyseRecordInterdependenciesMenu<T> : IRecordMenu<T> where T : class, IGameBase
	{
		/// <summary></summary>
		IList<string> SkipNameList { get; set; }

		/// <summary></summary>
		T ErrorRecord { get; set; }

		/// <summary></summary>
		bool ClearSkipNameList { get; set; }

		/// <summary></summary>
		bool ModifyFlag { get; set; }

		/// <summary></summary>
		bool ExitFlag { get; set; }

		/// <summary></summary>
		void ProcessInterdependency();
	}
}
