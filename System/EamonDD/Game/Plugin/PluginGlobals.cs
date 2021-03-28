
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Menus;
using EamonDD.Framework.Menus;
using EamonDD.Framework.Plugin;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Plugin
{
	public class PluginGlobals : Eamon.Game.Plugin.PluginGlobals, IPluginGlobals
	{
		public virtual string[] Argv { get; set; }

		public virtual long WordWrapCurrColumn { get; set; }

		public virtual char WordWrapLastChar { get; set; }

		public virtual string ConfigFileName { get; set; }

		public virtual new Framework.IEngine Engine
		{
			get
			{
				return (Framework.IEngine)base.Engine;
			}

			set
			{
				if (base.Engine != value)
				{
					base.Engine = value;
				}
			}
		}

		public virtual IDdMenu DdMenu { get; set; }

		public virtual IMenu Menu { get; set; }

		public virtual IModule Module { get; set; }

		public virtual IConfig Config { get; set; }

		public virtual bool ConfigsModified { get; set; }

		public virtual bool FilesetsModified { get; set; }

		public virtual bool CharactersModified { get; set; }

		public virtual bool ModulesModified { get; set; }

		public virtual bool RoomsModified { get; set; }

		public virtual bool ArtifactsModified { get; set; }

		public virtual bool EffectsModified { get; set; }

		public virtual bool MonstersModified { get; set; }

		public virtual bool HintsModified { get; set; }

		public virtual bool TriggersModified { get; set; }

		public virtual bool ScriptsModified { get; set; }

		public override void InitSystem()
		{
			base.InitSystem();

			ConfigFileName = "";

			if (RunGameEditor)
			{
				DdMenu = ClassMappings.CreateInstance<IDdMenu>();
			}

			Config = ClassMappings.CreateInstance<IConfig>();
		}
	}
}
