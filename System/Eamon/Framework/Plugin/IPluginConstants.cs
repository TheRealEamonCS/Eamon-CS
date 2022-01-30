
// IPluginConstants.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginConstants
	{
		/// <summary></summary>
		string[] CommandSepTokens { get; }

		/// <summary></summary>
		string[] PronounTokens { get; }

		/// <summary></summary>
		string ToughDesc { get; }

		/// <summary></summary>
		string CourageDesc { get; }

		/// <summary></summary>
		int ArtNameLen { get; }

		/// <summary></summary>
		int ArtStateDescLen { get; }

		/// <summary></summary>
		int ArtDescLen { get; }

		/// <summary></summary>
		int CharNameLen { get; }

		/// <summary></summary>
		int CharArtNameLen { get; }

		/// <summary></summary>
		int CharArtDescLen { get; }

		/// <summary></summary>
		int EffDescLen { get; }

		/// <summary></summary>
		int FsNameLen { get; }

		/// <summary></summary>
		int FsFileNameLen { get; }

		/// <summary></summary>
		int HntQuestionLen { get; }

		/// <summary></summary>
		int HntAnswerLen { get; }

		/// <summary></summary>
		int ModNameLen { get; }

		/// <summary></summary>
		int ModDescLen { get; }

		/// <summary></summary>
		int ModAuthorLen { get; }

		/// <summary></summary>
		int ModVolLabelLen { get; }

		/// <summary></summary>
		int ModSerialNumLen { get; }

		/// <summary></summary>
		int MonNameLen { get; }

		/// <summary></summary>
		int MonStateDescLen { get; }

		/// <summary></summary>
		int MonDescLen { get; }

		/// <summary></summary>
		int RmNameLen { get; }

		/// <summary></summary>
		int RmDescLen { get; }

		/// <summary></summary>
		long AxePrice { get; }

		/// <summary></summary>
		long BowPrice { get; }

		/// <summary></summary>
		long MacePrice { get; }

		/// <summary></summary>
		long SpearPrice { get; }

		/// <summary></summary>
		long SwordPrice { get; }

		/// <summary></summary>
		long ShieldPrice { get; }

		/// <summary></summary>
		long LeatherArmorPrice { get; }

		/// <summary></summary>
		long ChainMailPrice { get; }

		/// <summary></summary>
		long PlateMailPrice { get; }

		/// <summary></summary>
		long BlastPrice { get; }

		/// <summary></summary>
		long HealPrice { get; }

		/// <summary></summary>
		long SpeedPrice { get; }

		/// <summary></summary>
		long PowerPrice { get; }

		/// <summary></summary>
		long RecallPrice { get; }

		/// <summary></summary>
		long StatGainPrice { get; }

		/// <summary></summary>
		long WeaponTrainingPrice { get; }

		/// <summary></summary>
		long ArmorTrainingPrice { get; }

		/// <summary></summary>
		long SpellTrainingPrice { get; }

		/// <summary></summary>
		long InfoBoothPrice { get; }

		/// <summary></summary>
		long FountainPrice { get; }

		/// <summary></summary>
		long NumDatabases { get; }

		/// <summary></summary>
		long NumRulesetVersions { get; }

		/// <summary></summary>
		long NumArtifactCategories { get; }

		/// <summary></summary>
		int BufSize { get; }

		/// <summary></summary>
		int BufSize01 { get; }

		/// <summary></summary>
		int BufSize02 { get; }

		/// <summary></summary>
		int BufSize03 { get; }

		/// <summary></summary>
		string ResolveEffectRegexPattern { get; }

		/// <summary></summary>
		string ResolveUidMacroRegexPattern { get; }

		/// <summary></summary>
		string ValidWorkDirRegexPattern { get; }

		/// <summary></summary>
		string CoreLibRegexPattern { get; }

		/// <summary></summary>
		string MscorlibRegexPattern { get; }

		/// <summary></summary>
		string CoreLibName { get; }

		/// <summary></summary>
		string MscorlibName { get; }

		/// <summary></summary>
		string RecIdepErrorFmtStr { get; }

		/// <summary></summary>
		string AndroidAdventuresDir { get; }

		/// <summary></summary>
		string AdventuresDir { get; }

		/// <summary></summary>
		string QuickLaunchDir { get; }

		/// <summary></summary>
		string DefaultWorkDir { get; }

		/// <summary></summary>
		string ProcessMutexName { get; }

		/// <summary></summary>
		string EamonDesktopSlnFile { get; }

		/// <summary></summary>
		string StackTraceFile { get; }

		/// <summary></summary>
		string ProgVersion { get; }

		/// <summary></summary>
		long InfiniteDrinkableEdible { get; }

		/// <summary></summary>
		long DirectionExit { get; }

		/// <summary></summary>
		long LimboLocation { get; }

		/// <summary></summary>
		long MinWeaponComplexity { get; }

		/// <summary></summary>
		long MaxWeaponComplexity { get; }

		/// <summary></summary>
		long MinGoldValue { get; }

		/// <summary></summary>
		long MaxGoldValue { get; }

		/// <summary></summary>
		long MaxPathLen { get; }

		/// <summary></summary>
		long MaxRecursionLevel { get; }

		/// <summary></summary>
		int WindowWidth { get; }

		/// <summary></summary>
		int WindowHeight { get; }

		/// <summary></summary>
		int BufferWidth { get; }

		/// <summary></summary>
		int BufferHeight { get; }

		/// <summary></summary>
		long RightMargin { get; }

		/// <summary></summary>
		long NumRows { get; }
	}
}
