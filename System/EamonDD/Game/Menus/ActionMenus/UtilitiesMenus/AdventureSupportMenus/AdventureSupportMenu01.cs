
// AdventureSupportMenu01.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using EamonDD.Framework.Primitive.Enums;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AdventureSupportMenu01 : Menu, IAdventureSupportMenu01
	{
		/// <summary></summary>
		public virtual bool GotoCleanup { get; set; }

		/// <summary></summary>
		public virtual bool IncludeInterface { get; set; }

		/// <summary></summary>
		public virtual SupportMenuType SupportMenuType { get; set; }

		/// <summary></summary>
		public virtual string AdventureName { get; set; }

		/// <summary></summary>
		public virtual string AdventureName01 { get; set; }

		/// <summary></summary>
		public virtual string AuthorName { get; set; }

		/// <summary></summary>
		public virtual string AuthorInitials { get; set; }

		/// <summary></summary>
		public virtual string ParentClassFileName { get; set; }

		/// <summary></summary>
		public virtual string HintsXmlText { get; set; } =
@"<Complex name=""Root"" type=""Eamon.Game.DataStorage.HintDbTable, Eamon, Version=1.8.0.0, Culture=neutral, PublicKeyToken=null"">
  <Properties>
    <Collection name=""Records"" type=""Eamon.ThirdParty.BTree`1[[Eamon.Framework.IHint, Eamon, Version=1.8.0.0, Culture=neutral, PublicKeyToken=null]], Eamon, Version=1.8.0.0, Culture=neutral, PublicKeyToken=null"">
      <Properties>
        <Simple name=""IsReadOnly"" value=""False"" />
        <Simple name=""AllowDuplicates"" value=""False"" />
      </Properties>
      <Items>
        <Complex type=""YourAdventureName.Game.Hint, YourAdventureName, Version=1.8.0.0, Culture=neutral, PublicKeyToken=null"">
          <Properties>
            <Simple name=""Uid"" value=""1"" />
            <Simple name=""IsUidRecycled"" value=""True"" />
            <Simple name=""Active"" value=""True"" />
            <Simple name=""Question"" value=""EAMON CS 1.8 GENERAL HELP."" />
            <Simple name=""NumAnswers"" value=""8"" />
            <SingleArray name=""Answers"">
              <Items>
                <Simple value=""1. Commands may be abbreviated on the left or right side.  Examples:  &quot;A DR&quot; or &quot;A GON&quot; for ATTACK DRAGON, &quot;REQ RAY FROM AL&quot; or &quot;REQ GUN FROM IEN&quot; for REQUEST RAY GUN FROM ALIEN, &quot;PUT PUM IN LAR&quot; or &quot;PUT PIE IN OVEN&quot; for PUT PUMPKIN PIE IN **950"" />
                <Simple value=""2. Sometimes items may be in a room but won't show up until you EXAMINE them.  Pay close attention to all descriptions.  Note:  LOOK only repeats the room description and -nothing- else.  Use EXAMINE to, well, examine things.  **951"" />
                <Simple value=""3. Before you can manipulate items that are inside of, under or behind other items, you must REMOVE them from that item.  There is no REMOVE ALL command, but if you ATTACK a (breakable) container, all of its contents will be emptied to the **952"" />
                <Simple value=""4. Type SAVE and a number for a desired save position to Quick Save (save without having to verify).  Adding a description will rename that slot.  Examples:  &quot;SAVE 2&quot; or &quot;SAVE 5 Dark Room (NO GRUES)&quot;.  Also works with RESTORE.  (Type RESTORE 1, etc.)"" />
                <Simple value=""5. You can INVENTORY companions (normally anyone whom, when you SMILE, smiles back at you) to see what they are carrying and wearing.  You can then REQUEST those items from them.  You can also INVENTORY certain containers to get a list of contents."" />
                <Simple value=""6. If you GIVE food or a beverage to a friend, they will take a bite or drink and give it back to you."" />
                <Simple value=""7. To give money to someone, type GIVE and an amount.  For example, GIVE 1000 TO IRS AGENT would pay 1000 gold coins to the nice IRS Agent.  In most standard adventures, if you GIVE 5000 or more, a neutral monster will -usually- become your friend."" />
                <Simple value=""8. The POWER spell has been known to have strange and marvelous effects in many adventures.  When all else fails (or just for fun) try POWER."" />
              </Items>
            </SingleArray>
          </Properties>
        </Complex>
        <Complex type=""YourAdventureName.Game.Hint, YourAdventureName, Version=1.8.0.0, Culture=neutral, PublicKeyToken=null"">
          <Properties>
            <Simple name=""Uid"" value=""2"" />
            <Simple name=""IsUidRecycled"" value=""True"" />
            <Simple name=""Active"" value=""True"" />
            <Simple name=""Question"" value=""EAMON CS 1.8 NEW COMMANDS."" />
            <Simple name=""NumAnswers"" value=""3"" />
            <SingleArray name=""Answers"">
              <Items>
                <Simple value=""1. The GO command allows you to travel through any door/gate you encounter, including those that are free-standing (not associated with a room directional link)."" />
                <Simple value=""2. The SETTINGS command allows you to change a variety of configuration options, which are persisted across saved games.  The settings available and their effects on gameplay can vary between adventures, so trying this command is usually helpful."" />
                <Simple value=""3. The LOOK, EXAMINE, REMOVE and PUT commands have been enhanced to work with prepositions.  You can LOOK or EXAMINE [in|on|under|behind] [container] to get a list of contents and then REMOVE or PUT [item] [in|on|under|behind] [container] **953"" />
                <Simple value="""" />
                <Simple value="""" />
                <Simple value="""" />
                <Simple value="""" />
                <Simple value="""" />
              </Items>
            </SingleArray>
          </Properties>
        </Complex>
      </Items>
    </Collection>
    <Collection name=""FreeUids"" type=""System.Collections.Generic.List`1[[System.Int64, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"">
      <Properties>
        <Simple name=""Capacity"" value=""0"" />
      </Properties>
      <Items />
    </Collection>
    <Simple name=""CurrUid"" value=""2"" />
  </Properties>
</Complex>";

		/// <summary></summary>
		public virtual string EditAdventureShText { get; set; }

		/// <summary></summary>
		public virtual string ResumeAdventureShText { get; set; }

		/// <summary></summary>
		public virtual string EditAdventureBatText { get; set; } =
@"@echo off
cd ..\..\..\System\Bin
.\DetectDotnetAndLaunch.bat -pfn YourLibraryName.dll -wd ..\..\Adventures\YourAdventureName -la -rge
";

		/// <summary></summary>
		public virtual string ResumeAdventureBatText { get; set; } =
@"@echo off
cd ..\..\..\System\Bin
.\DetectDotnetAndLaunch.bat -pfn YourLibraryName.dll -wd ..\..\Adventures\YourAdventureName
";

		/// <summary></summary>
		public virtual string InterfaceCsText { get; set; } =
@"
// YourInterfaceName.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.YourFrameworkNamespaceName
{
	public interface YourInterfaceName : EamonLibraryName.YourFrameworkNamespaceName.YourInterfaceName
	{

	}
}
";

		/// <summary></summary>
		public virtual string InterfaceCsText01 { get; set; } =
@"
// YourInterfaceName.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

YourEamonRTUsingStatementusing static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.YourFrameworkNamespaceName
{
	public interface YourInterfaceName : EamonRTInterfaceName
	{

	}
}
";

		/// <summary></summary>
		public virtual string ClassWithInterfaceCsText { get; set; } =
@"
// YourClassName.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

YourEamonUsingStatementusing Eamon.Game.Attributes;
YourEamonRTUsingStatementusing static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.YourGameNamespaceName
{
	[ClassMappings(typeof(YourInterfaceName))]
	public class YourClassName : EamonLibraryName.YourGameNamespaceName.YourClassName, YourFrameworkNamespaceName.YourInterfaceName
	{

	}
}
";

		/// <summary></summary>
		public virtual string ClassWithInterfaceCsText01 { get; set; } =
@"
// YourClassName.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

using Eamon.Game.Attributes;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.YourGameNamespaceName
{
	[ClassMappings]
	public class YourClassName : EamonLibraryName.YourGameNamespaceName.EamonRTClassName, YourFrameworkNamespaceName.YourInterfaceName
	{

	}
}
";

		/// <summary></summary>
		public virtual string ClassCsText { get; set; } =
@"
// YourClassName.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

YourEamonUsingStatementusing Eamon.Game.Attributes;
YourEamonRTUsingStatementusing static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.YourGameNamespaceName
{
	[ClassMappings]
	public class YourClassName : EamonLibraryName.YourGameNamespaceName.YourClassName, YourInterfaceName
	{

	}
}
";

		/// <summary></summary>
		public virtual IList<string> SelectedAdvDbDataFileList { get; set; }

		/// <summary></summary>
		public virtual IList<string> SelectedClassFileList { get; set; }

		/// <summary></summary>
		/// <param name="fileText"></param>
		/// <returns></returns>
		public virtual string ReplaceMacros(string fileText)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fileText));

			return fileText.Replace("YourAdventureName", AdventureName).Replace("YourAuthorName", AuthorName).Replace("YourAuthorInitials", AuthorInitials);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool IsAdventureNameValid()
		{
			Debug.Assert(AdventureName != null);

			var regex = new Regex("^[a-zA-Z0-9]+$");

			return regex.IsMatch(AdventureName);
		}

		/// <summary></summary>
		public virtual void CheckForPrerequisites()
		{
			if (!Globals.File.Exists(Globals.Path.GetFullPath(Constants.EamonDesktopSlnFile)))
			{
				if (SupportMenuType == SupportMenuType.DeleteAdventure)
				{
					gOut.Print("{0}", Globals.LineSep);
				}

				gOut.Print("Could not locate the Eamon.Desktop solution at the following path:");

				gOut.WordWrap = false;

				gOut.Print(Globals.Path.GetFullPath(Constants.EamonDesktopSlnFile));

				gOut.WordWrap = true;

				gOut.Print(@"This Eamon CS repository does not support custom adventure development.");

				GotoCleanup = true;
			}

			if (GotoCleanup)
			{
				Globals.In.KeyPress(Buf);

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not {0}.", 
					SupportMenuType == SupportMenuType.AddAdventure ? "created" :
					SupportMenuType == SupportMenuType.DeleteAdventure ? "deleted" :
					"processed");
			}
		}

		/// <summary></summary>
		public virtual void GetAdventureName()
		{
			var invalidAdventureNames = new string[] { "Adventures", "Catalog", "Characters", "Contemporary", "Fantasy", "SciFi", "Test", "Workbench", "WorkInProgress", "AdventureSupportMenu", "LoadAdventureSupportMenu", "YourAdventureName", "YourAuthorName", "YourAuthorInitials", "Con", "Prn", "Aux", "Nul", "Null", "Com1", "Com2", "Com3", "Com4", "Com5", "Com6", "Com7", "Com8", "Com9", "Com0", "Lpt1", "Lpt2", "Lpt3", "Lpt4", "Lpt5", "Lpt6", "Lpt7", "Lpt8", "Lpt9", "Lpt0"	};

			if (SupportMenuType == SupportMenuType.AddAdventure)
			{
				gOut.Print("You must enter a name for your new adventure (e.g., The Beginner's Cave).  This should be the formal name of the adventure shown in the Main Hall's list of adventures; input should always be properly title-cased.");

				gOut.Print("Note:  the name will be used to produce a shortened form suitable for use as a folder name under the Adventures directory and also as a C# namespace (e.g., TheBeginnersCave).");
			}
			else
			{
				if (SupportMenuType == SupportMenuType.AddClasses)
				{
					gOut.Print(@"Note:  this menu option will allow you to enter the file paths for interfaces or classes you wish to add to the adventure; the actual addition will occur after you are given a final warning.  Your working directory is System and you should enter relative file paths (e.g., .\Eamon\Game\Monster.cs or .\EamonRT\Game\Components\Combat\CombatComponent.cs).  For any classes added, the corresponding .DAT datafiles (if any) will be updated appropriately.");
				}
				else if (SupportMenuType == SupportMenuType.DeleteClasses)
				{
					gOut.Print(@"Note:  this menu option will allow you to enter the file paths for interfaces or classes you wish to remove from the adventure; the actual deletion will occur after you are given a final warning.  Your working directory is the adventure folder for the game you've selected and you should enter relative file paths (e.g., .\Game\Monster.cs).  For any classes deleted, the corresponding .DAT datafiles (if any) will be updated appropriately.");
				}

				gOut.Print("You must enter the name of the adventure you wish to {0} (e.g., The Beginner's Cave).  This should be the formal name of the adventure shown in the Main Hall's list of adventures; input should always be properly title-cased.", SupportMenuType == SupportMenuType.DeleteAdventure ? "delete" : "process");
			}

			AdventureName = string.Empty;

			while (AdventureName.Length == 0)
			{
				gOut.Write("{0}Enter the name of the {1}adventure: ", Environment.NewLine, SupportMenuType == SupportMenuType.AddAdventure ? "new " : "");

				Buf.Clear();

				var rc = Globals.In.ReadField(Buf, Constants.FsNameLen, null, '_', '\0', false, null, null, gEngine.IsCharAnyButBackForwardSlash, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Buf.SetFormat("{0}", Regex.Replace(Buf.ToString(), @"\s+", " ").Trim());

				AdventureName01 = Buf.ToString();

				var tempStr = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(AdventureName01);

				AdventureName = new string((from char ch in tempStr where gEngine.IsCharAlnum(ch) select ch).ToArray());

				if (AdventureName.Length > 0 && (gEngine.IsCharDigit(AdventureName[0]) || invalidAdventureNames.FirstOrDefault(n => AdventureName.Equals(n, StringComparison.OrdinalIgnoreCase)) != null))
				{
					AdventureName = string.Empty;
				}

				if (AdventureName.Length > Constants.FsFileNameLen - 4)
				{
					AdventureName = AdventureName.Substring(0, Constants.FsFileNameLen - 4);
				}

				if (AdventureName.Length == 0)
				{
					gOut.Print("{0}", Globals.LineSep);
				}
			}

			if (SupportMenuType == SupportMenuType.AddAdventure && Globals.Directory.Exists(Constants.AdventuresDir + @"\" + AdventureName))
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure already exists.");

				GotoCleanup = true;
			}
			else if (SupportMenuType != SupportMenuType.AddAdventure && !Globals.Directory.Exists(Constants.AdventuresDir + @"\" + AdventureName))
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure does not exist.");

				GotoCleanup = true;
			}
			else if (SupportMenuType == SupportMenuType.AddClasses || SupportMenuType == SupportMenuType.DeleteClasses)
			{
				if (!Globals.File.Exists(Constants.AdventuresDir + @"\" + AdventureName + @"\" + AdventureName + @".csproj"))
				{
					gOut.Print("{0}", Globals.LineSep);

					gOut.Print("That is not a custom adventure.");

					GotoCleanup = true;
				}
				else if (!Globals.File.Exists(@".\" + AdventureName + ".dll"))
				{
					gOut.Print("{0}", Globals.LineSep);

					gOut.Print("The custom adventure library (.dll) does not exist.");

					GotoCleanup = true;
				}
				else if (Globals.File.Exists(Constants.AdventuresDir + @"\" + AdventureName + @"\FRESHMEAT.DAT"))
				{
					gOut.Print("{0}", Globals.LineSep);

					gOut.Print("The adventure is being played through.");

					GotoCleanup = true;
				}
			}
		}

		/// <summary></summary>
		public virtual void GetAuthorName()
		{
			AuthorName = string.Empty;

			while (AuthorName.Length == 0)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Enter the name(s) of the adventure's Eamon CS author(s): ", Environment.NewLine);

				Buf.Clear();

				var rc = Globals.In.ReadField(Buf, Constants.ModAuthorLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				AuthorName = Buf.Trim().ToString();
			}
		}

		/// <summary></summary>
		public virtual void GetAuthorInitials()
		{
			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}Enter the initials of the adventure's main Eamon CS author: ", Environment.NewLine);

			Buf.Clear();

			var rc = Globals.In.ReadField(Buf, Constants.ModVolLabelLen - 4, null, '_', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharAlpha, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			AuthorInitials = Buf.Trim().ToString();
		}

		/// <summary></summary>
		public virtual void SelectAdvDbDataFiles()
		{
			RetCode rc;

			SelectedAdvDbDataFileList = new List<string>();

			var advDbDataFiles = new string[] { "ADVENTURES.DAT", "FANTASY.DAT", "SCIFI.DAT", "CONTEMPORARY.DAT", "TEST.DAT", "WIP.DAT" };

			if (SupportMenuType == SupportMenuType.AddAdventure)
			{
				var inputDefaultValue = "Y";

				foreach (var advDbDataFile in advDbDataFiles)
				{
					gOut.Print("{0}", Globals.LineSep);

					gOut.Write("{0}Add this game to adventure database \"{1}\" (Y/N) [{2}]: ", Environment.NewLine, advDbDataFile, inputDefaultValue);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, inputDefaultValue, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (Buf.Length == 0 || Buf[0] != 'N')
					{
						SelectedAdvDbDataFileList.Add(advDbDataFile);

						if (!advDbDataFile.Equals("ADVENTURES.DAT"))
						{
							inputDefaultValue = "N";
						}
					}
				}
			}
			else
			{
				SelectedAdvDbDataFileList.AddRange(advDbDataFiles);
			}

			var customAdvDbDataFile = string.Empty;

			while (true)
			{
				gOut.Print("{0}", Globals.LineSep);

				if (customAdvDbDataFile.Length == 0)
				{
					gOut.Print("If you would like to {0} one or more custom adventure databases, enter those file names now (e.g., HORROR.DAT).  To skip this step, or if you are done, just press enter.", SupportMenuType == SupportMenuType.AddAdventure ? "add this adventure to" : "delete this adventure from");
				}

				gOut.Write("{0}Enter name of custom adventure database: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, null, gEngine.ModifyCharToUpper, gEngine.IsCharAlnumPeriodUnderscore, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				customAdvDbDataFile = Buf.Trim().ToString();

				if (customAdvDbDataFile.Length > 0)
				{
					if (SelectedAdvDbDataFileList.FirstOrDefault(fn => fn.Equals(customAdvDbDataFile, StringComparison.OrdinalIgnoreCase)) == null)
					{
							SelectedAdvDbDataFileList.Add(customAdvDbDataFile);
					}
				}
				else
				{
					break;
				}
			}
		}

		/// <summary></summary>
		public virtual void QueryToAddAdventure()
		{
			RetCode rc;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}Would you like to add this adventure to Eamon CS (Y/N): ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not created.");

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		public virtual void QueryToProcessAdventure()
		{
			RetCode rc;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("WARNING:  you are about to {0} the following classes and update any associated .DAT files.  If you have any doubts, you should select 'N' and backup your Eamon CS repository before proceeding.  This action is PERMANENT!", SupportMenuType == SupportMenuType.DeleteClasses ? "delete" : "add");

			foreach (var selectedClassFile in SelectedClassFileList)
			{
				gOut.Write("{0}{1}", Environment.NewLine, selectedClassFile);
			}

			gOut.WriteLine();

			gOut.Write("{0}Would you like to {1} these classes {2} the adventure (Y/N): ", 
				Environment.NewLine, 
				SupportMenuType == SupportMenuType.DeleteClasses ? "delete" : "add",
				SupportMenuType == SupportMenuType.DeleteClasses ? "from" : "to");

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Buf.Length == 0 || Buf[0] != 'Y')
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not processed.");

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		public virtual void CreateQuickLaunchFiles()
		{
			var yourLibraryName = this is IAddCustomAdventureMenu ? AdventureName : "EamonRT";

			// Note: QuickLaunch files missing in Eamon CS Mobile

			if (Globals.Directory.Exists(Constants.QuickLaunchDir + @"\Unix\EamonDD"))
			{
				var fileText = EditAdventureShText.Replace("YourLibraryName", yourLibraryName);

				Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Unix\EamonDD\Edit" + AdventureName + ".sh", ReplaceMacros(fileText), new ASCIIEncoding());
			}

			if (Globals.Directory.Exists(Constants.QuickLaunchDir + @"\Unix\EamonRT"))
			{
				var fileText = ResumeAdventureShText.Replace("YourLibraryName", yourLibraryName);

				Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Unix\EamonRT\Resume" + AdventureName + ".sh", ReplaceMacros(fileText), new ASCIIEncoding());
			}

			if (Globals.Directory.Exists(Constants.QuickLaunchDir + @"\Windows\EamonDD"))
			{
				var fileText = EditAdventureBatText.Replace("YourLibraryName", yourLibraryName);

				Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Windows\EamonDD\Edit" + AdventureName + ".bat", ReplaceMacros(fileText), new ASCIIEncoding());
			}

			if (Globals.Directory.Exists(Constants.QuickLaunchDir + @"\Windows\EamonRT"))
			{
				var fileText = ResumeAdventureBatText.Replace("YourLibraryName", yourLibraryName);

				Globals.File.WriteAllText(Constants.QuickLaunchDir + @"\Windows\EamonRT\Resume" + AdventureName + ".bat", ReplaceMacros(fileText), new ASCIIEncoding());
			}
		}

		/// <summary></summary>
		public virtual void CreateAdventureFolder()
		{
			Globals.Directory.CreateDirectory(Constants.AdventuresDir + @"\" + AdventureName);
		}

		/// <summary></summary>
		public virtual void CreateCustomClassFile()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(ParentClassFileName));

			var parentClassFileExists = Globals.File.Exists(ParentClassFileName);

			var eamonLibraryName = ParentClassFileName.StartsWith(@"..\Eamon\") ? "Eamon" : ParentClassFileName.StartsWith(@"..\EamonDD\") ? "EamonDD" : "EamonRT";

			if (ParentClassFileName.Contains(@"\Game\"))
			{
				var yourClassName = Globals.Path.GetFileNameWithoutExtension(ParentClassFileName);

				var yourInterfaceName = "I" + yourClassName;

				var fileText = parentClassFileExists ? Globals.File.ReadAllText(ParentClassFileName) : ParentClassFileName.Contains(@"\States\") ? "namespace EamonRT.Game.States" : "namespace EamonRT.Game.Commands";

				var matches = Regex.Matches(fileText, @".*namespace (.+[^ {\n\r\t])");

				if (matches.Count == 1 && matches[0].Groups.Count == 2)
				{
					var yourGameNamespaceName = matches[0].Groups[1].Value + ".";

					var yourFrameworkNamespaceName = yourGameNamespaceName.Replace(".Game.", ".Framework.");

					yourGameNamespaceName = yourGameNamespaceName.Replace(eamonLibraryName + ".", "").TrimEnd('.');

					yourFrameworkNamespaceName = yourFrameworkNamespaceName.Replace(eamonLibraryName + ".", "").TrimEnd('.');

					var childClassFileName = ParentClassFileName.Replace(@"..\" + eamonLibraryName, Constants.AdventuresDir + @"\" + AdventureName);

					var childClassPath = Globals.Path.GetDirectoryName(childClassFileName);

					Globals.Directory.CreateDirectory(childClassPath);

					var yourEamonUsingStatement = string.Empty;

					var yourEamonRTUsingStatement = string.Empty;

					if (eamonLibraryName.Equals("Eamon"))
					{
						yourEamonUsingStatement = string.Format("using {0}.{1};\r\n", eamonLibraryName, yourFrameworkNamespaceName);
					}
					else
					{
						yourEamonRTUsingStatement = string.Format("using {0}.{1};\r\n", eamonLibraryName, yourFrameworkNamespaceName);
					}

					if (IncludeInterface)
					{
						var childInterfaceFileName = childClassFileName.Replace(@"\Game\", @"\Framework\").Replace(@"\" + yourClassName + ".cs", @"\" + yourInterfaceName + ".cs");

						if (!Globals.File.Exists(childInterfaceFileName))
						{
							var childInterfacePath = Globals.Path.GetDirectoryName(childInterfaceFileName);

							Globals.Directory.CreateDirectory(childInterfacePath);

							if (parentClassFileExists)
							{
								fileText = InterfaceCsText.Replace("EamonLibraryName", eamonLibraryName).Replace("YourFrameworkNamespaceName", yourFrameworkNamespaceName).Replace("YourInterfaceName", yourInterfaceName);
							}
							else
							{
								var eamonRTInterfaceName = ParentClassFileName.Contains(@"\States\") ? "IState" : "ICommand";

								fileText = InterfaceCsText01.Replace("YourEamonRTUsingStatement", yourEamonRTUsingStatement).Replace("EamonRTInterfaceName", eamonRTInterfaceName).Replace("YourFrameworkNamespaceName", yourFrameworkNamespaceName).Replace("YourInterfaceName", yourInterfaceName);
							}

							Globals.File.WriteAllText(childInterfaceFileName, ReplaceMacros(fileText));
						}
					}

					if (parentClassFileExists)
					{
						fileText = IncludeInterface ? ClassWithInterfaceCsText : ClassCsText;

						fileText = fileText.Replace("EamonLibraryName", eamonLibraryName).Replace("YourEamonUsingStatement", yourEamonUsingStatement).Replace("YourEamonRTUsingStatement", yourEamonRTUsingStatement).Replace("YourFrameworkNamespaceName", yourFrameworkNamespaceName).Replace("YourGameNamespaceName", yourGameNamespaceName).Replace("YourInterfaceName", yourInterfaceName).Replace("YourClassName", yourClassName);
					}
					else
					{
						var eamonRTClassName = ParentClassFileName.Contains(@"\States\") ? "State" : "Command";

						fileText = ClassWithInterfaceCsText01.Replace("EamonLibraryName", eamonLibraryName).Replace("EamonRTClassName", eamonRTClassName).Replace("YourFrameworkNamespaceName", yourFrameworkNamespaceName).Replace("YourGameNamespaceName", yourGameNamespaceName).Replace("YourInterfaceName", yourInterfaceName).Replace("YourClassName", yourClassName);
					}

					Globals.File.WriteAllText(childClassFileName, ReplaceMacros(fileText));
				}
			}
			else
			{
				var yourInterfaceName = Globals.Path.GetFileNameWithoutExtension(ParentClassFileName);

				var fileText = parentClassFileExists ? Globals.File.ReadAllText(ParentClassFileName) : ParentClassFileName.Contains(@"\States\") ? "namespace EamonRT.Framework.States" : "namespace EamonRT.Framework.Commands";

				var matches = Regex.Matches(fileText, @".*namespace (.+[^ {\n\r\t])");

				if (matches.Count == 1 && matches[0].Groups.Count == 2)
				{
					var yourFrameworkNamespaceName = matches[0].Groups[1].Value + ".";

					yourFrameworkNamespaceName = yourFrameworkNamespaceName.Replace(eamonLibraryName + ".", "").TrimEnd('.');

					var childInterfaceFileName = ParentClassFileName.Replace(@"..\" + eamonLibraryName, Constants.AdventuresDir + @"\" + AdventureName);

					var childInterfacePath = Globals.Path.GetDirectoryName(childInterfaceFileName);

					Globals.Directory.CreateDirectory(childInterfacePath);

					var yourEamonRTUsingStatement = string.Format("using {0}.{1};\r\n", eamonLibraryName, yourFrameworkNamespaceName);

					if (parentClassFileExists)
					{
						fileText = InterfaceCsText.Replace("EamonLibraryName", eamonLibraryName).Replace("YourFrameworkNamespaceName", yourFrameworkNamespaceName).Replace("YourInterfaceName", yourInterfaceName);
					}
					else
					{
						var eamonRTInterfaceName = ParentClassFileName.Contains(@"\States\") ? "IState" : "ICommand";

						fileText = InterfaceCsText01.Replace("YourEamonRTUsingStatement", yourEamonRTUsingStatement).Replace("EamonRTInterfaceName", eamonRTInterfaceName).Replace("YourFrameworkNamespaceName", yourFrameworkNamespaceName).Replace("YourInterfaceName", yourInterfaceName);
					}

					Globals.File.WriteAllText(childInterfaceFileName, ReplaceMacros(fileText));
				}
			}
		}

		/// <summary></summary>
		public virtual void CreateHintsXml()
		{
			var yourAdventureName = this is IAddCustomAdventureMenu ? AdventureName : "Eamon";

			var fileText = HintsXmlText.Replace("YourAdventureName", yourAdventureName);
				
			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\HINTS.DAT", ReplaceMacros(fileText));
		}

		/// <summary></summary>
		public virtual void UpdateAdvDbDataFiles()
		{
			RetCode rc;

			foreach (var advDbDataFile in SelectedAdvDbDataFileList)
			{
				rc = Globals.PushDatabase();

				Debug.Assert(gEngine.IsSuccess(rc));

				var fsfn = Globals.Path.Combine(".", advDbDataFile);

				rc = Globals.Database.LoadFilesets(fsfn, printOutput: false);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (SupportMenuType == SupportMenuType.AddAdventure)
				{
					var fileset = Globals.CreateInstance<IFileset>(x =>
					{
						x.Uid = Globals.Database.GetFilesetUid();

						x.IsUidRecycled = true;

						x.Name = AdventureName01;

						x.WorkDir = Constants.AdventuresDir + @"\" + AdventureName;

						x.PluginFileName = this is IAddStandardAdventureMenu ? "EamonRT.dll" : AdventureName + ".dll";

						x.ConfigFileName = "NONE";

						x.FilesetFileName = "NONE";

						x.CharacterFileName = "NONE";

						x.ModuleFileName = "MODULE.DAT";

						x.RoomFileName = "ROOMS.DAT";

						x.ArtifactFileName = "ARTIFACTS.DAT";

						x.EffectFileName = "EFFECTS.DAT";

						x.MonsterFileName = "MONSTERS.DAT";

						x.HintFileName = "HINTS.DAT";

						x.TriggerFileName = "TRIGGERS.DAT";

						x.ScriptFileName = "SCRIPTS.DAT";

						x.GameStateFileName = "NONE";
					});

					rc = Globals.Database.AddFileset(fileset);

					Debug.Assert(gEngine.IsSuccess(rc));
				}
				else
				{
					var fileset = Globals.Database.FilesetTable.Records.FirstOrDefault(fs => fs.WorkDir.Equals(Constants.AdventuresDir + @"\" + AdventureName, StringComparison.OrdinalIgnoreCase));

					if (fileset != null)
					{
						Globals.Database.RemoveFileset(fileset.Uid);

						fileset.Dispose();
					}
				}

				rc = Globals.Database.SaveFilesets(fsfn, printOutput: false);

				Debug.Assert(gEngine.IsSuccess(rc));

				rc = Globals.PopDatabase();

				Debug.Assert(gEngine.IsSuccess(rc));
			}
		}

		/// <summary></summary>
		public virtual void UpdateDatFileClasses()
		{
			foreach (var selectedClassFile in SelectedClassFileList)
			{
				var className = Globals.Path.GetFileNameWithoutExtension(selectedClassFile);

				var fileName = @".\" + className.ToUpper() + (className.Equals("Module") ? ".DAT" : "S.DAT");

				if (Globals.File.Exists(fileName))
				{
					if (SupportMenuType == SupportMenuType.AddClasses)
					{
						var fileText = Globals.File.ReadAllText(fileName);

						Globals.File.WriteAllText(fileName, fileText.Replace("Eamon.Game." + className + ", Eamon", AdventureName + ".Game." + className + ", " + AdventureName));
					}
					else if (SupportMenuType == SupportMenuType.DeleteClasses)
					{
						var fileText = Globals.File.ReadAllText(fileName);

						Globals.File.WriteAllText(fileName, fileText.Replace(AdventureName + ".Game." + className + ", " + AdventureName, "Eamon.Game." + className + ", Eamon"));
					}
				}
			}
		}

		/// <summary></summary>
		public virtual void RebuildProject()
		{
			var result = RetCode.Failure;

			if (IsAdventureNameValid())
			{
				var projName = Globals.Path.GetFullPath(Globals.Path.Combine(Constants.AdventuresDir + @"\" + AdventureName, AdventureName + ".csproj"));

				Debug.Assert(!string.IsNullOrWhiteSpace(projName));

				try
				{
					using (var process = new Process())
					{
						process.StartInfo.RedirectStandardOutput = true;
						process.StartInfo.RedirectStandardError = true;
						process.StartInfo.UseShellExecute = false;
						process.StartInfo.CreateNoWindow = true;

						process.StartInfo.FileName = "dotnet";
						process.StartInfo.Arguments = string.Format("build {0} --no-dependencies", projName);
						process.StartInfo.WorkingDirectory = string.Format("..{0}..", Globals.Path.DirectorySeparatorChar);

						gOut.Write("Rebuilding {0} project... ", Globals.Path.GetFileNameWithoutExtension(projName));

						process.Start();

						result = process.WaitForExit(300000) && process.ExitCode == 0 ? RetCode.Success : RetCode.Failure;

						try { process.Kill(); } catch (Exception) { }

						if (result == RetCode.Success)
						{
							gOut.WriteLine("succeeded");
						}
						else
						{
							gOut.WriteLine("failed");
						}
					}
				}
				catch (Exception ex)
				{
					gOut.WriteLine(ex.ToString());

					result = RetCode.Failure;
				}
			}

			if (result == RetCode.Failure)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not {0}.", SupportMenuType == SupportMenuType.AddAdventure ? "created" : "processed");

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		public virtual void DeleteAdvBinaryFiles()
		{
			if (IsAdventureNameValid())
			{
				var srcFileName = @".\" + AdventureName + ".dll";

				if (Globals.File.Exists(srcFileName))
				{
					Globals.File.Delete(srcFileName);
				}

				srcFileName = @".\" + AdventureName + ".pdb";

				if (Globals.File.Exists(srcFileName))
				{
					Globals.File.Delete(srcFileName);
				}

				srcFileName = @".\" + AdventureName + ".xml";

				if (Globals.File.Exists(srcFileName))
				{
					Globals.File.Delete(srcFileName);
				}

				srcFileName = @".\" + AdventureName + ".deps.json";

				if (Globals.File.Exists(srcFileName))
				{
					Globals.File.Delete(srcFileName);
				}
			}
		}

		/// <summary></summary>
		public virtual void PrintAdventureCreated()
		{
			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("The adventure was successfully created.");
		}

		/// <summary></summary>
		public virtual void PrintAdventureProcessed()
		{
			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("The adventure was successfully processed.");
		}

		public AdventureSupportMenu01()
		{
			Buf = Globals.Buf;

			SupportMenuType = this is IAddStandardAdventureMenu || this is IAddCustomAdventureMenu ? SupportMenuType.AddAdventure :
									this is IAddCustomAdventureClassesMenu ? SupportMenuType.AddClasses :
									this is IDeleteAdventureMenu ? SupportMenuType.DeleteAdventure :
									SupportMenuType.DeleteClasses;

			EditAdventureShText = string.Format(@"#!/bin/sh{0}cd ../../../System/Bin{0}./DetectDotnetAndLaunch.sh -pfn YourLibraryName.dll -wd ../../Adventures/YourAdventureName -la -rge{0}", '\n');

			ResumeAdventureShText = string.Format(@"#!/bin/sh{0}cd ../../../System/Bin{0}./DetectDotnetAndLaunch.sh -pfn YourLibraryName.dll -wd ../../Adventures/YourAdventureName{0}", '\n');
		}
	}
}
