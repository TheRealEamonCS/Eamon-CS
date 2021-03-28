
// UnrecognizedCommandState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
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
			RetCode rc;

			NewSeen = false;

			CharMonster = gCharMonster;

			Debug.Assert(CharMonster != null);

			EnabledCommandList = Globals.CommandList.Where(x => x.IsEnabled(CharMonster) && x.IsListed).ToList();

			gOut.Print("Movement Commands:");

			Globals.Buf.Clear();

			rc = gEngine.BuildCommandList(EnabledCommandList, CommandType.Movement, Globals.Buf, ref _newSeen);
			
			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);

			gOut.Print("Artifact Manipulation:");

			Globals.Buf.Clear();

			rc = gEngine.BuildCommandList(EnabledCommandList, CommandType.Manipulation, Globals.Buf, ref _newSeen);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);

			gOut.Print("Interactive:");

			Globals.Buf.Clear();

			rc = gEngine.BuildCommandList(EnabledCommandList, CommandType.Interactive, Globals.Buf, ref _newSeen);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);

			gOut.Print("Miscellaneous:");

			Globals.Buf.Clear();

			rc = gEngine.BuildCommandList(EnabledCommandList, CommandType.Miscellaneous, Globals.Buf, ref _newSeen);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);

			if (NewSeen)
			{
				gOut.Print("(*) New Command");
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

			Globals.NextState = NextState;
		}

		public UnrecognizedCommandState()
		{
			Uid = 7;

			Name = "UnrecognizedCommandState";
		}
	}
}
