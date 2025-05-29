
// AddRecordCopyMenu.cs

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
	public abstract class AddRecordCopyMenu<T, U> : RecordMenu<T>, IAddRecordCopyMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public virtual long NewRecordUid { get; set; }

		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle(Title, true);

			var recordCount = RecordTable.GetRecordCount();

			if (recordCount + 1 > gEngine.NumRecords)
			{
				gOut.Print("{0} database table has exhausted all available Uids.", RecordTypeName);

				goto Cleanup;
			}

			gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(55, '\0', 0, string.Format("Enter the Uid of the {0} record to copy", RecordTypeName), "1"));

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			var recordUid = Convert.ToInt64(Buf.Trim().ToString());

			gOut.Print("{0}", gEngine.LineSep);

			var record = RecordTable.FindRecord(recordUid);

			if (record == null)
			{
				gOut.Print("{0} record not found.", RecordTypeName);

				goto Cleanup;
			}

			var record01 = gEngine.CloneInstance(record);

			Debug.Assert(record01 != null);

			if (!gEngine.Config.GenerateUids)
			{
				do
				{
					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(55, '\0', 0, string.Format("Enter the Uid of the {0} record copy", RecordTypeName), null));

					Buf.Clear();

					rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', false, null, null, gEngine.IsCharDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					NewRecordUid = Convert.ToInt64(Buf.Trim().ToString());
				}
				while (NewRecordUid < 0 || NewRecordUid > gEngine.NumRecords);

				if (NewRecordUid > 0)
				{
					gOut.Print("{0}", gEngine.LineSep);

					record = RecordTable.FindRecord(NewRecordUid);

					if (record != null)
					{
						gOut.Print("{0} record already exists.", RecordTypeName);

						goto Cleanup;
					}
				}
				else
				{
					goto Cleanup;
				}
			}
			else
			{
				NewRecordUid = RecordTable.GetRecordUid();
			}

			record01.Uid = NewRecordUid;

			var character = record01 as ICharacter;

			var artifact = record01 as IArtifact;

			if (character == null && artifact != null)
			{
				var characterUid = artifact.IsCarriedByCharacter() ? artifact.GetCarriedByCharacterUid() : artifact.GetWornByCharacterUid();

				character = gDatabase.FindCharacter(characterUid);
			}

			if (artifact != null && gDatabase.ArtifactTableType == ArtifactTableType.CharArt)
			{
				artifact.SetArtifactCategoryCount(1);

				artifact.SetInLimbo();
			}

			var helper = gEngine.CreateInstance<U>(x =>
			{
				x.RecordTable = RecordTable;

				x.Record = record01;
			});
			
			helper.ListRecord(true, true, false, true, false, false);

			RecordTable.FreeUids.Remove(record01.Uid);

			PrintPostListLineSep();

			gOut.Write("{0}Would you like to save this {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Buf.Length > 0 && Buf[0] == 'N')
			{
				record01.Dispose();

				goto Cleanup;
			}

			rc = RecordTable.AddRecord(record01);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (character != null && gDatabase.ArtifactTableType == ArtifactTableType.CharArt)
			{
				var result1 = gEngine.SwapGreaterArmorUidWithLesserShieldUid(character);

				var result2 = character.StripUniqueCharsFromWeaponNames();

				var result3 = character.AddUniqueCharsToWeaponNames();

				if (result1 || result2 || result3)
				{
					gEngine.CharArtsModified = true;
				}
			}

			UpdateGlobals();

		Cleanup:

			NewRecordUid = 0;
		}
	}
}
