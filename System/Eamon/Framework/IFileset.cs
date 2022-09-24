
// IFileset.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IFileset : IGameBase, IComparable<IFileset>
	{
		#region Properties

		/// <summary></summary>
		string WorkDir { get; set; }

		/// <summary></summary>
		string PluginFileName { get; set; }

		/// <summary></summary>
		string ConfigFileName { get; set; }

		/// <summary></summary>
		string FilesetFileName { get; set; }

		/// <summary></summary>
		string CharacterFileName { get; set; }

		/// <summary></summary>
		string ModuleFileName { get; set; }

		/// <summary></summary>
		string RoomFileName { get; set; }

		/// <summary></summary>
		string ArtifactFileName { get; set; }

		/// <summary></summary>
		string EffectFileName { get; set; }

		/// <summary></summary>
		string MonsterFileName { get; set; }

		/// <summary></summary>
		string HintFileName { get; set; }

		/// <summary></summary>
		string GameStateFileName { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		RetCode DeleteFiles(string fieldName);

		#endregion
	}
}
