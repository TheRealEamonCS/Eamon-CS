
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
		/// Full credit:  referencing DUNGEON LIST Z by John Nelson and Tom Zuchowski
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
						throw new Exception("Error: IsNullOrWhiteSpace function call returned true for Name");
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
						adventure._ver = tokens[2];
					}
					catch (Exception)
					{
						adventure._ver = "???";
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
						throw new Exception("Error: TryParse function call failed for _nr");
					}

					if (!long.TryParse(tokens[1].Trim(), out adventure._na))
					{
						throw new Exception("Error: TryParse function call failed for _na");
					}

					if (!long.TryParse(tokens[2].Trim(), out adventure._ne))
					{
						throw new Exception("Error: TryParse function call failed for _ne");
					}

					if (!long.TryParse(tokens[3].Trim(), out adventure._nm))
					{
						throw new Exception("Error: TryParse function call failed for _nm");
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
		/// Full credit:  referencing DUNGEON LIST Z by John Nelson and Tom Zuchowski
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
					using (var roomNameDatStream = Globals.File.OpenRead(roomNameDatFile))
					{
						using (var roomDatStream = Globals.File.OpenRead(roomDatFile))
						{
							for (var i = 0; i < Adventure._nr; i++)
							{
								var room = new A2ERoom();

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

								for (var j = tokens.Length; j < Adventure._nd; j++)
								{
									tokens = tokens.Append("-9999").ToArray();
								}

								if (!short.TryParse(tokens[0].Trim(), out room._rd1))
								{
									throw new Exception("Error: TryParse function call failed for _rd1");
								}

								if (!short.TryParse(tokens[1].Trim(), out room._rd2))
								{
									throw new Exception("Error: TryParse function call failed for _rd2");
								}

								if (!short.TryParse(tokens[2].Trim(), out room._rd3))
								{
									throw new Exception("Error: TryParse function call failed for _rd3");
								}

								if (!short.TryParse(tokens[3].Trim(), out room._rd4))
								{
									throw new Exception("Error: TryParse function call failed for _rd4");
								}

								if (!short.TryParse(tokens[4].Trim(), out room._rd5))
								{
									throw new Exception("Error: TryParse function call failed for _rd5");
								}

								if (!short.TryParse(tokens[5].Trim(), out room._rd6))
								{
									throw new Exception("Error: TryParse function call failed for _rd6");
								}

								if (Adventure._nd == 10)
								{
									if (!short.TryParse(tokens[6].Trim(), out room._rd7))
									{
										throw new Exception("Error: TryParse function call failed for _rd7");
									}

									if (!short.TryParse(tokens[7].Trim(), out room._rd8))
									{
										throw new Exception("Error: TryParse function call failed for _rd8");
									}

									if (!short.TryParse(tokens[8].Trim(), out room._rd9))
									{
										throw new Exception("Error: TryParse function call failed for _rd9");
									}

									if (!short.TryParse(tokens[9].Trim(), out room._rd10))
									{
										throw new Exception("Error: TryParse function call failed for _rd10");
									}
								}

								Adventure.RoomList.Add(room);
							}
						}
					}

					using (var artifactDatStream = Globals.File.OpenRead(artifactDatFile))
					{
						for (var i = 0; i < Adventure._na; i++)
						{
							var artifact = new A2EArtifact();

							buffer = new byte[Adventure._dlen];

							descDatStream.Seek((int)((i + 101) * Adventure._dlen), SeekOrigin.Begin);

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
								throw new Exception("Error: TryParse function call failed for _ad1");
							}

							if (!short.TryParse(tokens[2].Trim(), out artifact._ad2))
							{
								throw new Exception("Error: TryParse function call failed for _ad2");
							}

							if (!short.TryParse(tokens[3].Trim(), out artifact._ad3))
							{
								throw new Exception("Error: TryParse function call failed for _ad3");
							}

							if (!short.TryParse(tokens[4].Trim(), out artifact._ad4))
							{
								throw new Exception("Error: TryParse function call failed for _ad4");
							}

							if (artifact._ad2 > 1)
							{
								if (!short.TryParse(tokens[5].Trim(), out artifact._ad5))
								{
									throw new Exception("Error: TryParse function call failed for _ad5");
								}

								if (!short.TryParse(tokens[6].Trim(), out artifact._ad6))
								{
									throw new Exception("Error: TryParse function call failed for _ad6");
								}

								if (!short.TryParse(tokens[7].Trim(), out artifact._ad7))
								{
									throw new Exception("Error: TryParse function call failed for _ad7");
								}

								if (!short.TryParse(tokens[8].Trim(), out artifact._ad8))
								{
									throw new Exception("Error: TryParse function call failed for _ad8");
								}
							}

							Adventure.ArtifactList.Add(artifact);
						}
					}

					for (var i = 0; i < Adventure._ne; i++)
					{
						var effect = new A2EEffect();

						buffer = new byte[Adventure._dlen];

						descDatStream.Seek((int)((i + 201) * Adventure._dlen), SeekOrigin.Begin);

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

							descDatStream.Seek((int)((i + 301) * Adventure._dlen), SeekOrigin.Begin);

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
								throw new Exception("Error: TryParse function call failed for _md1");
							}

							if (!short.TryParse(tokens[2].Trim(), out monster._md2))
							{
								throw new Exception("Error: TryParse function call failed for _md2");
							}

							if (!short.TryParse(tokens[3].Trim(), out monster._md3))
							{
								throw new Exception("Error: TryParse function call failed for _md3");
							}

							if (!short.TryParse(tokens[4].Trim(), out monster._md4))
							{
								throw new Exception("Error: TryParse function call failed for _md4");
							}

							if (!short.TryParse(tokens[5].Trim(), out monster._md5))
							{
								throw new Exception("Error: TryParse function call failed for _md5");
							}

							if (!short.TryParse(tokens[6].Trim(), out monster._md6))
							{
								throw new Exception("Error: TryParse function call failed for _md6");
							}

							if (!short.TryParse(tokens[7].Trim(), out monster._md7))
							{
								throw new Exception("Error: TryParse function call failed for _md7");
							}

							if (!short.TryParse(tokens[8].Trim(), out monster._md8))
							{
								throw new Exception("Error: TryParse function call failed for _md8");
							}

							if (!short.TryParse(tokens[9].Trim(), out monster._md9))
							{
								throw new Exception("Error: TryParse function call failed for _md9");
							}

							if (!short.TryParse(tokens[10].Trim(), out monster._md10))
							{
								throw new Exception("Error: TryParse function call failed for _md10");
							}

							if (!short.TryParse(tokens[11].Trim(), out monster._md11))
							{
								throw new Exception("Error: TryParse function call failed for _md11");
							}

							if (!short.TryParse(tokens[12].Trim(), out monster._md12))
							{
								throw new Exception("Error: TryParse function call failed for _md12");
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
