
// ArtifactDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IArtifact>))]
	public class ArtifactDb : IRecordDb<IArtifact>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual IArtifact this[long uid]
		{
			get
			{
				return gDatabase?.FindArtifact(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					gDatabase?.RemoveArtifact(uid);
				}

				if (value != null && value.Uid == uid)
				{
					gDatabase?.AddArtifact(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IArtifact> Records 
		{ 
			get
			{
				return gDatabase?.ArtifactTable?.Records;
			}
		}

		public ArtifactDb()
		{
			CopyAddedRecord = true;
		}
	}
}
