
// AddCustomAdventureMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddCustomAdventureMenu : AdventureSupportMenu01, IAddCustomAdventureMenu
	{
		/// <summary></summary>
		public virtual string ProgramCsText { get; set; } =
@"
// Program.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

/*

*/

using Eamon.Framework.Portability;

namespace YourAdventureName
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = ""YourAdventureName"";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
";

		/// <summary></summary>
		public virtual string[] IPluginCsText { get; set; } = new string[]
		{
@"
// IPluginClassMappings.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

namespace YourAdventureName.Framework.Plugin
{
	public interface IPluginClassMappings : EamonRT.Framework.Plugin.IPluginClassMappings
	{

	}
}
",
@"
// IPluginConstants.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

namespace YourAdventureName.Framework.Plugin
{
	public interface IPluginConstants : EamonRT.Framework.Plugin.IPluginConstants
	{

	}
}
",
@"
// IPluginGlobals.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

namespace YourAdventureName.Framework.Plugin
{
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{

	}
}
"
		};

		/// <summary></summary>
		public virtual string[] PluginCsText { get; set; } = new string[]
		{
@"
// PluginClassMappings.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

using System.Reflection;
using Eamon;

namespace YourAdventureName.Game.Plugin
{
	public class PluginClassMappings : EamonRT.Game.Plugin.PluginClassMappings, Framework.Plugin.IPluginClassMappings
	{
		public override RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = base.LoadPluginClassMappings();

			if (rc != RetCode.Success)
			{
				goto Cleanup;
			}

			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());

		Cleanup:

			return rc;
		}
	}
}
",
@"
// PluginConstants.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

namespace YourAdventureName.Game.Plugin
{
	public class PluginConstants : EamonRT.Game.Plugin.PluginConstants, Framework.Plugin.IPluginConstants
	{

	}
}
",
@"
// PluginContext.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;

namespace YourAdventureName.Game.Plugin
{
	public static class PluginContext
	{
		public static Framework.Plugin.IPluginConstants Constants
		{
			get
			{
				return (Framework.Plugin.IPluginConstants)EamonRT.Game.Plugin.PluginContext.Constants;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Constants = value;
			}
		}

		public static Framework.Plugin.IPluginClassMappings ClassMappings
		{
			get
			{
				return (Framework.Plugin.IPluginClassMappings)EamonRT.Game.Plugin.PluginContext.ClassMappings;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.ClassMappings = value;
			}
		}

		public static Framework.Plugin.IPluginGlobals Globals
		{
			get
			{
				return (Framework.Plugin.IPluginGlobals)EamonRT.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Globals = value;
			}
		}

		public static ITextWriter gOut
		{
			get
			{
				return EamonRT.Game.Plugin.PluginContext.gOut;
			}
		}

		public static EamonRT.Framework.IEngine gEngine
		{
			get
			{
				return (EamonRT.Framework.IEngine)EamonRT.Game.Plugin.PluginContext.gEngine;
			}
		}

