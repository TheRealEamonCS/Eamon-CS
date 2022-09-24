
// PluginContext.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;

namespace Eamon.Game.Plugin
{
	public static class PluginContext
	{
		public static IPluginConstants Constants { get; set; }

		public static IPluginClassMappings ClassMappings { get; set; }

		public static IPluginGlobals Globals { get; set; }

		public static ITextWriter gOut 
		{
			get 
			{
				return Globals?.Out;
			}
		}

		public static IEngine gEngine 
		{
			get 
			{
				return Globals?.Engine;
			}
		}

		public static IRecordDb<IRoom> gRDB 
		{
			get 
			{
				return Globals?.RDB;
			}
		}

		public static IRecordDb<IArtifact> gADB 
		{
			get 
			{
				return Globals?.ADB;
			}
		}

		public static IRecordDb<IEffect> gEDB 
		{
			get 
			{
				return Globals?.EDB;
			}
		}

		public static IRecordDb<IMonster> gMDB 
		{
			get
			{
				return Globals?.MDB;
			}
		}
	}
}
