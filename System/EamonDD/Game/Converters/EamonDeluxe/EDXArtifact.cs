
// EDXArtifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;

namespace EamonDD.Game.Converters.EamonDeluxe
{
	/// <summary>
	/// </summary>
	public class EDXArtifact
	{
		[Layout(35)]
		public string _artname;

		public string _artdesc;

		[Layout(2)]
		public short _ad1;

		[Layout(2)]
		public short _ad2;

		[Layout(2)]
		public short _ad3;

		[Layout(2)]
		public short _ad4;

		[Layout(2)]
		public short _ad5;

		[Layout(2)]
		public short _ad6;

		[Layout(2)]
		public short _ad7;

		[Layout(2)]
		public short _ad8;
	}
}
