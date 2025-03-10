﻿
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Text;
using Eamon.Framework.Primitive.Classes;

namespace ARuncibleCargo.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		/// <summary></summary>
		new StringBuilder Buf01 { get; set; }

		/// <summary></summary>
		string SnapshotFileName { get; }

		/// <summary></summary>
		IList<IArtifactLinkage> DoubleDoorList { get; set; }
	}
}
