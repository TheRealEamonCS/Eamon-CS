
// SentenceParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Game.Exceptions;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Parsing
{
	[ClassMappings]
	public class SentenceParser : ISentenceParser
	{
		public bool _mySeen;

		public virtual StringBuilder InputBuf { get; set; }

		public virtual string LastInputStr { get; set; }

		public virtual IList<string> ParserInputStrList { get; set; }

		public virtual string ParserInputStr
		{
			get
			{
				return ParserInputStrList.Count > 0 ? ParserInputStrList[0] : null;
			}
		}

		/// <summary></summary>
		public virtual ICommand TokenCommand { get; set; }

		/// <summary></summary>
		public virtual string CurrInputStr { get; set; }

		/// <summary></summary>
		public virtual string OrigInputStr { get; set; }

		/// <summary></summary>
		public virtual string DobjNameStr { get; set; }

		/// <summary></summary>
		public virtual string IobjNameStr { get; set; }

		/// <summary></summary>
		public virtual string CommandFormatStr { get; set; }

		/// <summary></summary>
		public virtual string NewCommandStr { get; set; }

		/// <summary></summary>
		public virtual string[] Tokens { get; set; }

		/// <summary></summary>
		public virtual string[] DobjNameTokens { get; set; }

		/// <summary></summary>
		public virtual long NumDobjNameTokens { get; set; }

		/// <summary></summary>
		public virtual string[] IobjNameTokens { get; set; }

		/// <summary></summary>
		public virtual long NumIobjNameTokens { get; set; }

		/// <summary></summary>
		public virtual long CurrToken { get; set; }

		/// <summary></summary>
		public virtual long StartToken { get; set; }

		/// <summary></summary>
		public virtual long CurrIndex { get; set; }

		/// <summary></summary>
		public virtual long RemoveIndex { get; set; }

		/// <summary></summary>
		public virtual long NameIndex { get; set; }

		/// <summary></summary>
		public virtual long PrepTokenIndex { get; set; }

		/// <summary></summary>
		public virtual long LoopCounter { get; set; }

		/// <summary></summary>
		public virtual bool IsValidTokenCommandMatch()
		{
			// Disallow match of QuitCommand in the middle of a sentence using "it" (which is reserved for a pronoun)

			return !(TokenCommand is IQuitCommand) || !Tokens[CurrToken + 1].Equals("it", StringComparison.OrdinalIgnoreCase);
		}

		public virtual void PrintDiscardingCommands()
		{
			if (gGameState.EnhancedParser)
			{
				for (var i = 0; i < ParserInputStrList.Count; i++)
				{
					gOut.Print("{{Discarding:  \"{0}\"}}", ParserInputStrList[i]);
				}
			}
		}

		public virtual void Clear()
		{
			InputBuf.Clear();

			ParserInputStrList.Clear();

			TokenCommand = null;

			CurrInputStr = "";

			OrigInputStr = "";

			DobjNameStr = "";

			IobjNameStr = "";

			CommandFormatStr = "";

			NewCommandStr = "";

			Tokens = null;

			DobjNameTokens = null;

			NumDobjNameTokens = 0;

			IobjNameTokens = null;

			NumIobjNameTokens = 0;

			CurrToken = 0;

			StartToken = 0;

			CurrIndex = 0;

			RemoveIndex = -1;

			NameIndex = -1;

			PrepTokenIndex = -1;

			LoopCounter = 0;
		}

		public virtual void ReplacePronounsAndProcessDobjNameList()
		{
			RetCode rc;

			if (gGameState.EnhancedParser)
			{
				for (LoopCounter = 0; true; LoopCounter++)
				{
					if (LoopCounter >= 250)			// "Can't happen"
					{
						throw new GeneralParsingErrorException();
					}

					CurrInputStr = string.Format(" {0} ", ParserInputStr);

					DobjNameStr = "";

					IobjNameStr = "";

					CurrToken = 0;

					NameIndex = -1;

					PrepTokenIndex = -1;

					Globals.Buf.SetFormat("{0}", CurrInputStr.Trim());

					Globals.Buf.SetFormat("{0}", gEngine.ReplacePrepositions(Globals.Buf).ToString());

					Tokens = Globals.Buf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

					if (CurrToken < Tokens.Length)
					{
						if (Tokens.Length == 1)
						{
							Globals.Buf.SetFormat("{0}", Tokens[CurrToken]);

							Tokens[CurrToken] = Globals.Buf.TrimEndPunctuationMinusUniqueChars().ToString().Trim();
						}

						if (Tokens[CurrToken].Length == 0)
						{
							Tokens[CurrToken] = "???";
						}
						else if (Tokens[CurrToken].Equals("at", StringComparison.OrdinalIgnoreCase))
						{
							Tokens[CurrToken] = "a";
						}

						TokenCommand = gEngine.GetCommandUsingToken(gCharMonster, Tokens[CurrToken]);

						if (TokenCommand != null && TokenCommand.IsSentenceParserEnabled)
						{
							CurrToken++;

							if (TokenCommand.IsDobjPrepEnabled || TokenCommand.IsIobjEnabled)
							{
								Predicate<string> prepFindFunc = token => gEngine.Preps.FirstOrDefault(prep => prep.Name.Equals(token, StringComparison.OrdinalIgnoreCase) && TokenCommand.IsPrepEnabled(prep)) != null;

								PrepTokenIndex = TokenCommand.IsDobjPrepEnabled ? Array.FindIndex(Tokens, prepFindFunc) : TokenCommand.IsIobjEnabled ? Array.FindLastIndex(Tokens, prepFindFunc) : -1;
							}

							if (TokenCommand.IsDobjPrepEnabled && PrepTokenIndex == CurrToken)
							{
								CurrToken++;

								NumDobjNameTokens = Tokens.Length - CurrToken;

								DobjNameStr = string.Join(" ", Tokens.Skip((int)CurrToken).Take((int)NumDobjNameTokens));

								CurrToken += NumDobjNameTokens;
							}
							else if (TokenCommand.IsIobjEnabled && PrepTokenIndex >= CurrToken)
							{
								NumDobjNameTokens = PrepTokenIndex - CurrToken;

								DobjNameStr = string.Join(" ", Tokens.Skip((int)CurrToken).Take((int)NumDobjNameTokens));

								CurrToken += (NumDobjNameTokens + 1);

								NumIobjNameTokens = Tokens.Length - CurrToken;

								IobjNameStr = string.Join(" ", Tokens.Skip((int)CurrToken).Take((int)NumIobjNameTokens));

								CurrToken += NumIobjNameTokens;
							}
							else
							{
								DobjNameStr = string.Join(" ", Tokens.Skip((int)CurrToken));

								CurrToken = Tokens.Length;
							}

							if (!string.IsNullOrWhiteSpace(DobjNameStr))
							{
								DobjNameStr = string.Format(" {0} ", DobjNameStr);

								NameIndex = CurrInputStr.IndexOf(DobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								CommandFormatStr = CurrInputStr.Substring(0, (int)NameIndex) + "{0}" + CurrInputStr.Substring((int)NameIndex + DobjNameStr.Length);

								DobjNameTokens = DobjNameStr.IndexOf(" , ") >= 0 ? DobjNameStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { DobjNameStr };

								DobjNameTokens = DobjNameTokens.Where(dobjNameToken => !string.IsNullOrWhiteSpace(dobjNameToken) && Array.FindIndex(Constants.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? dobjNameToken.IndexOf(" " + token + " ") >= 0 : token[0] != ',' && dobjNameToken.IndexOf(token) >= 0) < 0).ToArray();

								for (var j = 0; j < DobjNameTokens.Length; j++)
								{
									_mySeen = false;

									Globals.Buf.SetFormat("{0}", DobjNameTokens[j].Trim());

									rc = gEngine.StripPrepsAndArticles(Globals.Buf, ref _mySeen);

									Debug.Assert(gEngine.IsSuccess(rc));

									DobjNameTokens[j] = string.Format(" {0} ", Globals.Buf.ToString().Trim());
								}

								DobjNameStr = string.Join(",", DobjNameTokens);

								CurrInputStr = string.Format(CommandFormatStr, DobjNameStr);

								ParserInputStrList[0] = CurrInputStr.Trim();
							}

							if (!string.IsNullOrWhiteSpace(IobjNameStr))
							{
								IobjNameStr = string.Format(" {0} ", IobjNameStr);

								NameIndex = CurrInputStr.LastIndexOf(IobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								CommandFormatStr = CurrInputStr.Substring(0, (int)NameIndex) + "{0}" + CurrInputStr.Substring((int)NameIndex + IobjNameStr.Length);

								IobjNameTokens = IobjNameStr.IndexOf(" , ") >= 0 ? IobjNameStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { IobjNameStr };

								IobjNameTokens = IobjNameTokens.Where(iobjNameToken => !string.IsNullOrWhiteSpace(iobjNameToken) && Array.FindIndex(Constants.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? iobjNameToken.IndexOf(" " + token + " ") >= 0 : token[0] != ',' && iobjNameToken.IndexOf(token) >= 0) < 0).ToArray();

								for (var j = 0; j < IobjNameTokens.Length; j++)
								{
									_mySeen = false;

									Globals.Buf.SetFormat("{0}", IobjNameTokens[j].Trim());

									rc = gEngine.StripPrepsAndArticles(Globals.Buf, ref _mySeen);

									Debug.Assert(gEngine.IsSuccess(rc));

									IobjNameTokens[j] = string.Format(" {0} ", Globals.Buf.ToString().Trim());
								}

								IobjNameStr = string.Join(",", IobjNameTokens);

								CurrInputStr = string.Format(CommandFormatStr, IobjNameStr);

								ParserInputStrList[0] = CurrInputStr.Trim();
							}

							if (!string.IsNullOrWhiteSpace(DobjNameStr) && DobjNameStr.IndexOf(" him ") >= 0 && !string.IsNullOrWhiteSpace(gCommandParser.LastHimNameStr))
							{
								NameIndex = CurrInputStr.IndexOf(DobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								CurrInputStr = string.Format("{0}{1}{2}", CurrInputStr.Substring(0, (int)NameIndex), DobjNameStr.Replace(" him ", " " + gCommandParser.LastHimNameStr + " "), CurrInputStr.Substring((int)NameIndex + DobjNameStr.Length));

								ParserInputStrList[0] = CurrInputStr.Trim();
							}
							else if (!string.IsNullOrWhiteSpace(DobjNameStr) && DobjNameStr.IndexOf(" her ") >= 0 && !string.IsNullOrWhiteSpace(gCommandParser.LastHerNameStr))
							{
								NameIndex = CurrInputStr.IndexOf(DobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								CurrInputStr = string.Format("{0}{1}{2}", CurrInputStr.Substring(0, (int)NameIndex), DobjNameStr.Replace(" her ", " " + gCommandParser.LastHerNameStr + " "), CurrInputStr.Substring((int)NameIndex + DobjNameStr.Length));

								ParserInputStrList[0] = CurrInputStr.Trim();
							}
							else if (!string.IsNullOrWhiteSpace(DobjNameStr) && (DobjNameStr.IndexOf(" it ") >= 0 || DobjNameStr.IndexOf(" that ") >= 0) && !string.IsNullOrWhiteSpace(gCommandParser.LastItNameStr))
							{
								NameIndex = CurrInputStr.IndexOf(DobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								CurrInputStr = string.Format("{0}{1}{2}", CurrInputStr.Substring(0, (int)NameIndex), DobjNameStr.Replace(" it ", " " + gCommandParser.LastItNameStr + " ").Replace(" that ", " " + gCommandParser.LastItNameStr + " "), CurrInputStr.Substring((int)NameIndex + DobjNameStr.Length));

								ParserInputStrList[0] = CurrInputStr.Trim();
							}
							else if (!string.IsNullOrWhiteSpace(DobjNameStr) && (DobjNameStr.IndexOf(" them ") >= 0 || DobjNameStr.IndexOf(" those ") >= 0) && !string.IsNullOrWhiteSpace(gCommandParser.LastThemNameStr))
							{
								NameIndex = CurrInputStr.IndexOf(DobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								CurrInputStr = string.Format("{0}{1}{2}", CurrInputStr.Substring(0, (int)NameIndex), DobjNameStr.Replace(" them ", " " + gCommandParser.LastThemNameStr + " ").Replace(" those ", " " + gCommandParser.LastThemNameStr + " "), CurrInputStr.Substring((int)NameIndex + DobjNameStr.Length));

								ParserInputStrList[0] = CurrInputStr.Trim();
							}
							else if (!string.IsNullOrWhiteSpace(IobjNameStr) && IobjNameStr.IndexOf(" him ") >= 0 && !string.IsNullOrWhiteSpace(gCommandParser.LastHimNameStr))
							{
								NameIndex = CurrInputStr.LastIndexOf(IobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								CurrInputStr = string.Format("{0}{1}{2}", CurrInputStr.Substring(0, (int)NameIndex), IobjNameStr.Replace(" him ", " " + gCommandParser.LastHimNameStr + " "), CurrInputStr.Substring((int)NameIndex + IobjNameStr.Length));

								ParserInputStrList[0] = CurrInputStr.Trim();
							}
							else if (!string.IsNullOrWhiteSpace(IobjNameStr) && IobjNameStr.IndexOf(" her ") >= 0 && !string.IsNullOrWhiteSpace(gCommandParser.LastHerNameStr))
							{
								NameIndex = CurrInputStr.LastIndexOf(IobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								CurrInputStr = string.Format("{0}{1}{2}", CurrInputStr.Substring(0, (int)NameIndex), IobjNameStr.Replace(" her ", " " + gCommandParser.LastHerNameStr + " "), CurrInputStr.Substring((int)NameIndex + IobjNameStr.Length));

								ParserInputStrList[0] = CurrInputStr.Trim();
							}
							else if (!string.IsNullOrWhiteSpace(IobjNameStr) && (IobjNameStr.IndexOf(" it ") >= 0 || IobjNameStr.IndexOf(" that ") >= 0) && !string.IsNullOrWhiteSpace(gCommandParser.LastItNameStr))
							{
								NameIndex = CurrInputStr.LastIndexOf(IobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								CurrInputStr = string.Format("{0}{1}{2}", CurrInputStr.Substring(0, (int)NameIndex), IobjNameStr.Replace(" it ", " " + gCommandParser.LastItNameStr + " ").Replace(" that ", " " + gCommandParser.LastItNameStr + " "), CurrInputStr.Substring((int)NameIndex + IobjNameStr.Length));

								ParserInputStrList[0] = CurrInputStr.Trim();
							}
							else if (!string.IsNullOrWhiteSpace(IobjNameStr) && (IobjNameStr.IndexOf(" them ") >= 0 || IobjNameStr.IndexOf(" those ") >= 0) && !string.IsNullOrWhiteSpace(gCommandParser.LastThemNameStr))
							{
								NameIndex = CurrInputStr.LastIndexOf(IobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								CurrInputStr = string.Format("{0}{1}{2}", CurrInputStr.Substring(0, (int)NameIndex), IobjNameStr.Replace(" them ", " " + gCommandParser.LastThemNameStr + " ").Replace(" those ", " " + gCommandParser.LastThemNameStr + " "), CurrInputStr.Substring((int)NameIndex + IobjNameStr.Length));

								ParserInputStrList[0] = CurrInputStr.Trim();
							}
							else if (!string.IsNullOrWhiteSpace(DobjNameStr) && DobjNameStr.IndexOf(" , ") >= 0)
							{
								NameIndex = CurrInputStr.IndexOf(DobjNameStr);

								if (NameIndex < 0)
								{
									throw new GeneralParsingErrorException();
								}

								if (Array.FindIndex(Constants.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? DobjNameStr.IndexOf(" " + token + " ") >= 0 : token[0] != ',' && DobjNameStr.IndexOf(token) >= 0) < 0 && Array.FindIndex(Constants.PronounTokens, token => DobjNameStr.IndexOf(" " + token + " ") >= 0) < 0)
								{
									gCommandParser.LastThemNameStr = Globals.CloneInstance(DobjNameStr.Trim());
								}

								CommandFormatStr = CurrInputStr.Substring(0, (int)NameIndex) + "{0}" + CurrInputStr.Substring((int)NameIndex + DobjNameStr.Length);

								DobjNameTokens = DobjNameStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

								for (var j = DobjNameTokens.Length - 1; j >= 0; j--)
								{
									NewCommandStr = string.Format(CommandFormatStr, DobjNameTokens[j]).Trim();

									if (ParserInputStrList.Count > 0)
									{
										ParserInputStrList.Insert(0, NewCommandStr);
									}
									else
									{
										ParserInputStrList.Add(NewCommandStr);
									}
								}

								ParserInputStrList.RemoveAt(DobjNameTokens.Length);
							}
							else
							{
								break;
							}
						}
						else
						{
							break;
						}
					}
					else
					{
						break;
					}
				}
			}
		}

		public virtual void Execute()
		{
			if (!gGameState.EnhancedParser)
			{
				ParserInputStrList.Add(InputBuf.ToString());

				goto Cleanup;
			}

			InputBuf.SetFormat("{0}", Regex.Replace(InputBuf.ToString(), @"\s+", " ").Trim());

			if (InputBuf.Length == 0)
			{
				InputBuf.SetFormat("{0}", LastInputStr);

				if (InputBuf.Length > 0)
				{
					if (Environment.NewLine.Length == 1 && Globals.CursorPosition.Y > -1 && Globals.CursorPosition.Y + 1 >= gOut.GetBufferHeight())
					{
						Globals.CursorPosition.Y--;
					}

					gOut.SetCursorPosition(Globals.CursorPosition);

					if (Globals.LineWrapUserInput)
					{
						gEngine.LineWrap(InputBuf.ToString(), Globals.Buf, Globals.CommandPrompt.Length);
					}
					else
					{
						Globals.Buf.SetFormat("{0}", InputBuf.ToString());
					}

					gOut.WordWrap = false;

					gOut.WriteLine(Globals.Buf);

					gOut.WordWrap = true;
				}
			}

			OrigInputStr = InputBuf.ToString();

			LastInputStr = InputBuf.ToString();

			InputBuf = gEngine.NormalizePlayerInput(InputBuf);

			Tokens = InputBuf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			for (CurrToken = 0; CurrToken < Tokens.Length; CurrToken++)
			{
				if (CurrToken == 0)
				{
					TokenCommand = gEngine.GetCommandUsingToken(gCharMonster, Tokens[CurrToken]);

					if (TokenCommand != null && gEngine.IsQuotedStringCommand(TokenCommand))
					{
						ParserInputStrList.Add(OrigInputStr);

						goto Cleanup;
					}
				}
				else if (Tokens[CurrToken] == "," && CurrToken + 1 < Tokens.Length)
				{
					TokenCommand = gEngine.GetCommandUsingToken(gCharMonster, Tokens[CurrToken + 1]);

					if (TokenCommand != null && IsValidTokenCommandMatch())
					{
						CurrInputStr = string.Join(" ", Tokens.Skip((int)(StartToken)).Take((int)(CurrToken - StartToken)));

						if (CurrInputStr.Length > 0)
						{
							ParserInputStrList.Add(CurrInputStr);
						}

						StartToken = CurrToken + 1;
					}
				}
			}

			if (Tokens.Length > 0)
			{
				CurrInputStr = string.Join(" ", Tokens.Skip((int)(StartToken)).Take((int)(Tokens.Length - StartToken)));

				if (CurrInputStr.Length > 0)
				{
					ParserInputStrList.Add(CurrInputStr);
				}
			}

			if (ParserInputStrList.Count > 1)
			{
				for (CurrIndex = 0; CurrIndex < ParserInputStrList.Count; CurrIndex++)
				{
					CurrInputStr = ParserInputStrList[(int)CurrIndex];

					Tokens = CurrInputStr.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

					if (Tokens.Length > 0)
					{
						TokenCommand = gEngine.GetCommandUsingToken(gCharMonster, Tokens[0]);

						if (TokenCommand == null)
						{
							RemoveIndex = CurrIndex;

							break;
						}
						else if (!TokenCommand.IsSentenceParserEnabled)
						{
							RemoveIndex = CurrIndex > 0 ? CurrIndex : CurrIndex + 1;

							break;
						}
						else if (TokenCommand.Type == CommandType.Movement)
						{
							RemoveIndex = CurrIndex + 1;

							break;
						}
					}
				}

				while (RemoveIndex >= 0 && ParserInputStrList.Count > RemoveIndex)
				{
					gOut.Print("{{Discarding:  \"{0}\"}}", ParserInputStrList[(int)RemoveIndex]);

					ParserInputStrList.RemoveAt((int)RemoveIndex);
				}
			}

			if (ParserInputStrList.Count < 1 && OrigInputStr.Length > 0)
			{
				ParserInputStrList.Add(OrigInputStr);
			}

		Cleanup:

			;
		}

		public SentenceParser()
		{
			InputBuf = new StringBuilder(Constants.BufSize);

			LastInputStr = "";

			ParserInputStrList = new List<string>();

			Clear();
		}
	}
}
