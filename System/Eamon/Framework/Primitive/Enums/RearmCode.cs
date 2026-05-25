
// RearmCode.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
    public enum RearmCode : long
    {
        None = 0,       // Never voluntarily switches weapons while armed. Fully backward compatible.

        BestForRange,   // Switches to weapon whose optimal band best fits current range to focus target.

        Strongest,      // Switches to highest average damage weapon in reach regardless of range.

        Random,         // Switches to a random available weapon in reach.

		User1,

		User2,

		User3
	}
}
