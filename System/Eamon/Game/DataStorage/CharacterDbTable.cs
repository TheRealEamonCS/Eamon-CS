
// CharacterDbTable.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.DataStorage.Generic;

namespace Eamon.Game.DataStorage
{
	[ClassMappings(typeof(IDbTable<ICharacter>))]
	public class CharacterDbTable : DbTable<ICharacter>
	{

	}
}
