
// ComponentImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Components
{
	[ClassMappings]
	public class ComponentImpl : EamonRT.Game.Components.ComponentImpl, IComponentImpl
	{
		public override void PrintSpellOverloadsBrain(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s) && spell != null);

			gOut.Print("The strain of attempting to cast {0} overloads your brain and you forget it completely.", spell.Name);
		}
	}
}
