
// ConfigHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class ConfigHelper : Helper<IConfig>, IConfigHelper
	{
		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameShowDesc()
		{
			return "Show Descs";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameResolveEffects()
		{
			return "Resolve Effects";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameGenerateUids()
		{
			return "Generate Uids";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameFieldDesc()
		{
			return "Field Descs";
		}

		#endregion

		#region GetName Methods

		// do nothing

		#endregion

		#region GetValue Methods

		// do nothing

		#endregion

		#region Validate Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateFieldDesc()
		{
			return Enum.IsDefined(typeof(FieldDesc), Record.FieldDesc);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWordWrapMargin()
		{
			return Record.WordWrapMargin == Constants.RightMargin;
		}

		#endregion

		#region ValidateInterdependencies Methods

		// do nothing

		#endregion

		#region PrintDesc Methods

		/// <summary></summary>
		public virtual void PrintDescShowDesc()
		{
			var fullDesc = "Enter whether to omit or show descriptions during record detail listing.";

			var briefDesc = string.Format("{0}=Omit descriptions; {1}=Show descriptions", Convert.ToInt64(false), Convert.ToInt64(true));

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescResolveEffects()
		{
			var fullDesc = "Enter whether to show or resolve Effect Uids in descriptions during record detail listing.";

			var briefDesc = string.Format("{0}=Show Effect Uids; {1}=Resolve Effect Uids", Convert.ToInt64(false), Convert.ToInt64(true));

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescGenerateUids()
		{
			var fullDesc = "Enter whether to allow user input of Uids or use system generated Uids when adding new records.";

			var briefDesc = string.Format("{0}=Allow user input of Uids; {1}=Use system generated Uids", Convert.ToInt64(false), Convert.ToInt64(true));

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescFieldDesc()
		{
			var fullDesc = "Enter the verbosity of the field descriptions shown during record input.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var fieldDescValues = EnumUtil.GetValues<FieldDesc>();

			for (var j = 0; j < fieldDescValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)fieldDescValues[j], gEngine.GetFieldDescNames(fieldDescValues[j]));
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		#endregion

		#region List Methods

		/// <summary></summary>
		public virtual void ListUid()
		{
			if (!ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Uid"), null), Record.Uid);
			}
		}

		/// <summary></summary>
		public virtual void ListShowDesc()
		{
			var listNum = NumberFields ? ListNum++ : 0;

			gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ShowDesc"), null), Convert.ToInt64(Record.ShowDesc));
		}

		/// <summary></summary>
		public virtual void ListResolveEffects()
		{
			var listNum = NumberFields ? ListNum++ : 0;

			gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ResolveEffects"), null), Convert.ToInt64(Record.ResolveEffects));
		}

		/// <summary></summary>
		public virtual void ListGenerateUids()
		{
			var listNum = NumberFields ? ListNum++ : 0;

			gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("GenerateUids"), null), Convert.ToInt64(Record.GenerateUids));
		}

		/// <summary></summary>
		public virtual void ListFieldDesc()
		{
			var listNum = NumberFields ? ListNum++ : 0;

			gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("FieldDesc"), null), gEngine.GetFieldDescNames(Record.FieldDesc));
		}

		#endregion

		#region Input Methods

		/// <summary></summary>
		public virtual void InputUid()
		{
			gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputShowDesc()
		{
			var fieldDesc = FieldDesc;

			var showDesc = Record.ShowDesc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(showDesc));

				PrintFieldDesc("ShowDesc", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("ShowDesc"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ShowDesc = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("ShowDesc"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputResolveEffects()
		{
			var fieldDesc = FieldDesc;

			var resolveEffects = Record.ResolveEffects;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(resolveEffects));

				PrintFieldDesc("ResolveEffects", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("ResolveEffects"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ResolveEffects = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("ResolveEffects"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputGenerateUids()
		{
			var fieldDesc = FieldDesc;

			var generateUids = Record.GenerateUids;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(generateUids));

				PrintFieldDesc("GenerateUids", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("GenerateUids"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.GenerateUids = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("GenerateUids"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputFieldDesc()
		{
			var fieldDesc = FieldDesc;

			var fieldDesc01 = Record.FieldDesc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)fieldDesc01);

				PrintFieldDesc("FieldDesc", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("FieldDesc"), "2"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "2", null, gEngine.IsChar0To2, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.FieldDesc = (FieldDesc)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("FieldDesc"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class ConfigHelper

		public override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetConfigUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		public ConfigHelper()
		{
			FieldNameList = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"ShowDesc",
				"ResolveEffects",
				"GenerateUids",
				"FieldDesc",
				"WordWrapMargin",
				"DdFilesetFileName",
				"DdCharacterFileName",
				"DdModuleFileName",
				"DdRoomFileName",
				"DdArtifactFileName",
				"DdEffectFileName",
				"DdMonsterFileName",
				"DdHintFileName",
				"MhWorkDir",
				"MhFilesetFileName",
				"MhCharacterFileName",
				"MhEffectFileName",
				"RtFilesetFileName",
				"RtCharacterFileName",
				"RtModuleFileName",
				"RtRoomFileName",
				"RtArtifactFileName",
				"RtEffectFileName",
				"RtMonsterFileName",
				"RtHintFileName",
				"RtGameStateFileName",
				"DdEditingFilesets",
				"DdEditingCharacters",
				"DdEditingModules",
				"DdEditingRooms",
				"DdEditingArtifacts",
				"DdEditingEffects",
				"DdEditingMonsters",
				"DdEditingHints",
			};
		}

		#endregion

		#endregion
	}
}
