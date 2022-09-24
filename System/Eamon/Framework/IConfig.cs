
// IConfig.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IConfig : IGameBase, IComparable<IConfig>
	{
		#region Properties

		/// <summary></summary>
		bool ShowDesc { get; set; }

		/// <summary></summary>
		bool ResolveEffects { get; set; }

		/// <summary></summary>
		bool GenerateUids { get; set; }

		/// <summary></summary>
		FieldDesc FieldDesc { get; set; }

		/// <summary></summary>
		long WordWrapMargin { get; set; }

		/// <summary></summary>
		string DdFilesetFileName { get; set; }

		/// <summary></summary>
		string DdCharacterFileName { get; set; }

		/// <summary></summary>
		string DdModuleFileName { get; set; }

		/// <summary></summary>
		string DdRoomFileName { get; set; }

		/// <summary></summary>
		string DdArtifactFileName { get; set; }

		/// <summary></summary>
		string DdEffectFileName { get; set; }

		/// <summary></summary>
		string DdMonsterFileName { get; set; }

		/// <summary></summary>
		string DdHintFileName { get; set; }

		/// <summary></summary>
		string MhWorkDir { get; set; }

		/// <summary></summary>
		string MhFilesetFileName { get; set; }

		/// <summary></summary>
		string MhCharacterFileName { get; set; }

		/// <summary></summary>
		string MhEffectFileName { get; set; }

		/// <summary></summary>
		string RtFilesetFileName { get; set; }

		/// <summary></summary>
		string RtCharacterFileName { get; set; }

		/// <summary></summary>
		string RtModuleFileName { get; set; }

		/// <summary></summary>
		string RtRoomFileName { get; set; }

		/// <summary></summary>
		string RtArtifactFileName { get; set; }

		/// <summary></summary>
		string RtEffectFileName { get; set; }

		/// <summary></summary>
		string RtMonsterFileName { get; set; }

		/// <summary></summary>
		string RtHintFileName { get; set; }

		/// <summary></summary>
		string RtGameStateFileName { get; set; }

		/// <summary></summary>
		bool DdEditingFilesets { get; set; }

		/// <summary></summary>
		bool DdEditingCharacters { get; set; }

		/// <summary></summary>
		bool DdEditingModules { get; set; }

		/// <summary></summary>
		bool DdEditingRooms { get; set; }

		/// <summary></summary>
		bool DdEditingArtifacts { get; set; }

		/// <summary></summary>
		bool DdEditingEffects { get; set; }

		/// <summary></summary>
		bool DdEditingMonsters { get; set; }

		/// <summary></summary>
		bool DdEditingHints { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadGameDatabase(bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveGameDatabase(bool printOutput = true);

		/// <summary></summary>
		/// <param name="configFileName"></param>
		/// <param name="startOver"></param>
		/// <returns></returns>
		RetCode DeleteGameState(string configFileName, bool startOver);

		/// <summary></summary>
		/// <param name="config"></param>
		/// <returns></returns>
		RetCode CopyProperties(IConfig config);

		#endregion
	}
}
