
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Eamon;
using Eamon.Framework.Portability;
using static Eamon.Game.Plugin.ContextStack;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM
{
	public class Program
	{
		public static string WorkDir { get; set; }

		public static string[] NextArgs { get; set; }

		public static void ExecutePlugin(string[] args, bool enableStdio = true)
		{
			Debug.Assert(args != null);

			Debug.Assert(args.Length > 1);

			Debug.Assert(args[0].Equals("-pfn", StringComparison.OrdinalIgnoreCase));

			var pluginFileName = Path.GetFileNameWithoutExtension(args[1]);

			var plugin = LoadAssembly(Path.Combine(WorkDir, args[1]));

			Debug.Assert(plugin != null);

			var typeName = string.Format("{0}.Program", pluginFileName);

			var type = plugin.GetType(typeName);

			Debug.Assert(type != null);

			var program = (IProgram)Activator.CreateInstance(type);

			Debug.Assert(program != null);

			program.EnableStdio = enableStdio;

			program.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

			program.Main(args.Skip(2).ToArray());
		}

		public static Assembly LoadAssembly(string assemblyPath)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(assemblyPath));

			var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);

			Debug.Assert(assembly != null);

			foreach (var dependency in assembly.GetReferencedAssemblies())
			{
				var dependencyPath = Path.Combine(WorkDir, dependency.Name + ".dll");

				if (File.Exists(dependencyPath))
				{
					LoadAssembly(dependencyPath);
				}
			}

			return assembly;
		}

		public static void LoadPortabilityClassMappings(IDictionary<Type, Type> classMappings)
		{
			Debug.Assert(classMappings != null);

			classMappings[typeof(ITextReader)] = typeof(Game.Portability.TextReader);

			classMappings[typeof(ITextWriter)] = typeof(Game.Portability.TextWriter);

			classMappings[typeof(IMutex)] = typeof(Game.Portability.Mutex);

			classMappings[typeof(ITransferProtocol)] = typeof(Game.Portability.TransferProtocol);

			classMappings[typeof(IDirectory)] = typeof(Game.Portability.Directory);

			classMappings[typeof(IFile)] = typeof(Game.Portability.File);

			classMappings[typeof(IPath)] = typeof(Game.Portability.Path);

			classMappings[typeof(ISharpSerializer)] = typeof(Game.Portability.SharpSerializer);

			classMappings[typeof(IThread)] = typeof(Game.Portability.Thread);
		}

		public static void Main(string[] args)
		{
			RetCode rc;

			try
			{
				rc = RetCode.Success;

				WorkDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

				PushEngine();

				gEngine.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

				gEngine.ResolvePortabilityClassMappings();

				if (args == null || args.Length < 2 || !args[0].Equals("-pfn", StringComparison.OrdinalIgnoreCase))
				{
					rc = RetCode.InvalidArg;

					gEngine.Error.WriteLine("{0}Usage: dotnet EamonPM.WindowsUnix.dll -pfn PluginFileName [PluginArgs]", Environment.NewLine);

					goto Cleanup;
				}

				try
				{
					while (true)
					{
						if (args == null || args.Length < 2 || !args[0].Equals("-pfn", StringComparison.OrdinalIgnoreCase))
						{
							goto Cleanup;
						}

						var systemBinDir = string.Format("{0}System{0}Bin", Path.DirectorySeparatorChar);

						var currWorkDir = Directory.GetCurrentDirectory();

						// if current working directory invalid, bail out

						if (!currWorkDir.EndsWith(systemBinDir) || currWorkDir.Length <= systemBinDir.Length || !Directory.Exists(gEngine.AdventuresDir.Replace('\\', Path.DirectorySeparatorChar)))
						{
							rc = RetCode.Failure;

							gEngine.Error.WriteLine("{0}Usage: to run Eamon CS change your working directory to System{1}Bin", Environment.NewLine, Path.DirectorySeparatorChar);

							goto Cleanup;
						}

						ExecutePlugin(args);

						args = NextArgs;

						NextArgs = null;
					}
				}
				catch (Exception ex)
				{
					rc = RetCode.Failure;

					gEngine.HandleException
					(
						ex,
						gEngine.StackTraceFile,
						string.Format("{0}Error: Caught fatal exception; terminating program.", Environment.NewLine)
					);

					goto Cleanup;
				}

			Cleanup:

				if (rc != RetCode.Success)
				{
					gEngine.Error.WriteLine("{0}{1}", Environment.NewLine, gEngine.EnableScreenReaderMode ? "" : new string('-', (int)gEngine.RightMargin));

					gEngine.Error.Write("{0}Press any key to continue: ", Environment.NewLine);

					gEngine.In.ReadKey(true);

					gEngine.Error.WriteLine();

					gEngine.Thread.Sleep(150);
				}

				gEngine.Out.CursorVisible = true;
			}
			catch (Exception)
			{
				rc = RetCode.Failure;

				// do nothing
			}
			finally
			{
				PopEngine();
			}

			Environment.Exit(rc == RetCode.Success ? 0 : -1);
		}
	}
}
