
// EDXAdventureConverter.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Eamon.Game.Utilities;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Converters.EamonDeluxe
{
	/// <summary>
	/// </summary>
	public class EDXAdventureConverter
	{
		public long _ver;

		public long _nadv;

		public long _nh;

		public virtual IList<EDXAdventure> AdventureList { get; set; }

		public virtual IList<EDXHint> HintList { get; set; }

		public virtual string AdventureFolderPath { get; set; }

		public virtual string ErrorMessage { get; set; }

		public virtual bool LoadAdventureList(params long[] advNums)         // Note: Adventure numbers are 1-based
		{
			var result = true;

			try
			{
				var line = "";

				var nameDatFile = gEngine.Path.Combine(AdventureFolderPath, "NAME.DAT");

				using (var file = new System.IO.StreamReader(nameDatFile))
				{
					var adventure = new EDXAdventure();

					line = file.ReadLine();

					var tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if (!long.TryParse(tokens[0], out adventure._nr))
					{
						throw new Exception("TryParse function call failed for _nr.");
					}

					if (tokens.Length > 1)
					{
						if (!long.TryParse(tokens[1], out adventure._na))
						{
							throw new Exception("TryParse function call failed for _na.");
						}

						if (!long.TryParse(tokens[2], out adventure._ne))
						{
							throw new Exception("TryParse function call failed for _ne.");
						}

						if (!long.TryParse(tokens[3], out adventure._nm))
						{
							throw new Exception("TryParse function call failed for _nm.");
						}
					}
					else
					{
						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out adventure._na))
						{
							throw new Exception("TryParse function call failed for _na.");
						}

						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out adventure._ne))
						{
							throw new Exception("TryParse function call failed for _ne.");
						}

						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out adventure._nm))
						{
							throw new Exception("TryParse function call failed for _nm.");
						}
					}

					line = file.ReadLine();

					adventure.Name = line.Trim();

					line = file.ReadLine();

					tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if (!long.TryParse(tokens[0], out adventure._nd))
					{
						throw new Exception("TryParse function call failed for _nd.");
					}

					if (tokens.Length > 1)
					{
						if (!long.TryParse(tokens[1], out _ver))
						{
							throw new Exception("TryParse function call failed for _ver.");
						}

						if (!long.TryParse(tokens[2], out _nadv))
						{
							throw new Exception("TryParse function call failed for _nadv.");
						}
					}
					else
					{
						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out _ver))
						{
							throw new Exception("TryParse function call failed for _ver.");
						}

						line = file.ReadLine();

						if (!long.TryParse(line.Trim(), out _nadv))
						{
							throw new Exception("TryParse function call failed for _nadv.");
						}
					}

					if (_nadv > 1)
					{
						for (var i = 0; i < _nadv; i++)
						{
							adventure = new EDXAdventure();

							line = file.ReadLine();

							adventure.Name = line.Trim();

							line = file.ReadLine();

							tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

							if (!long.TryParse(tokens[0], out adventure._nr))
							{
								throw new Exception(string.Format("TryParse function call failed for record number {0} _nr.", i));
							}

							if (tokens.Length > 1)
							{
								if (!long.TryParse(tokens[1], out adventure._na))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _na.", i));
								}

								if (!long.TryParse(tokens[2], out adventure._ne))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _ne.", i));
								}

								if (!long.TryParse(tokens[3], out adventure._nm))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _nm.", i));
								}
							}
							else
							{
								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out adventure._na))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _na.", i));
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out adventure._ne))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _ne.", i));
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out adventure._nm))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _nm.", i));
								}
							}

							line = file.ReadLine();

							tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

							if (!long.TryParse(tokens[0], out adventure._rptr))
							{
								throw new Exception(string.Format("TryParse function call failed for record number {0} _rptr.", i));
							}

							if (tokens.Length > 1)
							{
								if (!long.TryParse(tokens[1], out adventure._aptr))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _aptr.", i));
								}

								if (!long.TryParse(tokens[2], out adventure._eptr))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _eptr.", i));
								}

								if (!long.TryParse(tokens[3], out adventure._mptr))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _mptr.", i));
								}

								if (!long.TryParse(tokens[4], out adventure._nd))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _nd.", i));
								}
							}
							else
							{
								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out adventure._aptr))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _aptr.", i));
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out adventure._eptr))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _eptr.", i));
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out adventure._mptr))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _mptr.", i));
								}

								line = file.ReadLine();

								if (!long.TryParse(line.Trim(), out adventure._nd))
								{
									throw new Exception(string.Format("TryParse function call failed for record number {0} _nd.", i));
								}
							}

							if (advNums == null || advNums.Length <= 0 || advNums.Contains(i + 1))
							{
								AdventureList.Add(adventure);
							}
						}
					}
					else
					{
						adventure._rptr = 1;

						adventure._aptr = 1;

						adventure._eptr = 1;

						adventure._mptr = 1;

						if (advNums == null || advNums.Length <= 0 || advNums.Contains(1))
						{
							AdventureList.Add(adventure);
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

		public virtual bool LoadHintList(params long[] hintNums)			// Note: Hint numbers are 1-based
		{
			var result = true;

			try
			{
				var line = "";

				var hintDatFile = gEngine.Path.Combine(AdventureFolderPath, "HINTDIR.DAT");

				using (var file = new System.IO.StreamReader(hintDatFile))
				{
					line = file.ReadLine();

					if (!long.TryParse(line.Trim(), out _nh))
					{
						throw new Exception("TryParse function call failed for _nh.");
					}

					for (var i = 0; i < _nh; i++)
					{
						var hint = new EDXHint();

						line = file.ReadLine();

						hint.Question = line.Trim();

						line = file.ReadLine();

						var tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

						if (!long.TryParse(tokens[0], out hint._hptr))
						{
							throw new Exception(string.Format("TryParse function call failed for record number {0} _hptr.", i));
						}

						if (tokens.Length > 1)
						{
							if (!long.TryParse(tokens[1], out hint._nh))
							{
								throw new Exception(string.Format("TryParse function call failed for record number {0} _nh.", i));
							}
						}
						else
						{
							line = file.ReadLine();

							if (!long.TryParse(line.Trim(), out hint._nh))
							{
								throw new Exception(string.Format("TryParse function call failed for record number {0} _nh.", i));
							}
						}

						if (hintNums == null || hintNums.Length <= 0 || hintNums.Contains(i + 1))
						{
							HintList.Add(hint);
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

		public virtual bool ConvertAdventures()
		{
			var result = true;

			try
			{
				var roomDatFile = gEngine.Path.Combine(AdventureFolderPath, "ROOMS.DAT");

				var roomDscFile = gEngine.Path.Combine(AdventureFolderPath, "ROOMS.DSC");

				var artifactDatFile = gEngine.Path.Combine(AdventureFolderPath, "ARTIFACT.DAT");

				var artifactDscFile = gEngine.Path.Combine(AdventureFolderPath, "ARTIFACT.DSC");

				var monsterDatFile = gEngine.Path.Combine(AdventureFolderPath, "MONSTERS.DAT");

				var monsterDscFile = gEngine.Path.Combine(AdventureFolderPath, "MONSTERS.DSC");

				var effectDscFile = gEngine.Path.Combine(AdventureFolderPath, "EFFECT.DSC");

				foreach (var adventure in AdventureList)
				{
					using (var roomDatStream = gEngine.File.OpenRead(roomDatFile))
					{
						var roomDatFlr = new FixedLengthReader(roomDatStream, (adventure._rptr - 1) * 101, true);

						using (var roomDscStream = gEngine.File.OpenRead(roomDscFile))
						{
							var roomDscFlr = new FixedLengthReader(roomDscStream, (adventure._rptr - 1) * 255, true);

							for (var i = 0; i < adventure._nr; i++)
							{
								var room = new EDXRoom();

								roomDatFlr.read(room);

								var desc = new EDXDesc();

								roomDscFlr.read(desc);

								room._rdesc = desc._text;

								adventure.RoomList.Add(room);
							}
						}
					}

					using (var artifactDatStream = gEngine.File.OpenRead(artifactDatFile))
					{
						var artifactDatFlr = new FixedLengthReader(artifactDatStream, (adventure._aptr - 1) * 51, true);

						using (var artifactDscStream = gEngine.File.OpenRead(artifactDscFile))
						{
							var artifactDscFlr = new FixedLengthReader(artifactDscStream, (adventure._aptr - 1) * 255, true);

							for (var i = 0; i < adventure._na; i++)
							{
								var artifact = new EDXArtifact();

								artifactDatFlr.read(artifact);

								var desc = new EDXDesc();

								artifactDscFlr.read(desc);

								artifact._artdesc = desc._text;

								adventure.ArtifactList.Add(artifact);
							}
						}
					}

					using (var monsterDatStream = gEngine.File.OpenRead(monsterDatFile))
					{
						var monsterDatFlr = new FixedLengthReader(monsterDatStream, (adventure._mptr - 1) * 61, true);

						using (var monsterDscStream = gEngine.File.OpenRead(monsterDscFile))
						{
							var monsterDscFlr = new FixedLengthReader(monsterDscStream, (adventure._mptr - 1) * 255, true);

							for (var i = 0; i < adventure._nm; i++)
							{
								var monster = new EDXMonster();

								monsterDatFlr.read(monster);

								var desc = new EDXDesc();

								monsterDscFlr.read(desc);

								monster._mdesc = desc._text;

								adventure.MonsterList.Add(monster);
							}
						}
					}

					using (var effectDscStream = gEngine.File.OpenRead(effectDscFile))
					{
						var effectDscFlr = new FixedLengthReader(effectDscStream, (adventure._eptr - 1) * 255, true);

						for (var i = 0; i < adventure._ne; i++)
						{
							var effect = new EDXDesc();

							effectDscFlr.read(effect);

							adventure.EffectList.Add(effect);
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

		public virtual bool ConvertHints()
		{
			var result = true;

			try
			{
				var hintDscFile = gEngine.Path.Combine(AdventureFolderPath, "HINTS.DSC");

				foreach (var hint in HintList)
				{
					using (var hintDscStream = gEngine.File.OpenRead(hintDscFile))
					{
						var hintDscFlr = new FixedLengthReader(hintDscStream, (hint._hptr - 1) * 255, true);

						for (var i = 0; i < hint._nh; i++)
						{
							var answer = new EDXDesc();

							hintDscFlr.read(answer);

							hint.AnswerList.Add(answer);
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

		public EDXAdventureConverter()
		{
			AdventureList = new List<EDXAdventure>();

			HintList = new List<EDXHint>();
		}
	}
}
