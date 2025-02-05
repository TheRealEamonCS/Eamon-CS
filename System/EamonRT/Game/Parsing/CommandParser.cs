
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using EamonRT.Game.Exceptions;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : ICommandParser
	{
		public string _lastHimNameStr;

		public string _lastHerNameStr;

		public string _lastItNameStr;

		public string _lastThemNameStr;

		public IMonster _actorMonster;

		public IRoom _actorRoom;

		public virtual StringBuilder InputBuf { get; set; }

		public virtual string LastInputStr { get; set; }

		public virtual string LastHimNameStr 
		{ 
			get
			{
				return _lastHimNameStr;
			}

			set
			{
				if (gGameState != null && gGameState.ShowPronounChanges && !_lastHimNameStr.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintChangingHim(value);
				}

				_lastHimNameStr = value;
			}
		}

		public virtual string LastHerNameStr
		{
			get
			{
				return _lastHerNameStr;
			}

			set
			{
				if (gGameState != null && gGameState.ShowPronounChanges && !_lastHerNameStr.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintChangingHer(value);
				}

				_lastHerNameStr = value;
			}
		}

		public virtual string LastItNameStr
		{
			get
			{
				return _lastItNameStr;
			}

			set
			{
				if (gGameState != null && gGameState.ShowPronounChanges && !_lastItNameStr.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintChangingIt(value);
				}

				_lastItNameStr = value;
			}
		}

		public virtual string LastThemNameStr
		{
			get
			{
				return _lastThemNameStr;
			}

			set
			{
				if (gGameState != null && gGameState.ShowPronounChanges && !_lastThemNameStr.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintChangingThem(value);
				}

				_lastThemNameStr = value;
			}
		}

		public virtual string CurrInputStr { get; set; }

		public virtual string OrigInputStr { get; set; }

		public virtual string CommandFormatStr { get; set; }

		public virtual string NewCommandStr { get; set; }

		public virtual string[] Tokens { get; set; }

		public virtual long CurrToken { get; set; }

		public virtual long NameIndex { get; set; }

		public virtual long PrepTokenIndex { get; set; }

		public virtual IPrep Prep { get; set; }

		public virtual IMonster ActorMonster 
		{ 
			get
			{
				return NextCommand != null ? NextCommand.ActorMonster : _actorMonster;
			}

			set
			{
				if (NextCommand != null)
				{
					NextCommand.ActorMonster = value;
				}
				else
				{
					_actorMonster = value;
				}
			}
		}

		public virtual IRoom ActorRoom
		{
			get
			{
				return NextCommand != null ? NextCommand.ActorRoom : _actorRoom;
			}

			set
			{
				if (NextCommand != null)
				{
					NextCommand.ActorRoom = value;
				}
				else
				{
					_actorRoom = value;
				}
			}
		}

		public virtual IGameBase Dobj
		{
			get
			{
				return NextCommand?.Dobj;
			}

			set
			{
				if (NextCommand != null)
				{
					NextCommand.Dobj = value;
				}
			}
		}

		public virtual IArtifact DobjArtifact
		{
			get
			{
				return NextCommand?.DobjArtifact;
			}
		}

		public virtual IMonster DobjMonster
		{
			get
			{
				return NextCommand?.DobjMonster;
			}
		}

		public virtual IGameBase Iobj
		{
			get
			{
				return NextCommand?.Iobj;
			}

			set
			{
				if (NextCommand != null)
				{
					NextCommand.Iobj = value;
				}
			}
		}

		public virtual IArtifact IobjArtifact
		{
			get
			{
				return NextCommand?.IobjArtifact;
			}
		}

		public virtual IMonster IobjMonster
		{
			get
			{
				return NextCommand?.IobjMonster;
			}
		}

		public virtual IParserData DobjData { get; set; }

		public virtual IParserData IobjData { get; set; }

		public virtual IParserData ObjData { get; set; }

		public virtual IState NextState { get; set; }

		public virtual ICommand NextCommand
		{
			get
			{
				return NextState as ICommand;
			}
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListAttackCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster,
				r => r is IArtifact a && a.IsInRoom(ActorRoom),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListBlastCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster,
				r => r is IArtifact a && a.IsInRoom(ActorRoom),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListFreeCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && (a.IsCarriedByMonster(ActorMonster) || a.IsInRoom(ActorRoom)),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)),
				r => r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListGiveCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && (a.IsCarriedByMonster(ActorMonster) || a.IsInRoom(ActorRoom)),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)),
				r => r is IArtifact a && a.IsWornByMonster(ActorMonster)
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListRequestCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && a.IsCarriedByMonster(IobjMonster),
				r => r is IArtifact a && a.IsWornByMonster(IobjMonster)
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListDropCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && a.IsCarriedByMonster(ActorMonster),
				r => r is IArtifact a && a.IsWornByMonster(ActorMonster),
				r => r is IArtifact a && a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively)
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListExamineCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => (r is IArtifact a && (a.IsCarriedByMonster(ActorMonster) || a.IsInRoom(ActorRoom))) || (r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)),
				r => r is IArtifact a && a.IsWornByMonster(ActorMonster)
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListGetCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && a.IsInRoom(ActorRoom),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListRemoveCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && a.IsWornByMonster(ActorMonster)
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListRemoveCommand01()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && a.IsCarriedByContainer(IobjArtifact) && a.GetCarriedByContainerContainerType() == NextCommand.ContainerType
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListUseCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => (r is IArtifact a && (a.IsCarriedByMonster(ActorMonster) || a.IsInRoom(ActorRoom))) || (r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListWearCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && (a.IsCarriedByMonster(ActorMonster) || a.IsInRoom(ActorRoom)),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)),
				r => r is IArtifact a && a.IsWornByMonster(ActorMonster)
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListBortCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListBortCommand01()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IMonster
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListInventoryCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => (r is IArtifact a && (a.IsCarriedByMonster(ActorMonster) || a.IsInRoom(ActorRoom))) || (r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListFleeCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && a.IsInRoom(ActorRoom),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListGoCommand()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && a.IsInRoom(ActorRoom),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListResolveRecord()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster,
				r => r is IArtifact a && (a.IsCarriedByMonster(ActorMonster) || a.IsInRoom(ActorRoom)),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListResolveRecord01()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster
			};
		}

		public virtual IList<Func<IGameBase, bool>> GetRecordWhereClauseListResolveRecord02()
		{
			return new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && (a.IsCarriedByMonster(ActorMonster) || a.IsInRoom(ActorRoom)),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
			};
		}

		public virtual void FinishParsingAttackCommand()
		{
			ObjData.RecordWhereClauseList = GetRecordWhereClauseListAttackCommand();

			ResolveRecord();
		}

		public virtual void FinishParsingBlastCommand()
		{
			ObjData.RecordWhereClauseList = GetRecordWhereClauseListBlastCommand();

			ResolveRecord();
		}

		public virtual void FinishParsingFreeCommand()
		{
			ObjData.RecordWhereClauseList = GetRecordWhereClauseListFreeCommand();

			ResolveRecord();
		}

		public virtual void FinishParsingGiveCommand()
		{
			long i;

			var giveCommand = NextCommand as IGiveCommand;

			Debug.Assert(giveCommand != null);

			ParseName();

			if (long.TryParse(ObjData.Name, out i) && i > 0)
			{
				giveCommand.GoldAmount = i;
			}

			if (giveCommand.GoldAmount == 0)
			{
				ObjData.RecordWhereClauseList = GetRecordWhereClauseListGiveCommand();

				ResolveRecord(false);
			}

			if (NextCommand != null && (giveCommand.GoldAmount > 0 || DobjArtifact != null))
			{
				ObjData = IobjData;

				ObjData.QueryPrintFunc = () => gEngine.PrintToWhom();

				ResolveRecord(true, false);
			}
		}

		public virtual void FinishParsingHealCommand()
		{
			if (!gEngine.IsRulesetVersion(5) && CurrToken < Tokens.Length)
			{
				ResolveRecord(true, false);
			}
			else
			{
				Dobj = gCharMonster;
			}
		}

		public virtual void FinishParsingRequestCommand()
		{
			ParseName();

			ObjData = IobjData;

			ObjData.QueryPrintFunc = () => gEngine.PrintFromWhom();

			ResolveRecord(true, false);

			if (IobjMonster != null)
			{
				ObjData = DobjData;

				ObjData.RecordWhereClauseList = GetRecordWhereClauseListRequestCommand();

				ObjData.RecordMatchFunc = RecordMatch01;

				ObjData.RecordNotFoundFunc = () =>
				{
					gEngine.PrintDoesntHaveIt(IobjMonster);
				};

				ResolveRecord(false);
			}
		}

		public virtual void FinishParsingCloseCommand()
		{
			ResolveRecord(false);
		}

		public virtual void FinishParsingDrinkCommand()
		{
			ResolveRecord(false);
		}

		public virtual void FinishParsingDropCommand()
		{
			ParseName();

			if (ObjData.Name.Equals("all", StringComparison.OrdinalIgnoreCase))
			{
				NextCommand.Cast<IDropCommand>().DropAll = true;
			}
			else
			{
				ObjData.RecordWhereClauseList = GetRecordWhereClauseListDropCommand();

				ObjData.RecordNotFoundFunc = NextCommand.PrintDontHaveIt;

				ResolveRecord(false);
			}
		}

		public virtual void FinishParsingEatCommand()
		{
			ResolveRecord(false);
		}

		public virtual void FinishParsingExamineCommand()
		{
			ParseName();

			NextCommand.ContainerType = NextCommand.Prep != null ? NextCommand.Prep.ContainerType : (ContainerType)(-1);

			if (ObjData.Name.Equals("room", StringComparison.OrdinalIgnoreCase) || ObjData.Name.Equals("area", StringComparison.OrdinalIgnoreCase))
			{
				var command = gEngine.CreateInstance<ILookCommand>();

				NextCommand.CopyCommandData(command);

				NextState = command;
			}
			else
			{
				ObjData.RecordWhereClauseList = GetRecordWhereClauseListExamineCommand();

				if (!Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType))
				{
					ObjData.RevealEmbeddedArtifactFunc = (r, a) => { };
				}

				ObjData.RecordMatchFunc = RecordMatch01;

				ObjData.RecordNotFoundFunc = NextCommand.PrintYouSeeNothingSpecial;

				ResolveRecord();
			}
		}

		public virtual void FinishParsingGetCommand()
		{
			ParseName();

			if (ObjData.Name.Equals("all", StringComparison.OrdinalIgnoreCase))
			{
				NextCommand.Cast<IGetCommand>().GetAll = true;
			}
			else
			{
				ObjData.RecordWhereClauseList = GetRecordWhereClauseListGetCommand();

				ObjData.RecordNotFoundFunc = NextCommand.PrintCantVerbThat;

				ResolveRecord(false);
			}
		}

		public virtual void FinishParsingLightCommand()
		{
			if (!ActorMonster.IsInRoomLit())
			{
				ObjData.RecordNotFoundFunc = () => { };
			}

			ResolveRecord(false);
		}

		public virtual void FinishParsingOpenCommand()
		{
			ResolveRecord(false);
		}

		public virtual void FinishParsingPutCommand()
		{
			ResolveRecord(false);

			if (DobjArtifact != null)
			{
				NextCommand.ContainerType = NextCommand.Prep != null ? NextCommand.Prep.ContainerType : (ContainerType)(-1);

				ObjData = IobjData;

				ObjData.QueryPrintFunc = () => gEngine.PrintPutObjPrepWhat(NextCommand, DobjArtifact);

				ResolveRecord(false);

				if (IobjArtifact != null)
				{
					if (!Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType))
					{
						var artTypes = new ArtifactType[] { ArtifactType.InContainer, ArtifactType.OnContainer };

						var defaultAc = IobjArtifact.GetArtifactCategory(artTypes);

						NextCommand.ContainerType = defaultAc != null ? gEngine.GetContainerType(defaultAc.Type) : ContainerType.In;
					}
				}
			}
		}

		public virtual void FinishParsingReadCommand()
		{
			ResolveRecord(false);
		}

		public virtual void FinishParsingReadyCommand()
		{
			ResolveRecord(false);
		}

		public virtual void FinishParsingRemoveCommand()
		{
			ObjData.RecordWhereClauseList = GetRecordWhereClauseListRemoveCommand();

			ObjData.RecordMatchFunc = () =>
			{
				NextCommand.ContainerType = NextCommand.Prep != null ? NextCommand.Prep.ContainerType : (ContainerType)(-1);

				if (ObjData.FilterRecordList.Count > 1)
				{
					NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterRecordList[0], ObjData.FilterRecordList[1]);

					NextState = gEngine.CreateInstance<IStartState>();
				}
				else if (ObjData.FilterRecordList.Count == 1)
				{
					if (ObjData.FilterRecordList[0] is IArtifact artifact)
					{
						ObjData.RevealEmbeddedArtifactFunc(ActorRoom, artifact);
					}

					SetRecord(ObjData.FilterRecordList[0]);
				}
			};

			ResolveRecord(false);

			if (NextCommand != null && DobjArtifact == null)
			{
				ObjData = IobjData;

				ObjData.QueryPrintFunc = () => gEngine.PrintFromPrepWhat(NextCommand);

				ResolveRecord(false);

				if (IobjArtifact != null)
				{
					if (!Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType))
					{
						var artTypes = new ArtifactType[] { ArtifactType.InContainer, ArtifactType.OnContainer };

						var defaultAc = IobjArtifact.GetArtifactCategory(artTypes);

						NextCommand.ContainerType = defaultAc != null ? gEngine.GetContainerType(defaultAc.Type) : ContainerType.In;
					}

					var ac = gEngine.EvalContainerType(NextCommand.ContainerType, IobjArtifact.InContainer, IobjArtifact.OnContainer, IobjArtifact.UnderContainer, IobjArtifact.BehindContainer);

					if (ac != null)
					{
						if (ac != IobjArtifact.InContainer || ac.IsOpen() || IobjArtifact.ShouldExposeInContentsWhenClosed())
						{
							ObjData = DobjData;

							ObjData.RecordWhereClauseList = GetRecordWhereClauseListRemoveCommand01();

							ObjData.RecordMatchFunc = RecordMatch;

							ObjData.RecordNotFoundFunc = NextCommand.PrintDontFollowYou;

							ResolveRecord(false);
						}
						else
						{
							NextCommand.PrintMustFirstOpen(IobjArtifact);

							NextState = gEngine.CreateInstance<IStartState>();
						}
					}
					else
					{
						NextCommand.PrintDontFollowYou();

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
			}
		}

		public virtual void FinishParsingUseCommand()
		{
			ResolveRecord(false);

			if (DobjArtifact != null && NextCommand.IsIobjEnabled && CurrToken < Tokens.Length)
			{
				ObjData = IobjData;

				ObjData.QueryPrintFunc = () => gEngine.PrintUseObjOnWhoOrWhat(DobjArtifact);

				ObjData.RecordWhereClauseList = GetRecordWhereClauseListUseCommand();

				ObjData.RecordMatchFunc = RecordMatch01;

				ObjData.RecordNotFoundFunc = NextCommand.PrintDontHaveItNotHere;

				ResolveRecord();
			}
		}

		public virtual void FinishParsingWearCommand()
		{
			ObjData.RecordWhereClauseList = GetRecordWhereClauseListWearCommand();

			ResolveRecord(false);
		}

		public virtual void FinishParsingBortCommand()
		{
			var bortCommand = NextCommand as IBortCommand;

			Debug.Assert(bortCommand != null);

			gEngine.ShouldPreTurnProcess = false;

			if (CurrToken + 1 < Tokens.Length)
			{
				var action = Tokens[CurrToken];

				if (action.Equals("visitartifact", StringComparison.OrdinalIgnoreCase) || action.Equals("recallartifact", StringComparison.OrdinalIgnoreCase))
				{
					CurrToken++;

					long artifactUid;

					if (long.TryParse(Tokens[CurrToken], out artifactUid))
					{
						CurrToken++;

						Dobj = gADB[artifactUid];
					}
					else
					{
						ObjData.RecordWhereClauseList = GetRecordWhereClauseListBortCommand();

						ObjData.RecordMatchFunc = () =>
						{
							if (ObjData.FilterRecordList.Count > 0)
							{
								SetRecord(ObjData.FilterRecordList[0]);
							}
						};

						ResolveRecord(false);
					}

					if (DobjArtifact != null)
					{
						bortCommand.Action = action.ToLower();

						bortCommand.Record = DobjArtifact;
					}
					else
					{
						bortCommand.PrintBortArtifactInvalid();

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
				else if (action.Equals("visitmonster", StringComparison.OrdinalIgnoreCase) || action.Equals("recallmonster", StringComparison.OrdinalIgnoreCase))
				{
					CurrToken++;

					long monsterUid;

					if (long.TryParse(Tokens[CurrToken], out monsterUid))
					{
						CurrToken++;

						Dobj = gMDB[monsterUid];
					}
					else
					{
						ObjData.RecordWhereClauseList = GetRecordWhereClauseListBortCommand01();

						ObjData.RecordMatchFunc = () =>
						{
							if (ObjData.FilterRecordList.Count > 0)
							{
								SetRecord(ObjData.FilterRecordList[0]);
							}
						};

						ResolveRecord(true, false);
					}

					if (DobjMonster != null)
					{
						bortCommand.Action = action.ToLower();

						bortCommand.Record = DobjMonster;
					}
					else
					{
						bortCommand.PrintBortMonsterInvalid();

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
				else if (action.Equals("visitroom", StringComparison.OrdinalIgnoreCase))
				{
					CurrToken++;

					IRoom room = null;

					long roomUid;

					if (long.TryParse(Tokens[CurrToken], out roomUid))
					{
						CurrToken++;

						room = gRDB[roomUid];
					}
					else
					{
						var roomName = string.Join(" ", Tokens, (int)CurrToken, (int)(Tokens.Length - CurrToken));

						room = gRDB.Records.FirstOrDefault(r => r.Name.Equals(roomName, StringComparison.OrdinalIgnoreCase));

						if (room == null)
						{
							room = gRDB.Records.FirstOrDefault(r => r.Name.StartsWith(roomName, StringComparison.OrdinalIgnoreCase) || r.Name.EndsWith(roomName, StringComparison.OrdinalIgnoreCase));
						}
					}

					if (room != null)
					{
						bortCommand.Action = action.ToLower();

						bortCommand.Record = room;
					}
					else
					{
						bortCommand.PrintBortRoomInvalid();

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
				else
				{
					bortCommand.PrintBortUsage();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else if (CurrToken < Tokens.Length && Tokens[CurrToken].Equals("rungameeditor", StringComparison.OrdinalIgnoreCase))
			{
				bortCommand.Action = "rungameeditor";
			}
			else
			{
				bortCommand.PrintBortUsage();

				NextState = gEngine.CreateInstance<IStartState>();
			}
		}

		public virtual void FinishParsingInventoryCommand()
		{
			if (CurrToken < Tokens.Length)
			{
				if (ActorRoom.IsViewable())
				{
					if (gEngine.IsRulesetVersion(5, 62))
					{
						ResolveRecord(false);
					}
					else
					{
						ObjData.RecordWhereClauseList = GetRecordWhereClauseListInventoryCommand();

						ObjData.RecordNotFoundFunc = NextCommand.PrintDontHaveItNotHere;

						ResolveRecord();
					}
				}
				else
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else
			{
				Dobj = gCharMonster;
			}
		}

		public virtual void FinishParsingLookCommand()
		{
			if (CurrToken < Tokens.Length)
			{
				var command = gEngine.CreateInstance<IExamineCommand>();

				NextCommand.CopyCommandData(command);

				NextState = command;

				FinishParsing();
			}
		}

		public virtual void FinishParsingQuitCommand()
		{
			gEngine.ShouldPreTurnProcess = false;

			if (CurrToken < Tokens.Length && Tokens[CurrToken].Equals("hall", StringComparison.OrdinalIgnoreCase))
			{
				NextCommand.Cast<IQuitCommand>().GoToMainHall = true;

				CurrToken++;
			}
		}

		public virtual void FinishParsingRestoreCommand()
		{
			RetCode rc;
			long i;

			var restoreCommand = NextCommand as IRestoreCommand;

			Debug.Assert(restoreCommand != null);

			gEngine.ShouldPreTurnProcess = false;

			if (CurrToken < Tokens.Length && long.TryParse(Tokens[CurrToken], out i) && i >= 1 && i <= gEngine.NumSaveSlots)
			{
				restoreCommand.SaveSlot = i;

				CurrToken++;
			}

			var filesets = gDatabase.FilesetTable.Records.ToList();

			var filesetsCount = filesets.Count();

			if (restoreCommand.SaveSlot < 1 || restoreCommand.SaveSlot > filesetsCount)
			{
				while (true)
				{
					if (gGameState.Die == 1)
					{
						gOut.Print("{0}", gEngine.LineSep);
					}

					gEngine.PrintSavedGames();

					for (i = 0; i < gEngine.NumSaveSlots; i++)
					{
						gEngine.PrintSaveSlot(i + 1, i < filesets.Count ? filesets[(int)i].Name : "(none)");
					}

					gEngine.PrintSaveSlot(i + 1, "(Don't restore, return to game)", true);

					if (gGameState.Die == 1)
					{
						gOut.Print("{0}", gEngine.LineSep);
					}

					gEngine.PrintEnterRestoreSlotChoice(i + 1);

					gEngine.Buf.Clear();

					rc = gEngine.In.ReadField(gEngine.Buf, 3, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					i = Convert.ToInt64(gEngine.Buf.Trim().ToString());

					if (i >= 1 && i <= filesetsCount)
					{
						restoreCommand.SaveSlot = i;

						break;
					}
					else if (i == gEngine.NumSaveSlots + 1)
					{
						break;
					}
				}
			}

			if (restoreCommand.SaveSlot < 1 || restoreCommand.SaveSlot > filesetsCount)
			{
				if (gGameState.Die == 1)
				{
					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});
				}
				else
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
		}

		public virtual void FinishParsingSaveCommand()
		{
			RetCode rc;
			long i;

			var saveCommand = NextCommand as ISaveCommand;

			Debug.Assert(saveCommand != null);

			gEngine.ShouldPreTurnProcess = false;

			if (CurrToken < Tokens.Length && long.TryParse(Tokens[CurrToken], out i) && i >= 1 && i <= gEngine.NumSaveSlots)
			{
				saveCommand.SaveSlot = i;

				CurrToken++;
			}

			if (CurrToken < Tokens.Length && saveCommand.SaveSlot >= 1 && saveCommand.SaveSlot <= gEngine.NumSaveSlots)
			{
				saveCommand.SaveName = string.Join(" ", Tokens.Skip((int)CurrToken));

				if (saveCommand.SaveName.Length > gEngine.FsNameLen)
				{
					saveCommand.SaveName = saveCommand.SaveName.Substring(0, gEngine.FsNameLen);
				}

				CurrToken += (Tokens.Length - CurrToken);
			}
			else
			{
				saveCommand.SaveName = "Quick Saved Game";
			}

			var filesets = gDatabase.FilesetTable.Records.ToList();

			var filesetsCount = filesets.Count();

			if (saveCommand.SaveSlot < 1 || saveCommand.SaveSlot > gEngine.NumSaveSlots)
			{
				saveCommand.SaveName = "";

				while (true)
				{
					gEngine.PrintSavedGames();

					for (i = 0; i < gEngine.NumSaveSlots; i++)
					{
						gEngine.PrintSaveSlot(i + 1, i < filesets.Count ? filesets[(int)i].Name : "(none)");
					}

					gEngine.PrintSaveSlot(i + 1, "(Don't save, return to game)", true);

					gEngine.PrintEnterSaveSlotChoice(i + 1);

					gEngine.Buf.Clear();

					rc = gEngine.In.ReadField(gEngine.Buf, 3, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					i = Convert.ToInt64(gEngine.Buf.Trim().ToString());

					if (i >= 1 && i <= gEngine.NumSaveSlots)
					{
						saveCommand.SaveSlot = i;

						break;
					}
					else if (i == gEngine.NumSaveSlots + 1)
					{
						break;
					}
				}
			}

			if (saveCommand.SaveSlot > filesetsCount + 1 && saveCommand.SaveSlot <= gEngine.NumSaveSlots)
			{
				saveCommand.SaveSlot = filesetsCount + 1;

				gEngine.PrintUsingSlotInstead(saveCommand.SaveSlot);
			}

			if (saveCommand.SaveSlot < 1 || saveCommand.SaveSlot > filesetsCount + 1)
			{
				NextState = gEngine.CreateInstance<IStartState>();
			}
		}

		public virtual void FinishParsingSayCommand()
		{
			var sayCommand = NextCommand as ISayCommand;

			Debug.Assert(sayCommand != null);

			if (CurrToken < Tokens.Length)
			{
				sayCommand.OriginalPhrase = InputBuf.ToString().Substring((Tokens[0] + " ").Length);

				CurrToken += (Tokens.Length - CurrToken);
			}

			while (true)
			{
				if (string.IsNullOrWhiteSpace(sayCommand.OriginalPhrase))
				{
					gEngine.PrintVerbWhoOrWhat(NextCommand);

					gEngine.Buf.SetFormat("{0}", gEngine.In.ReadLine());

					sayCommand.OriginalPhrase = Regex.Replace(gEngine.Buf.ToString(), @"\s+", " ").Trim();
				}
				else
				{
					break;
				}
			}
		}

		public virtual void FinishParsingSettingsCommand()
		{
			long longValue = 0;

			bool boolValue = false;

			var settingsCommand = NextCommand as ISettingsCommand;

			Debug.Assert(settingsCommand != null);

			gEngine.ShouldPreTurnProcess = false;

			if (CurrToken + 1 < Tokens.Length)
			{
				if (Tokens[CurrToken].Equals("verboserooms", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.VerboseRooms = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("verbosemonsters", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.VerboseMonsters = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("verboseartifacts", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.VerboseArtifacts = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("verbosenames", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.VerboseNames = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("maturecontent", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.MatureContent = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("interactivefiction", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.InteractiveFiction = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("enhancedparser", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.EnhancedParser = boolValue;

					CurrToken += 2;
				}
				else if (gGameState.EnhancedParser && Tokens[CurrToken].Equals("iobjpronounaffinity", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.IobjPronounAffinity = boolValue;

					CurrToken += 2;
				}
				else if (gGameState.EnhancedParser && Tokens[CurrToken].Equals("showpronounchanges", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.ShowPronounChanges = boolValue;

					CurrToken += 2;
				}
				else if (gGameState.EnhancedParser && Tokens[CurrToken].Equals("showfulfillmessages", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.ShowFulfillMessages = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("pausecombatms", StringComparison.OrdinalIgnoreCase) && long.TryParse(Tokens[CurrToken + 1], out longValue) && longValue >= 0 && longValue <= 10000)
				{
					settingsCommand.PauseCombatMs = longValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("pausecombatactions", StringComparison.OrdinalIgnoreCase) && long.TryParse(Tokens[CurrToken + 1], out longValue) && longValue >= 0 && longValue <= 25)
				{
					settingsCommand.PauseCombatActions = longValue;

					CurrToken += 2;
				}
				else
				{
					settingsCommand.PrintSettingsUsage();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else
			{
				settingsCommand.PrintSettingsUsage();

				NextState = gEngine.CreateInstance<IStartState>();
			}
		}

		public virtual void FinishParsingFleeCommand()
		{
			if (CurrToken < Tokens.Length)
			{
				NextCommand.Cast<IFleeCommand>().Direction = gEngine.GetDirection(Tokens[CurrToken]);

				if (NextCommand.Cast<IFleeCommand>().Direction != 0)
				{
					CurrToken++;
				}
				else if (ActorRoom.IsViewable())
				{
					ParseName();

					ObjData.RecordWhereClauseList = GetRecordWhereClauseListFleeCommand();

					ObjData.RecordNotFoundFunc = NextCommand.PrintNothingHereByThatName;

					ResolveRecord(false);
				}
				else
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
		}

		public virtual void FinishParsingGoCommand()
		{
			ParseName();

			ObjData.RecordWhereClauseList = GetRecordWhereClauseListGoCommand();

			ObjData.RecordNotFoundFunc = NextCommand.PrintNothingHereByThatName;

			ResolveRecord(false);
		}

		// Note: could be dual-moded based on call to ActorMonster.IsCharacterMonster

		public virtual void RecordMatch()
		{
			if (ObjData.FilterRecordList.Count > 1)
			{
				NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterRecordList[0], ObjData.FilterRecordList[1]);

				NextState = gEngine.CreateInstance<IStartState>();
			}
			else if (ObjData.FilterRecordList.Count < 1)
			{
				ObjData.RecordNotFoundFunc();

				NextState = gEngine.CreateInstance<IStartState>();
			}
			else 
			{
				if (ObjData.FilterRecordList[0] is IArtifact artifact)
				{
					ObjData.RevealEmbeddedArtifactFunc(ActorRoom, artifact);
				}

				SetRecord(ObjData.FilterRecordList[0]);
			}
		}

		// Note: could be dual-moded based on call to ActorMonster.IsCharacterMonster

		public virtual void RecordMatch01()
		{
			if (ObjData.FilterRecordList.Count > 1)
			{
				NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterRecordList[0], ObjData.FilterRecordList[1]);

				NextState = gEngine.CreateInstance<IStartState>();
			}
			else if (ObjData.FilterRecordList.Count < 1)
			{
				ObjData.RecordNotFoundFunc();

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				if (ObjData.FilterRecordList[0] is IArtifact artifact)
				{
					ObjData.RevealEmbeddedArtifactFunc(ActorRoom, artifact);
				}

				SetRecord(ObjData.FilterRecordList[0]);
			}
		}

		public virtual void ResolveRecord(bool includeMonsters = true, bool includeArtifacts = true)
		{
			if (GetRecord() == null)
			{
				if (string.IsNullOrWhiteSpace(ObjData.Name))
				{
					ParseName();
				}

				if (!string.IsNullOrWhiteSpace(ObjData.Name))
				{
					if (ObjData.RecordWhereClauseList == null)
					{
						ObjData.RecordWhereClauseList = 
							includeMonsters && includeArtifacts ? GetRecordWhereClauseListResolveRecord() :
							includeMonsters ? GetRecordWhereClauseListResolveRecord01() :
							includeArtifacts ? GetRecordWhereClauseListResolveRecord02() :
							new List<Func<IGameBase, bool>>();
					}

					if (ObjData.RevealEmbeddedArtifactFunc == null)
					{
						ObjData.RevealEmbeddedArtifactFunc = gEngine.RevealEmbeddedArtifact;
					}

					if (ObjData.GetRecordListFunc == null)
					{
						ObjData.GetRecordListFunc = gEngine.GetRecordList;
					}

					if (ObjData.FilterRecordListFunc == null)
					{
						ObjData.FilterRecordListFunc = gEngine.FilterRecordList;
					}

					if (ObjData.RecordMatchFunc == null)
					{
						ObjData.RecordMatchFunc = RecordMatch;
					}

					if (ObjData.RecordNotFoundFunc == null)
					{
						if (includeMonsters)
						{
							ObjData.RecordNotFoundFunc = NextCommand.PrintNobodyHereByThatName;
						}
						else
						{
							ObjData.RecordNotFoundFunc = NextCommand.PrintDontHaveItNotHere;
						}
					}

					ResolveRecordProcessWhereClauseList();

					ObjData.RecordMatchFunc();
				}
				else
				{
					NextState = gEngine.CreateInstance<IErrorState>(x =>
					{
						x.ErrorMessage = string.Format("{0}: string.IsNullOrWhiteSpace({1}.Name)", NextCommand.Name, GetActiveObjData());
					});
				}
			}
		}

		public virtual void ResolveRecordProcessWhereClauseList()
		{
			ObjData.GetRecordList = new List<IGameBase>();

			foreach (var whereClauseFunc in ObjData.RecordWhereClauseList)
			{
				ObjData.GetRecordList = ObjData.GetRecordListFunc(whereClauseFunc);

				Debug.Assert(ObjData.GetRecordList != null);

				ObjData.FilterRecordList = ObjData.FilterRecordListFunc(ObjData.GetRecordList, ObjData.Name);

				Debug.Assert(ObjData.FilterRecordList != null);

				if (ObjData.FilterRecordList.Count > 0)
				{
					break;
				}
			}
		}

		public virtual void SetLastNameStrings(IGameBase obj, string objDataName, IArtifact artifact, IMonster monster)
		{
			if (gGameState.EnhancedParser && obj != null && !string.IsNullOrWhiteSpace(objDataName))
			{
				var objDataName01 = string.Format(" {0} ", objDataName);

				if (Array.FindIndex(gEngine.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? objDataName01.IndexOf(" " + token + " ") >= 0 : objDataName01.IndexOf(token) >= 0) < 0 && Array.FindIndex(gEngine.PronounTokens, token => objDataName01.IndexOf(" " + token + " ") >= 0) < 0)
				{
					if (artifact != null)
					{
						if (artifact.IsPlural)
						{
							LastThemNameStr = gEngine.CloneInstance(objDataName);
						}
						else
						{
							LastItNameStr = gEngine.CloneInstance(objDataName);
						}
					}
					else
					{
						Debug.Assert(monster != null);

						if (monster.GroupCount > 1)
						{
							LastThemNameStr = gEngine.CloneInstance(objDataName);
						}

						if (monster.CurrGroupCount == 1)
						{
							if (monster.Gender == Gender.Male)
							{
								LastHimNameStr = gEngine.CloneInstance(objDataName);
							}
							else if (monster.Gender == Gender.Female)
							{
								LastHerNameStr = gEngine.CloneInstance(objDataName);
							}
							else
							{
								LastItNameStr = gEngine.CloneInstance(objDataName);
							}
						}
					}
				}
			}
		}

		public virtual void SetLastNameStrings(IList<IArtifact> artifactList)
		{
			Debug.Assert(artifactList != null);

			if (gGameState.EnhancedParser && artifactList.Count > 0)
			{
				var themIndex = Array.FindLastIndex(artifactList.ToArray(), artifact => artifact.IsPlural);

				if (themIndex >= 0)
				{
					LastThemNameStr = gEngine.CloneInstance(artifactList[themIndex].Name.ToLower());
				}
				else if (artifactList.Count > 1)
				{
					var themStr = "";

					for (var i = 0; i < artifactList.Count; i++)
					{
						themStr += artifactList[i].Name.ToLower();

						if (i < artifactList.Count - 1)
						{
							themStr += " , ";
						}
					}

					LastThemNameStr = gEngine.CloneInstance(themStr);
				}

				var itIndex = Array.FindLastIndex(artifactList.ToArray(), artifact => !artifact.IsPlural);

				if (itIndex >= 0)
				{
					LastItNameStr = gEngine.CloneInstance(artifactList[itIndex].Name.ToLower());
				}
			}
		}

		public virtual void FinishParsing()
		{
			Debug.Assert(NextCommand != null);

			Debug.Assert(ActorMonster != null);

			Debug.Assert(ActorRoom != null);

			var methodName = string.Format("FinishParsing{0}", NextCommand.GetType().Name);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			// Note: verb-only Commands can omit the handler; those that accept Dobj/Iobj require it

			if (methodInfo != null)
			{
				try
				{
					methodInfo.Invoke(this, null);

					if (Dobj != null)
					{
						if (gGameState.IobjPronounAffinity)
						{
							SetLastNameStrings(Dobj, DobjData.Name, DobjArtifact, DobjMonster);

							SetLastNameStrings(Iobj, IobjData.Name, IobjArtifact, IobjMonster);
						}
						else
						{
							SetLastNameStrings(Iobj, IobjData.Name, IobjArtifact, IobjMonster);

							SetLastNameStrings(Dobj, DobjData.Name, DobjArtifact, DobjMonster);
						}
					}
				}
				catch (TargetInvocationException ex)
				{
					if (ex.InnerException != null)
					{
						ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
					}
					else
					{
						throw;
					}
				}
			}
		}

		public virtual bool ShouldStripTrailingPunctuation()
		{
			return NextCommand is ISaveCommand || NextCommand is ISayCommand ? false : true;
		}

		public virtual string GetActiveObjData()
		{
			return ObjData == DobjData ? "DobjData" : "IobjData";
		}

		public virtual void SetRecord(IGameBase record)
		{
			Debug.Assert(NextCommand != null);

			if (ObjData == DobjData)
			{
				Dobj = record;
			}
			else
			{
				Iobj = record;
			}

			ObjData.Obj = record;
		}

		public virtual IGameBase GetRecord()
		{
			Debug.Assert(NextCommand != null);

			return ObjData == DobjData ? Dobj : Iobj;
		}

		public virtual void Clear()
		{
			InputBuf.Clear();

			CurrInputStr = "";

			OrigInputStr = "";

			CommandFormatStr = "";

			NewCommandStr = "";

			Tokens = null;

			CurrToken = 0;

			NameIndex = -1;

			PrepTokenIndex = -1;

			Prep = null;

			_actorMonster = null;

			_actorRoom = null;

			DobjData = gEngine.CreateInstance<IParserData>();

			IobjData = gEngine.CreateInstance<IParserData>();

			ObjData = DobjData;

			NextState = null;
		}

		public virtual void ParseName()
		{
			RetCode rc;

			Debug.Assert(NextCommand != null);

			if (ObjData.Name == null)
			{
				ObjData.Name = "";

				while (string.IsNullOrWhiteSpace(ObjData.Name))
				{
					if (NextCommand.IsDobjPrepEnabled || NextCommand.IsIobjEnabled)
					{
						Predicate<string> prepFindFunc = token => gEngine.Preps.FirstOrDefault(prep => prep.Name.Equals(token, StringComparison.OrdinalIgnoreCase) && NextCommand.IsPrepEnabled(prep)) != null;

						PrepTokenIndex = NextCommand.IsDobjPrepEnabled ? Array.FindIndex(Tokens, prepFindFunc) : NextCommand.IsIobjEnabled ? Array.FindLastIndex(Tokens, prepFindFunc) : -1;

						Prep = PrepTokenIndex >= 0 ? gEngine.Preps.FirstOrDefault(prep => prep.Name.Equals(Tokens[PrepTokenIndex], StringComparison.OrdinalIgnoreCase) && NextCommand.IsPrepEnabled(prep)) : null;

						if (Prep != null)
						{
							NextCommand.Prep = gEngine.CloneInstance(Prep);

							NextCommand.ContainerType = Prep.ContainerType;
						}
					}

					var numTokens = Tokens.Length - CurrToken;

					if (((ObjData == DobjData && NextCommand.IsDobjPrepEnabled) || (ObjData == IobjData && NextCommand.IsIobjEnabled)) && PrepTokenIndex == CurrToken)
					{
						CurrToken++;

						numTokens--;
					}
					else if (ObjData == DobjData && NextCommand.IsIobjEnabled && PrepTokenIndex >= CurrToken)
					{
						numTokens = PrepTokenIndex - CurrToken;
					}

					ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken).Take((int)numTokens));

					CurrToken += numTokens;

					if (string.IsNullOrWhiteSpace(ObjData.Name))
					{
						Debug.Assert(ActorMonster.IsCharacterMonster());

						if (ObjData == DobjData)
						{
							if (ObjData.QueryPrintFunc == null)
							{
								ObjData.QueryPrintFunc = () => gEngine.PrintVerbPrepWhoOrWhat(NextCommand);
							}
						}
						else
						{
							Debug.Assert(ObjData.QueryPrintFunc != null);
						}

						ObjData.QueryPrintFunc();

						gEngine.Buf.SetFormat("{0}", gEngine.In.ReadLine());

						gEngine.Buf.SetFormat("{0}", Regex.Replace(gEngine.Buf.ToString(), @"\s+", " ").Trim());

						var newTokenList = gEngine.Buf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();

						var origTokenList = OrigInputStr.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();

						if (CurrToken == 1 && CurrToken < origTokenList.Count)
						{
							origTokenList.InsertRange((int)CurrToken, newTokenList);
						}
						else
						{
							origTokenList.AddRange(newTokenList);
						}

						gEngine.Buf.SetFormat("{0}", string.Join(" ", origTokenList.ToArray()));

						OrigInputStr = gEngine.Buf.ToString();

						gEngine.Buf.SetFormat("{0}", gEngine.NormalizePlayerInput(gEngine.Buf).ToString());

						CurrInputStr = string.Format(" {0} ", gEngine.Buf.ToString());

						gEngine.Buf.SetFormat("{0}", gEngine.ReplacePrepositions(gEngine.Buf).ToString());

						if (ShouldStripTrailingPunctuation())
						{
							gEngine.Buf.SetFormat("{0}", gEngine.Buf.TrimEndPunctuationMinusUniqueChars().ToString().Trim());
						}

						Tokens = gEngine.Buf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					}
					else
					{
						ObjData.Name = string.Format(" {0} ", ObjData.Name);

						if (gGameState.EnhancedParser && ObjData == DobjData)
						{
							NameIndex = CurrInputStr.IndexOf(ObjData.Name);

							if (NameIndex < 0)
							{
								throw new GeneralParsingErrorException();
							}

							CommandFormatStr = CurrInputStr.Substring(0, (int)NameIndex) + "{0}" + CurrInputStr.Substring((int)NameIndex + ObjData.Name.Length);
						}

						var objNameTokens = ObjData.Name.IndexOf(" , ") >= 0 ? ObjData.Name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { ObjData.Name };

						objNameTokens = objNameTokens.Where(objNameToken => !string.IsNullOrWhiteSpace(objNameToken) && Array.FindIndex(gEngine.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? objNameToken.IndexOf(" " + token + " ") >= 0 : token[0] != ',' && objNameToken.IndexOf(token) >= 0) < 0).ToArray();

						for (var i = 0; i < objNameTokens.Length; i++)
						{
							var mySeen = false;

							gEngine.Buf.SetFormat("{0}", objNameTokens[i].Trim());

							rc = gEngine.StripPrepsAndArticles(gEngine.Buf, ref mySeen);

							Debug.Assert(gEngine.IsSuccess(rc));

							objNameTokens[i] = string.Format(" {0} ", gEngine.Buf.ToString().Trim());
						}

						ObjData.Name = string.Join(",", objNameTokens).Trim();
					}
				}

				if (gGameState.EnhancedParser)
				{
					if (ObjData.Name.Equals("him", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(LastHimNameStr))
					{
						ObjData.Name = LastHimNameStr;
					}
					else if (ObjData.Name.Equals("her", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(LastHerNameStr))
					{
						ObjData.Name = LastHerNameStr;
					}
					else if ((ObjData.Name.Equals("it", StringComparison.OrdinalIgnoreCase) || ObjData.Name.Equals("that", StringComparison.OrdinalIgnoreCase)) && !string.IsNullOrWhiteSpace(LastItNameStr))
					{
						ObjData.Name = LastItNameStr;
					}
					else if ((ObjData.Name.Equals("them", StringComparison.OrdinalIgnoreCase) || ObjData.Name.Equals("those", StringComparison.OrdinalIgnoreCase)) && !string.IsNullOrWhiteSpace(LastThemNameStr) && (ObjData == DobjData || LastThemNameStr.IndexOf(" , ") < 0))
					{
						ObjData.Name = LastThemNameStr;
					}

					if (ObjData == DobjData && ObjData.Name.IndexOf(" , ") >= 0)
					{
						throw new InvalidDobjNameListException(string.Format(" {0} ", ObjData.Name));
					}
				}
			}
		}

		public virtual void CheckPlayerCommand(bool afterFinishParsing)
		{
			Debug.Assert(NextCommand != null);

			// Do nothing
		}

		public virtual void Execute()
		{
			Debug.Assert(ActorMonster != null);

			ActorRoom = ActorMonster.GetInRoom();

			Debug.Assert(ActorRoom != null);

			InputBuf.SetFormat("{0}", Regex.Replace(InputBuf.ToString(), @"\s+", " ").Trim());

			if (InputBuf.Length == 0)
			{
				InputBuf.SetFormat("{0}", LastInputStr);

				if (InputBuf.Length > 0 && ActorMonster.IsCharacterMonster())
				{
					if (gEngine.LineWrapUserInput)
					{
						gEngine.LineWrap(InputBuf.ToString(), gEngine.Buf, gEngine.CommandPrompt.Length);
					}
					else
					{
						gEngine.Buf.SetFormat("{0}", InputBuf.ToString());
					}

					gOut.WordWrap = false;

					gOut.BackpatchLastCommand(gEngine.Buf.ToString());

					gOut.WordWrap = true;
				}
			}

			OrigInputStr = InputBuf.ToString();

			LastInputStr = InputBuf.ToString();

			InputBuf = gEngine.NormalizePlayerInput(InputBuf);

			CurrInputStr = string.Format(" {0} ", InputBuf.ToString());

			InputBuf = gEngine.ReplacePrepositions(InputBuf);

			Tokens = InputBuf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			try
			{
				if (CurrToken < Tokens.Length)
				{
					if (Tokens.Length == 1)
					{
						gEngine.Buf.SetFormat("{0}", Tokens[CurrToken]);

						Tokens[CurrToken] = gEngine.Buf.TrimEndPunctuationMinusUniqueChars().ToString().Trim();
					}

					if (Tokens[CurrToken].Length == 0)
					{
						Tokens[CurrToken] = "???";
					}
					else if (Tokens[CurrToken].Equals("at", StringComparison.OrdinalIgnoreCase))
					{
						Tokens[CurrToken] = "a";
					}

					var command = gEngine.GetCommandUsingToken(ActorMonster, Tokens[CurrToken]);

					if (command != null)
					{
						CurrToken++;

						if (gEngine.IsQuotedStringCommand(command))
						{
							InputBuf.SetFormat("{0}", OrigInputStr);

							Tokens = InputBuf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
						}

						NextState = Activator.CreateInstance(command.GetType()) as IState;

						Debug.Assert(NextCommand != null);

						NextCommand.CommandParser = this;

						NextCommand.ActorMonster = _actorMonster;

						NextCommand.ActorRoom = _actorRoom;

						NextCommand.Dobj = DobjData.Obj;

						NextCommand.Iobj = IobjData.Obj;

						if (ShouldStripTrailingPunctuation() && Tokens.Length > 1)
						{
							gEngine.Buf.SetFormat("{0}", Tokens[Tokens.Length - 1]);

							Tokens[Tokens.Length - 1] = gEngine.Buf.TrimEndPunctuationMinusUniqueChars().ToString().Trim();
						}

						if (ActorMonster.IsCharacterMonster())
						{
							CheckPlayerCommand(false);

							if (NextCommand != null)
							{
								if (ActorRoom.IsLit() || NextCommand.IsDarkEnabled)
								{
									FinishParsing();

									if (NextCommand != null)
									{
										CheckPlayerCommand(true);
									}
								}
								else
								{
									NextState = null;
								}
							}
						}
						else
						{
							FinishParsing();
						}

						if (NextState == null)
						{
							NextState = gEngine.CreateInstance<IStartState>();
						}
					}
				}
			}
			catch (InvalidDobjNameListException ex)
			{
				Debug.Assert(!string.IsNullOrWhiteSpace(ex.DobjNameStr));

				Debug.Assert(!string.IsNullOrWhiteSpace(CommandFormatStr));

				NewCommandStr = string.Format(CommandFormatStr, ex.DobjNameStr).Trim();

				if (gSentenceParser.ParserInputStrList.Count > 0)
				{
					gSentenceParser.ParserInputStrList.Insert(0, NewCommandStr);
				}
				else
				{
					gSentenceParser.ParserInputStrList.Add(NewCommandStr);
				}

				NextState = gEngine.CreateInstance<IGetPlayerInputState>(x =>
				{
					x.RestartCommand = true;
				});
			}
			catch (GeneralParsingErrorException)
			{
				Debug.Assert(NextState != null);

				NextState.PrintDontFollowYou02();

				NextState = gEngine.CreateInstance<IStartState>();
			}
		}

		public CommandParser()
		{
			InputBuf = new StringBuilder(gEngine.BufSize);

			LastInputStr = "";

			LastHimNameStr = "";

			LastHerNameStr = "";

			LastItNameStr = "";

			LastThemNameStr = "";

			Clear();
		}
	}
}
