
// PluginClassMappings.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Plugin
{
	public class PluginClassMappings : IPluginClassMappings
	{
		public virtual IDictionary<Type, Type> ClassMappingsDictionary { get; set; } = new Dictionary<Type, Type>();

		public virtual ITextReader In { get; set; }

		public virtual ITextWriter Out { get; set; }

		public virtual ITextWriter Error { get; set; }

		public virtual IMutex Mutex { get; set; }

		public virtual ITransferProtocol TransferProtocol { get; set; }

		public virtual IDirectory Directory { get; set; }

		public virtual IFile File { get; set; }

		public virtual IPath Path { get; set; }

		public virtual ISharpSerializer SharpSerializer { get; set; }

		public virtual IThread Thread { get; set; }

		public virtual MemoryStream CloneStream { get; set; } = new MemoryStream();

		public virtual string WorkDir { get; set; } = "";

		public virtual string FilePrefix { get; set; } = "";

		public virtual long RulesetVersion
		{
			get
			{
				return RulesetVersions != null && RvStackTop >= 0 && RvStackTop < RulesetVersions.Length ? RulesetVersions[RvStackTop] : 0;
			}
		}

		public virtual bool EnableGameOverrides
		{
			get
			{
				return SharpSerializer == null || SharpSerializer.IsActive == false;
			}
		}

		public virtual bool EnableStdio { get; set; } = true;

		public virtual bool IgnoreMutex { get; set; }

		public virtual bool DisableValidation { get; set; }
		
		public virtual bool RunGameEditor { get; set; }

		public virtual bool DeleteGameStateFromMainHall { get; set; }

		public virtual bool ConvertDatafileToMscorlib { get; set; }

		public virtual Action<IDictionary<Type, Type>> LoadPortabilityClassMappings { get; set; }

		/// <summary></summary>
		public virtual long[] RulesetVersions { get; set; } = new long[Constants.NumRulesetVersions];

		/// <summary></summary>
		public virtual long RvStackTop { get; set; } = -1;

		public virtual void HandleException(Exception ex, string stackTraceFile, string errorMessage)
		{
			Debug.Assert(ex != null && !string.IsNullOrWhiteSpace(stackTraceFile) && !string.IsNullOrWhiteSpace(errorMessage));

			stackTraceFile = Path.Combine(".", stackTraceFile);

			try
			{
				File.WriteAllText(stackTraceFile, ex.ToString());
			}
			catch (Exception)
			{
				// do nothing
			}

			var errorBuf = new StringBuilder(Constants.BufSize);

			var exceptionMessage = !string.IsNullOrWhiteSpace(ex.Message) ? ex.Message.Trim() : null;

			errorBuf.AppendFormat("{0}Error: {1}{2}{3}.",
				Environment.NewLine,
				exceptionMessage != null ? exceptionMessage : "",
				exceptionMessage != null && exceptionMessage.EndsWith(".") ? " " : exceptionMessage != null ? ". " : "",
				string.Format("See stack trace file [{0}] for more details", stackTraceFile));

			errorBuf.AppendFormat("{0}", errorMessage);

			Debug.Assert(Error != null);

			Error.WriteLine(errorBuf);
		}

		public virtual void ResolvePortabilityClassMappings()
		{
			Debug.Assert(LoadPortabilityClassMappings != null);

			LoadPortabilityClassMappings(ClassMappingsDictionary);

			In = CreateInstance<ITextReader>(x =>
			{
				x.EnableInput = EnableStdio;
			});

			Debug.Assert(In != null);

			Out = CreateInstance<ITextWriter>(x =>
			{
				x.EnableOutput = EnableStdio;
			});

			Debug.Assert(Out != null);

			Error = CreateInstance<ITextWriter>(x =>
			{
				x.EnableOutput = EnableStdio;

				x.Stdout = false;
			});

			Debug.Assert(Error != null);

			Mutex = CreateInstance<IMutex>();

			Debug.Assert(Mutex != null);

			TransferProtocol = CreateInstance<ITransferProtocol>();

			Debug.Assert(TransferProtocol != null);

			Directory = CreateInstance<IDirectory>();

			Debug.Assert(Directory != null);

			File = CreateInstance<IFile>();

			Debug.Assert(File != null);

			Path = CreateInstance<IPath>();

			Debug.Assert(Path != null);

			SharpSerializer = CreateInstance<ISharpSerializer>();

			Debug.Assert(SharpSerializer != null);

			Thread = CreateInstance<IThread>();

			Debug.Assert(Thread != null);
		}

		public virtual void ProcessArgv(string[] args)
		{
			if (args == null)
			{
				// PrintError

				goto Cleanup;
			}

			for (var i = 0; i < args.Length; i++)
			{
				if (args[i].Equals("--workingDirectory", StringComparison.OrdinalIgnoreCase) || args[i].Equals("-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < args.Length)
					{
						WorkDir = args[i].Trim();

						var regex = new Regex(Constants.ValidWorkDirRegexPattern);

						if (WorkDir.Equals("NONE", StringComparison.OrdinalIgnoreCase) || !regex.IsMatch(WorkDir))
						{
							WorkDir = Constants.DefaultWorkDir;
						}
					}
				}
				else if (args[i].Equals("--filePrefix", StringComparison.OrdinalIgnoreCase) || args[i].Equals("-fp", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < args.Length)
					{
						FilePrefix = args[i].Trim();
					}
				}
				else if (args[i].Equals("--ignoreMutex", StringComparison.OrdinalIgnoreCase) || args[i].Equals("-im", StringComparison.OrdinalIgnoreCase))
				{
					IgnoreMutex = true;
				}
				else if (args[i].Equals("--disableValidation", StringComparison.OrdinalIgnoreCase) || args[i].Equals("-dv", StringComparison.OrdinalIgnoreCase))
				{
					DisableValidation = true;
				}
				else if (args[i].Equals("--runGameEditor", StringComparison.OrdinalIgnoreCase) || args[i].Equals("-rge", StringComparison.OrdinalIgnoreCase))
				{
					RunGameEditor = true;
				}
			}

		Cleanup:

			;
		}

		public virtual RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());

			return rc;
		}

		public virtual RetCode LoadPluginClassMappings01(Assembly plugin)
		{
			RetCode rc;

			if (plugin == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var types = plugin.GetTypes().Where(t => t.GetCustomAttributes(typeof(ClassMappingsAttribute), true).FirstOrDefault() != null);

			foreach (var t in types)
			{
				var attributes = t.GetCustomAttributes(typeof(ClassMappingsAttribute), true);

				Debug.Assert(attributes != null);

				foreach (ClassMappingsAttribute a in attributes)
				{
					var ift = a.InterfaceType;

					if (ift == null)
					{
						var ifaces = t.GetInterfaces();

						ift = ifaces != null ? ifaces.FirstOrDefault(t01 => t01.Name.Equals(string.Format("I{0}", t.Name), StringComparison.Ordinal)) : null;
					}

					if (ift == null)
					{
						var errorBuf = new StringBuilder(Constants.BufSize);

						errorBuf.AppendFormat("{0}Error: Couldn't find ClassMappings interface for class [{1}].", Environment.NewLine, t.Name);

						errorBuf.AppendFormat("{0}Error: LoadPluginClassMappings01 function call failed for plugin [{1}].", Environment.NewLine, plugin.GetShortName());

						rc = RetCode.Failure;

						Error.WriteLine(errorBuf);

						goto Cleanup;
					}

					ClassMappingsDictionary[ift] = t;
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode PushRulesetVersion(long rulesetVersion)
		{
			RetCode rc;

			rc = RetCode.Success;

			if (RulesetVersions == null || RvStackTop + 1 >= RulesetVersions.Length)
			{
				rc = RetCode.IsFull;

				goto Cleanup;
			}

			RulesetVersions[++RvStackTop] = rulesetVersion;

		Cleanup:

			return rc;
		}

		public virtual RetCode PopRulesetVersion()
		{
			RetCode rc;

			rc = RetCode.Success;

			if (RulesetVersions == null || RvStackTop < 0)
			{
				rc = RetCode.IsEmpty;

				goto Cleanup;
			}

			RulesetVersions[RvStackTop--] = 0;

		Cleanup:

			return rc;
		}

		public virtual RetCode ClearRvStack()
		{
			RetCode rc;

			rc = RetCode.Success;

			while (RvStackTop >= 0)
			{
				rc = PopRulesetVersion();

				if (rc != RetCode.Success)
				{
					break;
				}
			}

			return rc;
		}

		public virtual RetCode GetRvStackTop(ref long rvStackTop)
		{
			RetCode rc;

			rc = RetCode.Success;

			rvStackTop = RvStackTop;

			return rc;
		}

		public virtual RetCode GetRvStackSize(ref long rvStackSize)
		{
			RetCode rc;

			rc = RetCode.Success;

			rvStackSize = RulesetVersions != null ? RulesetVersions.Length : 0;

			return rc;
		}

		public virtual T CreateInstance<T>(Type ifaceType, Action<T> initialize = null) where T : class
		{
			T result = default(T);

			if (ifaceType == null)
			{
				// PrintError

				goto Cleanup;
			}

			Type mappedType;

			if (ClassMappingsDictionary.TryGetValue(ifaceType, out mappedType))
			{
				result = Activator.CreateInstance(mappedType) as T;

				if (result != null && initialize != null)
				{
					initialize(result);
				}
			}

		Cleanup:

			return result;
		}

		public virtual T CreateInstance<T>(Action<T> initialize = null) where T : class
		{
			return CreateInstance(typeof(T), initialize);
		}

		public virtual T CloneInstance<T>(T source) where T : class
		{
			T result = default(T);

			if (source == null)
			{
				// PrintError

				goto Cleanup;
			}

			try
			{
				CloneStream.SetLength(0);

				SharpSerializer.Serialize(source, CloneStream, true);

				CloneStream.Position = 0;

				result = SharpSerializer.Deserialize<T>(CloneStream, true);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				var igb = result as IGameBase;

				if (igb != null)
				{
					igb.SetParentReferences();
				}
			}

		Cleanup:

			return result;
		}

		public virtual bool CompareInstances<T>(T object1, T object2) where T : class
		{
			var result = false;

			if (object1 == null || object2 == null)
			{
				// PrintError

				goto Cleanup;
			}

			byte[] object1Bytes = null;

			byte[] object2Bytes = null;

			CloneStream.SetLength(0);

			SharpSerializer.Serialize(object1, CloneStream, true);

			object1Bytes = CloneStream.ToArray();

			Debug.Assert(object1Bytes != null);

			CloneStream.SetLength(0);

			SharpSerializer.Serialize(object2, CloneStream, true);

			object2Bytes = CloneStream.ToArray();

			Debug.Assert(object2Bytes != null);

			result = object1Bytes.SequenceEqual(object2Bytes);

		Cleanup:

			return result;
		}

		public virtual bool IsRulesetVersion(params long[] versions)
		{
			var result = false;

			if (versions == null)
			{
				// PrintError

				goto Cleanup;
			}

			result = versions.Contains(RulesetVersion);

		Cleanup:

			return result;
		}

		public virtual string GetPrefixedFileName(string fileName)
		{
			string result = null;

			if (string.IsNullOrWhiteSpace(fileName))
			{
				// PrintError

				goto Cleanup;
			}

			var path = Path.GetDirectoryName(fileName);

			var fileName01 = Path.GetFileName(fileName);

			result = Path.Combine(path, string.Format("{0}{1}", !string.IsNullOrWhiteSpace(FilePrefix) ? FilePrefix + "_" : "", fileName01));

		Cleanup:

			return result;
		}

		public virtual void ConvertDatafileFromXmlToDat(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				// PrintError

				goto Cleanup;
			}

			var xmlFileName = Path.ChangeExtension(fileName, ".XML");

			if (File.Exists(xmlFileName))
			{
				var contents = File.ReadAllText(xmlFileName);

				File.WriteAllText(fileName, contents);

				File.Delete(xmlFileName);
			}

		Cleanup:

			;
		}
	}
}
