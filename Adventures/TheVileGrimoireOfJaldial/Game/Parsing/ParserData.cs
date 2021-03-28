
// ParserData.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Parsing;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Parsing
{
	[ClassMappings(typeof(IParserData))]
	public class ParserData : EamonRT.Game.Parsing.ParserData, Framework.Parsing.IParserData
	{
		public virtual Eamon.Framework.IArtifact DecorationArtifact { get; set; }
	}
}
