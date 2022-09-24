
// PluginContext.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonDD.Framework;
using EamonDD.Framework.Plugin;

namespace EamonDD.Game.Plugin
{
	public static class PluginContext
	{
		public static IPluginConstants Constants
		{
			get
			{
				return (IPluginConstants)Eamon.Game.Plugin.PluginContext.Constants;
			}
			set
			{
				Eamon.Game.Plugin.PluginContext.Constants = value;
			}
		}

		public static IPluginClassMappings ClassMappings
		{
			get
			{
				return (IPluginClassMappings)Eamon.Game.Plugin.PluginContext.ClassMappings; 
			}
			set
			{
				Eamon.Game.Plugin.PluginContext.ClassMappings = value;
			}
		}

		public static IPluginGlobals Globals
		{
			get
			{
				return (IPluginGlobals)Eamon.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				Eamon.Game.Plugin.PluginContext.Globals = value;
			}
		}

		public static ITextWriter gOut 
		{
			get 
			{
				return Eamon.Game.Plugin.PluginContext.gOut;
			}
		}

		public static IEngine gEngine 
		{
			get 
			{
				return (IEngine)Eamon.Game.Plugin.PluginContext.gEngine;
			}
		}

		public static IRecordDb<Eamon.Framework.IRoom> gRDB 
		{
			get 
			{
				return Eamon.Game.Plugin.PluginContext.gRDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IArtifact> gADB 
		{
			get 
			{
				return Eamon.Game.Plugin.PluginContext.gADB;
			}
		}

		public static IRecordDb<Eamon.Framework.IEffect> gEDB 
		{
			get 
			{
				return Eamon.Game.Plugin.PluginContext.gEDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IMonster> gMDB 
		{
			get 
			{
				return Eamon.Game.Plugin.PluginContext.gMDB;
			}
		}
	}
}
