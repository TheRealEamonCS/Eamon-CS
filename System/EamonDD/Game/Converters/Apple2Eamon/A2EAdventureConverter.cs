
// A2EAdventureConverter.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Converters.Apple2Eamon
{
	/// <summary>
	/// </summary>
	public class A2EAdventureConverter
	{
		public virtual A2EAdventure Adventure { get; set; }

		public virtual string AdventureFolderPath { get; set; }

		public virtual string ErrorMessage { get; set; }

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Full credit:  https://github.com/malfunct/eamon.net/blob/master/AppleTextFileConvertor/Program.cs
		/// </remarks>
		public virtual byte[] ConvertApple2ByteBuffer(byte[] buffer)
		{
			for (var i = 0; i < buffer.Length; i++)
			{
				if (buffer[i] == 0)
				{
					buffer[i] = (byte)' ';
				}
				else if (buffer[i] == 141)
				{
					buffer[i] = (byte)'\n';
				}
				else if (buffer[i] >= 160)
				{
					buffer[i] = (byte)(buffer[i] - 128);
				}
			}

			return buffer;
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Full credit:  referencing DUNGEON LIST Z by John Nelson and Tom Zuchowski
		/// </remarks>
		public virtual bool LoadAdventure()
		{
			var result = true;

			try
			{

			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;

				result = false;
			}

			return result;
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Full credit:  referencing DUNGEON LIST Z by John Nelson and Tom Zuchowski
		/// </remarks>
		public virtual bool ConvertAdventure()
		{
			var result = true;

			try
			{

			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;

				result = false;
			}

			return result;
		}

		public A2EAdventureConverter()
		{

		}
	}
}