		public static IRecordDb<Eamon.Framework.IRoom> gRDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IRoom>)EamonRT.Game.Plugin.PluginContext.gRDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IArtifact> gADB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IArtifact>)EamonRT.Game.Plugin.PluginContext.gADB;
			}
		}

		public static IRecordDb<Eamon.Framework.IEffect> gEDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IEffect>)EamonRT.Game.Plugin.PluginContext.gEDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IMonster> gMDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IMonster>)EamonRT.Game.Plugin.PluginContext.gMDB;
			}
		}

		public static IRecordDb<Eamon.Framework.ITrigger> gTDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.ITrigger>)EamonRT.Game.Plugin.PluginContext.gTDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IScript> gSDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IScript>)EamonRT.Game.Plugin.PluginContext.gSDB;
			}
		}

		public static EamonRT.Framework.Parsing.ISentenceParser gSentenceParser
		{
			get
			{
				return (EamonRT.Framework.Parsing.ISentenceParser)EamonRT.Game.Plugin.PluginContext.gSentenceParser;
			}
		}

		public static EamonRT.Framework.Parsing.ICommandParser gCommandParser
		{
			get
			{
				return (EamonRT.Framework.Parsing.ICommandParser)EamonRT.Game.Plugin.PluginContext.gCommandParser;
			}
		}

		public static Framework.IGameState gGameState
		{
			get
			{
				return (Framework.IGameState)EamonRT.Game.Plugin.PluginContext.gGameState;
			}
		}

		public static Eamon.Framework.ICharacter gCharacter
		{
			get
			{
				return (Eamon.Framework.ICharacter)EamonRT.Game.Plugin.PluginContext.gCharacter;
			}
		}

		public static Eamon.Framework.IMonster gCharMonster
		{
			get
			{
				return (Eamon.Framework.IMonster)EamonRT.Game.Plugin.PluginContext.gCharMonster;
			}
		}

		public static Eamon.Framework.IMonster gActorMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IMonster)commandParser?.ActorMonster;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IMonster)command?.ActorMonster;
			}
			else
			{
				return null;
			}
		}

		public static Eamon.Framework.IRoom gActorRoom(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IRoom)commandParser?.ActorRoom;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IRoom)command?.ActorRoom;
			}
			else
			{
				return null;
			}
		}

		public static Eamon.Framework.IArtifact gDobjArtifact(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IArtifact)commandParser?.DobjArtifact;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IArtifact)command?.DobjArtifact;
			}
			else
			{
				return null;
			}
		}

		public static Eamon.Framework.IMonster gDobjMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IMonster)commandParser?.DobjMonster;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IMonster)command?.DobjMonster;
			}
			else
			{
				return null;
			}
		}

		public static Eamon.Framework.IArtifact gIobjArtifact(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IArtifact)commandParser?.IobjArtifact;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IArtifact)command?.IobjArtifact;
			}
			else
			{
				return null;
			}
		}

		public static Eamon.Framework.IMonster gIobjMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IMonster)commandParser?.IobjMonster;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IMonster)command?.IobjMonster;
			}
			else
			{
				return null;
			}
		}
	}
}
",
@"
// PluginGlobals.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

namespace YourAdventureName.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
	{

	}
}
"
		};

		/// <summary></summary>
		public virtual string EngineCsText { get; set; } =
@"
// Engine.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{

	}
}
";

		/// <summary></summary>
		public virtual string ChangeLogText { get; set; } =
@"
==================================================================================================================================
ChangeLog: YourAdventureName
==================================================================================================================================

Date            Version            Who            Notes
----------------------------------------------------------------------------------------------------------------------------------
20XXXXXX        1.8.0              YourAuthorInitials             Code complete 1.8.0
";

		/// <summary></summary>
		public virtual string AdventureCsprojText { get; set; } =
@"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <Version>1.8.0.0</Version>
    <Authors>YourAuthorName</Authors>
    <Company>YourAuthorName</Company>
    <Product>The Wonderful World of Eamon CS</Product>
    <Description>Eamon CS Adventure Plugin</Description>
    <Copyright>Copyright (C) 2014+</Copyright>
  </PropertyGroup>

  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|AnyCPU'"">
    <DefineConstants>TRACE;DEBUG;NETSTANDARD2_0;PORTABLE</DefineConstants>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
    <OutputPath>..\..\System\Bin\</OutputPath>
    <DocumentationFile>..\..\System\Bin\YourAdventureName.xml</DocumentationFile>
    <NoWarn>0419;1574;1591;1701;1702;1705</NoWarn>
  </PropertyGroup>

  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|AnyCPU'"">
    <DefineConstants>TRACE;RELEASE;NETSTANDARD2_0;PORTABLE</DefineConstants>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
    <OutputPath>..\..\System\Bin\</OutputPath>
    <DocumentationFile>..\..\System\Bin\YourAdventureName.xml</DocumentationFile>
    <NoWarn>0419;1574;1591;1701;1702;1705</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include=""..\..\System\EamonDD\EamonDD.csproj"" />
    <ProjectReference Include=""..\..\System\EamonRT\EamonRT.csproj"" />
    <ProjectReference Include=""..\..\System\Eamon\Eamon.csproj"" />
  </ItemGroup>

