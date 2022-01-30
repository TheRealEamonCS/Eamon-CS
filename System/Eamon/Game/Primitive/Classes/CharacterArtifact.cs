
// CharacterArtifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class CharacterArtifact : ICharacterArtifact
	{
		[ExcludeFromSerialization]
		public virtual ICharacter Parent { get; set; }

		public virtual string Name { get; set; }

		public virtual string Desc { get; set; }

		public virtual bool IsPlural { get; set; }

		public virtual Enums.PluralType PluralType { get; set; }

		public virtual Enums.ArticleType ArticleType { get; set; }

		public virtual long Value { get; set; }

		public virtual long Weight { get; set; }

		public virtual Enums.ArtifactType Type { get; set; }

		public virtual long Field1 { get; set; }

		public virtual long Field2 { get; set; }

		public virtual long Field3 { get; set; }

		public virtual long Field4 { get; set; }

		public virtual long Field5 { get; set; }

		public virtual bool IsActive()
		{
			return !string.IsNullOrWhiteSpace(Name) && !Name.Equals("NONE", StringComparison.OrdinalIgnoreCase);
		}

		public virtual void ClearExtraFields()
		{
			Desc = "";

			Value = 0;

			Weight = 0;

			Type = 0;
		}

		public CharacterArtifact()
		{
			Name = "NONE";

			Desc = "";
		}
	}
}
