
// DeleteRecordMenu.cs

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
	public abstract class DeleteRecordMenu<T, U> : RecordMenu<T>, IDeleteRecordMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle(Title, true);

			gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(55, '\0', 0, string.Format("Enter the Uid of the {0} record to delete", RecordTypeName), null));

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', false, null, null, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			var recordUid = Convert.ToInt64(Buf.Trim().ToString());

			gOut.Print("{0}", gEngine.LineSep);

			var record = RecordTable.FindRecord(recordUid);

			if (record == null)
			{
				gOut.Print("{0} record not found.", RecordTypeName);

				goto Cleanup;
			}

			var character = record as ICharacter;

			var artifact = record as IArtifact;

			if (character == null && artifact != null)
			{
				var characterUid = artifact.IsCarriedByCharacter() ? artifact.GetCarriedByCharacterUid() : artifact.GetWornByCharacterUid();

				character = gDatabase.FindCharacter(characterUid);
			}

			var character01 = record as ICharacter;

			if (character01 != null && character01.Status != Status.Alive && character01.Status != Status.Dead)
			{
				gOut.Print("{0} record Status not marked as Alive or Dead.", RecordTypeName);

				goto Cleanup;
			}

			var helper = gEngine.CreateInstance<U>(x =>
			{
				x.RecordTable = RecordTable;
				
				x.Record = record;
			});
			
			helper.ListRecord(true, true, false, true, false, false);

			PrintPostListLineSep();

			gOut.Write("{0}Would you like to delete this {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

			Buf.Clear();

			rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Buf.Length > 0 && Buf[0] == 'N')
			{
				goto Cleanup;
			}

			if (character01 != null && gEngine.Config.DeleteCharArts && gDatabase.ArtifactTableType == ArtifactTableType.CharArt)
			{
				var artifactList = character01.GetContainedList();

				foreach (var artifact01 in artifactList)
				{
					var artifact02 = gDatabase.RemoveArtifact(artifact01.Uid);

					Debug.Assert(artifact02 != null);

					artifact02.Dispose();

					artifact02 = null;
				}

				if (artifactList.Count > 0)
				{
					gEngine.CharArtsModified = true;
				}
			}

			record = RecordTable.RemoveRecord(recordUid);

			Debug.Assert(record != null);

			if (character != null && artifact != null && gDatabase.ArtifactTableType == ArtifactTableType.CharArt)
			{
				var result1 = gEngine.SwapGreaterArmorUidWithLesserShieldUid(character);

				var result2 = character.StripUniqueCharsFromWeaponNames();

				var result3 = character.AddUniqueCharsToWeaponNames();

				if (result1 || result2 || result3)
				{
					gEngine.CharArtsModified = true;
				}
			}

			record.Dispose();

			UpdateGlobals();

		Cleanup:

			;
		}
	}
}