</Project>
";

		public override void Execute()
		{
			gOut.WriteLine();

			gEngine.PrintTitle("ADD CUSTOM ADVENTURE", true);

			Debug.Assert(!gEngine.IsAdventureFilesetLoaded());

			GotoCleanup = false;

			var workDir = Globals.Directory.GetCurrentDirectory();

			CheckForPrerequisites();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			GetAdventureName();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			GetAuthorName();

			GetAuthorInitials();

			SelectAdvDbDataFiles();

			QueryToAddAdventure();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			CreateQuickLaunchFiles();

			CreateAdventureFolder();

			CreateHintsXml();

			CreateCustomFiles();

			UpdateAdvDbDataFiles();

			AddProjectToSolution();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			RebuildProject();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			PrintAdventureCreated();

		Cleanup:

			if (GotoCleanup)
			{
				// TODO: rollback adventure buildout if necessary
			}

			Globals.Directory.SetCurrentDirectory(workDir);
		}

		/// <summary></summary>
		public virtual void CreateCustomFiles()
		{
			Globals.Directory.CreateDirectory(Constants.AdventuresDir + @"\" + AdventureName + @"\Framework\Plugin");

			Globals.Directory.CreateDirectory(Constants.AdventuresDir + @"\" + AdventureName + @"\Game\Plugin");

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\ChangeLog.txt", ReplaceMacros(ChangeLogText));

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Program.cs", ReplaceMacros(ProgramCsText));

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\" + AdventureName + @".csproj", ReplaceMacros(AdventureCsprojText));

			var fileNames = new string[] { "IPluginClassMappings.cs", "IPluginConstants.cs", "IPluginGlobals.cs" };

			for (var i = 0; i < fileNames.Length; i++)
			{
				Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Framework\Plugin\" + fileNames[i], ReplaceMacros(IPluginCsText[i]));
			}

			fileNames = new string[] { "PluginClassMappings.cs", "PluginConstants.cs", "PluginContext.cs", "PluginGlobals.cs" };

			for (var i = 0; i < fileNames.Length; i++)
			{
				Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Game\Plugin\" + fileNames[i], ReplaceMacros(PluginCsText[i]));
			}

			Globals.File.WriteAllText(Constants.AdventuresDir + @"\" + AdventureName + @"\Game\Engine.cs", ReplaceMacros(EngineCsText));

			fileNames = new string[] { "Artifact.cs", "Effect.cs", "GameState.cs", "Hint.cs", "Module.cs", "Monster.cs", "Room.cs", "Trigger.cs", "Script.cs" };

			for (var i = 0; i < fileNames.Length; i++)
			{
				IncludeInterface = (i == 2);

				ParentClassFileName = @"..\Eamon\Game\" + fileNames[i];

				CreateCustomClassFile();
			}
		}

		/// <summary></summary>
		public virtual void AddProjectToSolution()
		{
			var result = RetCode.Failure;

			gOut.Print("{0}", Globals.LineSep);

			gOut.WriteLine();

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
					process.StartInfo.Arguments = string.Format("sln Eamon.Desktop.sln add {0}", projName);
					process.StartInfo.WorkingDirectory = string.Format("..{0}..", Globals.Path.DirectorySeparatorChar);

					gOut.Write("Adding {0} project ... ", Globals.Path.GetFileNameWithoutExtension(projName));

					process.Start();

					result = process.WaitForExit(120000) && process.ExitCode == 0 ? RetCode.Success : RetCode.Failure;

					try { process.Kill(); } catch (Exception) { }

					if (result == RetCode.Success)
					{
						gOut.WriteLine("succeeded.");
					}
					else
					{
						gOut.WriteLine("failed.");
					}
				}
			}
			catch (Exception ex)
			{
				gOut.WriteLine(ex.ToString());

				result = RetCode.Failure;
			}

			if (result == RetCode.Failure)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("The adventure was not created.");

				GotoCleanup = true;
			}
		}

		public AddCustomAdventureMenu()
		{

		}
	}
}
