
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Engine : IEngine
	{
		#region Public Properties

		public virtual IDictionary<long, Func<string>> MacroFuncs { get; set; }

		public virtual IList<IArtifact> ArtifactContainedList { get; set; }

		public virtual Action<IArtifact, long, bool> RevealContainerContentsFunc { get; set; }

		public virtual IPrep[] Preps { get; set; }

		public virtual string[] Articles { get; set; }

		public virtual string UnknownName { get; set; }

		/// <summary></summary>
		public virtual Random Rand { get; set; }

		/// <summary></summary>
		public virtual string[] NumberStrings { get; set; }

		/// <summary></summary>
		public virtual string[] FieldDescNames { get; set; }

		/// <summary>
		/// Gets or sets an array containing the name for each <see cref="Status"/>.
		/// </summary>
		public virtual string[] StatusNames { get; set; }

		/// <summary>
		/// Gets or sets an array containing the name for each <see cref="Clothing"/>.
		/// </summary>
		public virtual string[] ClothingNames { get; set; }

		/// <summary>
		/// Gets or sets an array containing the description for each <see cref="CombatCode"/>.
		/// </summary>
		public virtual string[] CombatCodeDescs { get; set; }

		/// <summary></summary>
		public virtual string[] ContainerDisplayCodeDescs { get; set; }

		/// <summary>
		/// Gets or sets an array containing the name for each <see cref="LightLevel"/>.
		/// </summary>
		public virtual string[] LightLevelNames { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="Stat"/>.
		/// </summary>
		public virtual IStat[] Stats { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="Spell"/>.
		/// </summary>
		public virtual ISpell[] Spells { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="Weapon"/>.
		/// </summary>
		public virtual IWeapon[] Weapons { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="Armor"/>.
		/// </summary>
		public virtual IArmor[] Armors { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="Direction"/>.
		/// </summary>
		public virtual IDirection[] Directions { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="ArtifactType"/>.
		/// </summary>
		public virtual IArtifactType[] ArtifactTypes { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="TriggerType"/>.
		/// </summary>
		public virtual ITriggerType[] TriggerTypes { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="ScriptType"/>.
		/// </summary>
		public virtual IScriptType[] ScriptTypes { get; set; }

		#endregion

		#region Public Methods

		#region Interface IEngine

		public virtual IPrep GetPreps(long index)
		{
			return Preps[index];
		}

		public virtual string GetArticles(long index)
		{
			return Articles[index];
		}

		public virtual string GetNumberStrings(long index)
		{
			return NumberStrings[index];
		}

		public virtual string GetFieldDescNames(long index)
		{
			return FieldDescNames[index];
		}

		public virtual string GetFieldDescNames(FieldDesc fieldDesc)
		{
			return Enum.IsDefined(typeof(FieldDesc), fieldDesc) ? GetFieldDescNames((long)fieldDesc) : UnknownName;
		}

		public virtual string GetStatusNames(long index)
		{
			return StatusNames[index];
		}

		public virtual string GetStatusNames(Status status)
		{
			return Enum.IsDefined(typeof(Status), status) ? GetStatusNames((long)status) : UnknownName;
		}

		public virtual string GetClothingNames(long index)
		{
			return ClothingNames[index];
		}

		public virtual string GetClothingNames(Clothing clothing)
		{
			return Enum.IsDefined(typeof(Clothing), clothing) ? GetClothingNames((long)clothing) : UnknownName;
		}

		public virtual string GetCombatCodeDescs(long index)
		{
			return CombatCodeDescs[index];
		}

		public virtual string GetCombatCodeDescs(CombatCode combatCode)
		{
			return Enum.IsDefined(typeof(CombatCode), combatCode) ? GetCombatCodeDescs((long)combatCode + 2) : UnknownName;
		}

		public virtual string GetContainerDisplayCodeDescs(long index)
		{
			return ContainerDisplayCodeDescs[index];
		}

		public virtual string GetContainerDisplayCodeDescs(ContainerDisplayCode containerDisplayCode)
		{
			return Enum.IsDefined(typeof(ContainerDisplayCode), containerDisplayCode) ? GetContainerDisplayCodeDescs((long)containerDisplayCode) : UnknownName;
		}

		public virtual string GetLightLevelNames(long index)
		{
			return LightLevelNames[index];
		}

		public virtual string GetLightLevelNames(LightLevel lightLevel)
		{
			return Enum.IsDefined(typeof(LightLevel), lightLevel) ? GetLightLevelNames((long)lightLevel) : UnknownName;
		}

		public virtual IStat GetStats(long index)
		{
			return Stats[index];
		}

		public virtual IStat GetStats(Stat stat)
		{
			return Enum.IsDefined(typeof(Stat), stat) ? GetStats((long)stat - 1) : null;
		}

		public virtual ISpell GetSpells(long index)
		{
			return Spells[index];
		}

		public virtual ISpell GetSpells(Spell spell)
		{
			return Enum.IsDefined(typeof(Spell), spell) ? GetSpells((long)spell - 1) : null;
		}

		public virtual IWeapon GetWeapons(long index)
		{
			return Weapons[index];
		}

		public virtual IWeapon GetWeapons(Weapon weapon)
		{
			return Enum.IsDefined(typeof(Weapon), weapon) ? GetWeapons((long)weapon - 1) : null;
		}

		public virtual IArmor GetArmors(long index)
		{
			return Armors[index];
		}

		public virtual IArmor GetArmors(Armor armor)
		{
			return Enum.IsDefined(typeof(Armor), armor) ? GetArmors((long)armor) : null;
		}

		public virtual IDirection GetDirections(long index)
		{
			return Directions[index];
		}

		public virtual IDirection GetDirections(Direction direction)
		{
			return Enum.IsDefined(typeof(Direction), direction) ? GetDirections((long)direction - 1) : null;
		}

		public virtual IArtifactType GetArtifactTypes(long index)
		{
			return ArtifactTypes[index];
		}

		public virtual IArtifactType GetArtifactTypes(ArtifactType artifactType)
		{
			return IsValidArtifactType(artifactType) ? GetArtifactTypes((long)artifactType) : null;
		}

		public virtual ITriggerType GetTriggerTypes(long index)
		{
			return TriggerTypes[index];
		}

		public virtual ITriggerType GetTriggerTypes(TriggerType triggerType)
		{
			return IsValidTriggerType(triggerType) ? GetTriggerTypes((long)triggerType) : null;
		}

		public virtual IScriptType GetScriptTypes(long index)
		{
			return ScriptTypes[index];
		}

		public virtual IScriptType GetScriptTypes(ScriptType scriptType)
		{
			return IsValidScriptType(scriptType) ? GetScriptTypes((long)scriptType) : null;
		}

		public virtual bool IsSuccess(RetCode rc)
		{
			return (long)rc >= (long)RetCode.Success;
		}

		public virtual bool IsFailure(RetCode rc)
		{
			return !IsSuccess(rc);
		}

		public virtual bool IsValidPluralType(PluralType pluralType)
		{
			return Enum.IsDefined(typeof(PluralType), pluralType) || (long)pluralType > 1000;
		}

		public virtual bool IsValidArtifactType(ArtifactType artifactType)
		{
			return Enum.IsDefined(typeof(ArtifactType), artifactType) && artifactType != ArtifactType.None;
		}

		public virtual bool IsValidTriggerType(TriggerType triggerType)
		{
			return Enum.IsDefined(typeof(TriggerType), triggerType) && triggerType != TriggerType.None;
		}

		public virtual bool IsValidScriptType(ScriptType scriptType)
		{
			return Enum.IsDefined(typeof(ScriptType), scriptType) && scriptType != ScriptType.None;
		}

		public virtual bool IsValidArtifactArmor(long armor)
		{
			return Enum.IsDefined(typeof(Armor), armor) && (armor == (long)Armor.ClothesShield || armor % 2 == 0);
		}

		public virtual bool IsValidMonsterArmor(long armor)
		{
			return armor >= 0;
		}

		public virtual bool IsValidMonsterCourage(long courage)
		{
			return courage >= 0 && courage <= 200;
		}

		public virtual bool IsValidMonsterFriendliness(Friendliness friendliness)
		{
			return Enum.IsDefined(typeof(Friendliness), friendliness) || IsValidMonsterFriendlinessPct(friendliness);
		}

		public virtual bool IsValidMonsterFriendlinessPct(Friendliness friendliness)
		{
			return (long)friendliness >= 100 && (long)friendliness <= 200;
		}

		public virtual bool IsValidDirection(Direction dir)
		{
			var module = GetModule();

			var numDirs = module != null ? module.NumDirs : 6;

			return (long)dir <= numDirs;
		}

		public virtual bool IsValidRoomUid01(long roomUid)
		{
			return roomUid != 0 && roomUid < 1001;
		}

		public virtual bool IsValidRoomDirectionDoorUid01(long roomUid)
		{
			return roomUid > 1000;
		}

		public virtual bool IsArtifactFieldStrength(long value)
		{
			return value >= 1000;
		}

		public virtual bool IsUnmovable(long weight)
		{
			return weight == -999 || weight == 999;
		}

		public virtual bool IsUnmovable01(long weight)
		{
			return weight == -999;
		}

		public virtual long GetWeightCarryableGronds(long hardiness)
		{
			return hardiness * 10;
		}

		public virtual long GetWeightCarryableDos(long hardiness)
		{
			return GetWeightCarryableGronds(hardiness) * 10;
		}

		public virtual long GetIntellectBonusPct(long intellect)
		{
			return (intellect - 13) * 2;
		}

		public virtual long GetCharmMonsterPct(long charisma)
		{
			return (charisma - 10) * 2;
		}

		public virtual long GetPluralTypeEffectUid(PluralType pluralType)
		{
			return (long)pluralType > 1000 ? (long)pluralType - 1000 : 0;
		}

		public virtual long GetArmorFactor(long armorUid, long shieldUid)
		{
			long af = 0;

			if (armorUid > 0)
			{
				var artifact = gADB[armorUid];

				Debug.Assert(artifact != null);

				var ac = artifact.Wearable;

				Debug.Assert(ac != null);

				var f = ac.Field1 / 2;

				if (f > 3)
				{
					f = 3;
				}
				
				af -= (10 * f);

				if (f == 3)
				{
					af -= 30;
				}
			}

			if (shieldUid > 0)
			{
				var artifact = gADB[shieldUid];

				Debug.Assert(artifact != null);

				var ac = artifact.Wearable;

				Debug.Assert(ac != null);

				af -= (5 * ac.Field1);
			}

			return af;
		}

		public virtual long GetCharismaFactor(long charisma)
		{
			var f = GetCharmMonsterPct(charisma);

			if (f > 28)
			{
				f = 28;
			}

			return f;
		}

		public virtual long GetMonsterFriendlinessPct(Friendliness friendliness)
		{
			return (long)friendliness - 100;
		}

		public virtual long GetArtifactFieldStrength(long value)
		{
			return value - 1000;
		}

		public virtual long GetMerchantAskPrice(double price, double rtio)
		{
			return (long)((price) * (rtio) + .5);
		}

		public virtual long GetMerchantBidPrice(double price, double rtio)
		{
			return (long)((price) / (rtio) + .5);
		}

		public virtual long GetMerchantAdjustedCharisma(long charisma)
		{
			var j = RollDice(1, 11, -6);

			var c2 = charisma + j;

			var stat = GetStats(Stat.Charisma);

			Debug.Assert(stat != null);

			if (c2 < stat.MinValue)
			{
				c2 = stat.MinValue;
			}
			else if (c2 > stat.MaxValue)
			{
				c2 = stat.MaxValue;
			}

			return c2;
		}

		public virtual double GetMerchantRtio(long charisma)
		{
			var stat = GetStats(Stat.Charisma);

			Debug.Assert(stat != null);

			var min = 0;

			var max = 1;

			var a = 0.70;

			var b = 1.30;

			var x = (double)((stat.MaxValue - stat.MinValue) - (charisma - stat.MinValue)) / (double)(stat.MaxValue - stat.MinValue);

			return (((b - a) * (x - min)) / (max - min)) + a;
		}

		public virtual bool IsCharYOrN(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'Y' || ch == 'N';
		}

		public virtual bool IsCharSOrTOrROrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'S' || ch == 'T' || ch == 'R' || ch == 'X';
		}

		public virtual bool IsChar0Or1(char ch)
		{
			return ch == '0' || ch == '1';
		}

		public virtual bool IsChar0To2(char ch)
		{
			return ch >= '0' && ch <= '2';
		}

		public virtual bool IsChar0To3(char ch)
		{
			return ch >= '0' && ch <= '3';
		}

		public virtual bool IsChar1To3(char ch)
		{
			return ch >= '1' && ch <= '3';
		}

		public virtual bool IsCharDigit(char ch)
		{
			return Char.IsDigit(ch);
		}

		public virtual bool IsCharDigitOrX(char ch)
		{
			return Char.IsDigit(ch) || Char.ToUpper(ch) == 'X';
		}

		public virtual bool IsCharPlusMinusDigit(char ch)
		{
			return ch == '+' || ch == '-' || Char.IsDigit(ch);
		}

		public virtual bool IsCharAlpha(char ch)
		{
			return Char.IsLetter(ch);
		}

		public virtual bool IsCharAlphaSpace(char ch)
		{
			return Char.IsLetter(ch) || Char.IsWhiteSpace(ch);
		}

		public virtual bool IsCharAlnum(char ch)
		{
			return Char.IsLetterOrDigit(ch);
		}

		public virtual bool IsCharAlnumSpace(char ch)
		{
			return Char.IsLetterOrDigit(ch) || Char.IsWhiteSpace(ch);
		}

		public virtual bool IsCharAlnumPeriodUnderscore(char ch)
		{
			return Char.IsLetterOrDigit(ch) || ch == '.' || ch == '_';
		}

		public virtual bool IsCharPrint(char ch)
		{
			return Char.IsControl(ch) == false;
		}

		public virtual bool IsCharPound(char ch)
		{
			return ch == '#';
		}

		public virtual bool IsCharQuote(char ch)
		{
			return ch == '\'' || ch == '`' || ch == '"';
		}

		public virtual bool IsCharAny(char ch)
		{
			return true;
		}

		public virtual bool IsCharAnyButDquoteCommaColon(char ch)
		{
			return ch != '"' && ch != ',' && ch != ':';
		}

		public virtual bool IsCharAnyButBackForwardSlash(char ch)
		{
			return ch != '\\' && ch != '/';
		}

		public virtual char ModifyCharToUpper(char ch)
		{
			return Char.ToUpper(ch);
		}

		public virtual char ModifyCharToNullOrX(char ch)
		{
			return Char.ToUpper(ch) == 'X' ? 'X' : '\0';
		}

		public virtual char ModifyCharToNull(char ch)
		{
			return '\0';
		}

		public virtual Direction GetDirection(string directionName)
		{
			Direction result = 0;

			Debug.Assert(!string.IsNullOrWhiteSpace(directionName));

			var module = GetModule();

			var numDirs = module != null ? module.NumDirs : 6;

			var directionValues = EnumUtil.GetValues<Direction>();

			for (var i = 0; i < numDirs; i++)
			{
				if (GetDirections(i).PrintedName.Equals(directionName, StringComparison.OrdinalIgnoreCase))
				{
					result = directionValues[i];

					break;
				}
			}

			if (result == 0)
			{
				for (var i = 0; i < numDirs; i++)
				{
					if (GetDirections(i).Name.Equals(directionName, StringComparison.OrdinalIgnoreCase))
					{
						result = directionValues[i];

						break;
					}
				}
			}

			if (result == 0)
			{
				for (var i = 0; i < numDirs; i++)
				{
					if (GetDirections(i).PrintedName.StartsWith(directionName, StringComparison.OrdinalIgnoreCase) || GetDirections(i).PrintedName.EndsWith(directionName, StringComparison.OrdinalIgnoreCase))
					{
						result = directionValues[i];

						break;
					}
				}
			}

			if (result == 0)
			{
				for (var i = 0; i < numDirs; i++)
				{
					if (GetDirections(i).Name.StartsWith(directionName, StringComparison.OrdinalIgnoreCase) || GetDirections(i).Name.EndsWith(directionName, StringComparison.OrdinalIgnoreCase))
					{
						result = directionValues[i];

						break;
					}
				}
			}

			return result;
		}

		public virtual ContainerType GetContainerType(ArtifactType artifactType)
		{
			return artifactType == ArtifactType.InContainer ? ContainerType.In :
						artifactType == ArtifactType.OnContainer ? ContainerType.On :
						artifactType == ArtifactType.UnderContainer ? ContainerType.Under :
						artifactType == ArtifactType.BehindContainer ? ContainerType.Behind :
						(ContainerType)(-1);
		}

		public virtual IConfig GetConfig()
		{
			return Globals?.Database?.ConfigTable?.Records?.FirstOrDefault();
		}

		public virtual IGameState GetGameState()
		{
			return Globals?.Database?.GameStateTable?.Records?.FirstOrDefault();
		}

		public virtual IModule GetModule()
		{
			return Globals?.Database?.ModuleTable?.Records?.FirstOrDefault();
		}

		public virtual T GetRandomElement<T>(T[] array, Func<long> indexFunc = null)
		{
			var result = default(T);

			Debug.Assert(array != null && array.Length > 0);

			if (indexFunc == null)
			{
				indexFunc = () => RollDice(1, array.Length, -1);
			}

			var i = indexFunc();

			if (i >= 0 && i < array.Length)
			{
				result = array[i];
			}

			return result;
		}

		public virtual T EvalFriendliness<T>(Friendliness friendliness, T enemyValue, T neutralValue, T friendValue)
		{
			return friendliness == Friendliness.Enemy ? enemyValue : friendliness == Friendliness.Neutral ? neutralValue : friendValue;
		}

		public virtual T EvalGender<T>(Gender gender, T maleValue, T femaleValue, T neutralValue)
		{
			return gender == Gender.Male ? maleValue : gender == Gender.Female ? femaleValue : neutralValue;
		}

		public virtual T EvalContainerType<T>(ContainerType containerType, T inValue, T onValue, T underValue, T behindValue)
		{
			return containerType == ContainerType.On ? onValue : containerType == ContainerType.Under ? underValue : containerType == ContainerType.Behind ? behindValue : inValue;
		}

		public virtual T EvalRoomType<T>(RoomType roomType, T indoorsValue, T outdoorsValue)
		{
			return roomType == RoomType.Indoors ? indoorsValue : outdoorsValue;
		}

		public virtual T EvalLightLevel<T>(LightLevel lightLevel, T darkValue, T lightValue)
		{
			return lightLevel == LightLevel.Dark ? darkValue : lightValue;
		}

		public virtual T EvalPlural<T>(bool isPlural, T singularValue, T pluralValue)
		{
			return isPlural ? pluralValue : singularValue;
		}

		public virtual string BuildPrompt(long bufSize, char fillChar, long number, string msg, string emptyVal)
		{
			StringBuilder buf;
			int i, p, q, sz;
			string result;

			if (bufSize < 8 || number < 0)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (fillChar == '\0')
			{
				fillChar = ' ';
			}

			buf = new StringBuilder(Constants.BufSize);

			buf.Append(fillChar, (int)bufSize);

			p = 0;

			q = (int)bufSize;

			if (number > 0)
			{
				buf.Replace(p, 4, string.Format("{0,2}. ", number));

				p += 4;
			}

			if (msg != null)
			{
				sz = msg.Length;

				for (i = 0; i < sz && p < q; i++)
				{
					buf[p++] = msg[i];
				}
			}

			if (emptyVal != null)
			{
				sz = emptyVal.Length;

				p = Math.Max(q - (sz + 4), 0);

				buf.Replace(p, Math.Min(sz + 4, q), string.Format("[{0}]{1} ", emptyVal, fillChar == ' ' ? ':' : fillChar));
			}
			else
			{
				p = q - 2;

				buf.Replace(p, 2, string.Format("{0} ", fillChar == ' ' ? ':' : fillChar));
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string BuildValue(long bufSize, char fillChar, long offset, long longVal, string stringVal, string lookupMsg)
		{
			StringBuilder buf;
			int p, q, sz;
			string result;
			string s;

			if (bufSize < 8 || offset < 0 || offset > bufSize - 1)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (fillChar == '\0')
			{
				fillChar = ' ';
			}

			buf = new StringBuilder(Constants.BufSize);

			buf.Append(fillChar, (int)bufSize);

			p = 0;

			q = (int)bufSize;

			s = stringVal ?? longVal.ToString();

			sz = Math.Min(s.Length, q);

			buf.Replace(p, sz, s);

			p += sz;

			if (lookupMsg != null)
			{
				p = (int)offset;

				s = string.Format("[{0}]", lookupMsg.Length > (q - p) - 2 ? lookupMsg.Substring(0, (q - p) - 2) : lookupMsg);

				sz = Math.Min(s.Length, q - p);

				buf.Replace(p, sz, s);

				p += sz;
			}

			buf.Length = p;

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string WordWrap(string str, StringBuilder buf, long margin, IWordWrapArgs args, bool clearBuf = true)
		{
			int i, p, q, r;
			int currMargin;
			bool hyphenSeen;
			string result;
			string line;

			if (str == null || buf == null || margin < 1)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (clearBuf)
			{
				buf.Clear();
			}

			if (args != null)
			{
				if (args.CurrColumn == margin)
				{
					buf.Append(Environment.NewLine);

					args.CurrColumn = 0;
				}

				currMargin = (int)(margin - args.CurrColumn);
			}
			else
			{
				currMargin = (int)margin;
			}

			var delims = Environment.NewLine.Length > 1 ?
					new string[] { Environment.NewLine, Environment.NewLine[1].ToString() } :
					new string[] { Environment.NewLine };

			var lines = str.Split(delims, StringSplitOptions.None);

			for (i = 0; i < lines.Length; i++)
			{
				if (i > 0)
				{
					buf.Append(Environment.NewLine);
				}

				line = lines[i];

				p = 0;

				q = line.Length;

				while (true)
				{
					if (p + currMargin >= q)
					{
						buf.Append(line.Substring(p));

						if (args != null)
						{
							args.CurrColumn = (q - p);
						}

						p += (q - p);

						break;
					}
					else
					{
						r = p + currMargin;

						hyphenSeen = false;

						while (r > p && !Char.IsWhiteSpace(line[r]) && line[r] != '-')
						{
							r--;
						}

						if (r > p)
						{
							if (line[r] == '-')
							{
								hyphenSeen = true;
							}

							buf.Append(line.Substring(p, (r - p)));

							p += (r - p) + 1;

							if (p < q && Char.IsWhiteSpace(line[p]) && (p + 2 < q) && (!Char.IsWhiteSpace(line[p + 1]) || !Char.IsWhiteSpace(line[p + 2])))
							{
								p++;
							}

							if (p < q && Char.IsWhiteSpace(line[p]) && (p + 1 < q) && !Char.IsWhiteSpace(line[p + 1]))
							{
								p++;
							}

							if (hyphenSeen)
							{
								buf.Append('-');
							}
						}
						else
						{
							if (r > 0 || args == null || (!Char.IsWhiteSpace(args.LastChar) && args.LastChar != '-'))
							{
								buf.Append(line.Substring(p, currMargin));

								p += currMargin;
							}
						}

						buf.Append(Environment.NewLine);
					}

					currMargin = (int)margin;
				}
			}

			if (args != null)
			{
				args.LastChar = buf.Length > 0 ? buf[buf.Length - 1] : '\0';
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string WordWrap(string str, StringBuilder buf, bool clearBuf = true)
		{
			var config = GetConfig();

			return WordWrap(str, buf, config != null ? config.WordWrapMargin : Constants.RightMargin, null, clearBuf);
		}

		public virtual string LineWrap(string str, StringBuilder buf, long startColumn, bool clearBuf = true)
		{
			string result;

			var config = GetConfig();

			var rightMargin = config != null ? config.WordWrapMargin : Constants.RightMargin;

			if (str == null || buf == null || startColumn < 0 || startColumn >= rightMargin)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (clearBuf)
			{
				buf.Clear();
			}

			var chunkSize = rightMargin - startColumn;

			while (str.Length > chunkSize)
			{
				buf.AppendFormat("{0}{1}", str.Substring(0, (int)chunkSize), Environment.NewLine);

				str = str.Substring((int)chunkSize);

				chunkSize = rightMargin;
			}

			buf.Append(str);

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetStringFromNumber(long num, bool addSpace, StringBuilder buf)
		{
			string result;

			if (buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			buf.SetFormat("{0}{1}", 
				num >= 0 && num <= 10 ? GetNumberStrings(num) : num.ToString(), 
				addSpace ? " " : "");

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual long GetNumberFromString(string str)
		{
			long result = -1;
			long i;

			if (string.IsNullOrWhiteSpace(str))
			{
				// PrintError

				goto Cleanup;
			}

			for (i = 0; i < NumberStrings.Length; i++)
			{
				if (NumberStrings[i].Equals(str, StringComparison.OrdinalIgnoreCase))
				{
					result = i;

					goto Cleanup;
				}
			}

			if (long.TryParse(str, out i))
			{
				result = i;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		public virtual string GetContainerContentsDesc(IArtifact artifact)
		{
			var artTypes = new ArtifactType[] { ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer };

			var result = "";

			if (artifact == null)
			{
				// PrintError

				goto Cleanup;
			}

			var buf = new StringBuilder(Constants.BufSize);

			var ac = artifact.Categories.FirstOrDefault(ac01 => ac01 != null && artTypes.Contains(ac01.Type) && ac01.Field5 == (long)ContainerDisplayCode.ArtifactNameSomeStuff && (ac01.Type != ArtifactType.InContainer || ac01.IsOpen() || artifact.ShouldExposeInContentsWhenClosed()));

			if (ac == null)
			{
				ac = artifact.Categories.FirstOrDefault(ac01 => ac01 != null && artTypes.Contains(ac01.Type) && ac01.Field5 == (long)ContainerDisplayCode.SomethingSomeStuff && (ac01.Type != ArtifactType.InContainer || ac01.IsOpen() || artifact.ShouldExposeInContentsWhenClosed()));
			}

			if (ac != null)
			{
				var containerType = GetContainerType(ac.Type);

				var contentsList = artifact.GetContainedList(containerType: containerType);

				var showCharOwned = !artifact.IsCarriedByCharacter() && !artifact.IsWornByCharacter();

				if (contentsList.Count > 0)
				{
					result = string.Format
					(
						" with {0} {1} {2}",
						contentsList.Count > 1 || contentsList[0].IsPlural ? "some stuff" : ac.Field5 == (long)ContainerDisplayCode.ArtifactNameSomeStuff ? contentsList[0].GetArticleName(false, showCharOwned, false, false, buf) : "something",
						EvalContainerType(containerType, "inside", "on", "under", "behind"),
						artifact.EvalPlural("it", "them")
					);
				}
			}

		Cleanup:

			return result;
		}

		public virtual RetCode RollDice(long numDice, long numSides, ref long[] dieRolls)
		{
			RetCode rc;

			if (numDice < 0 || numSides < 0 || dieRolls == null || dieRolls.Length < numDice)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			for (var i = 0; i < numDice; i++)
			{
				dieRolls[i] = numSides > 0 ? Rand.Next((int)numSides) + 1 : 0;
			}

		Cleanup:

			return rc;
		}

		public virtual long RollDice(long numDice, long numSides, long modifier)
		{
			var result = 0L;

			var dieRolls = new long[numDice > 0 ? numDice : 1];

			var rc = RollDice(numDice, numSides, ref dieRolls);

			if (IsSuccess(rc))
			{
				result = dieRolls.Sum() + modifier;
			}

			return result;
		}

		public virtual RetCode SumHighestRolls(long[] dieRolls, long numRollsToSum, ref long result)
		{
			RetCode rc;

			if (dieRolls == null || numRollsToSum < 0 || numRollsToSum > dieRolls.Length)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			Array.Sort(dieRolls);

			result = dieRolls.Skip((int)(dieRolls.Length - numRollsToSum)).Take((int)numRollsToSum).Sum();

		Cleanup:

			return rc;
		}

		public virtual string Capitalize(string str)
		{
			StringBuilder buf;
			bool spaceSeen;
			int p, q, sz;
			string result;

			if (str == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			spaceSeen = true;

			buf = new StringBuilder(Constants.BufSize);

			buf.Append(str);

			sz = buf.Length;

			for (p = 0, q = sz; p < q; p++)
			{
				if (spaceSeen)
				{
					if (Char.IsLetter(buf[p]))
					{
						buf[p] = Char.ToUpper(buf[p]);

						spaceSeen = false;
					}
				}
				else
				{
					if (Char.IsWhiteSpace(buf[p]))
					{
						spaceSeen = true;
					}
				}
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual void UnlinkOnFailure()
		{
			try
			{
				Globals.File.Delete("EAMONCFG.DAT");
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}

			try
			{
				Globals.File.Delete("FRESHMEAT.DAT");
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}

			try
			{
				Globals.File.Delete("SAVEGAME.DAT");
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}
		}

		public virtual void TruncatePluralTypeEffectDesc(PluralType pluralType, long maxSize)
		{
			if (maxSize < 0)
			{
				// PrintError

				goto Cleanup;
			}

			var effectUid = GetPluralTypeEffectUid(pluralType);

			if (effectUid > 0)
			{
				var effect = gEDB[effectUid];

				if (effect != null && effect.Desc.Length > maxSize)
				{
					effect.Desc = effect.Desc.Substring(0, (int)maxSize);
				}
			}

		Cleanup:

			;
		}

		public virtual void TruncatePluralTypeEffectDesc(IEffect effect)
		{
			if (effect == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (effect.Desc.Length > Constants.ArtNameLen && Globals.Database.ArtifactTable.Records.FirstOrDefault(a => GetPluralTypeEffectUid(a.PluralType) == effect.Uid) != null)
			{
				effect.Desc = effect.Desc.Substring(0, Constants.ArtNameLen);
			}

			if (effect.Desc.Length > Constants.MonNameLen && Globals.Database.MonsterTable.Records.FirstOrDefault(m => GetPluralTypeEffectUid(m.PluralType) == effect.Uid) != null)
			{
				effect.Desc = effect.Desc.Substring(0, Constants.MonNameLen);
			}

		Cleanup:

			;
		}

		public virtual RetCode SplitPath(string fullPath, ref string directory, ref string fileName, ref string extension, bool appendDirectorySeparatorChar = true)
		{
			RetCode rc;

			if (fullPath == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			directory = Globals.Path.GetDirectoryName(fullPath);

			fileName = Globals.Path.GetFileNameWithoutExtension(fullPath);

			extension = Globals.Path.GetExtension(fullPath);

			var directorySeparatorString = Globals.Path.DirectorySeparatorChar.ToString();

			if (appendDirectorySeparatorChar && directory.Length > 0 && !directory.EndsWith(directorySeparatorString))
			{
				directory += directorySeparatorString;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode StripPrepsAndArticles(StringBuilder buf, ref bool mySeen)
		{
			RetCode rc;
			bool found;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			mySeen = false;

			while (true)
			{
				found = false;

				foreach (var p in Preps)
				{
					var s = p.Name + " ";

					if (buf.StartsWith(s, true))
					{
						buf.Remove(0, s.Length);

						buf.TrimStart();

						found = true;

						break;
					}
				}

				foreach (var a in Articles)
				{
					var s = a + " ";

					if (buf.StartsWith(s, true))
					{
						buf.Remove(0, s.Length);

						buf.TrimStart();

						if (a == "my" || a == "your")
						{
							mySeen = true;
						}

						found = true;

						break;
					}
				}

				if (!found)
				{
					break;
				}
			}

		Cleanup:

			return rc;
		}

		public virtual void PrintTitle(string title, bool inBox)
		{
			long spaces;
			long size;

			if (string.IsNullOrWhiteSpace(title))
			{
				// PrintError

				goto Cleanup;
			}

			size = title.Length;

			if (inBox)
			{
				gOut.Write("{0}{1}|",
					Globals.LineSep,
					Environment.NewLine);
			}

			spaces = ((Constants.RightMargin - 2) / 2) - (size / 2);

			gOut.Write("{0}{1}", new string(' ', (int)spaces), title);

			if (inBox)
			{
				gOut.Write("{0}|{1}{2}",
					new string(' ', (int)((Constants.RightMargin - 1) - (1 + spaces + size))),
					Environment.NewLine,
					Globals.LineSep);
			}

			gOut.WriteLine();

		Cleanup:

			;
		}

		public virtual void PrintEffectDesc(IEffect effect, bool printFinalNewLine = true)
		{
			gOut.Write("{0}{1}{2}", Environment.NewLine, effect != null ? effect.Desc : "???", printFinalNewLine ? Environment.NewLine : "");
		}

		public virtual void PrintEffectDesc(long effectUid, bool printFinalNewLine = true)
		{
			var effect = gEDB[effectUid];

			PrintEffectDesc(effect, printFinalNewLine);
		}

		public virtual void PrintZapDirectHit()
		{
			gOut.Print("ZAP!  Direct hit!");
		}

		public virtual RetCode ValidateRecordsAfterDatabaseLoaded()
		{
			RetCode rc;

			rc = RetCode.Success;

			// Note: currently only validating Monster records but could do any type if needed

			var monsterHelper = Globals.CreateInstance<IMonsterHelper>();

			var monsterList = Globals.Database.MonsterTable.Records.ToList();

			long i = 1;

			foreach (var r in monsterList)
			{
				monsterHelper.Record = r;

				if (monsterHelper.ValidateRecordAfterDatabaseLoaded() == false)
				{
					rc = RetCode.Failure;

					Globals.Error.WriteLine("{0}Error: expected valid [{1} value], found [{2}]", Environment.NewLine, monsterHelper.GetName(monsterHelper.ErrorFieldName), monsterHelper.GetValue(monsterHelper.ErrorFieldName) ?? "null");

					Globals.Error.WriteLine("Error: ValidateAfterDatabaseLoaded function call failed for Monster record number {0}", i);

					goto Cleanup;
				}

				i++;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode StatDisplay(IStatDisplayArgs args)
		{
			StringBuilder buf01, buf02;
			RetCode rc;
			long i, j;

			IWeapon weapon;
			ISpell spell;

			if (args == null || args.Character == null || args.Monster == null || args.ArmorString == null || args.SpellAbilities == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var origPunctSpaceCode = gOut.PunctSpaceCode;

			gOut.PunctSpaceCode = PunctSpaceCode.None;

			buf01 = new StringBuilder(Constants.BufSize);

			buf02 = new StringBuilder(Constants.BufSize);

			var omitSkillStats = Globals.IsRulesetVersion(15, 25) && GetGameState() != null;

			gOut.Print("{0,-36}Gender: {1,-9}Damage Taken: {2}/{3}",
				args.Monster.Name.ToUpper(),
				args.Character.EvalGender("Male", "Female", "Neutral"),
				args.Monster.DmgTaken,
				args.Monster.Hardiness);

			var ibp = GetIntellectBonusPct(args.Character.GetStats(Stat.Intellect));

			buf01.AppendFormat("{0}{1}{2}%)",
				"(Learning: ",
				ibp > 0 ? "+" : "",
				ibp);

			buf02.AppendFormat("{0}{1}",
				args.Speed > 0 ? args.Monster.Agility / 2 : args.Monster.Agility,
				args.Speed > 0 ? "x2" : "");

			gOut.WriteLine("{0}{1}{2,-2}{3,20}{4,15}{5}{0}{6}{7,-3}{8,34}{9,-2}{10,15}{11}{12}%)",
				Environment.NewLine,
				"Intellect:  ", args.Character.GetStats(Stat.Intellect),
				buf01.ToString(),
				"Agility :  ", buf02.ToString(),
				"Hardiness:  ", args.Monster.Hardiness,
				"Charisma:  ", args.Character.GetStats(Stat.Charisma),
				"(Charm Mon: ",
				args.CharmMon > 0 ? "+" : "",
				args.CharmMon);

			if (!omitSkillStats)
			{
				gOut.Write("{0}{1}{2,39}",
					Environment.NewLine,
					"Weapon Abilities:",
					"Spell Abilities:");

				var weaponValues = EnumUtil.GetValues<Weapon>();

				var spellValues = EnumUtil.GetValues<Spell>();

				i = Math.Min((long)weaponValues[0], (long)spellValues[0]);

				j = Math.Max((long)weaponValues[weaponValues.Count - 1], (long)spellValues[spellValues.Count - 1]);

				while (i <= j)
				{
					gOut.WriteLine();

					if (Enum.IsDefined(typeof(Weapon), i))
					{
						weapon = GetWeapons((Weapon)i);

						Debug.Assert(weapon != null);

						gOut.Write(" {0,-5}: {1,3}%",
							weapon.Name,
							args.Character.GetWeaponAbilities(i));
					}
					else
					{
						gOut.Write("{0,12}", "");
					}

					if (Enum.IsDefined(typeof(Spell), i))
					{
						spell = GetSpells((Spell)i);

						Debug.Assert(spell != null);

						gOut.Write("{0,29}{1,-5}: {2,3}% / {3}%",
							"",
							spell.Name,
							args.GetSpellAbilities(i),
							args.Character.GetSpellAbilities(i));
					}

					i++;
				}
			}

			gOut.WriteLine("{0}{0}{1}{2,-30}{3}{4,-6}",
				Environment.NewLine,
				"Gold: ",
				args.Character.HeldGold,
				"In bank: ",
				args.Character.BankGold);

			gOut.Print("Armor:  {0}{1}",
				args.ArmorString.PadTRight(28, ' '),
				!omitSkillStats ? string.Format(" Armor Expertise:  {0}%", args.Character.ArmorExpertise) : "");

			var wcg = GetWeightCarryableGronds(args.Monster.Hardiness);

			gOut.Print("Weight carried: {0}/{1} Gronds (One Grond = Ten DOS)",
				args.Weight,
				wcg);

			gOut.PunctSpaceCode = origPunctSpaceCode;

		Cleanup:

			return rc;
		}

		public virtual RetCode GetRecordNameList(IList<IGameBase> recordList, ArticleType articleType, bool showCharOwned, StateDescDisplayCode stateDescCode, bool showContents, bool groupCountOne, StringBuilder buf)
		{
			StringBuilder buf01;
			RetCode rc;
			long i;

			if (recordList == null || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf01 = new StringBuilder(Constants.BufSize);

			for (i = 0; i < recordList.Count; i++)
			{
				var r = recordList[(int)i];

				var a = r as IArtifact;

				var m = r as IMonster;

				var contentsDesc = "";

				if (showContents && a != null)
				{
					contentsDesc = GetContainerContentsDesc(a);
				}

				var showStateDesc = stateDescCode == StateDescDisplayCode.AllStateDescs;

				if (!showStateDesc && a != null)
				{
					showStateDesc = stateDescCode == StateDescDisplayCode.SideNotesOnly && a.IsStateDescSideNotes();
				}

				if (!showStateDesc && m != null)
				{
					showStateDesc = stateDescCode == StateDescDisplayCode.SideNotesOnly && m.IsStateDescSideNotes();
				}

				buf.AppendFormat("{0}{1}{2}",
					i == 0 ? "" : i == recordList.Count - 1 && recordList.Count > 2 ? ", and " : i == recordList.Count - 1 ? " and " : ", ",
					r.GetDecoratedName
					(
						"Name",
						articleType == ArticleType.None || articleType == ArticleType.The ? articleType : r.ArticleType,
						false,
						showCharOwned,
						showStateDesc && contentsDesc.Length == 0,
						groupCountOne,
						buf01
					),
					contentsDesc
				);
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode GetRecordNameCount(IList<IGameBase> recordList, string name, bool exactMatch, ref long count)
		{
			RetCode rc;

			if (recordList == null || string.IsNullOrWhiteSpace(name))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			count = 0;

			foreach (var r in recordList)
			{
				if (exactMatch)
				{
					if (r.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
					{
						count++;
					}
				}
				else
				{
					if (r.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
					{
						count++;
					}
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode ListRecords(IList<IGameBase> recordList, bool capitalize, bool showExtraInfo, StringBuilder buf)
		{
			RetCode rc;
			long i;

			if (recordList == null || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			for (i = 0; i < recordList.Count; i++)
			{
				var r = recordList[(int)i];

				var a = r as IArtifact;

				if (showExtraInfo && a != null && a.GeneralWeapon != null)
				{
					var ac = a.GeneralWeapon;

					Debug.Assert(ac != null);

					var weapon = GetWeapons((Weapon)ac.Field2);

					Debug.Assert(weapon != null);

					buf.AppendFormat("{0}{1,2}. {2} ({3,-8}/{4,3}%/{5,2}D{6,-2}/{7}H)",
						Environment.NewLine,
						i + 1,
						capitalize ? Capitalize(a.Name.PadTRight(31, ' ')) : a.Name.PadTRight(31, ' '),
						weapon.Name,
						ac.Field1,
						ac.Field3,
						ac.Field4,
						ac.Field5);
				}
				else
				{
					buf.AppendFormat("{0}{1,2}. {2}",
						Environment.NewLine,
						i + 1,
						capitalize ? Capitalize(r.Name) : r.Name);
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse, ref long invalidUid)
		{
			StringBuilder buf01, buf02, srcBuf, dstBuf;
			MatchCollection matches;
			long numStars, numAts;
			long m, currUid;
			IEffect effect;
			Func<string> func;
			RetCode rc;
			int i;

			if (str == null || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			invalidUid = 0;

			if (str.Length < 4)
			{
				buf.Append(str);

				goto Cleanup;
			}

			if ((str[0] == '*' || (resolveFuncs && str[0] == '@')) && Char.IsDigit(str[1]) && Char.IsDigit(str[2]) && Char.IsDigit(str[3]) && long.TryParse(str.Substring(1, 3), out currUid) == true)
			{
				if (str[0] == '*')
				{
					effect = gEDB[currUid];

					if (effect != null)
					{
						str = " " + str;
					}
				}
				else
				{
					if (MacroFuncs.TryGetValue(currUid, out func) && func != null)
					{
						str = " " + str;
					}
				}
			}

			matches = Regex.Matches(str, resolveFuncs ? Constants.ResolveUidMacroRegexPattern : Constants.ResolveEffectRegexPattern);

			if (matches.Count == 0)
			{
				buf.Append(str);

				goto Cleanup;
			}

			buf01 = new StringBuilder(Constants.BufSize);

			buf02 = new StringBuilder(Constants.BufSize);

			buf01.Append(str);

			srcBuf = buf01;

			dstBuf = buf02;

			m = 0;

			do
			{
				dstBuf.Clear();

				i = 0;

				foreach (Match match in matches)
				{
					foreach (Capture capture in match.Captures)
					{
						effect = null;

						func = null;

						numStars = 0;

						numAts = 0;

						if (capture.Value[1] == '*')
						{
							numStars = 1 + (capture.Value[0] == '*' ? 1 : 0);
						}
						else
						{
							numAts = 1 + (capture.Value[0] == '@' ? 1 : 0);
						}

						if (long.TryParse(capture.Value.Substring(2), out currUid) == false || currUid < 0)
						{
							currUid = 0;
						}

						if (numStars > 0)
						{
							effect = gEDB[currUid];
						}
						else
						{
							if (MacroFuncs.TryGetValue(currUid, out func) == false)
							{
								func = null;
							}
						}

						dstBuf.Append(srcBuf.ToString().Substring(i, (capture.Index + (numStars == 1 || numAts == 1 ? 1 : 0)) - i));

						if (numStars > 0)
						{
							if (effect != null)
							{
								dstBuf.AppendFormat("{0}{1}{2}",
									numStars == 1 ? Environment.NewLine : "",
									numStars == 1 ? Environment.NewLine : "",
									effect.Desc);
							}
							else
							{
								if (invalidUid == 0 || (currUid > 0 && currUid < invalidUid))
								{
									invalidUid = currUid;
								}

								dstBuf.Append(capture.Value.Substring(numStars == 1 ? 1 : 0));
							}
						}
						else
						{
							if (func != null)
							{
								var desc = func();

								dstBuf.AppendFormat("{0}{1}{2}",
									numAts == 1 && !string.IsNullOrWhiteSpace(desc) ? Environment.NewLine : "",
									numAts == 1 && !string.IsNullOrWhiteSpace(desc) ? Environment.NewLine : "",
									desc);
							}
							else
							{
								dstBuf.Append(capture.Value.Substring(numAts == 1 ? 1 : 0));
							}
						}

						i = capture.Index + capture.Length;
					}
				}

				dstBuf.Append(srcBuf.ToString().Substring(i));

				if (++m >= Constants.MaxRecursionLevel)
				{
					recurse = false;
				}

				if (recurse)
				{
					matches = Regex.Matches(dstBuf.ToString(), resolveFuncs ? Constants.ResolveUidMacroRegexPattern : Constants.ResolveEffectRegexPattern);

					if (matches.Count > 0)
					{
						if (srcBuf == buf01)
						{
							srcBuf = buf02;

							dstBuf = buf01;
						}
						else
						{
							srcBuf = buf01;

							dstBuf = buf02;
						}
					}
				}
			}
			while (recurse && matches.Count > 0);

			buf.Append(dstBuf);

		Cleanup:

			return rc;
		}

		public virtual RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse)
		{
			long invalidUid = 0;

			return ResolveUidMacros(str, buf, resolveFuncs, recurse, ref invalidUid);
		}

		public virtual double GetWeaponPriceOrValue(string name, long complexity, Weapon type, long dice, long sides, long numHands, bool calcPrice, ref bool isMarcosWeapon)
		{
			double wp;

			wp = 0.0;

			isMarcosWeapon = false;

			if (!string.IsNullOrWhiteSpace(name))
			{
				name = name.Trim().TrimEnd('#');
			}

			if (string.IsNullOrWhiteSpace(name) || !Enum.IsDefined(typeof(Weapon), type) || dice < 1 || sides < 1)
			{
				// PrintError

				goto Cleanup;
			}

			var weapon = GetWeapons(type);

			Debug.Assert(weapon != null);

			wp = weapon.MarcosPrice;

			if (complexity >= 0 && complexity < 10)
			{
				wp *= 0.80;
			}
			else if (complexity < 0)
			{
				wp *= 0.60;
			}

			isMarcosWeapon = name.Equals(weapon.MarcosName ?? weapon.Name, StringComparison.OrdinalIgnoreCase) && (complexity == -10 || complexity == 0 || complexity == 10) && dice == weapon.MarcosDice && sides == weapon.MarcosSides && numHands == weapon.MarcosNumHands;

			if (!isMarcosWeapon)
			{
				if (complexity > 10)
				{
					wp += (calcPrice ? 40 : 5);
				}

				if (complexity > 15)
				{
					wp += (calcPrice ? 80 : 10);
				}

				if (complexity > 25)
				{
					wp += (calcPrice ? 320 : 20);
				}

				if (complexity > 35)
				{
					wp += (calcPrice ? 960 : 50);
				}

				if (dice * sides > 10)
				{
					wp += (calcPrice ? 40 : 5);
				}

				if (dice * sides > 15)
				{
					wp += (calcPrice ? 80 : 10);
				}

				if (dice * sides > 25)
				{
					wp += (calcPrice ? 320 : 20);
				}

				if (dice * sides > 35)
				{
					wp += (calcPrice ? 960 : 50);
				}
			}

		Cleanup:

			return wp;
		}

		public virtual double GetWeaponPriceOrValue(ICharacterArtifact weapon, bool calcPrice, ref bool isMarcosWeapon)
		{
			Debug.Assert(weapon != null);

			return GetWeaponPriceOrValue(weapon.Name, weapon.Field1, (Weapon)weapon.Field2, weapon.Field3, weapon.Field4, weapon.Field5, calcPrice, ref isMarcosWeapon);
		}

		public virtual double GetArmorPriceOrValue(Armor armor, bool calcPrice, ref bool isMarcosArmor)
		{
			double ap;

			ap = 0.0;

			isMarcosArmor = false;

			if (!Enum.IsDefined(typeof(Armor), armor))
			{
				// PrintError

				goto Cleanup;
			}

			var armor01 = ((long)armor / 2) * 2;

			if (armor01 > 0)
			{
				var armor02 = GetArmors((Armor)armor01);

				Debug.Assert(armor02 != null);

				if (armor02.MarcosPrice > 0)
				{
					if (calcPrice)
					{
						ap = armor02.MarcosPrice;
					}
					else
					{
						ap = armor02.ArtifactValue;
					}

					isMarcosArmor = true;
				}
				else
				{
					if (calcPrice)
					{
						armor02 = GetArmors(Armor.PlateMail);

						Debug.Assert(armor02 != null);

						ap = armor02.MarcosPrice + (((armor01 - (long)Armor.PlateMail) / 2) * 1000);
					}
					else
					{
						ap = armor02.ArtifactValue;
					}
				}
			}

		Cleanup:

			return ap;
		}

		public virtual void AppendFieldDesc(FieldDesc fieldDesc, StringBuilder buf, StringBuilder fullDesc, StringBuilder briefDesc)
		{
			AppendFieldDesc(fieldDesc, buf, fullDesc != null ? fullDesc.ToString() : null, briefDesc != null ? briefDesc.ToString() : null);
		}

		public virtual void AppendFieldDesc(FieldDesc fieldDesc, StringBuilder buf, string fullDesc, string briefDesc)
		{
			Debug.Assert(buf != null && fullDesc != null);

			if (briefDesc != null)
			{
				if (fieldDesc == FieldDesc.Full)
				{
					buf.AppendFormat("{0}{1}{0}{0}{2}{0}", Environment.NewLine, fullDesc, briefDesc);
				}
				else if (fieldDesc == FieldDesc.Brief)
				{
					buf.AppendPrint("{0}", briefDesc);
				}
			}
			else
			{
				if (fieldDesc == FieldDesc.Full)
				{
					buf.AppendPrint("{0}", fullDesc);
				}
			}
		}

		public virtual void CopyCharacterArtifactFields(ICharacterArtifact destCa, ICharacterArtifact sourceCa)
		{
			Debug.Assert(destCa != null);

			Debug.Assert(sourceCa != null);

			destCa.Name = Globals.CloneInstance(sourceCa.Name);

			destCa.Desc = Globals.CloneInstance(sourceCa.Desc);

			destCa.IsPlural = sourceCa.IsPlural;

			destCa.PluralType = sourceCa.PluralType;

			destCa.ArticleType = sourceCa.ArticleType;

			destCa.Value = sourceCa.Value;

			destCa.Weight = sourceCa.Weight;

			destCa.Type = sourceCa.Type;

			destCa.Field1 = sourceCa.Field1;

			destCa.Field2 = sourceCa.Field2;

			destCa.Field3 = sourceCa.Field3;

			destCa.Field4 = sourceCa.Field4;

			destCa.Field5 = sourceCa.Field5;
		}

		public virtual void CopyArtifactCategoryFields(IArtifactCategory destAc, IArtifactCategory sourceAc)
		{
			Debug.Assert(destAc != null);

			Debug.Assert(sourceAc != null);

			destAc.Field1 = sourceAc.Field1;

			destAc.Field2 = sourceAc.Field2;

			destAc.Field3 = sourceAc.Field3;

			destAc.Field4 = sourceAc.Field4;

			destAc.Field5 = sourceAc.Field5;
		}

		public virtual IList<IArtifact> GetArtifactList(params Func<IArtifact, bool>[] whereClauseFuncs)
		{
			Debug.Assert(whereClauseFuncs != null);

			var artifactList = new List<IArtifact>();

			foreach (var whereClauseFunc in whereClauseFuncs)
			{
				Debug.Assert(whereClauseFunc != null);

				artifactList.AddRange(Globals.Database.ArtifactTable.Records.Where(whereClauseFunc));
			}

			return artifactList;
		}

		public virtual IList<IMonster> GetMonsterList(params Func<IMonster, bool>[] whereClauseFuncs)
		{
			Debug.Assert(whereClauseFuncs != null);

			var monsterList = new List<IMonster>();

			foreach (var whereClauseFunc in whereClauseFuncs)
			{
				Debug.Assert(whereClauseFunc != null);

				monsterList.AddRange(Globals.Database.MonsterTable.Records.Where(whereClauseFunc));
			}

			return monsterList;
		}

		public virtual IList<IGameBase> GetRecordList(params Func<IGameBase, bool>[] whereClauseFuncs)
		{
			Debug.Assert(whereClauseFuncs != null);

			var recordList = new List<IGameBase>();

			foreach (var whereClauseFunc in whereClauseFuncs)
			{
				Debug.Assert(whereClauseFunc != null);

				recordList.AddRange(Globals.Database.ArtifactTable.Records.Where(whereClauseFunc));

				recordList.AddRange(Globals.Database.MonsterTable.Records.Where(whereClauseFunc));
			}

			return recordList;
		}

		public virtual IArtifact GetNthArtifact(IList<IArtifact> artifactList, long which, Func<IArtifact, bool> whereClauseFunc)
		{
			Debug.Assert(artifactList != null);

			Debug.Assert(which > 0);
			
			Debug.Assert(whereClauseFunc != null);

			return artifactList.Where(a => whereClauseFunc(a)).OrderBy(a => a.Uid).Skip((int)(which - 1)).Take(1).FirstOrDefault();
		}

		public virtual IMonster GetNthMonster(IList<IMonster> monsterList, long which, Func<IMonster, bool> whereClauseFunc)
		{
			Debug.Assert(monsterList != null);

			Debug.Assert(which > 0);

			Debug.Assert(whereClauseFunc != null);

			return monsterList.Where(m => whereClauseFunc(m)).OrderBy(m => m.Uid).Skip((int)(which - 1)).Take(1).FirstOrDefault();
		}

		public virtual IGameBase GetNthRecord(IList<IGameBase> recordList, long which, Func<IGameBase, bool> whereClauseFunc)
		{
			Debug.Assert(recordList != null);

			Debug.Assert(which > 0);

			Debug.Assert(whereClauseFunc != null);

			return recordList.Where(r => whereClauseFunc(r)).OrderBy((r) =>
			{

				return string.Format("{0}_{1}", r.Name.ToLower(), r.Uid);

			}).Skip((int)(which - 1)).Take(1).FirstOrDefault();
		}

		public virtual void StripUniqueCharsFromRecordNames(IList<IGameBase> recordList)
		{
			Debug.Assert(recordList != null);

			var sz = recordList.Count();

			for (var i = 0; i < sz; i++)
			{
				recordList[i].Name = recordList[i].Name.TrimEnd(recordList[i] is IArtifact ? '#' : '%');
			}
		}

		public virtual void AddUniqueCharsToRecordNames(IList<IGameBase> recordList)
		{
			long c;

			Debug.Assert(recordList != null);

			var sz = recordList.Count();

			do
			{
				c = 0;

				for (var i = 0; i < sz; i++)
				{
					for (var j = i + 1; j < sz; j++)
					{
						if ((recordList[j] is IArtifact && recordList[i] is IArtifact) || (recordList[j] is IMonster && recordList[i] is IMonster))
						{
							if (recordList[j].Name.Equals(recordList[i].Name, StringComparison.OrdinalIgnoreCase))
							{
								recordList[j].Name += (recordList[j] is IArtifact ? "#" : "%");

								c = 1;
							}
						}
					}
				}
			}
			while (c == 1);
		}

		public virtual void ConvertWeaponToGoldOrTreasure(IArtifact artifact, bool convertToGold)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			if (artifact.GeneralWeapon != null)
			{
				var ac = artifact.GetArtifactCategory(convertToGold ? ArtifactType.Gold : ArtifactType.Treasure);

				if (ac == null)
				{
					ac = artifact.GeneralWeapon;

					Debug.Assert(ac != null);

					ac.Type = convertToGold ? ArtifactType.Gold : ArtifactType.Treasure;

					ac.Field1 = 0;

					ac.Field2 = 0;

					ac.Field3 = 0;

					ac.Field4 = 0;

					ac.Field5 = 0;
				}
				else
				{
					var acList = artifact.Categories.Where(ac01 => ac01 != null && !ac01.IsWeapon01()).ToList();

					for (var i = 0; i < artifact.Categories.Length; i++)
					{
						artifact.SetCategories(i, acList.Count > i ? acList[i] : null);
					}
				}

				rc = artifact.RemoveStateDesc(artifact.GetReadyWeaponDesc());

				Debug.Assert(IsSuccess(rc));

				var monster = Globals.Database.MonsterTable.Records.FirstOrDefault(m => m.Weapon == artifact.Uid || m.Weapon == -artifact.Uid - 1);

				if (monster != null)
				{
					monster.Weapon = -1;
				}
			}
		}

		public virtual void ConvertTreasureToContainer(IArtifact artifact, ContainerType containerType = ContainerType.In)
		{
			Debug.Assert(artifact != null);

			var ac = artifact.Treasure;

			Debug.Assert(ac != null);

			switch(containerType)
			{
				case ContainerType.In:

					ac.Type = ArtifactType.InContainer;

					ac.Field1 = 0;

					ac.Field2 = 1;

					ac.Field3 = 9999;

					ac.Field4 = 1;

					ac.Field5 = 0;

					break;

				case ContainerType.On:

					// TODO: implement

					break;

				case ContainerType.Under:

					// TODO: implement

					break;

				case ContainerType.Behind:

					// TODO: implement

					break;
			}
		}

		public virtual void ConvertContainerToTreasure(IArtifact artifact, ContainerType containerType = ContainerType.In)
		{
			Debug.Assert(artifact != null);

			var ac = EvalContainerType(containerType, artifact.InContainer, artifact.OnContainer, artifact.UnderContainer, artifact.BehindContainer);

			Debug.Assert(ac != null);

			ac.Type = ArtifactType.Treasure;

			ac.Field1 = 0;

			ac.Field2 = 0;

			ac.Field3 = 0;

			ac.Field4 = 0;

			ac.Field5 = 0;
		}

		#endregion

		#region Class Engine

		public Engine()
		{
			MacroFuncs = new Dictionary<long, Func<string>>();

			ArtifactContainedList = new List<IArtifact>();

			Preps = new IPrep[]
			{
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "in";
					x.ContainerType = ContainerType.In;
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "into";
					x.ContainerType = ContainerType.In;
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "fromin";
					x.ContainerType = ContainerType.In;
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "on";
					x.ContainerType = ContainerType.On;
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "onto";
					x.ContainerType = ContainerType.On;
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "fromon";
					x.ContainerType = ContainerType.On;
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "under";
					x.ContainerType = ContainerType.Under;
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "fromunder";
					x.ContainerType = ContainerType.Under;
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "behind";
					x.ContainerType = ContainerType.Behind;
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "frombehind";
					x.ContainerType = ContainerType.Behind;
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "to";
					x.ContainerType = (ContainerType)(-1);
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "at";
					x.ContainerType = (ContainerType)(-1);
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "from";
					x.ContainerType = (ContainerType)(-1);
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "with";
					x.ContainerType = (ContainerType)(-1);
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "through";
					x.ContainerType = (ContainerType)(-1);
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "along";
					x.ContainerType = (ContainerType)(-1);
				}),
				Globals.CreateInstance<IPrep>(x =>
				{
					x.Name = "across";
					x.ContainerType = (ContainerType)(-1);
				})
			};

			Articles = new string[]			// TODO: fix ???
			{
				"a",
				"an",
				"some",
				"the",
				"this",
				"these",
				"that",
				"those",
				"my",
				"your",
				"his",
				"her",
				"its"
			};

			UnknownName = "???";

			Rand = new Random();

			NumberStrings = new string[]
			{
				"zero",
				"one",
				"two",
				"three",
				"four",
				"five",
				"six",
				"seven",
				"eight",
				"nine",
				"ten"
			};

			FieldDescNames = new string[]
			{
				"None",
				"Brief",
				"Full"
			};
			
			StatusNames = new string[]
			{
				"Unknown",
				"Alive",
				"Dead",
				"Adventuring"
			};

			ClothingNames = new string[]
			{
				"Armor & Shields",
				"Overclothes",
				"Shoes & Boots",
				"Gloves",
				"Hats & Headwear",
				"Jewelry",
				"Undergarments",
				"Shirts",
				"Pants"
			};
			
			CombatCodeDescs = new string[]
			{
				"Doesn't fight",
				"Uses weapons or natural weapons",		// "Will use wep. or nat. weapons", 
				"Normal",
				"Uses 'attacks' only"						// "'ATTACKS' only"
			};

			ContainerDisplayCodeDescs = new string[]
			{
				"None",
				"Something/Some Stuff",
				"Artifact Name/Some Stuff"
			};

			LightLevelNames = new string[]
			{
				"Dark",
				"Light"
			};

			Stats = new IStat[]			// TODO: fix
			{
				Globals.CreateInstance<IStat>(x =>
				{
					x.Name = "Intellect";
					x.Abbr = "IN";
					x.EmptyVal = "14";
					x.MinValue = 1;
					x.MaxValue = 24;
				}),
				Globals.CreateInstance<IStat>(x =>
				{
					x.Name = "Hardiness";
					x.Abbr = "HD";
					x.EmptyVal = "16";
					x.MinValue = 1;
					x.MaxValue = 300;
				}),
				Globals.CreateInstance<IStat>(x =>
				{
					x.Name = "Agility";
					x.Abbr = "AG";
					x.EmptyVal = "18";
					x.MinValue = 1;
					x.MaxValue = 30;
				}),
				Globals.CreateInstance<IStat>(x =>
				{
					x.Name = "Charisma";
					x.Abbr = "CH";
					x.EmptyVal = "16";
					x.MinValue = 1;
					x.MaxValue = 24;
				})
			};

			Spells = new ISpell[]			// TODO: fix
			{
				Globals.CreateInstance<ISpell>(x =>
				{
					x.Name = "Blast";
					x.HokasName = null;
					x.HokasPrice = Constants.BlastPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				}),
				Globals.CreateInstance<ISpell>(x =>
				{
					x.Name = "Heal";
					x.HokasName = null;
					x.HokasPrice = Constants.HealPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				}),
				Globals.CreateInstance<ISpell>(x =>
				{
					x.Name = "Speed";
					x.HokasName = null;
					x.HokasPrice = Constants.SpeedPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				}),
				Globals.CreateInstance<ISpell>(x =>
				{
					x.Name = "Power";
					x.HokasName = null;
					x.HokasPrice = Constants.PowerPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				})
			};

			Weapons = new IWeapon[]			// TODO: fix
			{
				Globals.CreateInstance<IWeapon>(x =>
				{
					x.Name = "Axe";
					x.EmptyVal = "5";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = PluralType.S;
					x.MarcosArticleType = ArticleType.An;
					x.MarcosPrice = Constants.AxePrice;
					x.MarcosDice = 1;
					x.MarcosSides = 6;
					x.MarcosNumHands = 1;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				Globals.CreateInstance<IWeapon>(x =>
				{
					x.Name = "Bow";
					x.EmptyVal = "-10";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = PluralType.S;
					x.MarcosArticleType = ArticleType.A;
					x.MarcosPrice = Constants.BowPrice;
					x.MarcosDice = 1;
					x.MarcosSides = 6;
					x.MarcosNumHands = 2;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				Globals.CreateInstance<IWeapon>(x =>
				{
					x.Name = "Club";
					x.EmptyVal = "20";
					x.MarcosName = "Mace";
					x.MarcosIsPlural = false;
					x.MarcosPluralType = PluralType.S;
					x.MarcosArticleType = ArticleType.A;
					x.MarcosPrice = Constants.MacePrice;
					x.MarcosDice = 1;
					x.MarcosSides = 4;
					x.MarcosNumHands = 1;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				Globals.CreateInstance<IWeapon>(x =>
				{
					x.Name = "Spear";
					x.EmptyVal = "10";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = PluralType.S;
					x.MarcosArticleType = ArticleType.A;
					x.MarcosPrice = Constants.SpearPrice;
					x.MarcosDice = 1;
					x.MarcosSides = 5;
					x.MarcosNumHands = 1;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				Globals.CreateInstance<IWeapon>(x =>
				{
					x.Name = "Sword";
					x.EmptyVal = "0";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = PluralType.S;
					x.MarcosArticleType = ArticleType.A;
					x.MarcosPrice = Constants.SwordPrice;
					x.MarcosDice = 1;
					x.MarcosSides = 8;
					x.MarcosNumHands = 1;
					x.MinValue = -50;
					x.MaxValue = 122;
				})
			};

			Armors = new IArmor[]
			{
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Skin/Clothes";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Clothes & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Leather";
					x.MarcosName = "Leather Armor";
					x.MarcosPrice = Constants.LeatherArmorPrice;
					x.MarcosNum = 1;
					x.ArtifactValue = 100;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Leather & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Chain Mail";
					x.MarcosName = null;
					x.MarcosPrice = Constants.ChainMailPrice;
					x.MarcosNum = 2;
					x.ArtifactValue = 200;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Chain Mail & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Plate Mail";
					x.MarcosName = null;
					x.MarcosPrice = Constants.PlateMailPrice;
					x.MarcosNum = 3;
					x.ArtifactValue = 350;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Plate Mail & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 500;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 650;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 800;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 950;
				}),
				Globals.CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				})
			};

			Directions = new IDirection[]			// TODO: fix
			{
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "North";
					x.PrintedName = "North";
					x.Abbr = "N";
					x.EnterDir = Direction.South;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "South";
					x.PrintedName = "South";
					x.Abbr = "S";
					x.EnterDir = Direction.North;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "East";
					x.PrintedName = "East";
					x.Abbr = "E";
					x.EnterDir = Direction.West;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "West";
					x.PrintedName = "West";
					x.Abbr = "W";
					x.EnterDir = Direction.East;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "Up";
					x.PrintedName = "Up";
					x.Abbr = "U";
					x.EnterDir = Direction.Down;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "Down";
					x.PrintedName = "Down";
					x.Abbr = "D";
					x.EnterDir = Direction.Up;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "Northeast";
					x.PrintedName = "NE";
					x.Abbr = "NE";
					x.EnterDir = Direction.Southwest;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "Northwest";
					x.PrintedName = "NW";
					x.Abbr = "NW";
					x.EnterDir = Direction.Southeast;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "Southeast";
					x.PrintedName = "SE";
					x.Abbr = "SE";
					x.EnterDir = Direction.Northwest;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "Southwest";
					x.PrintedName = "SW";
					x.Abbr = "SW";
					x.EnterDir = Direction.Northeast;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "In";
					x.PrintedName = "In";
					x.Abbr = "I";
					x.EnterDir = Direction.Out;
				}),
				Globals.CreateInstance<IDirection>(x =>
				{
					x.Name = "Out";
					x.PrintedName = "Out";
					x.Abbr = "O";
					x.EnterDir = Direction.In;
				})
			};

			ArtifactTypes = new IArtifactType[]
			{
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Gold";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Treasure";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Weapon";
					x.WeightEmptyVal = "25";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Complexity";
					x.Field1EmptyVal = "5";
					x.Field2Name = "Wpn Type";
					x.Field2EmptyVal = "5";
					x.Field3Name = "Dice";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Sides";
					x.Field4EmptyVal = "6";
					x.Field5Name = "Number Of Hands";
					x.Field5EmptyVal = "1";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Magic Weapon";
					x.WeightEmptyVal = "25";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Complexity";
					x.Field1EmptyVal = "15";
					x.Field2Name = "Wpn Type";
					x.Field2EmptyVal = "5";
					x.Field3Name = "Dice";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Sides";
					x.Field4EmptyVal = "10";
					x.Field5Name = "Number Of Hands";
					x.Field5EmptyVal = "1";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "In Container";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Key Uid";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Open/Closed";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Max Weight Inside";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Max Items Inside";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Display Code";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "On Container";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Max Weight On";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Max Items On";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Display Code";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Under Container";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Max Weight Under";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Max Items Under";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Display Code";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Behind Container";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Max Weight Behind";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Max Items Behind";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Display Code";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Light Source";
					x.WeightEmptyVal = "10";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Counter";
					x.Field1EmptyVal = "999";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Drinkable";
					x.WeightEmptyVal = "10";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Heal Amount";
					x.Field1EmptyVal = "10";
					x.Field2Name = "Number Of Uses";
					x.Field2EmptyVal = "3";
					x.Field3Name = "Open/Closed";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Readable";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Effect #1";
					x.Field1EmptyVal = "1";
					x.Field2Name = "Number Of Effects";
					x.Field2EmptyVal = "1";
					x.Field3Name = "Open/Closed";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Door/Gate";
					x.WeightEmptyVal = "-999";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Room Uid Beyond";
					x.Field1EmptyVal = "1";
					x.Field2Name = "Key Uid";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Open/Closed";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Hidden";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Edible";
					x.WeightEmptyVal = "10";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Heal Amount";
					x.Field1EmptyVal = "10";
					x.Field2Name = "Number Of Uses";
					x.Field2EmptyVal = "4";
					x.Field3Name = "Open/Closed";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Bound Monster";
					x.WeightEmptyVal = "999";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Monster Uid";
					x.Field1EmptyVal = "1";
					x.Field2Name = "Key Uid";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Guard Uid";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Wearable";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Armor Class";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Clothing Type";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Disguised Monster";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Monster Uid";
					x.Field1EmptyVal = "1";
					x.Field2Name = "Effect #1";
					x.Field2EmptyVal = "1";
					x.Field3Name = "Number Of Effects";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Dead Body";
					x.WeightEmptyVal = "150";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Can Take";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "User Defined #1";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "User Defined #2";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				Globals.CreateInstance<IArtifactType>(x =>
				{
					x.Name = "User Defined #3";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				})
			};

			TriggerTypes = new ITriggerType[]
			{
				Globals.CreateInstance<ITriggerType>(x =>
				{
					x.Name = "Interval";
					x.Field1Name = "Start Round";
					x.Field1EmptyVal = "1";
					x.Field2Name = "Interval Rounds";
					x.Field2EmptyVal = "1";
					x.Field3Name = "Probability Pct";
					x.Field3EmptyVal = "100";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				})
			};

			ScriptTypes = new IScriptType[]
			{
				Globals.CreateInstance<IScriptType>(x =>
				{
					x.Name = "Print Random Effect";
					x.Field1Name = "Effect #1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Effect #2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Effect #3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Effect #4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Effect #5";
					x.Field5EmptyVal = "0";
				})
			};
		}

		#endregion

		#endregion
	}
}
