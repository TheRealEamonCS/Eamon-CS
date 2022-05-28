
// A2EAdventureConverter.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.IO;
using System.Linq;
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
		/// Full credit:  referencing DUNGEON LIST Z, DUNGEON LIST 7.1 by John Nelson and Tom Zuchowski
		/// </remarks>
		public virtual bool LoadAdventure()
		{
			var result = true;

			try
			{
				var line = "";

				var buffer = new byte[4];

				var tokens = new string[1];

				var nameDatFile = Globals.Path.Combine(AdventureFolderPath, "EAMON.NAME#040000");

				var adventure = new A2EAdventure();

				adventure._aptr = 101;

				adventure._eptr = 201;

				adventure._mptr = 301;

				adventure._type = 4;

				adventure._dlen = 256;

				adventure._rlen = 64;

				adventure._rnlen = 64;

				adventure._alen = 128;

				adventure._mlen = 128;

				using (var nameDatStream = Globals.File.OpenRead(nameDatFile))
				{
					using (var nameDatMemoryStream = new MemoryStream())
					{
						nameDatStream.CopyTo(nameDatMemoryStream);

						buffer = ConvertApple2ByteBuffer(nameDatMemoryStream.ToArray());

						line = System.Text.Encoding.Default.GetString(buffer).Trim();

						tokens = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
					}

					adventure.Name = tokens[0].Trim();

					if (string.IsNullOrWhiteSpace(adventure.Name))
					{
						throw new Exception("Error: IsNullOrWhiteSpace function call returned true for Name.");
					}

					try
					{
						if (!long.TryParse(tokens[1].Trim(), out adventure._nd))
						{
							adventure._nd = 6;
						}
					}
					catch (Exception)
					{
						adventure._nd = 6;
					}

					try
					{
						adventure._ver = tokens[2].Trim();
					}
					catch (Exception)
					{
						adventure._ver = "???";
					}

					if (!string.IsNullOrWhiteSpace(adventure._ver) && adventure._ver.StartsWith("7"))
					{
						adventure._aptr = 201;

						adventure._eptr = 401;

						adventure._mptr = 601;

						adventure._dlen = 242;
					}

					if (adventure.Name.Equals("THE BEGINNERS CAVE", StringComparison.OrdinalIgnoreCase))
					{
						adventure._type = 1;
					}
					else if (adventure.Name.Equals("THE LAIR OF THE MINOTAUR", StringComparison.OrdinalIgnoreCase))
					{
						adventure._type = 2;
					}
					else if (adventure.Name.Equals("THE CAVE OF THE MIND", StringComparison.OrdinalIgnoreCase))
					{
						adventure._type = 3;

						adventure._rnlen = 32;

						adventure._alen = 0;

						adventure._mlen = 0;
					}
					else if (adventure.Name.Equals("THE CAVES OF TREASURE ISLAND", StringComparison.OrdinalIgnoreCase))
					{
						adventure._type = 1;

						adventure._rnlen = 32;

						adventure._alen = 0;

						adventure._mlen = 0;
					}
				}

				var descDatFile = Globals.Path.Combine(AdventureFolderPath, "EAMON.DESC#040000");

				using (var descDatStream = Globals.File.OpenRead(descDatFile))
				{
					buffer = new byte[adventure._dlen];

					descDatStream.Read(buffer, 0, (int)adventure._dlen);

					buffer = ConvertApple2ByteBuffer(buffer);

					line = System.Text.Encoding.Default.GetString(buffer).Trim();

					tokens = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

					if (!long.TryParse(tokens[0].Trim(), out adventure._nr))
					{
						throw new Exception("Error: TryParse function call failed for _nr.");
					}

					if (!long.TryParse(tokens[1].Trim(), out adventure._na))
					{
						throw new Exception("Error: TryParse function call failed for _na.");
					}

					if (!long.TryParse(tokens[2].Trim(), out adventure._ne))
					{
						throw new Exception("Error: TryParse function call failed for _ne.");
					}

					if (!long.TryParse(tokens[3].Trim(), out adventure._nm))
					{
						throw new Exception("Error: TryParse function call failed for _nm.");
					}

					if (!string.IsNullOrWhiteSpace(adventure._ver) && adventure._ver.StartsWith("7"))
					{
						if (!long.TryParse(tokens[4].Trim(), out adventure._rlen))
						{
							throw new Exception("Error: TryParse function call failed for _rlen.");
						}

						if (!long.TryParse(tokens[5].Trim(), out adventure._mlen))
						{
							throw new Exception("Error: TryParse function call failed for _mlen.");
						}

						if (!long.TryParse(tokens[6].Trim(), out adventure._alen))
						{
							throw new Exception("Error: TryParse function call failed for _alen.");
						}
					}
				}

				Adventure = adventure;
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
		/// Full credit:  referencing DUNGEON LIST Z, DUNGEON LIST 7.1 by John Nelson and Tom Zuchowski
		/// </remarks>
		public virtual bool ConvertAdventure()
		{
			var result = true;

			try
			{
				var line = "";

				var buffer = new byte[4];

				var tokens = new string[1];

				var roomNameDatFile = Globals.Path.Combine(AdventureFolderPath, "EAMON.ROOM NAMES#040000");

				var roomDatFile = Globals.Path.Combine(AdventureFolderPath, "EAMON.ROOMS#040000");

				var artifactDatFile = Globals.Path.Combine(AdventureFolderPath, "EAMON.ARTIFACTS#040000");

				var monsterDatFile = Globals.Path.Combine(AdventureFolderPath, "EAMON.MONSTERS#040000");

				var descDatFile = Globals.Path.Combine(AdventureFolderPath, "EAMON.DESC#040000");

				using (var descDatStream = Globals.File.OpenRead(descDatFile))
				{
					using (var roomDatStream = Globals.File.OpenRead(roomDatFile))
					{
						for (var i = 0; i < Adventure._nr; i++)
						{
							var room = new A2ERoom();

							if (string.IsNullOrWhiteSpace(Adventure._ver) || !Adventure._ver.StartsWith("7"))
							{
								using (var roomNameDatStream = Globals.File.OpenRead(roomNameDatFile))
								{
									buffer = new byte[Adventure._rnlen];

									roomNameDatStream.Seek((int)((i + 1) * Adventure._rnlen), SeekOrigin.Begin);

									roomNameDatStream.Read(buffer, 0, (int)Adventure._rnlen);

									buffer = ConvertApple2ByteBuffer(buffer);

									line = System.Text.Encoding.Default.GetString(buffer).Trim();

									tokens = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

									if (tokens.Length < 1)
									{
										tokens = new string[] { "TODO" };
									}

									room._rname = tokens[0].Trim();
								}
							}

							buffer = new byte[Adventure._dlen];

							descDatStream.Seek((int)((i + 1) * Adventure._dlen), SeekOrigin.Begin);

							descDatStream.Read(buffer, 0, (int)Adventure._dlen);

							buffer = ConvertApple2ByteBuffer(buffer);

							line = System.Text.Encoding.Default.GetString(buffer).Trim();

							tokens = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

							if (tokens.Length < 1)
							{
								tokens = new string[] { "TODO" };
							}

							room._rdesc = tokens[0].Trim();

							buffer = new byte[Adventure._rlen];

							roomDatStream.Seek((int)((i + 1) * Adventure._rlen), SeekOrigin.Begin);

							roomDatStream.Read(buffer, 0, (int)Adventure._rlen);

							buffer = ConvertApple2ByteBuffer(buffer);

							line = System.Text.Encoding.Default.GetString(buffer).Trim();

							tokens = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

							var idx = 0;

							if (!string.IsNullOrWhiteSpace(Adventure._ver) && Adventure._ver.StartsWith("7"))
							{
								for (var j = tokens.Length; j < Adventure._nd + 2; j++)
								{
									tokens = tokens.Append(j == 0 ? "TODO" : j != Adventure._nd + 1 ? "-9999" : "1").ToArray();
								}

								room._rname = tokens[idx++].Trim();
							}
							else
							{
								for (var j = tokens.Length; j < Adventure._nd + 1; j++)
								{
									tokens = tokens.Append(j != Adventure._nd ? "-9999" : "1").ToArray();
								}
							}

							if (!short.TryParse(tokens[idx++].Trim(), out room._rd1))
							{
								room._rd1 = -9999;
							}

							if (!short.TryParse(tokens[idx++].Trim(), out room._rd2))
							{
								room._rd2 = -9999;
							}

							if (!short.TryParse(tokens[idx++].Trim(), out room._rd3))
							{
								room._rd3 = -9999;
							}

							if (!short.TryParse(tokens[idx++].Trim(), out room._rd4))
							{
								room._rd4 = -9999;
							}

							if (!short.TryParse(tokens[idx++].Trim(), out room._rd5))
							{
								room._rd5 = -9999;
							}

							if (!short.TryParse(tokens[idx++].Trim(), out room._rd6))
							{
								room._rd6 = -9999;
							}

							if (Adventure._nd == 10)
							{
								if (!short.TryParse(tokens[idx++].Trim(), out room._rd7))
								{
									room._rd7 = -9999;
								}

								if (!short.TryParse(tokens[idx++].Trim(), out room._rd8))
								{
									room._rd8 = -9999;
								}

								if (!short.TryParse(tokens[idx++].Trim(), out room._rd9))
								{
									room._rd9 = -9999;
								}

								if (!short.TryParse(tokens[idx++].Trim(), out room._rd10))
								{
									room._rd10 = -9999;
								}
							}

							if (!short.TryParse(tokens[idx++].Trim(), out room._rlight))
							{
								room._rlight = 1;
							}

							Adventure.RoomList.Add(room);
						}
					}

					using (var artifactDatStream = Globals.File.OpenRead(artifactDatFile))
					{
						for (var i = 0; i < Adventure._na; i++)
						{
							var artifact = new A2EArtifact();

							buffer = new byte[Adventure._dlen];

							descDatStream.Seek((int)((i + Adventure._aptr) * Adventure._dlen), SeekOrigin.Begin);

							descDatStream.Read(buffer, 0, (int)Adventure._dlen);

							buffer = ConvertApple2ByteBuffer(buffer);

							line = System.Text.Encoding.Default.GetString(buffer).Trim();

							tokens = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

							if (tokens.Length < 1)
							{
								tokens = new string[] { "TODO" };
							}

							artifact._artdesc = tokens[0].Trim();

							if (Adventure._alen > 0)
							{
								buffer = new byte[Adventure._alen];

								artifactDatStream.Seek((int)((i + 1) * Adventure._alen), SeekOrigin.Begin);

								artifactDatStream.Read(buffer, 0, (int)Adventure._alen);
							}
							else
							{
								buffer = new byte[4];         // TODO
							}

							buffer = ConvertApple2ByteBuffer(buffer);

							line = System.Text.Encoding.Default.GetString(buffer).Trim();

							tokens = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

							for (var j = tokens.Length; j < 9; j++)
							{
								tokens = tokens.Append(j > 0 ? "-9999" : "TODO").ToArray();
							}

							artifact._artname = tokens[0].Trim();

							if (!short.TryParse(tokens[1].Trim(), out artifact._ad1))
							{
								artifact._ad1 = -9999;
							}

							if (!short.TryParse(tokens[2].Trim(), out artifact._ad2))
							{
								artifact._ad2 = -9999;
							}

							if (!short.TryParse(tokens[3].Trim(), out artifact._ad3))
							{
								artifact._ad3 = -9999;
							}

							if (!short.TryParse(tokens[4].Trim(), out artifact._ad4))
							{
								artifact._ad4 = -9999;
							}

							if (artifact._ad2 > 1)
							{
								if (!short.TryParse(tokens[5].Trim(), out artifact._ad5))
								{
									artifact._ad5 = -9999;
								}

								if (!short.TryParse(tokens[6].Trim(), out artifact._ad6))
								{
									artifact._ad6 = -9999;
								}

								if (!short.TryParse(tokens[7].Trim(), out artifact._ad7))
								{
									artifact._ad7 = -9999;
								}

								if (!short.TryParse(tokens[8].Trim(), out artifact._ad8))
								{
									artifact._ad8 = -9999;
								}
							}

							Adventure.ArtifactList.Add(artifact);
						}
					}

					for (var i = 0; i < Adventure._ne; i++)
					{
						var effect = new A2EEffect();

						buffer = new byte[Adventure._dlen];

						descDatStream.Seek((int)((i + Adventure._eptr) * Adventure._dlen), SeekOrigin.Begin);

						descDatStream.Read(buffer, 0, (int)Adventure._dlen);

						buffer = ConvertApple2ByteBuffer(buffer);

						line = System.Text.Encoding.Default.GetString(buffer).Trim();

						tokens = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

						if (tokens.Length < 1)
						{
							tokens = new string[] { "TODO" };
						}

						effect._text = tokens[0].Trim();

						Adventure.EffectList.Add(effect);
					}

					using (var monsterDatStream = Globals.File.OpenRead(monsterDatFile))
					{
						for (var i = 0; i < Adventure._nm; i++)
						{
							var monster = new A2EMonster();

							buffer = new byte[Adventure._dlen];

							descDatStream.Seek((int)((i + Adventure._mptr) * Adventure._dlen), SeekOrigin.Begin);

							descDatStream.Read(buffer, 0, (int)Adventure._dlen);

							buffer = ConvertApple2ByteBuffer(buffer);

							line = System.Text.Encoding.Default.GetString(buffer).Trim();

							tokens = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

							if (tokens.Length < 1)
							{
								tokens = new string[] { "TODO" };
							}

							monster._mdesc = tokens[0].Trim();

							if (Adventure._mlen > 0)
							{
								buffer = new byte[Adventure._mlen];

								monsterDatStream.Seek((int)((i + 1) * Adventure._mlen), SeekOrigin.Begin);

								monsterDatStream.Read(buffer, 0, (int)Adventure._mlen);
							}
							else
							{
								buffer = new byte[4];         // TODO
							}

							buffer = ConvertApple2ByteBuffer(buffer);

							line = System.Text.Encoding.Default.GetString(buffer).Trim();

							tokens = line.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

							for (var j = tokens.Length; j < 13; j++)
							{
								tokens = tokens.Append(j > 0 ? "-9999" : "TODO").ToArray();
							}

							monster._mname = tokens[0].Trim();

							if (!short.TryParse(tokens[1].Trim(), out monster._md1))
							{
								monster._md1 = -9999;
							}

							if (!short.TryParse(tokens[2].Trim(), out monster._md2))
							{
								monster._md2 = -9999;
							}

							if (!short.TryParse(tokens[3].Trim(), out monster._md3))
							{
								monster._md3 = -9999;
							}

							if (!short.TryParse(tokens[4].Trim(), out monster._md4))
							{
								monster._md4 = -9999;
							}

							if (!short.TryParse(tokens[5].Trim(), out monster._md5))
							{
								monster._md5 = -9999;
							}

							if (!short.TryParse(tokens[6].Trim(), out monster._md6))
							{
								monster._md6 = -9999;
							}

							if (!short.TryParse(tokens[7].Trim(), out monster._md7))
							{
								monster._md7 = -9999;
							}

							if (!short.TryParse(tokens[8].Trim(), out monster._md8))
							{
								monster._md8 = -9999;
							}

							if (!short.TryParse(tokens[9].Trim(), out monster._md9))
							{
								monster._md9 = -9999;
							}

							if (!short.TryParse(tokens[10].Trim(), out monster._md10))
							{
								monster._md10 = -9999;
							}

							if (!short.TryParse(tokens[11].Trim(), out monster._md11))
							{
								monster._md11 = -9999;
							}

							if (!short.TryParse(tokens[12].Trim(), out monster._md12))
							{
								monster._md12 = -9999;
							}

							Adventure.MonsterList.Add(monster);
						}
					}
				}
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
