
// TravelCode.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
    public enum TravelCode : long
    {
        None = 0,       // Never repositions. Movement system inactive.

        Close,          // Always minimizes distance to focus target (ignores optimal band).

        Maintain,       // Stays within optimal band; approaches if too far, retreats if too close.

        Open,           // Always maximizes distance from focus target (ignores optimal band).

        User1,

        User2,

        User3
    }
}
