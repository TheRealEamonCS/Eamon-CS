
// ErrorState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class ErrorState : State, IErrorState
	{
		public virtual long ErrorCode { get; set; }

		public virtual string ErrorMessage { get; set; }

		public override void Execute()
		{
			Debug.Assert(false, ErrorMessage);
		}

		public ErrorState()
		{
			Uid = 4;

			Name = "ErrorState";

			ErrorCode = 1;

			ErrorMessage = "ErrorState: Unknown message";
		}
	}
}
