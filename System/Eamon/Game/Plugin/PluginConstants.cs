
// PluginConstants.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework.Plugin;

namespace Eamon.Game.Plugin
{
	public class PluginConstants : IPluginConstants
	{
		public virtual string[] CommandSepTokens { get; protected set; }

		public virtual string[] PronounTokens { get; protected set; }

		public virtual string ToughDesc { get; protected set; }

		public virtual string CourageDesc { get; protected set; }

		public virtual int ArtNameLen { get; protected set; } = 40;

		public virtual int ArtStateDescLen { get; protected set; } = 64;

		public virtual int ArtDescLen { get; protected set; } = 256;

		public virtual int CharNameLen { get; protected set; } = 32;

		public virtual int CharArtNameLen { get; protected set; } = 40;

		public virtual int CharArtDescLen { get; protected set; } = 256;

		public virtual int EffDescLen { get; protected set; } = 256;

		public virtual int FsNameLen { get; protected set; } = 40;

		public virtual int FsFileNameLen { get; protected set; } = 64;

		public virtual int HntQuestionLen { get; protected set; } = 256;

		public virtual int HntAnswerLen { get; protected set; } = 256;

		public virtual int ModNameLen { get; protected set; } = 40;

		public virtual int ModDescLen { get; protected set; } = 256;

		public virtual int ModAuthorLen { get; protected set; } = 40;

		public virtual int ModVolLabelLen { get; protected set; } = 7;

		public virtual int ModSerialNumLen { get; protected set; } = 3;

		public virtual int MonNameLen { get; protected set; } = 40;

		public virtual int MonStateDescLen { get; protected set; } = 64;

		public virtual int MonDescLen { get; protected set; } = 256;

		public virtual int RmNameLen { get; protected set; } = 50;

		public virtual int RmDescLen { get; protected set; } = 256;

		public virtual long AxePrice { get; protected set; } = 25;

		public virtual long BowPrice { get; protected set; } = 40;

		public virtual long MacePrice { get; protected set; } = 20;

		public virtual long SpearPrice { get; protected set; } = 25;

		public virtual long SwordPrice { get; protected set; } = 30;

		public virtual long ShieldPrice { get; protected set; } = 50;

		public virtual long LeatherArmorPrice { get; protected set; } = 100;

		public virtual long ChainMailPrice { get; protected set; } = 250;

		public virtual long PlateMailPrice { get; protected set; } = 500;

		public virtual long BlastPrice { get; protected set; } = 1000;

		public virtual long HealPrice { get; protected set; } = 500;

		public virtual long SpeedPrice { get; protected set; } = 4000;

		public virtual long PowerPrice { get; protected set; } = 100;

		public virtual long RecallPrice { get; protected set; } = 100;

		public virtual long StatGainPrice { get; protected set; } = 4500;

		public virtual long WeaponTrainingPrice { get; protected set; } = 1000;

		public virtual long ArmorTrainingPrice { get; protected set; } = 1000;

		public virtual long SpellTrainingPrice { get; protected set; } = 1000;

		public virtual long InfoBoothPrice { get; protected set; } = 30;

		public virtual long FountainPrice { get; protected set; } = 20;

		public virtual long NumDatabases { get; protected set; } = 10;

		public virtual long NumRulesetVersions { get; protected set; } = 10;

		public virtual long NumArtifactCategories { get; protected set; } = 4;

		public virtual int BufSize { get; protected set; } = 1024;

		public virtual int BufSize01 { get; protected set; } = 6;

		public virtual int BufSize02 { get; protected set; } = 1;

		public virtual int BufSize03 { get; protected set; } = 64;

		public virtual string ResolveEffectRegexPattern { get; protected set; } = @"[^\*]\*[0-9]{3}|\*\*[0-9]{3}";

		public virtual string ResolveUidMacroRegexPattern { get; protected set; } = @"[^\*]\*[0-9]{3}|\*\*[0-9]{3}|[^@]@[0-9]{3}|@@[0-9]{3}";

