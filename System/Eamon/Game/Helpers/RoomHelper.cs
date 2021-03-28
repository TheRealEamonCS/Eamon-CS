
// RoomHelper.cs

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
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class RoomHelper : Helper<IRoom>, IRoomHelper
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

			if (ErrorFieldName.Equals("DirsElement", StringComparison.OrdinalIgnoreCase))
			{
				var i = Index;

				gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '.', 0, GetPrintedName("DirsElement"), null), Record.GetDirs(i));
			}
		}

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameLightLvl()
		{
			return "Light Level";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameDirsElement()
		{
			var i = Index;

			var direction = gEngine.GetDirections((Direction)i);

			Debug.Assert(direction != null);

			return direction.Name;
		}

		#endregion

		#region GetName Methods

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameDirs(bool addToNameList)
		{
			var directionValues = EnumUtil.GetValues<Direction>();

			foreach (var dv in directionValues)
			{
				Index = (long)dv;

				GetName("DirsElement", addToNameList);
			}

			return "Dirs";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameDirsElement(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Dirs[{0}].Element", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		#endregion

		#region GetValue Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueDirsElement()
		{
			var i = Index;

			return Record.GetDirs(i);
		}

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

			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.RmNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateDesc()
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.RmDescLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateLightLvl()
		{
			return Enum.IsDefined(typeof(LightLevel), Record.LightLvl);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateType()
		{
			return Enum.IsDefined(typeof(RoomType), Record.Type);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateZone()
		{
			return Record.Zone > 0;
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
		public virtual bool ValidateInterdependenciesDirs()
		{
			var result = true;

			var directionValues = EnumUtil.GetValues<Direction>();

			foreach (var dv in directionValues)
			{
				Index = (long)dv;

				result = ValidateFieldInterdependencies("DirsElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesDirsElement()
		{
			var result = true;

			var i = Index;

			var dv = (Direction)i;

			if (gEngine.IsValidDirection(dv))
			{
				if (Record.IsDirectionRoom(dv))
				{
					var roomUid = Record.GetDirs(i);

					var room = gRDB[roomUid];

					if (room == null)
					{
						result = false;

						Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("DirsElement"), "Room", roomUid, "which doesn't exist");

						ErrorMessage = Buf.ToString();

						RecordType = typeof(IRoom);

						NewRecordUid = roomUid;

						goto Cleanup;
					}
				}
				else if (Record.IsDirectionDoor(dv))
				{
					var artUid = Record.GetDirectionDoorUid(dv);

					var artifact = gADB[artUid];

					if (artifact == null)
					{
						result = false;

						Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("DirsElement"), "Artifact", artUid, "which doesn't exist");

						ErrorMessage = Buf.ToString();

						RecordType = typeof(IArtifact);

						NewRecordUid = artUid;

						goto Cleanup;
					}
					else if (artifact.DoorGate == null)
					{
						result = false;

						Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("DirsElement"), "Artifact", artUid, "which should be a door/gate, but isn't");

						ErrorMessage = Buf.ToString();

						RecordType = typeof(IArtifact);

						EditRecord = artifact;

						goto Cleanup;
					}
					else if (!artifact.IsInRoom(Record) && !artifact.IsEmbeddedInRoom(Record))
					{
						result = false;

						Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("DirsElement"), "Artifact", artUid, "which should be located in this Room, but isn't");

						ErrorMessage = Buf.ToString();

						RecordType = typeof(IArtifact);

						EditRecord = artifact;

						goto Cleanup;
					}
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
			var fullDesc = "Enter the name of the Room." + Environment.NewLine + Environment.NewLine + "Room names should always be able to stand alone inside a pair of brackets: [Room name].";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescDesc()
		{
			var fullDesc = "Enter a detailed description of the Room.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescSeen()
		{
			var fullDesc = "Enter whether the player has seen the Room.";

			var briefDesc = "0=Not seen; 1=Seen";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescLightLvl()
		{
			var fullDesc = "Enter the level of light in the Room.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var lightLevelValues = EnumUtil.GetValues<LightLevel>();

			for (var j = 0; j < lightLevelValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)lightLevelValues[j], gEngine.GetLightLevelNames(lightLevelValues[j]));
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		public virtual void PrintDescType()
		{
			var fullDesc = "Enter the type of the Room.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var roomTypeValues = EnumUtil.GetValues<RoomType>();

			for (var j = 0; j < roomTypeValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)roomTypeValues[j], gEngine.EvalRoomType(roomTypeValues[j], "Indoors", "Outdoors"));
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		public virtual void PrintDescZone()
		{
			var fullDesc = "Enter the zone of the Room." + Environment.NewLine + Environment.NewLine + "This value allows the arbitrary grouping of Rooms.  Ignored by Eamon CS, utilizing it requires special (user-programmed) events.";

			var briefDesc = "(GT 0)=Valid value";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescDirsElement()
		{
			var i = Index;

			var fullDesc = "Enter a connection value for the direction.";

			var fullDesc01 = "Enter a connection value for each direction the player can move in.";

			var briefDesc = "-999=Return to Main Hall; (LT 0)=Special (user-programmed) event; 0=No connection; 1-1000=Room Uid; (1000 + N)=Door with Artifact Uid N";

			if ((EditRec && EditField) || i == 1)
			{
				if (FieldDesc == FieldDesc.Full)
				{
					Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}", Environment.NewLine, (EditRec && EditField) ? fullDesc : fullDesc01, briefDesc);
				}
				else if (FieldDesc == FieldDesc.Brief)
				{
					Buf01.AppendPrint("{0}", briefDesc);
				}
			}
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
		public virtual void ListSeen()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Seen"), null), Convert.ToInt64(Record.Seen));
			}
		}

		/// <summary></summary>
		public virtual void ListLightLvl()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("LightLvl"), null), (long)Record.LightLvl);
			}
		}

		/// <summary></summary>
		public virtual void ListType()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Type"), null),
						gEngine.BuildValue(51, ' ', 8, (long)Record.Type, null, Record.EvalRoomType("Indoors", "Outdoors")));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Type"), null), Record.Type);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListZone()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Zone"), null), Record.Zone);
			}
		}

		/// <summary></summary>
		public virtual void ListDirs()
		{
			var directionValues = EnumUtil.GetValues<Direction>();

			foreach (var dv in directionValues)
			{
				Index = (long)dv;

				ListField("DirsElement");
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		public virtual void ListDirsElement()
		{
			var i = Index;

			var dv = (Direction)i;

			if (FullDetail && gEngine.IsValidDirection(dv))
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					string lookupMsg = null;

					if (Record.IsDirectionRoom(dv))
					{
						var room = gRDB[Record.GetDirs(i)];

						lookupMsg = room != null ? gEngine.Capitalize(room.Name) : gEngine.UnknownName;
					}
					else if (Record.IsDirectionExit(dv))
					{
						lookupMsg = "Exit";
					}
					else if (Record.IsDirectionDoor(dv))
					{
						var artifact = Record.GetDirectionDoor(dv);

						lookupMsg = artifact != null ? gEngine.Capitalize(artifact.Name) : gEngine.UnknownName;
					}

					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("DirsElement"), null),
						gEngine.BuildValue(51, ' ', 8, Record.GetDirs(i), null, lookupMsg));
				}
				else
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("DirsElement"), null), Record.GetDirs(i));
				}
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

				var rc = Globals.In.ReadField(Buf, Constants.RmNameLen, null, '_', '\0', false, null, null, null, null);

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

				var rc = Globals.In.ReadField(Buf, Constants.RmDescLen, null, '_', '\0', false, null, null, null, null);

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
		public virtual void InputSeen()
		{
			var fieldDesc = FieldDesc;

			var seen = Record.Seen;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(seen));

				PrintFieldDesc("Seen", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Seen"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Seen = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("Seen"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputLightLvl()
		{
			var fieldDesc = FieldDesc;

			var lightLvl = Record.LightLvl;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)lightLvl);

				PrintFieldDesc("LightLvl", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("LightLvl"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.LightLvl = (LightLevel)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("LightLvl"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputType()
		{
			var fieldDesc = FieldDesc;

			var type = Record.Type;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)type);

				PrintFieldDesc("Type", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Type"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Type = (RoomType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Type"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputZone()
		{
			var fieldDesc = FieldDesc;

			var zone = Record.Zone;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", zone);

				PrintFieldDesc("Zone", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Zone"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Zone = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Zone"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputDirs()
		{
			var directionValues = EnumUtil.GetValues<Direction>();

			foreach (var dv in directionValues)
			{
				Index = (long)dv;

				InputField("DirsElement");
			}
		}

		/// <summary></summary>
		public virtual void InputDirsElement()
		{
			var i = Index;

			var dv = (Direction)i;

			if (gEngine.IsValidDirection(dv))
			{
				var fieldDesc = FieldDesc;

				var value = Record.GetDirs(i);

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", value);

					PrintFieldDesc("DirsElement", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("DirsElement"), "0"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.SetDirs(i, Convert.ToInt64(Buf.Trim().ToString()));
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("DirsElement"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				gOut.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.SetDirs(i, 0);
			}
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class RoomHelper

		public override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetRoomUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		public RoomHelper()
		{
			FieldNameList = new List<string>()
			{
						"Uid",
						"IsUidRecycled",
						"Name",
						"Desc",
						"Seen",
						"LightLvl",
						"Type",
						"Zone",
						"Dirs",
			};
		}

		#endregion

		#endregion
	}
}
