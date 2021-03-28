
// ModuleHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class ModuleHelper : Helper<IModule>, IModuleHelper
	{
		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

		public override void ListErrorField()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(ErrorFieldName));

			gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', 0, GetPrintedName("Uid"), null), Record.Uid);

			gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', 0, GetPrintedName("Name"), null), Record.Name);

			if (ErrorFieldName.Equals("Desc", StringComparison.OrdinalIgnoreCase) || ShowDesc)
			{
				gOut.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', 0, GetPrintedName("Desc"), null), Record.Desc);
			}

			if (ErrorFieldName.Equals("IntroStory", StringComparison.OrdinalIgnoreCase))
			{
				gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '.', 0, GetPrintedName("IntroStory"), null), Record.IntroStory);
			}
		}

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameVolLabel()
		{
			return "Volume Label";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameSerialNum()
		{
			return "Serial Number";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameLastMod()
		{
			return "Last Modified";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameIntroStory()
		{
			return "Intro Story";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameNumDirs()
		{
			return "Compass Directions";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameNumRooms()
		{
			return "Number Of Rooms";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameNumArtifacts()
		{
			return "Number Of Artifacts";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameNumEffects()
		{
			return "Number Of Effects";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameNumMonsters()
		{
			return "Number Of Monsters";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameNumHints()
		{
			return "Number Of Hints";
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
		public virtual bool ValidateName()
		{
			if (Record.Name != null)
			{
				Record.Name = Regex.Replace(Record.Name, @"\s+", " ").Trim();
			}

			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.ModNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateDesc()
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.ModDescLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateAuthor()
		{
			return string.IsNullOrWhiteSpace(Record.Author) == false && Record.Author.Length <= Constants.ModAuthorLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateVolLabel()
		{
			return string.IsNullOrWhiteSpace(Record.VolLabel) == false && Record.VolLabel.Length <= Constants.ModVolLabelLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSerialNum()
		{
			return string.IsNullOrWhiteSpace(Record.SerialNum) == false && Record.SerialNum.Length <= Constants.ModSerialNumLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateLastMod()
		{
			return Record.LastMod != null && Record.LastMod <= DateTime.Now;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateIntroStory()
		{
			return Record.IntroStory >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNumDirs()
		{
			return Record.NumDirs == 6 || Record.NumDirs == 12;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNumRooms()
		{
			return Record.NumRooms >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNumArtifacts()
		{
			return Record.NumArtifacts >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNumEffects()
		{
			return Record.NumEffects >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNumMonsters()
		{
			return Record.NumMonsters >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNumHints()
		{
			return Record.NumHints >= 0;
		}

		#endregion

		#region ValidateInterdependencies Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesDesc()
		{
			var result = true;

			long invalidUid = 0;

			var rc = gEngine.ResolveUidMacros(Record.Desc, Buf, false, false, ref invalidUid);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (invalidUid > 0)
			{
				result = false;

				Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Desc"), "Effect", invalidUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IEffect);

				NewRecordUid = invalidUid;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesIntroStory()
		{
			var result = true;

			if (Record.IntroStory > 0)
			{
				var effectUid = Record.IntroStory;

				var effect = gEDB[effectUid];

				if (effect == null)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("IntroStory"), "Effect", effectUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IEffect);

					NewRecordUid = effectUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		#endregion

		#region PrintDesc Methods

		/// <summary></summary>
		public virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the adventure.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescDesc()
		{
			var fullDesc = "Enter a detailed description of the adventure.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescAuthor()
		{
			var fullDesc = "Enter the name(s) of the adventure's author(s).";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescVolLabel()
		{
			var fullDesc = "Enter the volume label of the adventure, typically the author(s) initials followed by a private serial number.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescSerialNum()
		{
			var fullDesc = "Enter the global serial number of the adventure, typically assigned by an Eamon CS administrator.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescIntroStory()
		{
			var fullDesc = "Enter the Effect Uid of the introduction story for the Module." + Environment.NewLine + Environment.NewLine + "You can link multiple Effects together to create an extended story segment.";

			var briefDesc = "(GE 0)=Valid value";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescNumDirs()
		{
			var fullDesc = "Enter the number of compass directions to use for connections between Rooms in the adventure." + Environment.NewLine + Environment.NewLine + "Typically, the number of directions used determines the intricacy of a game's map.  Simple and complex adventures use six and twelve directions, respectively, but this is only a rule of thumb, not a requirement.";

			var briefDesc = "6=Six compass directions; 12=Twelve compass directions";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		#endregion

		#region List Methods

		/// <summary></summary>
		public virtual void ListUid()
		{
			if (FullDetail)
			{
				if (!ExcludeROFields)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Uid"), null), Record.Uid);
				}
			}
			else
			{
				gOut.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, gEngine.Capitalize(Record.Name));
			}
		}

		/// <summary></summary>
		public virtual void ListName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Name"), null), Record.Name);
			}
		}

		/// <summary></summary>
		public virtual void ListDesc()
		{
			if (FullDetail && ShowDesc)
			{
				Buf.Clear();

				if (ResolveEffects)
				{
					var rc = gEngine.ResolveUidMacros(Record.Desc, Buf, true, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}
				else
				{
					Buf.Append(Record.Desc);
				}

				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Desc"), null), Buf);
			}
		}

		/// <summary></summary>
		public virtual void ListAuthor()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Author"), null), Record.Author);
			}
		}

		/// <summary></summary>
		public virtual void ListVolLabel()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("VolLabel"), null), Record.VolLabel);
			}
		}

		/// <summary></summary>
		public virtual void ListSerialNum()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("SerialNum"), null), Record.SerialNum);
			}
		}

		/// <summary></summary>
		public virtual void ListLastMod()
		{
			if (FullDetail && !ExcludeROFields)
			{
				Buf.Clear();

				Buf.Append(Record.LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("LastMod"), null), Buf);
			}
		}

		/// <summary></summary>
		public virtual void ListIntroStory()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg && Record.IntroStory > 0)
				{
					Buf.Clear();

					var effect = gEDB[Record.IntroStory];

					if (effect != null)
					{
						Buf.Append(effect.Desc);

						if (Buf.Length > 40)
						{
							Buf.Length = 40;
						}

						if (Buf.Length == 40)
						{
							Buf[39] = '.';

							Buf[38] = '.';

							Buf[37] = '.';
						}
					}

					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("IntroStory"), null),
						gEngine.BuildValue(51, ' ', 8, Record.IntroStory, null, effect != null ? Buf.ToString() : gEngine.UnknownName));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("IntroStory"), null), Record.IntroStory);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListNumDirs()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("NumDirs"), null), Record.NumDirs);
			}
		}

		/// <summary></summary>
		public virtual void ListNumRooms()
		{
			if (FullDetail && !ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("NumRooms"), null), Record.NumRooms);
			}
		}

		/// <summary></summary>
		public virtual void ListNumArtifacts()
		{
			if (FullDetail && !ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("NumArtifacts"), null), Record.NumArtifacts);
			}
		}

		/// <summary></summary>
		public virtual void ListNumEffects()
		{
			if (FullDetail && !ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("NumEffects"), null), Record.NumEffects);
			}
		}

		/// <summary></summary>
		public virtual void ListNumMonsters()
		{
			if (FullDetail && !ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("NumMonsters"), null), Record.NumMonsters);
			}
		}

		/// <summary></summary>
		public virtual void ListNumHints()
		{
			if (FullDetail && !ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("NumHints"), null), Record.NumHints);
			}
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
		public virtual void InputName()
		{
			var fieldDesc = FieldDesc;

			var name = Record.Name;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", name);

				PrintFieldDesc("Name", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Name"), null));

				var rc = Globals.In.ReadField(Buf, Constants.ModNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Name = Buf.ToString();

				if (ValidateField("Name"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputDesc()
		{
			var fieldDesc = FieldDesc;

			var desc = Record.Desc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", desc);

				PrintFieldDesc("Desc", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Desc"), null));

				gOut.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, Constants.ModDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				Record.Desc = Buf.Trim().ToString();

				if (ValidateField("Desc"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputAuthor()
		{
			var fieldDesc = FieldDesc;

			var author = Record.Author;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", author);

				PrintFieldDesc("Author", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Author"), null));

				var rc = Globals.In.ReadField(Buf, Constants.ModAuthorLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Author = Buf.Trim().ToString();

				if (ValidateField("Author"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputVolLabel()
		{
			var fieldDesc = FieldDesc;

			var volLabel = Record.VolLabel;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", volLabel);

				PrintFieldDesc("VolLabel", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("VolLabel"), null));

				var rc = Globals.In.ReadField(Buf, Constants.ModVolLabelLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.VolLabel = Buf.Trim().ToString();

				if (ValidateField("VolLabel"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputSerialNum()
		{
			var fieldDesc = FieldDesc;

			var serialNum = Record.SerialNum;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", serialNum);

				PrintFieldDesc("SerialNum", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("SerialNum"), "000"));

				var rc = Globals.In.ReadField(Buf, Constants.ModSerialNumLen, null, '_', '\0', true, "000", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.SerialNum = Buf.Trim().ToString();

				if (ValidateField("SerialNum"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputLastMod()
		{
			if (!EditRec)
			{
				Record.LastMod = DateTime.Now;
			}

			gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("LastMod"), null), Record.LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputIntroStory()
		{
			var fieldDesc = FieldDesc;

			var introStory = Record.IntroStory;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", introStory);

				PrintFieldDesc("IntroStory", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("IntroStory"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.IntroStory = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("IntroStory"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputNumDirs()
		{
			var fieldDesc = FieldDesc;

			var numDirs = Record.NumDirs;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", numDirs);

				PrintFieldDesc("NumDirs", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("NumDirs"), "6"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "6", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.NumDirs = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("NumDirs"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputNumRooms()
		{
			if (!EditRec)
			{
				Record.NumRooms = Globals.Database.GetRoomsCount();
			}

			gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("NumRooms"), null), Record.NumRooms);

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputNumArtifacts()
		{
			if (!EditRec)
			{
				Record.NumArtifacts = Globals.Database.GetArtifactsCount();
			}

			gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("NumArtifacts"), null), Record.NumArtifacts);

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputNumEffects()
		{
			if (!EditRec)
			{
				Record.NumEffects = Globals.Database.GetEffectsCount();
			}

			gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("NumEffects"), null), Record.NumEffects);

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputNumMonsters()
		{
			if (!EditRec)
			{
				Record.NumMonsters = Globals.Database.GetMonstersCount();
			}

			gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("NumMonsters"), null), Record.NumMonsters);

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputNumHints()
		{
			if (!EditRec)
			{
				Record.NumHints = Globals.Database.GetHintsCount();
			}

			gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("NumHints"), null), Record.NumHints);

			gOut.Print("{0}", Globals.LineSep);
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class ModuleHelper

		public override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetModuleUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		public ModuleHelper()
		{
			FieldNameList = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Name",
				"Desc",
				"Author",
				"VolLabel",
				"SerialNum",
				"LastMod",
				"IntroStory",
				"NumDirs",
				"NumRooms",
				"NumArtifacts",
				"NumEffects",
				"NumMonsters",
				"NumHints",
			};
		}

		#endregion

		#endregion
	}
}
