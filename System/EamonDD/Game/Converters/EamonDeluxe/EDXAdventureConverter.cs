
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

		public virtual IList<EDXAdventure> EDXAdventureList { get; set; }

		public virtual IList<EDXHint> EDXHintList { get; set; }

		public string AdventureFolderPath { get; set; }

		public bool LoadEDXAdventureList(params long[] advNums)         // Note: Adventure numbers are 1-based
		{
			var result = true;

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
						throw new Exception("Error: TryParse function call failed for _nr");
					}

					if (tokens.Length > 1)
					{
						if (!long.TryParse(tokens[1], out edxa._na))
						{
							throw new Exception("Error: TryParse function call failed for _na");
						}

						if (!long.TryParse(tokens[2], out edxa._ne))
						{
							throw new Exception("Error: TryParse function call failed for _ne");
						}

						if (!long.TryParse(tokens[3], out edxa._nm))
						{
							throw new Exception("Error: TryParse function call failed for _nm");
						}
					}
					else
					{
						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out edxa._na))
						{
							throw new Exception("Error: TryParse function call failed for _na");
						}

						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out edxa._ne))
						{
							throw new Exception("Error: TryParse function call failed for _ne");
						}

						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out edxa._nm))
						{
							throw new Exception("Error: TryParse function call failed for _nm");
						}
					}

					line = file.ReadLine();

					edxa.AdvName = line.Trim();

					line = file.ReadLine();

					tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if (!long.TryParse(tokens[0], out edxa._nd))
					{
						throw new Exception("Error: TryParse function call failed for _nd");
					}

					if (tokens.Length > 1)
					{
						if (!long.TryParse(tokens[1], out _ver))
						{
							throw new Exception("Error: TryParse function call failed for _ver");
						}

						if (!long.TryParse(tokens[2], out _nadv))
						{
							throw new Exception("Error: TryParse function call failed for _nadv");
						}
					}
					else
					{
						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out _ver))
						{
							throw new Exception("Error: TryParse function call failed for _ver");
						}

						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out _nadv))
						{
							throw new Exception("Error: TryParse function call failed for _nadv");
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
								throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _nr", i));
							}

							if (tokens.Length > 1)
							{
								if (!long.TryParse(tokens[1], out edxa._na))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _na", i));
								}

								if (!long.TryParse(tokens[2], out edxa._ne))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _ne", i));
								}

								if (!long.TryParse(tokens[3], out edxa._nm))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _nm", i));
								}
							}
							else
							{
								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._na))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _na", i));
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._ne))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _ne", i));
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._nm))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _nm", i));
								}
							}

							line = file.ReadLine();

							tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

							if (!long.TryParse(tokens[0], out edxa._rptr))
							{
								throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _rptr", i));
							}

							if (tokens.Length > 1)
							{
								if (!long.TryParse(tokens[1], out edxa._aptr))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _aptr", i));
								}

								if (!long.TryParse(tokens[2], out edxa._eptr))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _eptr", i));
								}

								if (!long.TryParse(tokens[3], out edxa._mptr))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _mptr", i));
								}

								if (!long.TryParse(tokens[4], out edxa._nd))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _nd", i));
								}
							}
							else
							{
								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._aptr))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _aptr", i));
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._eptr))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _eptr", i));
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._mptr))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _mptr", i));
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out edxa._nd))
								{
									throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _nd", i));
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
				Globals.Error.WriteLine("{0}{1}", Environment.NewLine, ex.Message);

				result = false;
			}

			return result;
		}

		public bool LoadEDXHintList(params long[] hintNums)			// Note: Hint numbers are 1-based
		{
			var result = true;

			try
			{
				var line = "";

				var hintDatFile = Globals.Path.Combine(AdventureFolderPath, "HINTDIR.DAT");

				using (var file = new System.IO.StreamReader(hintDatFile))
				{
					line = file.ReadLine();

					if (!long.TryParse(line.Trim(), out _nh))
					{
						throw new Exception("Error: TryParse function call failed for _nh");
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
							throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _hptr", i));
						}

						if (tokens.Length > 1)
						{
							if (!long.TryParse(tokens[1], out edxh._nh))
							{
								throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _nh", i));
							}
						}
						else
						{
							line = file.ReadLine();

							if (!long.TryParse(line.Trim(), out edxh._nh))
							{
								throw new Exception(string.Format("Error: TryParse function call failed for record number {0} _nh", i));
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
				Globals.Error.WriteLine("{0}{1}", Environment.NewLine, ex.Message);

				result = false;
			}

			return result;
		}

		public bool ConvertEDXAdventures()
		{
			var result = true;

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
				Globals.Error.WriteLine("{0}{1}", Environment.NewLine, ex.Message);

				result = false;
			}

			return result;
		}

		public bool ConvertEDXHints()
		{
			var result = true;

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
				Globals.Error.WriteLine("{0}{1}", Environment.NewLine, ex.Message);

				result = false;
			}

			return result;
		}

		public EDXAdventureConverter()
		{
			EDXAdventureList = new List<EDXAdventure>();

			EDXHintList = new List<EDXHint>();
		}
	}
}
