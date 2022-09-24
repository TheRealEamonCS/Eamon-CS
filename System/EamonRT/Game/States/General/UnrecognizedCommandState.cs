
// UnrecognizedCommandState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class UnrecognizedCommandState : State, IUnrecognizedCommandState
	{
		public bool _newSeen;

		/// <summary></summary>
		public virtual IList<ICommand> EnabledCommandList { get; set; }

		/// <summary></summary>
		public virtual IMonster CharMonster { get; set; }

		/// <summary></summary>
		public virtual bool NewSeen
		{
			get
			{
				return _newSeen;
			}

			set
			{
				_newSeen = value;
			}
		}

		public override void Execute()
		{
			NewSeen = false;

			CharMonster = gCharMonster;

			Debug.Assert(CharMonster != null);

			EnabledCommandList = Globals.CommandList.Where(x => x.IsEnabled(CharMonster) && x.IsListed).ToList();

			PrintCommands(EnabledCommandList, CommandType.Movement, ref _newSeen);

			PrintCommands(EnabledCommandList, CommandType.Manipulation, ref _newSeen);

			PrintCommands(EnabledCommandList, CommandType.Interactive, ref _newSeen);

			PrintCommands(EnabledCommandList, CommandType.Miscellaneous, ref _newSeen);

			if (NewSeen)
			{
				PrintNewCommandSeen();
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

			Globals.NextState = NextState;
		}

		public UnrecognizedCommandState()
		{
			Name = "UnrecognizedCommandState";
		}
	}
}
