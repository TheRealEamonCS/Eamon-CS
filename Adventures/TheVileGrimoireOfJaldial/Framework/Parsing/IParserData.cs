
// IParserData.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework.Parsing
{
	public interface IParserData : EamonRT.Framework.Parsing.IParserData
	{
		Eamon.Framework.IArtifact DecorationArtifact { get; set; }
	}
}
