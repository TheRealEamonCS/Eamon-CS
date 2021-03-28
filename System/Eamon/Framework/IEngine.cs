
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Eamon.Framework.Args;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IEngine
	{
		#region Properties

		/// <summary>
		/// Gets or sets a collection of functions used to resolve macros embedded in <see cref="IGameBase.Desc">Desc</see>, 
		/// <see cref="IArtifact">Artifact</see><see cref="IArtifact.StateDesc"> StateDesc</see> and <see cref="IMonster">Monster</see>
		/// <see cref="IMonster.StateDesc"> StateDesc</see> properties.
		/// </summary>
		IDictionary<long, Func<string>> MacroFuncs { get; set; }

		/// <summary></summary>
		IList<IArtifact> ArtifactContainedList { get; set; }

		/// <summary>
		/// Gets or sets an array of sentence prepositions (eg, "to", "from", "inside", etc).
		/// </summary>
		IPrep[] Preps { get; set; }

		/// <summary>
		/// Gets or sets an array of sentence articles (eg, "a", "some", "the", etc).
		/// </summary>
		string[] Articles { get; set; }

		/// <summary>
		/// Gets or sets the number of records stored in each quick-lookup <see cref="IDbTable{T}.Cache">Cache</see> in the database.
		/// </summary>
		long NumCacheItems { get; set; }

		/// <summary>
		/// Gets or sets a generic string representing an unknown name (eg, "???").
		/// </summary>
		string UnknownName { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Gets the sentence preposition (eg, "to", "from", "inside", etc).
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IPrep GetPreps(long index);

		/// <summary>
		/// Gets the sentence article (eg, "a", "some", "the", etc).
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetArticles(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetNumberStrings(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetFieldDescNames(long index);

		/// <summary></summary>
		/// <param name="fieldDesc"></param>
		/// <returns></returns>
		string GetFieldDescNames(FieldDesc fieldDesc);

		/// <summary>
		/// Gets the name for a given <see cref="Status"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetStatusNames(long index);

		/// <summary>
		/// Gets the name for a given <see cref="Status"/>.
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		string GetStatusNames(Status status);

		/// <summary>
		/// Gets the name for a given <see cref="Clothing"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetClothingNames(long index);

		/// <summary>
		/// Gets the name for a given <see cref="Clothing"/>.
		/// </summary>
		/// <param name="clothing"></param>
		/// <returns></returns>
		string GetClothingNames(Clothing clothing);

		/// <summary>
		/// Gets the description for a given <see cref="CombatCode"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetCombatCodeDescs(long index);

		/// <summary>
		/// Gets the description for a given <see cref="CombatCode"/>.
		/// </summary>
		/// <param name="combatCode"></param>
		/// <returns></returns>
		string GetCombatCodeDescs(CombatCode combatCode);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetContainerDisplayCodeDescs(long index);

		/// <summary></summary>
		/// <param name="containerDisplayCode"></param>
		/// <returns></returns>
		string GetContainerDisplayCodeDescs(ContainerDisplayCode containerDisplayCode);

		/// <summary>
		/// Gets the name for a given <see cref="LightLevel"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetLightLevelNames(long index);

		/// <summary>
		/// Gets the name for a given <see cref="LightLevel"/>.
		/// </summary>
		/// <param name="lightLevel"></param>
		/// <returns></returns>
		string GetLightLevelNames(LightLevel lightLevel);

		/// <summary>
		/// Gets the data for a given <see cref="Stat"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IStat GetStats(long index);

		/// <summary>
		/// Gets the data for a given <see cref="Stat"/>.
		/// </summary>
		/// <param name="stat"></param>
		/// <returns></returns>
		IStat GetStats(Stat stat);

		/// <summary>
		/// Gets the data for a given <see cref="Spell"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		ISpell GetSpells(long index);

		/// <summary>
		/// Gets the data for a given <see cref="Spell"/>.
		/// </summary>
		/// <param name="spell"></param>
		/// <returns></returns>
		ISpell GetSpells(Spell spell);

		/// <summary>
		/// Gets the data for a given <see cref="Weapon"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IWeapon GetWeapons(long index);

		/// <summary>
		/// Gets the data for a given <see cref="Weapon"/>.
		/// </summary>
		/// <param name="weapon"></param>
		/// <returns></returns>
		IWeapon GetWeapons(Weapon weapon);

		/// <summary>
		/// Gets the data for a given <see cref="Armor"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IArmor GetArmors(long index);

		/// <summary>
		/// Gets the data for a given <see cref="Armor"/>.
		/// </summary>
		/// <param name="armor"></param>
		/// <returns></returns>
		IArmor GetArmors(Armor armor);

		/// <summary>
		/// Gets the data for a given <see cref="Direction"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IDirection GetDirections(long index);

		/// <summary>
		/// Gets the data for a given <see cref="Direction"/>.
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		IDirection GetDirections(Direction direction);

		/// <summary>
		/// Gets the data for a given <see cref="ArtifactType"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IArtifactType GetArtifactTypes(long index);

		/// <summary>
		/// Gets the data for a given <see cref="ArtifactType"/>.
		/// </summary>
		/// <param name="artifactType"></param>
		/// <returns></returns>
		IArtifactType GetArtifactTypes(ArtifactType artifactType);

		/// <summary>
		/// Gets the data for a given <see cref="TriggerType"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		ITriggerType GetTriggerTypes(long index);

		/// <summary>
		/// Gets the data for a given <see cref="TriggerType"/>.
		/// </summary>
		/// <param name="triggerType"></param>
		/// <returns></returns>
		ITriggerType GetTriggerTypes(TriggerType triggerType);

		/// <summary>
		/// Gets the data for a given <see cref="ScriptType"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IScriptType GetScriptTypes(long index);

		/// <summary>
		/// Gets the data for a given <see cref="ScriptType"/>.
		/// </summary>
		/// <param name="scriptType"></param>
		/// <returns></returns>
		IScriptType GetScriptTypes(ScriptType scriptType);

		/// <summary>
		/// Indicates whether an operation succeeded.
		/// </summary>
		/// <param name="rc"></param>
		/// <returns></returns>
		bool IsSuccess(RetCode rc);

		/// <summary>
		/// Indicates whether an operation failed.
		/// </summary>
		/// <param name="rc"></param>
		/// <returns></returns>
		bool IsFailure(RetCode rc);

		/// <summary>
		/// Indicates whether a plural type is valid.
		/// </summary>
		/// <param name="pluralType"></param>
		/// <returns></returns>
		bool IsValidPluralType(PluralType pluralType);

		/// <summary>
		/// Indicates whether an artifact type is valid.
		/// </summary>
		/// <param name="artifactType"></param>
		/// <returns></returns>
		bool IsValidArtifactType(ArtifactType artifactType);

		/// <summary>
		/// Indicates whether a trigger type is valid.
		/// </summary>
		/// <param name="triggerType"></param>
		/// <returns></returns>
		bool IsValidTriggerType(TriggerType triggerType);

		/// <summary>
		/// Indicates whether a script type is valid.
		/// </summary>
		/// <param name="scriptType"></param>
		/// <returns></returns>
		bool IsValidScriptType(ScriptType scriptType);

		/// <summary>
		/// Indicates whether an armor value is valid for an wearable <see cref="IArtifact">Artifact</see>.
		/// </summary>
		/// <param name="armor"></param>
		/// <returns></returns>
		bool IsValidArtifactArmor(long armor);

		/// <summary>
		/// Indicates whether an armor value is valid for a <see cref="IMonster">Monster</see>.
		/// </summary>
		/// <param name="armor"></param>
		/// <returns></returns>
		bool IsValidMonsterArmor(long armor);

		/// <summary>
		/// Indicates whether a courage value is valid for a <see cref="IMonster">Monster</see>.
		/// </summary>
		/// <param name="courage"></param>
		/// <returns></returns>
		bool IsValidMonsterCourage(long courage);

		/// <summary></summary>
		/// <param name="friendliness"></param>
		/// <returns></returns>
		bool IsValidMonsterFriendliness(Friendliness friendliness);

		/// <summary></summary>
		/// <param name="friendliness"></param>
		/// <returns></returns>
		bool IsValidMonsterFriendlinessPct(Friendliness friendliness);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsValidDirection(Direction dir);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <returns></returns>
		bool IsValidRoomUid01(long roomUid);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <returns></returns>
		bool IsValidRoomDirectionDoorUid01(long roomUid);

		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool IsArtifactFieldStrength(long value);

		/// <summary></summary>
		/// <param name="weight"></param>
		/// <returns></returns>
		bool IsUnmovable(long weight);

		/// <summary></summary>
		/// <param name="weight"></param>
		/// <returns></returns>
		bool IsUnmovable01(long weight);

		/// <summary></summary>
		/// <param name="hardiness"></param>
		/// <returns></returns>
		long GetWeightCarryableGronds(long hardiness);

		/// <summary></summary>
		/// <param name="hardiness"></param>
		/// <returns></returns>
		long GetWeightCarryableDos(long hardiness);

		/// <summary></summary>
		/// <param name="intellect"></param>
		/// <returns></returns>
		long GetIntellectBonusPct(long intellect);

		/// <summary></summary>
		/// <param name="charisma"></param>
		/// <returns></returns>
		long GetCharmMonsterPct(long charisma);

		/// <summary></summary>
		/// <param name="pluralType"></param>
		/// <returns></returns>
		long GetPluralTypeEffectUid(PluralType pluralType);

		/// <summary></summary>
		/// <param name="armorUid"></param>
		/// <param name="shieldUid"></param>
		/// <returns></returns>
		long GetArmorFactor(long armorUid, long shieldUid);

		/// <summary></summary>
		/// <param name="charisma"></param>
		/// <returns></returns>
		long GetCharismaFactor(long charisma);

		/// <summary></summary>
		/// <param name="friendliness"></param>
		/// <returns></returns>
		long GetMonsterFriendlinessPct(Friendliness friendliness);

		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		long GetArtifactFieldStrength(long value);

		/// <summary></summary>
		/// <param name="price"></param>
		/// <param name="rtio"></param>
		/// <returns></returns>
		long GetMerchantAskPrice(double price, double rtio);

		/// <summary></summary>
		/// <param name="price"></param>
		/// <param name="rtio"></param>
		/// <returns></returns>
		long GetMerchantBidPrice(double price, double rtio);

		/// <summary></summary>
		/// <param name="charisma"></param>
		/// <returns></returns>
		long GetMerchantAdjustedCharisma(long charisma);

		/// <summary></summary>
		/// <param name="charisma"></param>
		/// <returns></returns>
		double GetMerchantRtio(long charisma);

		/// <summary>
		/// Indicates whether a character is one of ['Y', 'N'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharYOrN(char ch);

		/// <summary>
		/// Indicates whether a character is one of ['S', 'T', 'R', 'X'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharSOrTOrROrX(char ch);

		/// <summary>
		/// Indicates whether a character is one of ['0', '1'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsChar0Or1(char ch);

		/// <summary>
		/// Indicates whether a character is one of ['0', '1', '2'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsChar0To2(char ch);

		/// <summary>
		/// Indicates whether a character is one of ['0', '1', '2', '3'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsChar0To3(char ch);

		/// <summary>
		/// Indicates whether a character is one of ['1', '2', '3'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsChar1To3(char ch);

		/// <summary>
		/// Indicates whether a character is a numeric digit.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharDigit(char ch);

		/// <summary>
		/// Indicates whether a character is a numeric digit or 'X'.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharDigitOrX(char ch);

		/// <summary>
		/// Indicates whether a character is a numeric digit or one of ['+', '-'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharPlusMinusDigit(char ch);

		/// <summary>
		/// Indicates whether a character is alphabetic.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAlpha(char ch);

		/// <summary>
		/// Indicates whether a character is alphabetic or space.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAlphaSpace(char ch);

		/// <summary>
		/// Indicates whether a character is alphabetic or numeric digit.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAlnum(char ch);

		/// <summary>
		/// Indicates whether a character is alphabetic, numeric digit or space.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAlnumSpace(char ch);

		/// <summary>
		/// Indicates whether a character is alphabetic, numeric digit, period or underscore.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAlnumPeriodUnderscore(char ch);

		/// <summary>
		/// Indicates whether a character is printable.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharPrint(char ch);

		/// <summary>
		/// Indicates whether a character is '#'.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharPound(char ch);

		/// <summary>
		/// Indicates whether a character is a quote.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharQuote(char ch);

		/// <summary>
		/// Indicates whether a character is any character at all.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAny(char ch);

		/// <summary>
		/// Indicates whether a character is any character but one of ['"', ',', ':'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAnyButDquoteCommaColon(char ch);

		/// <summary>
		/// Indicates whether a character is any character but one of ['\', '/'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAnyButBackForwardSlash(char ch);

		/// <summary>
		/// Given a character, produce its upper case equivalent, if any.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		char ModifyCharToUpper(char ch);

		/// <summary>
		/// Given a character, produce either 'X' or '\0'.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		char ModifyCharToNullOrX(char ch);

		/// <summary>
		/// Given a character, produce '\0'.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		char ModifyCharToNull(char ch);

		/// <summary></summary>
		/// <param name="directionName"></param>
		/// <returns></returns>
		Direction GetDirection(string directionName);

		/// <summary></summary>
		/// <param name="artifactType"></param>
		/// <returns></returns>
		ContainerType GetContainerType(ArtifactType artifactType);

		/// <summary></summary>
		/// <returns></returns>
		IConfig GetConfig();

		/// <summary>
		/// Gets the <see cref="IGameState">GameState</see> record.
		/// </summary>
		/// <returns></returns>
		IGameState GetGameState();

		/// <summary>
		/// Gets the <see cref="IModule">Module</see> record.
		/// </summary>
		/// <returns></returns>
		IModule GetModule();

		/// <summary></summary>
		/// <param name="array"></param>
		/// <param name="indexFunc"></param>
		/// <returns></returns>
		T GetRandomElement<T>(T[] array, Func<long> indexFunc = null);

		/// <summary>
		/// Evaluates the <see cref="Friendliness"/>, returning a value of type T.
		/// </summary>
		/// <param name="friendliness"></param>
		/// <param name="enemyValue"></param>
		/// <param name="neutralValue"></param>
		/// <param name="friendValue"></param>
		/// <returns></returns>
		T EvalFriendliness<T>(Friendliness friendliness, T enemyValue, T neutralValue, T friendValue);

		/// <summary>
		/// Evaluates the <see cref="Gender"/>, returning a value of type T.
		/// </summary>
		/// <param name="gender"></param>
		/// <param name="maleValue"></param>
		/// <param name="femaleValue"></param>
		/// <param name="neutralValue"></param>
		/// <returns></returns>
		T EvalGender<T>(Gender gender, T maleValue, T femaleValue, T neutralValue);

		/// <summary>
		/// Evaluates the <see cref="ContainerType"/>, returning a value of type T.
		/// </summary>
		/// <param name="containerType"></param>
		/// <param name="inValue"></param>
		/// <param name="onValue"></param>
		/// <param name="underValue"></param>
		/// <param name="behindValue"></param>
		/// <returns></returns>
		T EvalContainerType<T>(ContainerType containerType, T inValue, T onValue, T underValue, T behindValue);

		/// <summary>
		/// Evaluates the <see cref="RoomType"/>, returning a value of type T.
		/// </summary>
		/// <param name="roomType"></param>
		/// <param name="indoorsValue"></param>
		/// <param name="outdoorsValue"></param>
		/// <returns></returns>
		T EvalRoomType<T>(RoomType roomType, T indoorsValue, T outdoorsValue);

		/// <summary>
		/// Evaluates the <see cref="LightLevel"/>, returning a value of type T.
		/// </summary>
		/// <param name="lightLevel"></param>
		/// <param name="darkValue"></param>
		/// <param name="lightValue"></param>
		/// <returns></returns>
		T EvalLightLevel<T>(LightLevel lightLevel, T darkValue, T lightValue);

		/// <summary>
		/// Evaluates the plural value, returning a value of type T.
		/// </summary>
		/// <param name="isPlural"></param>
		/// <param name="singularValue"></param>
		/// <param name="pluralValue"></param>
		/// <returns></returns>
		T EvalPlural<T>(bool isPlural, T singularValue, T pluralValue);

		/// <summary></summary>
		/// <param name="bufSize"></param>
		/// <param name="fillChar"></param>
		/// <param name="number"></param>
		/// <param name="msg"></param>
		/// <param name="emptyVal"></param>
		/// <returns></returns>
		string BuildPrompt(long bufSize, char fillChar, long number, string msg, string emptyVal);

		/// <summary></summary>
		/// <param name="bufSize"></param>
		/// <param name="fillChar"></param>
		/// <param name="offset"></param>
		/// <param name="longVal"></param>
		/// <param name="stringVal"></param>
		/// <param name="lookupMsg"></param>
		/// <returns></returns>
		string BuildValue(long bufSize, char fillChar, long offset, long longVal, string stringVal, string lookupMsg);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		/// <param name="margin"></param>
		/// <param name="args"></param>
		/// <param name="clearBuf"></param>
		/// <returns></returns>
		string WordWrap(string str, StringBuilder buf, long margin, IWordWrapArgs args, bool clearBuf = true);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		/// <param name="clearBuf"></param>
		/// <returns></returns>
		string WordWrap(string str, StringBuilder buf, bool clearBuf = true);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		/// <param name="startColumn"></param>
		/// <param name="clearBuf"></param>
		/// <returns></returns>
		string LineWrap(string str, StringBuilder buf, long startColumn, bool clearBuf = true);

		/// <summary></summary>
		/// <param name="num"></param>
		/// <param name="addSpace"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetStringFromNumber(long num, bool addSpace, StringBuilder buf);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <returns></returns>
		long GetNumberFromString(string str);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		string GetContainerContentsDesc(IArtifact artifact);

		/// <summary>
		/// Gets the <see cref="Spell.Blast">Blast</see> spell description.
		/// </summary>
		/// <returns></returns>
		string GetBlastDesc();

		/// <summary>
		/// Rolls a number of dice, storing the resulting values in an array.
		/// </summary>
		/// <param name="numDice"></param>
		/// <param name="numSides"></param>
		/// <param name="dieRolls"></param>
		/// <returns></returns>
		RetCode RollDice(long numDice, long numSides, ref long[] dieRolls);

		/// <summary>
		/// Rolls a number of dice, returning a sum of the results.
		/// </summary>
		/// <param name="numDice"></param>
		/// <param name="numSides"></param>
		/// <param name="modifier"></param>
		/// <returns></returns>
		long RollDice(long numDice, long numSides, long modifier);

		/// <summary>
		/// Given an array of die rolls, sum the highest of them and return the result.
		/// </summary>
		/// <param name="dieRolls"></param>
		/// <param name="numRollsToSum"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		RetCode SumHighestRolls(long[] dieRolls, long numRollsToSum, ref long result);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <returns></returns>
		string Capitalize(string str);

		/// <summary>
		/// Deletes a set of game-related files from the filesystem.
		/// </summary>
		void UnlinkOnFailure();

		/// <summary></summary>
		/// <param name="pluralType"></param>
		/// <param name="maxSize"></param>
		void TruncatePluralTypeEffectDesc(PluralType pluralType, long maxSize);

		/// <summary></summary>
		/// <param name="effect"></param>
		void TruncatePluralTypeEffectDesc(IEffect effect);

		/// <summary></summary>
		/// <param name="buf"></param>
		void GetPossessiveName(StringBuilder buf);

		/// <summary></summary>
		/// <param name="fullPath"></param>
		/// <param name="directory"></param>
		/// <param name="fileName"></param>
		/// <param name="extension"></param>
		/// <param name="appendDirectorySeparatorChar"></param>
		/// <returns></returns>
		RetCode SplitPath(string fullPath, ref string directory, ref string fileName, ref string extension, bool appendDirectorySeparatorChar = true);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="mySeen"></param>
		/// <returns></returns>
		RetCode StripPrepsAndArticles(StringBuilder buf, ref bool mySeen);

		/// <summary></summary>
		/// <param name="title"></param>
		/// <param name="inBox"></param>
		void PrintTitle(string title, bool inBox);

		/// <summary></summary>
		/// <param name="effect"></param>
		/// <param name="printFinalNewLine"></param>
		void PrintEffectDesc(IEffect effect, bool printFinalNewLine = true);

		/// <summary></summary>
		/// <param name="effectUid"></param>
		/// <param name="printFinalNewLine"></param>
		void PrintEffectDesc(long effectUid, bool printFinalNewLine = true);

		/// <summary></summary>
		/// <returns></returns>
		RetCode ValidateRecordsAfterDatabaseLoaded();

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="articleType"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="stateDescCode"></param>
		/// <param name="showContents"></param>
		/// <param name="groupCountOne"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		RetCode GetRecordNameList(IList<IGameBase> recordList, ArticleType articleType, bool showCharOwned, StateDescDisplayCode stateDescCode, bool showContents, bool groupCountOne, StringBuilder buf);

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="name"></param>
		/// <param name="exactMatch"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		RetCode GetRecordNameCount(IList<IGameBase> recordList, string name, bool exactMatch, ref long count);

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="capitalize"></param>
		/// <param name="showExtraInfo"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		RetCode ListRecords(IList<IGameBase> recordList, bool capitalize, bool showExtraInfo, StringBuilder buf);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		/// <param name="resolveFuncs"></param>
		/// <param name="recurse"></param>
		/// <param name="invalidUid"></param>
		/// <returns></returns>
		RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse, ref long invalidUid);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		/// <param name="resolveFuncs"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse);

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="complexity"></param>
		/// <param name="type"></param>
		/// <param name="dice"></param>
		/// <param name="sides"></param>
		/// <param name="numHands"></param>
		/// <param name="calcPrice"></param>
		/// <param name="isMarcosWeapon"></param>
		/// <returns></returns>
		double GetWeaponPriceOrValue(string name, long complexity, Weapon type, long dice, long sides, long numHands, bool calcPrice, ref bool isMarcosWeapon);

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <param name="calcPrice"></param>
		/// <param name="isMarcosWeapon"></param>
		/// <returns></returns>
		double GetWeaponPriceOrValue(ICharacterArtifact weapon, bool calcPrice, ref bool isMarcosWeapon);

		/// <summary></summary>
		/// <param name="armor"></param>
		/// <param name="calcPrice"></param>
		/// <param name="isMarcosArmor"></param>
		/// <returns></returns>
		double GetArmorPriceOrValue(Armor armor, bool calcPrice, ref bool isMarcosArmor);

		/// <summary></summary>
		/// <param name="fieldDesc"></param>
		/// <param name="buf"></param>
		/// <param name="fullDesc"></param>
		/// <param name="briefDesc"></param>
		void AppendFieldDesc(FieldDesc fieldDesc, StringBuilder buf, StringBuilder fullDesc, StringBuilder briefDesc);

		/// <summary></summary>
		/// <param name="fieldDesc"></param>
		/// <param name="buf"></param>
		/// <param name="fullDesc"></param>
		/// <param name="briefDesc"></param>
		void AppendFieldDesc(FieldDesc fieldDesc, StringBuilder buf, string fullDesc, string briefDesc);

		/// <summary></summary>
		/// <param name="destCa"></param>
		/// <param name="sourceCa"></param>
		void CopyCharacterArtifactFields(ICharacterArtifact destCa, ICharacterArtifact sourceCa);

		/// <summary></summary>
		/// <param name="destAc"></param>
		/// <param name="sourceAc"></param>
		void CopyArtifactCategoryFields(IArtifactCategory destAc, IArtifactCategory sourceAc);

		/// <summary></summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		IList<IArtifact> GetArtifactList(params Func<IArtifact, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		IList<IMonster> GetMonsterList(params Func<IMonster, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		IList<IGameBase> GetRecordList(params Func<IGameBase, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="artifactList"></param>
		/// <param name="which"></param>
		/// <param name="whereClauseFunc"></param>
		/// <returns></returns>
		IArtifact GetNthArtifact(IList<IArtifact> artifactList, long which, Func<IArtifact, bool> whereClauseFunc);

		/// <summary></summary>
		/// <param name="monsterList"></param>
		/// <param name="which"></param>
		/// <param name="whereClauseFunc"></param>
		/// <returns></returns>
		IMonster GetNthMonster(IList<IMonster> monsterList, long which, Func<IMonster, bool> whereClauseFunc);

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="which"></param>
		/// <param name="whereClauseFunc"></param>
		/// <returns></returns>
		IGameBase GetNthRecord(IList<IGameBase> recordList, long which, Func<IGameBase, bool> whereClauseFunc);

		/// <summary></summary>
		/// <param name="recordList"></param>
		void StripUniqueCharsFromRecordNames(IList<IGameBase> recordList);

		/// <summary></summary>
		/// <param name="recordList"></param>
		void AddUniqueCharsToRecordNames(IList<IGameBase> recordList);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="convertToGold"></param>
		void ConvertWeaponToGoldOrTreasure(IArtifact artifact, bool convertToGold);

		#endregion
	}
}
