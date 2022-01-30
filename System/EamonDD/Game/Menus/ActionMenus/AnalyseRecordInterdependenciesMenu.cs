
// AnalyseRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Framework.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AnalyseRecordInterdependenciesMenu<T, U> : RecordMenu<T>, IAnalyseRecordInterdependenciesMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public virtual IList<string> SkipNameList { get; set; }

		public virtual IHelper<T> ValidateHelper { get; set; }

		public virtual T ErrorRecord { get; set; }

		public virtual bool ClearSkipNameList { get; set; }

		public virtual bool ModifyFlag { get; set; }

		public virtual bool ExitFlag { get; set; }

		public virtual void ProcessInterdependency()
		{
			Debug.Assert(ErrorRecord != null);

			var errorHelper = Globals.CreateInstance<U>(x =>
			{
				x.Record = ErrorRecord;

				x.Index = ValidateHelper.Index;

				x.ShowDesc = ValidateHelper.ShowDesc;

				x.ErrorFieldName = ValidateHelper.ErrorFieldName;
			});

			errorHelper.ListErrorField();

			gOut.Print("{0}", ValidateHelper.ErrorMessage);

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}S=Skip field, T=Edit this record, R={1} referred to record, X=Exit: ",
				Environment.NewLine,
				ValidateHelper.NewRecordUid > 0 ? "Add" : "Edit");

			ValidateHelper.Buf.Clear();

			var rc = Globals.In.ReadField(ValidateHelper.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharSOrTOrROrX, gEngine.IsCharSOrTOrROrX);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (ValidateHelper.Buf.Length == 0 || ValidateHelper.Buf[0] == 'X')
			{
				ExitFlag = true;
			}
			else if (ValidateHelper.Buf[0] == 'S')
			{
				var uniqueName = string.Format("{0}_{1}_{2}", typeof(T).Name, ErrorRecord.Uid, errorHelper.GetName(errorHelper.ErrorFieldName));

				SkipNameList.Add(uniqueName);
			}
			else if (ValidateHelper.Buf[0] == 'T')
			{
				IMenu menu;

				ModifyFlag = true;

				if (ErrorRecord is IArtifact)
				{
					menu = Globals.CreateInstance<IEditArtifactRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IArtifact)ErrorRecord;
					});
				}
				else if (ErrorRecord is IEffect)
				{
					menu = Globals.CreateInstance<IEditEffectRecordMenu>(x =>
					{
						x.EditRecord = (IEffect)ErrorRecord;
					});
				}
				else if (ErrorRecord is IHint)
				{
					menu = Globals.CreateInstance<IEditHintRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IHint)ErrorRecord;
					});
				}
				else if (ErrorRecord is IModule)
				{
					menu = Globals.CreateInstance<IEditModuleRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IModule)ErrorRecord;
					});
				}
				else if (ErrorRecord is IMonster)
				{
					menu = Globals.CreateInstance<IEditMonsterRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IMonster)ErrorRecord;
					});
				}
				else
				{
					Debug.Assert(ErrorRecord is IRoom);

					menu = Globals.CreateInstance<IEditRoomRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IRoom)ErrorRecord;
					});
				}

				menu.Execute();
			}
			else if (ValidateHelper.Buf[0] == 'R')
			{
				IMenu menu;

				ModifyFlag = true;

				if (ValidateHelper.NewRecordUid > 0)
				{
					if (ValidateHelper.RecordType == typeof(IArtifact))
					{
						menu = Globals.CreateInstance<IAddArtifactRecordManualMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IEffect))
					{
						menu = Globals.CreateInstance<IAddEffectRecordMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IHint))
					{
						menu = Globals.CreateInstance<IAddHintRecordMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IModule))
					{
						menu = Globals.CreateInstance<IAddModuleRecordMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IMonster))
					{
						menu = Globals.CreateInstance<IAddMonsterRecordManualMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else
					{
						Debug.Assert(ValidateHelper.RecordType == typeof(IRoom));

						menu = Globals.CreateInstance<IAddRoomRecordManualMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
				}
				else
				{
					if (ValidateHelper.RecordType == typeof(IArtifact))
					{
						menu = Globals.CreateInstance<IEditArtifactRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IArtifact)ValidateHelper.EditRecord;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IEffect))
					{
						menu = Globals.CreateInstance<IEditEffectRecordMenu>(x =>
						{
							x.EditRecord = (IEffect)ValidateHelper.EditRecord;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IHint))
					{
						menu = Globals.CreateInstance<IEditHintRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IHint)ValidateHelper.EditRecord;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IModule))
					{
						menu = Globals.CreateInstance<IEditModuleRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IModule)ValidateHelper.EditRecord;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IMonster))
					{
						menu = Globals.CreateInstance<IEditMonsterRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IMonster)ValidateHelper.EditRecord;
						});
					}
					else
					{
						Debug.Assert(ValidateHelper.RecordType == typeof(IRoom));

						menu = Globals.CreateInstance<IEditRoomRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IRoom)ValidateHelper.EditRecord;
						});
					}
				}

				menu.Execute();
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		public override void Execute()
		{
			gOut.WriteLine();

			gEngine.PrintTitle(Title, true);

			if (ClearSkipNameList)
			{
				SkipNameList.Clear();
			}

			ValidateHelper.Clear();

			ModifyFlag = false;

			ExitFlag = false;

			while (true)
			{
				ErrorRecord = default(T);

				foreach (var record in RecordTable.Records)
				{
					ValidateHelper.Record = record;

					var nameList = ValidateHelper.GetNameList((n) =>
					{
						var uniqueName = string.Format("{0}_{1}_{2}", typeof(T).Name, record.Uid, n);

						return !SkipNameList.Contains(uniqueName);
					});

					foreach (var name in nameList)
					{
						ValidateHelper.Clear();

						if (!ValidateHelper.ValidateFieldInterdependencies(ValidateHelper.GetFieldName(name)))
						{
							ErrorRecord = record;

							goto ProcessError;
						}
					}
				}

			ProcessError:

				if (ErrorRecord != null)
				{
					ValidateHelper.ShowDesc = Globals.Config.ShowDesc;

					ProcessInterdependency();

					if (ExitFlag)
					{
						break;
					}
				}
				else
				{
					break;
				}
			}

			gOut.Print("Done analysing {0} records.", RecordTypeName);
		}

		public AnalyseRecordInterdependenciesMenu()
		{
			SkipNameList = new List<string>();

			ValidateHelper = Globals.CreateInstance<U>();

			ClearSkipNameList = true;
		}
	}
}
