﻿
// AddRecordManualMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Framework.Primitive.Enums;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AddRecordManualMenu<T, U> : RecordMenu<T>, IAddRecordManualMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public virtual long NewRecordUid { get; set; }

		public override void Execute()
		{
			RetCode rc;

			T record;

			gOut.WriteLine();

			gEngine.PrintTitle(Title, true);

			if (!gEngine.Config.GenerateUids && NewRecordUid == 0)
			{
				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(55, '\0', 0, string.Format("Enter the Uid of the {0} record to add", RecordTypeName), null));

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', false, null, null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				NewRecordUid = Convert.ToInt64(Buf.Trim().ToString());

				gOut.Print("{0}", gEngine.LineSep);

				if (NewRecordUid > 0)
				{
					record = RecordTable.FindRecord(NewRecordUid);

					if (record != null)
					{
						gOut.Print("{0} record already exists.", RecordTypeName);

						goto Cleanup;
					}

					RecordTable.FreeUids.Remove(NewRecordUid);
				}
			}

			record = gEngine.CreateInstance<T>(x =>
			{
				x.Uid = NewRecordUid;
			});
			
			var helper = gEngine.CreateInstance<U>(x =>
			{
				x.Record = record;
			});
			
			helper.InputRecord(false, gEngine.Config.FieldDesc);

			gEngine.Thread.Sleep(150);

			gOut.Write("{0}Would you like to save this {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

			Debug.Assert(gEngine.IsSuccess(rc));

			gEngine.Thread.Sleep(150);

			if (Buf.Length > 0 && Buf[0] == 'N')
			{
				record.Dispose();

				goto Cleanup;
			}

			var character = record as ICharacter;

			if (character != null)
			{
				character.StripUniqueCharsFromWeaponNames();

				character.AddUniqueCharsToWeaponNames();
			}

			var artifact = record as IArtifact;

			if (artifact != null)
			{
				var i = Array.FindIndex(artifact.Categories, ac => ac != null && ac.Type == ArtifactType.None);
				
				if (i > 0)
				{
					rc = artifact.SetArtifactCategoryCount(i);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				gEngine.TruncatePluralTypeEffectDesc(artifact.PluralType, gEngine.ArtNameLen);
			}

			var effect = record as IEffect;

			if (effect != null)
			{
				gEngine.TruncatePluralTypeEffectDesc(effect);
			}

			var monster = record as IMonster;

			if (monster != null)
			{
				gEngine.TruncatePluralTypeEffectDesc(monster.PluralType, gEngine.MonNameLen);
			}

			rc = RecordTable.AddRecord(record);

			Debug.Assert(gEngine.IsSuccess(rc));

			UpdateGlobals();

		Cleanup:

			NewRecordUid = 0;
		}
	}
}
