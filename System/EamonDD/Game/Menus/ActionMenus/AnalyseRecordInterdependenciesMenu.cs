
// AnalyseRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Framework.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

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

			var errorHelper = gEngine.CreateInstance<U>(x =>
			{
				x.RecordTable = RecordTable;
				
				x.Record = ErrorRecord;

				x.Index = ValidateHelper.Index;

				x.ShowDesc = ValidateHelper.ShowDesc;

				x.ErrorFieldName = ValidateHelper.ErrorFieldName;
			});

			errorHelper.ListErrorField();

			gOut.Print("{0}", ValidateHelper.ErrorMessage);

			gOut.Print("{0}", gEngine.LineSep);

			gOut.Write("{0}S=Skip field; T=Edit this record; R={1} referred to record; X=Exit: ",
				Environment.NewLine,
				ValidateHelper.NewRecordUid > 0 ? "Add" : "Edit");

			ValidateHelper.Buf.Clear();

			var rc = gEngine.In.ReadField(ValidateHelper.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharSOrTOrROrX, gEngine.IsCharSOrTOrROrX);

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

				if (ErrorRecord is ICharacter)
				{
					menu = gEngine.CreateInstance<IEditCharacterRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (ICharacter)ErrorRecord;
					});
				}
				else if (ErrorRecord is IArtifact)
				{
					menu = gEngine.CreateInstance<IEditArtifactRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IArtifact)ErrorRecord;
					});
				}
				else if (ErrorRecord is IEffect)
				{
					menu = gEngine.CreateInstance<IEditEffectRecordMenu>(x =>
					{
						x.EditRecord = (IEffect)ErrorRecord;
					});
				}
				else if (ErrorRecord is IHint)
				{
					menu = gEngine.CreateInstance<IEditHintRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IHint)ErrorRecord;
					});
				}
				else if (ErrorRecord is IModule)
				{
					menu = gEngine.CreateInstance<IEditModuleRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IModule)ErrorRecord;
					});
				}
				else if (ErrorRecord is IMonster)
				{
					menu = gEngine.CreateInstance<IEditMonsterRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IMonster)ErrorRecord;
					});
				}
				else
				{
					Debug.Assert(ErrorRecord is IRoom);

					menu = gEngine.CreateInstance<IEditRoomRecordManyFieldsMenu>(x =>
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
					if (ValidateHelper.RecordType == typeof(ICharacter))
					{
						menu = gEngine.CreateInstance<IAddCharacterRecordManualMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IArtifact))
					{
						menu = gEngine.CreateInstance<IAddArtifactRecordManualMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IEffect))
					{
						menu = gEngine.CreateInstance<IAddEffectRecordMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IHint))
					{
						menu = gEngine.CreateInstance<IAddHintRecordMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IModule))
					{
						menu = gEngine.CreateInstance<IAddModuleRecordMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IMonster))
					{
						menu = gEngine.CreateInstance<IAddMonsterRecordManualMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
					else
					{
						Debug.Assert(ValidateHelper.RecordType == typeof(IRoom));

						menu = gEngine.CreateInstance<IAddRoomRecordManualMenu>(x =>
						{
							x.NewRecordUid = ValidateHelper.NewRecordUid;
						});
					}
				}
				else
				{
					if (ValidateHelper.RecordType == typeof(ICharacter))
					{
						menu = gEngine.CreateInstance<IEditCharacterRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (ICharacter)ValidateHelper.EditRecord;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IArtifact))
					{
						menu = gEngine.CreateInstance<IEditArtifactRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IArtifact)ValidateHelper.EditRecord;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IEffect))
					{
						menu = gEngine.CreateInstance<IEditEffectRecordMenu>(x =>
						{
							x.EditRecord = (IEffect)ValidateHelper.EditRecord;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IHint))
					{
						menu = gEngine.CreateInstance<IEditHintRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IHint)ValidateHelper.EditRecord;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IModule))
					{
						menu = gEngine.CreateInstance<IEditModuleRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IModule)ValidateHelper.EditRecord;
						});
					}
					else if (ValidateHelper.RecordType == typeof(IMonster))
					{
						menu = gEngine.CreateInstance<IEditMonsterRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IMonster)ValidateHelper.EditRecord;
						});
					}
					else
					{
						Debug.Assert(ValidateHelper.RecordType == typeof(IRoom));

						menu = gEngine.CreateInstance<IEditRoomRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IRoom)ValidateHelper.EditRecord;
						});
					}
				}

				menu.Execute();
			}

			gOut.Print("{0}", gEngine.LineSep);
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

				ValidateHelper.RecordTable = RecordTable;
				
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
					ValidateHelper.ShowDesc = gEngine.Config.ShowDesc;

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

			ValidateHelper = gEngine.CreateInstance<U>();

			ClearSkipNameList = true;
		}
	}
}