		public virtual string ValidWorkDirRegexPattern { get; protected set; } = @"^(NONE)$|^(\.)$|^(\.\.\\\.\.\\Adventures\\[a-zA-Z0-9]+)$|^(\.\.\/\.\.\/Adventures\/[a-zA-Z0-9]+)$";

		public virtual string CoreLibRegexPattern { get; protected set; } = @"System\.Private\.CoreLib, Version=6\.0\.0\.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";

		public virtual string MscorlibRegexPattern { get; protected set; } = @"mscorlib, Version=4\.0\.0\.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		public virtual string CoreLibName { get; protected set; } = @"System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";

		public virtual string MscorlibName { get; protected set; } = @"mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		public virtual string RecIdepErrorFmtStr { get; protected set; } = "The {0} field refers to {1} Uid {2}, {3}.";

		public virtual string AndroidAdventuresDir { get; protected set; } = @"..\EamonPM.Android\Assets\Adventures";

		public virtual string AdventuresDir { get; protected set; } = @"..\..\Adventures";

		public virtual string QuickLaunchDir { get; protected set; } = @"..\..\QuickLaunch";

		public virtual string DefaultWorkDir { get; protected set; } = ".";

		public virtual string ProcessMutexName { get; protected set; }

		public virtual string EamonDesktopSlnFile { get; protected set; } = @"..\..\Eamon.Desktop.sln";

		public virtual string StackTraceFile { get; protected set; } = "STACKTRACE.TXT";

		public virtual string ProgVersion { get; protected set; } = "1.8.0";

		public virtual long InfiniteDrinkableEdible { get; protected set; } = 9999;

		public virtual long DirectionExit { get; protected set; } = -999;

		public virtual long LimboLocation { get; protected set; } = 0;

		public virtual long MinWeaponComplexity { get; protected set; } = -50;

		public virtual long MaxWeaponComplexity { get; protected set; } = 50;

		public virtual long MinGoldValue { get; protected set; } = -99999;

		public virtual long MaxGoldValue { get; protected set; } = 999999;

		public virtual long MaxPathLen { get; protected set; } = 256;

		public virtual long MaxRecursionLevel { get; protected set; } = 100;

		public virtual int WindowWidth { get; protected set; } = 80;

		public virtual int WindowHeight { get; protected set; } = 50;

		public virtual int BufferWidth { get; protected set; } = 80;

		public virtual int BufferHeight { get; protected set; } = 9999;

		public virtual long RightMargin { get; protected set; } = 78;

		public virtual long NumRows { get; protected set; } = 22;

		public PluginConstants()
		{
			CommandSepTokens = new string[] { ".", "!", "?", ";", ",", "and", "then", "also" };

			PronounTokens = new string[] { "him", "her", "it", "that", "them", "those" };

			ToughDesc = string.Format("Monsters usually fall into one of the following categories, but it is possible to create hybrids that are weak in some areas and strong in others:{0}{0}Weak Monsters - wimps and small creatures like rats, kobolds, etc.{0}Medium Monsters - petty thugs, orcs, goblins, etc.{0}Tough Monsters - giants, trolls, highly skilled warriors, etc.{0}Exceptional Monsters - dragons, demons, special villians, etc.{0}{0}For group Monsters, enter data relating to a single member of the group and scale values down lower than usual for groups with five or more members.", Environment.NewLine);

			CourageDesc = string.Format("Courage works as follows:{0}{0}1-100% - the chance the Monster won't flee combat and/or follow a fleeing player (if enemy).  If the Monster is injured or gravely injured, then effective courage is reduced by 5% or 10%, respectively.{0}200% - the Monster will never flee and always follow the player.", Environment.NewLine);

			ProcessMutexName = string.Format(@"Global\Eamon_CS_{0}_Process_Mutex", ProgVersion);
		}
	}
}
