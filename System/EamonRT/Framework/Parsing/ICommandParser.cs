
// ICommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;

namespace EamonRT.Framework.Parsing
{
	/// <summary></summary>
	public interface ICommandParser
	{
		/// <summary></summary>
		StringBuilder InputBuf { get; set; }

		/// <summary></summary>
		string LastInputStr { get; set; }

		/// <summary></summary>
		string LastHimNameStr { get; set; }

		/// <summary></summary>
		string LastHerNameStr { get; set; }

		/// <summary></summary>
		string LastItNameStr { get; set; }

		/// <summary></summary>
		string LastThemNameStr { get; set; }

		/// <summary></summary>
		string[] Tokens { get; set; }

		/// <summary></summary>
		string[] BortTokens { get; set; }

		/// <summary></summary>
		long CurrToken { get; set; }

		/// <summary></summary>
		long BortCurrToken { get; set; }

		/// <summary></summary>
		long PrepTokenIndex { get; set; }

		/// <summary></summary>
		IPrep Prep { get; set; }

		/// <summary></summary>
		IMonster ActorMonster { get; set; }

		/// <summary></summary>
		IRoom ActorRoom { get; set; }

		/// <summary></summary>
		IGameBase Dobj { get; set; }

		/// <summary></summary>
		IArtifact DobjArtifact { get; }

		/// <summary></summary>
		IMonster DobjMonster { get; }

		/// <summary></summary>
		IGameBase Iobj { get; set; }

		/// <summary></summary>
		IArtifact IobjArtifact { get; }

		/// <summary></summary>
		IMonster IobjMonster { get; }

		/// <summary></summary>
		IParserData DobjData { get; set; }

		/// <summary></summary>
		IParserData IobjData { get; set; }

		/// <summary></summary>
		IParserData ObjData { get; set; }

		/// <summary></summary>
		IState NextState { get; set; }

		/// <summary></summary>
		ICommand NextCommand { get; }

		/// <summary></summary>
		void RecordMatch();

		/// <summary></summary>
		void RecordMatch01();

		/// <summary></summary>
		void ResolveRecord(bool includeMonsters = true, bool includeArtifacts = true);

		/// <summary></summary>
		void ResolveRecordProcessWhereClauseList();

		/// <summary></summary>
		void FinishParsing();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldStripTrailingPunctuation();

		/// <summary></summary>
		/// <returns></returns>
		string GetActiveObjData();

		/// <summary></summary>
		/// <param name="record"></param>
		void SetRecord(IGameBase record);

		/// <summary></summary>
		/// <returns></returns>
		IGameBase GetRecord();

		/// <summary></summary>
		void Clear();

		/// <summary></summary>
		void ParseName();

		/// <summary></summary>
		/// <param name="obj"></param>
		/// <param name="objDataName"></param>
		/// <param name="artifact"></param>
		/// <param name="monster"></param>
		void SetLastNameStrings(IGameBase obj, string objDataName, IArtifact artifact, IMonster monster);

		/// <summary></summary>
		/// <param name="afterFinishParsing"></param>
		void CheckPlayerCommand(bool afterFinishParsing);

		/// <summary></summary>
		void Execute();
	}
}
