
// EDXAdventureConverter.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Eamon.Game.Utilities;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Converters.EamonDeluxe
{
	/// <summary>
	/// </summary>
	public class EDXAdventureConverter
	{
		public long _ver;

		public long _nadv;

		public long _nh;

		public virtual List<EDXAdventure> EDXAdventureList { get; set; }

		public virtual List<EDXHint> EDXHintList { get; set; }

		public string AdventureFolderPath { get; set; }

		public void LoadEDXAdventureList(params long[] advNums)         // Note: Adventure numbers are 1-based
		{
			try
			{
				var line = "";

				var nameDatFilePath = Globals.Path.Combine(AdventureFolderPath, "NAME.DAT");

				using (var file = new System.IO.StreamReader(nameDatFilePath))
				{
					var edxa = new EDXAdventure();

					line = file.ReadLine();

					var tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if (!long.TryParse(tokens[0], out edxa._nr))
					{
						// handle failure
					}

					if (tokens.Length > 1)
					{
						if (!long.TryParse(tokens[1], out edxa._na))
						{
							// handle failure
						}

						if (!long.TryParse(tokens[2], out edxa._ne))
						{
							// handle failure
						}

						if (!long.TryParse(tokens[3], out edxa._nm))
						{
							// handle failure
						}
					}
					else
					{
						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out edxa._na))
						{
							// handle failure
						}

						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out edxa._ne))
						{
							// handle failure
						}

						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out edxa._nm))
						{
							// handle failure
						}
					}

					line = file.ReadLine();

					edxa.AdvName = line.Trim();

					line = file.ReadLine();

					tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if (!long.TryParse(tokens[0], out edxa._nd))
					{
						// handle failure
					}

					if (tokens.Length > 1)
					{
						if (!long.TryParse(tokens[1], out _ver))
						{
							// handle failure
						}

						if (!long.TryParse(tokens[2], out _nadv))
						{
							// handle failure
						}
					}
					else
					{
						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out _ver))
						{
							// handle failure
						}

						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out _nadv))
						{
							// handle failure
						}
					}

					if (_nadv > 1)
					{
						for (var i = 0; i < _nadv; i++)
						{
							edxa = new EDXAdventure();

							line = file.ReadLine();

							edxa.AdvName = line.Trim();

							line = file.ReadLine();

							tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

							if (!long.TryParse(tokens[0], out edxa._nr))
							{
								// handle failure
							}

							if (tokens.Length > 1)
							{
								if (!long.TryParse(tokens[1], out edxa._na))
								{
									// handle failure
								}

								if (!long.TryParse(tokens[2], out edxa._ne))
								{
									// handle failure
								}

								if (!long.TryParse(tokens[3], out edxa._nm))
								{
									// handle failure
								}
							}
							else
							{
								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._na))
								{
									// handle failure
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._ne))
								{
									// handle failure
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._nm))
								{
									// handle failure
								}
							}

							line = file.ReadLine();

							tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

							if (!long.TryParse(tokens[0], out edxa._rptr))
							{
								// handle failure
							}

							if (tokens.Length > 1)
							{
								if (!long.TryParse(tokens[1], out edxa._aptr))
								{
									// handle failure
								}

								if (!long.TryParse(tokens[2], out edxa._eptr))
								{
									// handle failure
								}

								if (!long.TryParse(tokens[3], out edxa._mptr))
								{
									// handle failure
								}

								if (!long.TryParse(tokens[4], out edxa._nd))
								{
									// handle failure
								}
							}
							else
							{
								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._aptr))
								{
									// handle failure
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._eptr))
								{
									// handle failure
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._mptr))
								{
									// handle failure
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._nd))
								{
									// handle failure
								}
							}

							if (advNums == null || advNums.Length <= 0 || advNums.Contains(i + 1))
							{
								EDXAdventureList.Add(edxa);
							}
						}
					}
					else
					{
						edxa._rptr = 1;

						edxa._aptr = 1;

						edxa._eptr = 1;

						edxa._mptr = 1;

						if (advNums == null || advNums.Length <= 0 || advNums.Contains(1))
						{
							EDXAdventureList.Add(edxa);
						}
					}
				}
			} 
			catch (Exception ex)
			{
				// handle failure
			}
		}

		public void LoadEDXHintList(params long[] hintNums)			// Note: Hint numbers are 1-based
		{
			try
			{
				var line = "";

				var hintDatFile = Globals.Path.Combine(AdventureFolderPath, "HINTDIR.DAT");

				using (var file = new System.IO.StreamReader(hintDatFile))
				{
					line = file.ReadLine();

					if (!long.TryParse(line.Trim(), out _nh))
					{
						// handle failure
					}

					for (var i = 0; i < _nh; i++)
					{
						var edxh = new EDXHint();

						line = file.ReadLine();

						edxh.Question = line.Trim();

						line = file.ReadLine();

						var tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

						if (!long.TryParse(tokens[0], out edxh._hptr))
						{
							// handle failure
						}

						if (tokens.Length > 1)
						{
							if (!long.TryParse(tokens[1], out edxh._nh))
							{
								// handle failure
							}
						}
						else
						{
							line = file.ReadLine();

							if (!long.TryParse(line.Trim(), out edxh._nh))
							{
								// handle failure
							}
						}

						if (hintNums == null || hintNums.Length <= 0 || hintNums.Contains(i + 1))
						{
							EDXHintList.Add(edxh);
						}
					}
				}
			}
			catch (Exception ex)
			{
				// handle failure
			}
		}

		public void ConvertEDXAdventures()
		{
			try
			{
				var roomDatFile = Globals.Path.Combine(AdventureFolderPath, "ROOMS.DAT");

				var roomDscFile = Globals.Path.Combine(AdventureFolderPath, "ROOMS.DSC");

				var artifactDatFile = Globals.Path.Combine(AdventureFolderPath, "ARTIFACT.DAT");

				var artifactDscFile = Globals.Path.Combine(AdventureFolderPath, "ARTIFACT.DSC");

				var monsterDatFile = Globals.Path.Combine(AdventureFolderPath, "MONSTERS.DAT");

				var monsterDscFile = Globals.Path.Combine(AdventureFolderPath, "MONSTERS.DSC");

				var effectDscFile = Globals.Path.Combine(AdventureFolderPath, "EFFECT.DSC");

				foreach (var edxa in EDXAdventureList)
				{
					using (var roomDatStream = Globals.File.OpenRead(roomDatFile))
					{
						var roomDatFlr = new FixedLengthReader(roomDatStream, (edxa._rptr - 1) * 101, true);

						using (var roomDscStream = Globals.File.OpenRead(roomDscFile))
						{
							var roomDscFlr = new FixedLengthReader(roomDscStream, (edxa._rptr - 1) * 255, true);

							for (var i = 0; i < edxa._nr; i++)
							{
								var room = new EDXRoom();

								roomDatFlr.read(room);

								var desc = new EDXDesc();

								roomDscFlr.read(desc);

								room._rdesc = desc._text;

								edxa.RoomList.Add(room);
							}
						}
					}

					using (var artifactDatStream = Globals.File.OpenRead(artifactDatFile))
					{
						var artifactDatFlr = new FixedLengthReader(artifactDatStream, (edxa._aptr - 1) * 51, true);

						using (var artifactDscStream = Globals.File.OpenRead(artifactDscFile))
						{
							var artifactDscFlr = new FixedLengthReader(artifactDscStream, (edxa._aptr - 1) * 255, true);

							for (var i = 0; i < edxa._na; i++)
							{
								var artifact = new EDXArtifact();

								artifactDatFlr.read(artifact);

								var desc = new EDXDesc();

								artifactDscFlr.read(desc);

								artifact._artdesc = desc._text;

								edxa.ArtifactList.Add(artifact);
							}
						}
					}

					using (var monsterDatStream = Globals.File.OpenRead(monsterDatFile))
					{
						var monsterDatFlr = new FixedLengthReader(monsterDatStream, (edxa._mptr - 1) * 61, true);

						using (var monsterDscStream = Globals.File.OpenRead(monsterDscFile))
						{
							var monsterDscFlr = new FixedLengthReader(monsterDscStream, (edxa._mptr - 1) * 255, true);

							for (var i = 0; i < edxa._nm; i++)
							{
								var monster = new EDXMonster();

								monsterDatFlr.read(monster);

								var desc = new EDXDesc();

								monsterDscFlr.read(desc);

								monster._mdesc = desc._text;

								edxa.MonsterList.Add(monster);
							}
						}
					}

					using (var effectDscStream = Globals.File.OpenRead(effectDscFile))
					{
						var effectDscFlr = new FixedLengthReader(effectDscStream, (edxa._eptr - 1) * 255, true);

						for (var i = 0; i < edxa._ne; i++)
						{
							var effect = new EDXDesc();

							effectDscFlr.read(effect);

							edxa.EffectList.Add(effect);
						}
					}
				}
			}
			catch (Exception ex)
			{
				// handle failure
			}
		}

		public void ConvertEDXHints()
		{
			try
			{
				var hintDscFile = Globals.Path.Combine(AdventureFolderPath, "HINTS.DSC");

				foreach (var edxh in EDXHintList)
				{
					using (var hintDscStream = Globals.File.OpenRead(hintDscFile))
					{
						var hintDscFlr = new FixedLengthReader(hintDscStream, (edxh._hptr - 1) * 255, true);

						for (var i = 0; i < edxh._nh; i++)
						{
							var answer = new EDXDesc();

							hintDscFlr.read(answer);

							edxh.AnswerList.Add(answer);
						}
					}
				}
			}
			catch (Exception ex)
			{
				// handle failure
			}
		}

		public EDXAdventureConverter()
		{
			EDXAdventureList = new List<EDXAdventure>();

			EDXHintList = new List<EDXHint>();
		}
	}
}
