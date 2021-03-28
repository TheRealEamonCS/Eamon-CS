
// EditRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class EditRecordManyFieldsMenu<T, U> : RecordMenu<T>, IEditRecordManyFieldsMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public virtual T EditRecord { get; set; }

		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle(Title, true);

			if (EditRecord == null)
			{
				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(55, '\0', 0, string.Format("Enter the Uid of the {0} record to edit", RecordTypeName), "1"));

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var recordUid = Convert.ToInt64(Buf.Trim().ToString());

				gOut.Print("{0}", Globals.LineSep);

				EditRecord = RecordTable.FindRecord(recordUid);

				if (EditRecord == null)
				{
					gOut.Print("{0} record not found.", RecordTypeName);

					goto Cleanup;
				}
			}

			var editRecord01 = Globals.CloneInstance(EditRecord);

			Debug.Assert(editRecord01 != null);
			
			var helper = Globals.CreateInstance<U>(x =>
			{
				x.Record = editRecord01;
			});
			
			helper.InputRecord(true, Globals.Config.FieldDesc);

			Globals.Thread.Sleep(150);

			if (!Globals.CompareInstances(EditRecord, editRecord01))
			{
				gOut.Write("{0}Would you like to save this updated {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				var character = editRecord01 as ICharacter;

				if (character != null)
				{
					character.StripUniqueCharsFromWeaponNames();

					character.AddUniqueCharsToWeaponNames();
				}

				var artifact = editRecord01 as IArtifact;

				if (artifact != null)
				{
					gEngine.TruncatePluralTypeEffectDesc(artifact.PluralType, Constants.ArtNameLen);
				}
				
				var effect = editRecord01 as IEffect;

				if (effect != null)
				{
					gEngine.TruncatePluralTypeEffectDesc(effect);
				}
				
				var monster = editRecord01 as IMonster;

				if (monster != null)
				{
					gEngine.TruncatePluralTypeEffectDesc(monster.PluralType, Constants.MonNameLen);
				}

				var record = RecordTable.RemoveRecord(EditRecord.Uid);

				Debug.Assert(record != null);

				rc = RecordTable.AddRecord(editRecord01);

				Debug.Assert(gEngine.IsSuccess(rc));

				UpdateGlobals();
			}
			else
			{
				gOut.Print("{0} record not modified.", RecordTypeName);
			}

		Cleanup:

			EditRecord = null;
		}
	}
}
