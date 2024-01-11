
// AddCustomAdventureMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

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

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
";

		/// <summary></summary>
		public virtual string[] IPluginCsText { get; set; } = new string[]
		{
@"
// IEngine.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

namespace YourAdventureName.Framework.Plugin
{
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{

	}
}
"
		};

		/// <summary></summary>
		public virtual string[] PluginCsText { get; set; } = new string[]
		{
@"
// Engine.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

using System.Reflection;
using Eamon;
using static YourAdventureName.Game.Plugin.Globals;

namespace YourAdventureName.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
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
// Globals.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved.

using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;

namespace YourAdventureName.Game.Plugin
{
	public static class Globals
	{
		public static Framework.Plugin.IEngine gEngine
		{
			get
			{
				return (Framework.Plugin.IEngine)EamonRT.Game.Plugin.Globals.gEngine;
			}
			set
			{
				EamonRT.Game.Plugin.Globals.gEngine = value;
			}
		}

		public static ITextWriter gOut
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gOut;
			}
		}

		public static IDatabase gDatabase
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gDatabase;
			}
		}

		public static IRecordDb<Eamon.Framework.IRoom> gRDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IRoom>)EamonRT.Game.Plugin.Globals.gRDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IArtifact> gADB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IArtifact>)EamonRT.Game.Plugin.Globals.gADB;
			}
		}

		public static IRecordDb<Eamon.Framework.IEffect> gEDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IEffect>)EamonRT.Game.Plugin.Globals.gEDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IMonster> gMDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IMonster>)EamonRT.Game.Plugin.Globals.gMDB;
			}
		}

		public static EamonRT.Framework.Parsing.ISentenceParser gSentenceParser
		{
			get
			{
				return (EamonRT.Framework.Parsing.ISentenceParser)EamonRT.Game.Plugin.Globals.gSentenceParser;
			}
		}

		public static EamonRT.Framework.Parsing.ICommandParser gCommandParser
		{
			get
			{
				return (EamonRT.Framework.Parsing.ICommandParser)EamonRT.Game.Plugin.Globals.gCommandParser;
			}
		}

		public static Framework.IGameState gGameState
		{
			get
			{
				return (Framework.IGameState)EamonRT.Game.Plugin.Globals.gGameState;
			}
		}

		public static Eamon.Framework.ICharacter gCharacter
		{
			get
			{
				return (Eamon.Framework.ICharacter)EamonRT.Game.Plugin.Globals.gCharacter;
			}
		}

		public static Eamon.Framework.IMonster gCharMonster
		{
			get
			{
				return (Eamon.Framework.IMonster)EamonRT.Game.Plugin.Globals.gCharMonster;
			}
		}

		public static Eamon.Framework.IRoom gCharRoom
		{
			get
			{
				return (Eamon.Framework.IRoom)EamonRT.Game.Plugin.Globals.gCharRoom;
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
"
		};

		/// <summary></summary>
		public virtual string ChangeLogText { get; set; } =
@"
==================================================================================================================================
ChangeLog: YourAdventureName
==================================================================================================================================

Date            Version            Who            Notes
----------------------------------------------------------------------------------------------------------------------------------
20XXXXXX        2.2.0              YourAuthorInitials             Code complete 2.2.0
";

		/// <summary></summary>
		public virtual string AdventureCsprojText { get; set; } =
@"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <Version>2.2.0.0</Version>
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

			var workDir = gEngine.Directory.GetCurrentDirectory();

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

			gEngine.Directory.SetCurrentDirectory(workDir);
		}

		/// <summary></summary>
		public virtual void CreateCustomFiles()
		{
			gEngine.Directory.CreateDirectory(gEngine.AdventuresDir + @"\" + AdventureName + @"\Framework\Plugin");

			gEngine.Directory.CreateDirectory(gEngine.AdventuresDir + @"\" + AdventureName + @"\Game\Plugin");

			gEngine.File.WriteAllText(gEngine.AdventuresDir + @"\" + AdventureName + @"\ChangeLog.txt", ReplaceMacros(ChangeLogText));

			gEngine.File.WriteAllText(gEngine.AdventuresDir + @"\" + AdventureName + @"\Program.cs", ReplaceMacros(ProgramCsText));

			gEngine.File.WriteAllText(gEngine.AdventuresDir + @"\" + AdventureName + @"\" + AdventureName + @".csproj", ReplaceMacros(AdventureCsprojText));

			var fileNames = new string[] { "IEngine.cs" };

			for (var i = 0; i < fileNames.Length; i++)
			{
				gEngine.File.WriteAllText(gEngine.AdventuresDir + @"\" + AdventureName + @"\Framework\Plugin\" + fileNames[i], ReplaceMacros(IPluginCsText[i]));
			}

			fileNames = new string[] { "Engine.cs", "Globals.cs" };

			for (var i = 0; i < fileNames.Length; i++)
			{
				gEngine.File.WriteAllText(gEngine.AdventuresDir + @"\" + AdventureName + @"\Game\Plugin\" + fileNames[i], ReplaceMacros(PluginCsText[i]));
			}

			fileNames = new string[] { "Artifact.cs", "Effect.cs", "GameState.cs", "Hint.cs", "Module.cs", "Monster.cs", "Room.cs" };

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

			gOut.Print("{0}", gEngine.LineSep);

			gOut.WriteLine();

			var projName = gEngine.Path.GetFullPath(gEngine.Path.Combine(gEngine.AdventuresDir + @"\" + AdventureName, AdventureName + ".csproj"));

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
					process.StartInfo.Arguments = string.Format("sln Eamon.Desktop.sln add \"{0}\"", projName);
					process.StartInfo.WorkingDirectory = string.Format("..{0}..", gEngine.Path.DirectorySeparatorChar);

					gOut.Write("Adding {0} project ... ", gEngine.Path.GetFileNameWithoutExtension(projName));

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
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("The adventure was not created.");

				GotoCleanup = true;
			}
		}

		public AddCustomAdventureMenu()
		{

		}
	}
}
