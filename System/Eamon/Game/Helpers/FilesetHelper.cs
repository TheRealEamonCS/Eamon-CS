
// FilesetHelper.cs

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
	public class FilesetHelper : Helper<IFileset>, IFilesetHelper
	{
		#region Public Properties

		/// <summary></summary>
		public virtual Regex WorkDirRegex { get; set; }

		#endregion

		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWorkDir()
		{
			return "Working Directory";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNamePluginFileName()
		{
			return "Plugin Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameConfigFileName()
		{
			return "Config Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameFilesetFileName()
		{
			return "Fileset Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCharacterFileName()
		{
			return "Character Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameModuleFileName()
		{
			return "Module Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameRoomFileName()
		{
			return "Room Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameArtifactFileName()
		{
			return "Artifact Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameEffectFileName()
		{
			return "Effect Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameMonsterFileName()
		{
			return "Monster Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameHintFileName()
		{
			return "Hint Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameGameStateFileName()
		{
			return "Game State Filename";
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

			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.FsNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWorkDir()
		{
			return WorkDirRegex.IsMatch(Record.WorkDir);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidatePluginFileName()
		{
			return string.IsNullOrWhiteSpace(Record.PluginFileName) == false && Record.PluginFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.PluginFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateConfigFileName()
		{
			return string.IsNullOrWhiteSpace(Record.ConfigFileName) == false && Record.ConfigFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.ConfigFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateFilesetFileName()
		{
			return string.IsNullOrWhiteSpace(Record.FilesetFileName) == false && Record.FilesetFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.FilesetFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCharacterFileName()
		{
			return string.IsNullOrWhiteSpace(Record.CharacterFileName) == false && Record.CharacterFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.CharacterFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateModuleFileName()
		{
			return string.IsNullOrWhiteSpace(Record.ModuleFileName) == false && Record.ModuleFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.ModuleFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateRoomFileName()
		{
			return string.IsNullOrWhiteSpace(Record.RoomFileName) == false && Record.RoomFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.RoomFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArtifactFileName()
		{
			return string.IsNullOrWhiteSpace(Record.ArtifactFileName) == false && Record.ArtifactFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.ArtifactFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateEffectFileName()
		{
			return string.IsNullOrWhiteSpace(Record.EffectFileName) == false && Record.EffectFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.EffectFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateMonsterFileName()
		{
			return string.IsNullOrWhiteSpace(Record.MonsterFileName) == false && Record.MonsterFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.MonsterFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateHintFileName()
		{
			return string.IsNullOrWhiteSpace(Record.HintFileName) == false && Record.HintFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.HintFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateGameStateFileName()
		{
			return string.IsNullOrWhiteSpace(Record.GameStateFileName) == false && Record.GameStateFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.GameStateFileName.Length <= Constants.FsFileNameLen;
		}

		#endregion

		#region ValidateInterdependencies Methods

		// do nothing

		#endregion

		#region PrintDesc Methods

		/// <summary></summary>
		public virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the Fileset." + Environment.NewLine + Environment.NewLine + "If the Fileset represents an adventure, use the adventure name; if it represents an author catalog use the catalog name.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescWorkDir()
		{
			var fullDesc = "Enter the working directory of the Fileset." + Environment.NewLine + Environment.NewLine + "This is where the files are found.  It can be an absolute or relative path, and should not end with a path separator.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescPluginFileName()
		{
			var fullDesc = "Enter the plugin filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescConfigFileName()
		{
			var fullDesc = "Enter the Config filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescFilesetFileName()
		{
			var fullDesc = "Enter the Fileset filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescCharacterFileName()
		{
			var fullDesc = "Enter the Character filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescModuleFileName()
		{
			var fullDesc = "Enter the Module filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescRoomFileName()
		{
			var fullDesc = "Enter the Room filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescArtifactFileName()
		{
			var fullDesc = "Enter the Artifact filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescEffectFileName()
		{
			var fullDesc = "Enter the Effect filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescMonsterFileName()
		{
			var fullDesc = "Enter the Monster filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescHintFileName()
		{
			var fullDesc = "Enter the Hint filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescGameStateFileName()
		{
			var fullDesc = "Enter the GameState filename of the Fileset.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
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
				gOut.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, Record.Name);
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
		public virtual void ListWorkDir()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WorkDir"), null), Record.WorkDir);
			}
		}

		/// <summary></summary>
		public virtual void ListPluginFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("PluginFileName"), null),
					Record.PluginFileName);
			}
		}

		/// <summary></summary>
		public virtual void ListConfigFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ConfigFileName"), null),
					Record.ConfigFileName);
			}
		}

		/// <summary></summary>
		public virtual void ListFilesetFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("FilesetFileName"), null),
					Record.FilesetFileName);
			}
		}

		/// <summary></summary>
		public virtual void ListCharacterFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CharacterFileName"), null),
					Record.CharacterFileName);
			}
		}

		/// <summary></summary>
		public virtual void ListModuleFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ModuleFileName"), null),
					Record.ModuleFileName);
			}
		}

		/// <summary></summary>
		public virtual void ListRoomFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("RoomFileName"), null),
					Record.RoomFileName);
			}
		}

		/// <summary></summary>
		public virtual void ListArtifactFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ArtifactFileName"), null),
					Record.ArtifactFileName);
			}
		}

		/// <summary></summary>
		public virtual void ListEffectFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("EffectFileName"), null),
					Record.EffectFileName);
			}
		}

		/// <summary></summary>
		public virtual void ListMonsterFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("MonsterFileName"), null),
					Record.MonsterFileName);
			}
		}

		/// <summary></summary>
		public virtual void ListHintFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("HintFileName"), null),
					Record.HintFileName);
			}
		}

		/// <summary></summary>
		public virtual void ListGameStateFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("GameStateFileName"), null),
					Record.GameStateFileName);
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

				var rc = Globals.In.ReadField(Buf, Constants.FsNameLen, null, '_', '\0', false, null, null, null, null);

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
		public virtual void InputWorkDir()
		{
			var fieldDesc = FieldDesc;

			var workDir = Record.WorkDir;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", workDir);

				PrintFieldDesc("WorkDir", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WorkDir"), null));

				gOut.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, Constants.MaxPathLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				Record.WorkDir = Buf.Trim().ToString();

				if (ValidateField("WorkDir"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputPluginFileName()
		{
			var fieldDesc = FieldDesc;

			var pluginFileName = Record.PluginFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", pluginFileName);

				PrintFieldDesc("PluginFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("PluginFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.PluginFileName = Buf.Trim().ToString();

				if (ValidateField("PluginFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputConfigFileName()
		{
			var fieldDesc = FieldDesc;

			var configFileName = Record.ConfigFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", configFileName);

				PrintFieldDesc("ConfigFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("ConfigFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ConfigFileName = Buf.Trim().ToString();

				if (ValidateField("ConfigFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputFilesetFileName()
		{
			var fieldDesc = FieldDesc;

			var filesetFileName = Record.FilesetFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", filesetFileName);

				PrintFieldDesc("FilesetFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("FilesetFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.FilesetFileName = Buf.Trim().ToString();

				if (ValidateField("FilesetFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputCharacterFileName()
		{
			var fieldDesc = FieldDesc;

			var characterFileName = Record.CharacterFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", characterFileName);

				PrintFieldDesc("CharacterFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("CharacterFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.CharacterFileName = Buf.Trim().ToString();

				if (ValidateField("CharacterFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputModuleFileName()
		{
			var fieldDesc = FieldDesc;

			var moduleFileName = Record.ModuleFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", moduleFileName);

				PrintFieldDesc("ModuleFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("ModuleFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ModuleFileName = Buf.Trim().ToString();

				if (ValidateField("ModuleFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputRoomFileName()
		{
			var fieldDesc = FieldDesc;

			var roomFileName = Record.RoomFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", roomFileName);

				PrintFieldDesc("RoomFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("RoomFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.RoomFileName = Buf.Trim().ToString();

				if (ValidateField("RoomFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputArtifactFileName()
		{
			var fieldDesc = FieldDesc;

			var artifactFileName = Record.ArtifactFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", artifactFileName);

				PrintFieldDesc("ArtifactFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("ArtifactFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ArtifactFileName = Buf.Trim().ToString();

				if (ValidateField("ArtifactFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputEffectFileName()
		{
			var fieldDesc = FieldDesc;

			var effectFileName = Record.EffectFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", effectFileName);

				PrintFieldDesc("EffectFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("EffectFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.EffectFileName = Buf.Trim().ToString();

				if (ValidateField("EffectFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputMonsterFileName()
		{
			var fieldDesc = FieldDesc;

			var monsterFileName = Record.MonsterFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", monsterFileName);

				PrintFieldDesc("MonsterFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("MonsterFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.MonsterFileName = Buf.Trim().ToString();

				if (ValidateField("MonsterFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputHintFileName()
		{
			var fieldDesc = FieldDesc;

			var hintFileName = Record.HintFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", hintFileName);

				PrintFieldDesc("HintFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("HintFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.HintFileName = Buf.Trim().ToString();

				if (ValidateField("HintFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputGameStateFileName()
		{
			var fieldDesc = FieldDesc;

			var gameStateFileName = Record.GameStateFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", gameStateFileName);

				PrintFieldDesc("GameStateFileName", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("GameStateFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.GameStateFileName = Buf.Trim().ToString();

				if (ValidateField("GameStateFileName"))
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

		#region Class FilesetHelper

		public override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetFilesetUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		public FilesetHelper()
		{
			FieldNameList = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Name",
				"WorkDir",
				"PluginFileName",
				"ConfigFileName",
				"FilesetFileName",
				"CharacterFileName",
				"ModuleFileName",
				"RoomFileName",
				"ArtifactFileName",
				"EffectFileName",
				"MonsterFileName",
				"HintFileName",
				"GameStateFileName",
			};

			WorkDirRegex = new Regex(Constants.ValidWorkDirRegexPattern);
		}

		#endregion

		#endregion
	}
}
