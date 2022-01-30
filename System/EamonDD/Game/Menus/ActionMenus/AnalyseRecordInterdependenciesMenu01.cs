
// AnalyseRecordInterdependenciesMenu01.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using EamonDD.Framework.Menus.ActionMenus;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AnalyseRecordInterdependenciesMenu01<T> : IAnalyseRecordInterdependenciesMenu01<T> where T : class, IGameBase
	{
		public virtual IList<string> SkipFieldNameList
		{
			get
			{
				return AnalyseMenu.SkipNameList;
			}

			set
			{
				AnalyseMenu.SkipNameList = value;
			}
		}

		public virtual bool ClearSkipFieldNameList
		{
			get
			{
				return AnalyseMenu.ClearSkipNameList;
			}

			set
			{
				AnalyseMenu.ClearSkipNameList = value;
			}
		}

		public virtual bool ModifyFlag
		{
			get
			{
				return AnalyseMenu.ModifyFlag;
			}

			set
			{
				AnalyseMenu.ModifyFlag = value;
			}
		}

		public virtual bool ExitFlag
		{
			get
			{
				return AnalyseMenu.ExitFlag;
			}

			set
			{
				AnalyseMenu.ExitFlag = value;
			}
		}

		/// <summary></summary>
		public virtual IAnalyseRecordInterdependenciesMenu<T> AnalyseMenu { get; set; }

		public virtual void Execute()
		{
			AnalyseMenu.Execute();
		}

		public AnalyseRecordInterdependenciesMenu01()
		{

		}
	}
}
