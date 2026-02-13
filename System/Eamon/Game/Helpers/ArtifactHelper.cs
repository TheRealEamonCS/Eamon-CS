
// ArtifactHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class ArtifactHelper : Helper<IArtifact>, IArtifactHelper
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

			if (!ErrorFieldName.Equals("Desc", StringComparison.OrdinalIgnoreCase))
			{
				gOut.Print("{0}{1}",
					gEngine.BuildPrompt(27, '.', 0, GetPrintedName(ErrorFieldName), null),
					Convert.ToInt64(GetValue(ErrorFieldName)));
			}
		}

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameStateDesc()
		{
			return "State Description";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameIsCharOwned()
		{
			return "Is Char Owned";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameIsPlural()
		{
			return "Is Plural";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameIsListed()
		{
			return "Is Listed";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNamePluralType()
		{
			return "Plural Type";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameUseExtendedAttributes()
		{
			return "Use Ext Attributes";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameExtendedAttributes()
		{
			return "Ext Attributes";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesType()
		{
			var i = Index;

			return string.Format("Cat #{0} Type", i + 1);
		}

		/// <summary></summary>
		/// <param name="fieldNumber"></param>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField(long fieldNumber)
		{
			var i = Index;

			var artType = gEngine.GetArtifactType(Record.GetCategory(i).Type);

			var property = artType != null ? artType.GetType().GetProperty(string.Format("Field{0}Name", fieldNumber)) : null;

			var fieldName = property != null ? (string)property.GetValue(artType) : string.Format("Field{0}", fieldNumber);

			return string.Format("Cat #{0} {1}", i + 1, fieldName);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField1()
		{
			return GetPrintedNameCategoriesField(1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField2()
		{
			return GetPrintedNameCategoriesField(2);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField3()
		{
			return GetPrintedNameCategoriesField(3);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField4()
		{
			return GetPrintedNameCategoriesField(4);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField5()
		{
			return GetPrintedNameCategoriesField(5);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField6()
		{
			return GetPrintedNameCategoriesField(6);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField7()
		{
			return GetPrintedNameCategoriesField(7);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField8()
		{
			return GetPrintedNameCategoriesField(8);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField9()
		{
			return GetPrintedNameCategoriesField(9);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField10()
		{
			return GetPrintedNameCategoriesField(10);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField11()
		{
			return GetPrintedNameCategoriesField(11);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField12()
		{
			return GetPrintedNameCategoriesField(12);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField13()
		{
			return GetPrintedNameCategoriesField(13);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField14()
		{
			return GetPrintedNameCategoriesField(14);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField15()
		{
			return GetPrintedNameCategoriesField(15);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField16()
		{
			return GetPrintedNameCategoriesField(16);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField17()
		{
			return GetPrintedNameCategoriesField(17);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField18()
		{
			return GetPrintedNameCategoriesField(18);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField19()
		{
			return GetPrintedNameCategoriesField(19);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField20()
		{
			return GetPrintedNameCategoriesField(20);
		}

		#endregion

		#region GetName Methods

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategories(bool addToNameList)
		{
			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				GetName("CategoriesType", addToNameList);
				GetName("CategoriesField1", addToNameList);
				GetName("CategoriesField2", addToNameList);
				GetName("CategoriesField3", addToNameList);
				GetName("CategoriesField4", addToNameList);
				GetName("CategoriesField5", addToNameList);
				GetName("CategoriesField6", addToNameList);
				GetName("CategoriesField7", addToNameList);
				GetName("CategoriesField8", addToNameList);
				GetName("CategoriesField9", addToNameList);
				GetName("CategoriesField10", addToNameList);
				GetName("CategoriesField11", addToNameList);
				GetName("CategoriesField12", addToNameList);
				GetName("CategoriesField13", addToNameList);
				GetName("CategoriesField14", addToNameList);
				GetName("CategoriesField15", addToNameList);
				GetName("CategoriesField16", addToNameList);
				GetName("CategoriesField17", addToNameList);
				GetName("CategoriesField18", addToNameList);
				GetName("CategoriesField19", addToNameList);
				GetName("CategoriesField20", addToNameList);
			}

			return "Categories";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesLength(bool addToNameList)
		{
			var result = "Categories.Length";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesType(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Type", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="fieldNumber"></param>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField(long fieldNumber, bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field{1}", i, fieldNumber);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField1(bool addToNameList)
		{
			return GetNameCategoriesField(1, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField2(bool addToNameList)
		{
			return GetNameCategoriesField(2, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField3(bool addToNameList)
		{
			return GetNameCategoriesField(3, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField4(bool addToNameList)
		{
			return GetNameCategoriesField(4, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField5(bool addToNameList)
		{
			return GetNameCategoriesField(5, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField6(bool addToNameList)
		{
			return GetNameCategoriesField(6, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField7(bool addToNameList)
		{
			return GetNameCategoriesField(7, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField8(bool addToNameList)
		{
			return GetNameCategoriesField(8, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField9(bool addToNameList)
		{
			return GetNameCategoriesField(9, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField10(bool addToNameList)
		{
			return GetNameCategoriesField(10, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField11(bool addToNameList)
		{
			return GetNameCategoriesField(11, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField12(bool addToNameList)
		{
			return GetNameCategoriesField(12, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField13(bool addToNameList)
		{
			return GetNameCategoriesField(13, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField14(bool addToNameList)
		{
			return GetNameCategoriesField(14, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField15(bool addToNameList)
		{
			return GetNameCategoriesField(15, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField16(bool addToNameList)
		{
			return GetNameCategoriesField(16, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField17(bool addToNameList)
		{
			return GetNameCategoriesField(17, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField18(bool addToNameList)
		{
			return GetNameCategoriesField(18, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField19(bool addToNameList)
		{
			return GetNameCategoriesField(19, addToNameList);
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField20(bool addToNameList)
		{
			return GetNameCategoriesField(20, addToNameList);
		}

		#endregion

		#region GetValue Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueLocation()
		{
			return Record.Location;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesLength()
		{
			return Record.Categories.Length;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesType()
		{
			var i = Index;

			return Record.GetCategory(i).Type;
		}

		/// <summary></summary>
		/// <param name="fieldNumber"></param>
		/// <returns></returns>
		public virtual object GetValueCategoriesField(long fieldNumber)
		{
			var i = Index;

			var category = Record.GetCategory(i);

			var fieldProperty = category != null ? category.GetType().GetProperty(string.Format("Field{0}", fieldNumber)) : null;

			return fieldProperty != null ? fieldProperty.GetValue(category) : 0L;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField1()
		{
			return GetValueCategoriesField(1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField2()
		{
			return GetValueCategoriesField(2);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField3()
		{
			return GetValueCategoriesField(3);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField4()
		{
			return GetValueCategoriesField(4);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField5()
		{
			return GetValueCategoriesField(5);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField6()
		{
			return GetValueCategoriesField(6);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField7()
		{
			return GetValueCategoriesField(7);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField8()
		{
			return GetValueCategoriesField(8);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField9()
		{
			return GetValueCategoriesField(9);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField10()
		{
			return GetValueCategoriesField(10);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField11()
		{
			return GetValueCategoriesField(11);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField12()
		{
			return GetValueCategoriesField(12);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField13()
		{
			return GetValueCategoriesField(13);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField14()
		{
			return GetValueCategoriesField(14);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField15()
		{
			return GetValueCategoriesField(15);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField16()
		{
			return GetValueCategoriesField(16);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField17()
		{
			return GetValueCategoriesField(17);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField18()
		{
			return GetValueCategoriesField(18);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField19()
		{
			return GetValueCategoriesField(19);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField20()
		{
			return GetValueCategoriesField(20);
		}

		#endregion

		#region Validate Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateUid()
		{
			return Record.Uid > 0 && Record.Uid <= gEngine.NumRecords;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateName()
		{
			if (Record.Name != null)
			{
				Record.Name = Regex.Replace(Record.Name, @"\s+", " ").Trim();
			}

			var result = !string.IsNullOrWhiteSpace(Record.Name);

			if (result && Record.Name.Length > gEngine.ArtNameLen)
			{
				for (var i = gEngine.ArtNameLen; i < Record.Name.Length; i++)
				{
					if (Record.Name[i] != '#')
					{
						result = false;

						break;
					}
				}
			}

			if (result)
			{
				var recordName = string.Format(" {0} ", Record.Name.ToLower());

				result = !Regex.IsMatch(recordName, gEngine.CommandSepRegexPattern) && !Regex.IsMatch(recordName, gEngine.PronounRegexPattern);

				// TODO: might need to disallow verb name matches as well
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateStateDesc()
		{
			return Record.StateDesc != null && Record.StateDesc.Length <= gEngine.ArtStateDescLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateDesc()
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= gEngine.ArtDescLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidatePluralType()
		{
			return gEngine.IsValidPluralType(Record.PluralType);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArticleType()
		{
			return Enum.IsDefined(typeof(ArticleType), Record.ArticleType);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateValue()
		{
			return Record.Value >= gEngine.MinGoldValue && Record.Value <= gEngine.MaxGoldValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateLocation()
		{
			var result = true;

			if (gDatabase.ArtifactTableType == ArtifactTableType.CharArt)
			{
				Debug.Assert(RecordTable != null);

				if (Record.IsCarriedByCharacter())
				{
					if (Record.GetCategory(0).Type != ArtifactType.None)
					{
						result = Record.GetCategory(0).Type == ArtifactType.Weapon || Record.GetCategory(0).Type == ArtifactType.MagicWeapon;
					}

					if (result)
					{
						var artifactList = RecordTable.Records.Where(a => a.Uid != Record.Uid && a.GeneralWeapon != null && a.Location == Record.Location).ToList();

						result = artifactList.Count < gEngine.NumCharacterWeapons;
					}
				}
				else if (Record.IsWornByCharacter())
				{
					if (Record.GetCategory(0).Type != ArtifactType.None)
					{
						result = Record.GetCategory(0).Type == ArtifactType.Wearable;

						if (result)
						{
							var wornArtifact = RecordTable.Records.FirstOrDefault(a => a.Uid != Record.Uid && a.Wearable != null && ((a.Wearable.Field1 >= 2 && Record.Wearable.Field1 >= 2) || (a.Wearable.Field1 == 1 && Record.Wearable.Field1 == 1)) && a.Location == Record.Location);

							result = wornArtifact == null;
						}
					}
					else
					{
						var artifactList = RecordTable.Records.Where(a => a.Uid != Record.Uid && a.Wearable != null && a.Location == Record.Location).ToList();

						result = artifactList.Count < 2;
					}
				}
				else
				{
					result = Record.Location == 0;
				}
			}
			else
			{
				result = Record.Location > -2001;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateExtendedAttributes()
		{
			return (!Record.UseExtendedAttributes && Record.ExtendedAttributes == 0) || (Record.UseExtendedAttributes && Record.ExtendedAttributes < 512);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategories()
		{
			var result = ValidateField("CategoriesLength");

			if (result)
			{
				for (Index = 0; Index < Record.Categories.Length; Index++)
				{
					result = ValidateField("CategoriesType") &&
									ValidateField("CategoriesField1") &&
									ValidateField("CategoriesField2") &&
									ValidateField("CategoriesField3") &&
									ValidateField("CategoriesField4") &&
									ValidateField("CategoriesField5") &&
									ValidateField("CategoriesField6") &&
									ValidateField("CategoriesField7") &&
									ValidateField("CategoriesField8") &&
									ValidateField("CategoriesField9") &&
									ValidateField("CategoriesField10") &&
									ValidateField("CategoriesField11") &&
									ValidateField("CategoriesField12") &&
									ValidateField("CategoriesField13") &&
									ValidateField("CategoriesField14") &&
									ValidateField("CategoriesField15") &&
									ValidateField("CategoriesField16") &&
									ValidateField("CategoriesField17") &&
									ValidateField("CategoriesField18") &&
									ValidateField("CategoriesField19") &&
									ValidateField("CategoriesField20");

					if (result == false)
					{
						break;
					}
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesLength()
		{
			return gDatabase.ArtifactTableType == ArtifactTableType.CharArt ? Record.Categories.Length == 1 : Record.Categories.Length >= 1 && Record.Categories.Length <= gEngine.NumArtifactCategories;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesType()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategory(h).Type == ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				if (gDatabase.ArtifactTableType == ArtifactTableType.CharArt)
				{
					if (Record.IsCarriedByCharacter())
					{
						result = Record.GetCategory(i).Type == ArtifactType.Weapon || Record.GetCategory(i).Type == ArtifactType.MagicWeapon;
					}
					else if (Record.IsWornByCharacter())
					{
						result = Record.GetCategory(i).Type == ArtifactType.Wearable;
					}
					else
					{
						result = Record.GetCategory(i).Type == ArtifactType.Weapon || Record.GetCategory(i).Type == ArtifactType.MagicWeapon || Record.GetCategory(i).Type == ArtifactType.Wearable;
					}
				}
				else
				{
					result = gEngine.IsValidArtifactType(Record.GetCategory(i).Type);
				}

				if (result)
				{
					for (var h = 0; h < Record.Categories.Length; h++)
					{
						if (h != i && Record.GetCategory(h).Type != ArtifactType.None)
						{
							if ((Record.GetCategory(h).Type == Record.GetCategory(i).Type) ||
									(Record.GetCategory(h).Type == ArtifactType.Gold && Record.GetCategory(i).Type == ArtifactType.Treasure) ||
									(Record.GetCategory(h).Type == ArtifactType.Treasure && Record.GetCategory(i).Type == ArtifactType.Gold) ||
									(Record.GetCategory(h).Type == ArtifactType.Weapon && Record.GetCategory(i).Type == ArtifactType.MagicWeapon) ||
									(Record.GetCategory(h).Type == ArtifactType.MagicWeapon && Record.GetCategory(i).Type == ArtifactType.Weapon) ||
									((Record.GetCategory(h).Type == ArtifactType.InContainer || Record.GetCategory(h).Type == ArtifactType.OnContainer || Record.GetCategory(h).Type == ArtifactType.UnderContainer || Record.GetCategory(h).Type == ArtifactType.BehindContainer) && Record.GetCategory(i).Type == ArtifactType.DoorGate) ||
									(Record.GetCategory(h).Type == ArtifactType.DoorGate && (Record.GetCategory(i).Type == ArtifactType.InContainer || Record.GetCategory(i).Type == ArtifactType.OnContainer || Record.GetCategory(i).Type == ArtifactType.UnderContainer || Record.GetCategory(i).Type == ArtifactType.BehindContainer)) ||
									(Record.GetCategory(h).Type == ArtifactType.BoundMonster && Record.GetCategory(i).Type == ArtifactType.DisguisedMonster) ||
									(Record.GetCategory(h).Type == ArtifactType.DisguisedMonster && Record.GetCategory(i).Type == ArtifactType.BoundMonster))
							{
								result = false;

								break;
							}
						}
					}
				}
			}
			else
			{
				result = Record.GetCategory(i).Type == ArtifactType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="fieldNumber"></param>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField(long fieldNumber)
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategory(h).Type == ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			var category = Record.GetCategory(i);

			var fieldProperty = category != null ? category.GetType().GetProperty(string.Format("Field{0}", fieldNumber)) : null;

			var fieldValue = fieldProperty != null ? Convert.ToInt64(fieldProperty.GetValue(category)) : 0L;

			if (activeCategory)
			{
				switch (category.Type)
				{
					case ArtifactType.Weapon:
					case ArtifactType.MagicWeapon:

						if (fieldNumber == 5 && fieldValue == 0)    // Auto-upgrade old weapons
						{
							fieldValue = category.Field2 == (long)Weapon.Bow ? 2 : 1;

							if (fieldProperty != null)
							{
								fieldProperty.SetValue(category, fieldValue);
							}
						}

						if (fieldNumber >= 6 && fieldNumber <= 13)  // Auto-upgrade old weapons
						{
							// TODO: implement
						}

						if (fieldNumber == 1)
						{
							result = fieldValue >= gEngine.MinWeaponComplexity && fieldValue <= gEngine.MaxWeaponComplexity;
						}
						else if (fieldNumber == 2)
						{
							result = Enum.IsDefined(typeof(Weapon), fieldValue);
						}
						else if (fieldNumber == 3 || fieldNumber == 4)
						{
							result = fieldValue >= 1 && fieldValue <= 25;
						}
						else if (fieldNumber == 5)
						{
							result = fieldValue >= 1 && fieldValue <= 2;
						}
						else if (fieldNumber >= 6 && fieldNumber <= 13)
						{
							// TODO: implement
						}

						break;

					case ArtifactType.InContainer:

						if (fieldNumber == 1)
						{
							result = fieldValue >= -2 && fieldValue <= gEngine.NumRecords;         // -2=Broken
						}
						else if (fieldNumber == 2)
						{
							result = (fieldValue >= 0 && fieldValue <= 1) || gEngine.IsArtifactFieldStrength(fieldValue);
						}
						else if (fieldNumber == 3 || fieldNumber == 4)
						{
							result = fieldValue >= 0;
						}
						else if (fieldNumber == 5)
						{
							result = Enum.IsDefined(typeof(ContainerDisplayCode), fieldValue);
						}

						break;

					case ArtifactType.OnContainer:
					case ArtifactType.UnderContainer:
					case ArtifactType.BehindContainer:

						if (fieldNumber == 3 || fieldNumber == 4)
						{
							result = fieldValue >= 0;
						}
						else if (fieldNumber == 5)
						{
							result = Enum.IsDefined(typeof(ContainerDisplayCode), fieldValue);
						}

						break;

					case ArtifactType.LightSource:

						if (fieldNumber == 1)
						{
							result = fieldValue >= -1;
						}

						break;

					case ArtifactType.Drinkable:
					case ArtifactType.Edible:

						if (fieldNumber == 2)
						{
							result = fieldValue >= 0;
						}
						else if (fieldNumber == 3)
						{
							result = fieldValue >= 0 && fieldValue <= 1;
						}

						break;

					case ArtifactType.Readable:

						if (fieldNumber == 1)
						{
							result = fieldValue > 0 && (fieldValue + Math.Max(1, category.Field2) - 1) <= gEngine.NumRecords;
						}
						else if (fieldNumber == 2)
						{
							result = fieldValue > 0 && (Math.Max(1, category.Field1) + fieldValue - 1) <= gEngine.NumRecords;
						}
						else if (fieldNumber == 3)
						{
							result = fieldValue >= 0 && fieldValue <= 1;
						}

						break;

					case ArtifactType.DoorGate:

						if (fieldNumber == 2)
						{
							result = fieldValue >= -2 && fieldValue <= gEngine.NumRecords;         // -2=Broken
						}
						else if (fieldNumber == 3)
						{
							result = (fieldValue >= 0 && fieldValue <= 1) || gEngine.IsArtifactFieldStrength(fieldValue);
						}
						else if (fieldNumber == 4)
						{
							result = fieldValue >= 0 && fieldValue <= 1;
						}

						break;

					case ArtifactType.BoundMonster:

						if (fieldNumber == 1)
						{
							result = fieldValue > 0 && fieldValue <= gEngine.NumRecords;
						}
						else if (fieldNumber == 2)
						{
							result = fieldValue >= -1 && fieldValue <= gEngine.NumRecords;
						}
						else if (fieldNumber == 3)
						{
							result = fieldValue >= 0 && fieldValue <= gEngine.NumRecords;
						}

						break;

					case ArtifactType.DisguisedMonster:

						if (fieldNumber == 1)
						{
							result = fieldValue > 0 && fieldValue <= gEngine.NumRecords;
						}
						else if (fieldNumber == 2)
						{
							result = fieldValue > 0 && (fieldValue + Math.Max(1, category.Field3) - 1) <= gEngine.NumRecords;
						}
						else if (fieldNumber == 3)
						{
							result = fieldValue > 0 && (Math.Max(1, category.Field2) + fieldValue - 1) <= gEngine.NumRecords;
						}

						break;

					case ArtifactType.Wearable:

						if (fieldNumber == 1)
						{
							result = gEngine.IsValidArtifactArmor(fieldValue);

							if (result && gDatabase.ArtifactTableType == ArtifactTableType.CharArt)
							{
								Debug.Assert(RecordTable != null);

								result = fieldValue >= 1;

								if (result && Record.IsWornByCharacter())
								{
									var wornArtifact = RecordTable.Records.FirstOrDefault(a => a.Uid != Record.Uid && a.Wearable != null && ((a.Wearable.Field1 >= 2 && fieldValue >= 2) || (a.Wearable.Field1 == 1 && fieldValue == 1)) && a.Location == Record.Location);

									result = wornArtifact == null;
								}
							}
						}
						else if (fieldNumber == 2)
						{
							result = Enum.IsDefined(typeof(Clothing), fieldValue);

							if (result && gDatabase.ArtifactTableType == ArtifactTableType.CharArt)
							{
								result = fieldValue == (long)Clothing.ArmorShields;
							}
						}

						break;

					case ArtifactType.DeadBody:

						if (fieldNumber == 1)
						{
							result = fieldValue >= 0 && fieldValue <= 1;
						}

						break;

					default:

						break;
				}
			}
			else
			{
				result = fieldValue == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField1()
		{
			return ValidateCategoriesField(1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField2()
		{
			return ValidateCategoriesField(2);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField3()
		{
			return ValidateCategoriesField(3);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField4()
		{
			return ValidateCategoriesField(4);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField5()
		{
			return ValidateCategoriesField(5);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField6()
		{
			return ValidateCategoriesField(6);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField7()
		{
			return ValidateCategoriesField(7);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField8()
		{
			return ValidateCategoriesField(8);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField9()
		{
			return ValidateCategoriesField(9);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField10()
		{
			return ValidateCategoriesField(10);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField11()
		{
			return ValidateCategoriesField(11);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField12()
		{
			return ValidateCategoriesField(12);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField13()
		{
			return ValidateCategoriesField(13);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField14()
		{
			return ValidateCategoriesField(14);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField15()
		{
			return ValidateCategoriesField(15);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField16()
		{
			return ValidateCategoriesField(16);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField17()
		{
			return ValidateCategoriesField(17);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField18()
		{
			return ValidateCategoriesField(18);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField19()
		{
			return ValidateCategoriesField(19);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField20()
		{
			return ValidateCategoriesField(20);
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

				Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Desc"), "Effect", invalidUid, "which doesn't exist");

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
		public virtual bool ValidateInterdependenciesPluralType()
		{
			var result = true;

			var effectUid = gEngine.GetPluralTypeEffectUid(Record.PluralType);

			if (effectUid > 0 && gEDB[effectUid] == null)
			{
				result = false;

				Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("PluralType"), "Effect", effectUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IEffect);

				NewRecordUid = effectUid;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesLocation()
		{
			var result = true;

			var charUid = Record.GetWornByCharacterUid();

			if (charUid > 0 && gCHRDB[charUid] == null)
			{
				result = false;

				Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Location"), "Character", charUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(ICharacter);

				NewRecordUid = charUid;

				goto Cleanup;
			}

			charUid = Record.GetCarriedByCharacterUid();

			if (charUid > 0 && gCHRDB[charUid] == null)
			{
				result = false;

				Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Location"), "Character", charUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(ICharacter);

				NewRecordUid = charUid;

				goto Cleanup;
			}

			var monUid = Record.GetWornByMonsterUid();

			if (monUid > 0 && gMDB[monUid] == null)
			{
				result = false;

				Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Location"), "Monster", monUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IMonster);

				NewRecordUid = monUid;

				goto Cleanup;
			}

			monUid = Record.GetCarriedByMonsterUid();

			if (monUid > 0 && gMDB[monUid] == null)
			{
				result = false;

				Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Location"), "Monster", monUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IMonster);

				NewRecordUid = monUid;

				goto Cleanup;
			}

			var roomUid = Record.GetInRoomUid();

			if (roomUid > 0 && gRDB[roomUid] == null)
			{
				result = false;

				Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Location"), "Room", roomUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IRoom);

				NewRecordUid = roomUid;

				goto Cleanup;
			}

			roomUid = Record.GetEmbeddedInRoomUid();

			if (roomUid > 0 && gRDB[roomUid] == null)
			{
				result = false;

				Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Location"), "Room", roomUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IRoom);

				NewRecordUid = roomUid;

				goto Cleanup;
			}

			var containerType = Record.GetCarriedByContainerContainerType();

			var artUid = Enum.IsDefined(typeof(ContainerType), containerType) ? Record.GetCarriedByContainerUid() : 0;

			if (artUid > 0)
			{
				var artifact = gADB[artUid];

				if (artifact == null)
				{
					result = false;

					Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Location"), "Artifact", artUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					NewRecordUid = artUid;

					goto Cleanup;
				}
				else if (artifact.GetArtifactCategory(gEngine.EvalContainerType(containerType, ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer)) == null)
				{
					result = false;

					Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Location"), "Artifact", artUid, string.Format("which should be {0}, but isn't", gEngine.EvalContainerType(containerType, "an In Container", "an On Container", "an Under Container", "a Behind Container")));

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					EditRecord = artifact;

					goto Cleanup;
				}
				else
				{
					var containedList = Record.GetContainedList(containerType: (ContainerType)(-1), recurse: true);

					containedList.Add(Record);

					if (containedList.Contains(artifact))
					{
						result = false;

						Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Location"), "Artifact", artUid, "which produces a cyclic graph");

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

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategories()
		{
			var result = true;

			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				result = ValidateFieldInterdependencies("CategoriesType") &&
								ValidateFieldInterdependencies("CategoriesField1") &&
								ValidateFieldInterdependencies("CategoriesField2") &&
								ValidateFieldInterdependencies("CategoriesField3") &&
								ValidateFieldInterdependencies("CategoriesField4") &&
								ValidateFieldInterdependencies("CategoriesField5") &&
								ValidateFieldInterdependencies("CategoriesField6") &&
								ValidateFieldInterdependencies("CategoriesField7") &&
								ValidateFieldInterdependencies("CategoriesField8") &&
								ValidateFieldInterdependencies("CategoriesField9") &&
								ValidateFieldInterdependencies("CategoriesField10") &&
								ValidateFieldInterdependencies("CategoriesField11") &&
								ValidateFieldInterdependencies("CategoriesField12") &&
								ValidateFieldInterdependencies("CategoriesField13") &&
								ValidateFieldInterdependencies("CategoriesField14") &&
								ValidateFieldInterdependencies("CategoriesField15") &&
								ValidateFieldInterdependencies("CategoriesField16") &&
								ValidateFieldInterdependencies("CategoriesField17") &&
								ValidateFieldInterdependencies("CategoriesField18") &&
								ValidateFieldInterdependencies("CategoriesField19") &&
								ValidateFieldInterdependencies("CategoriesField20");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="fieldNumber"></param>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField(long fieldNumber)
		{
			var result = true;

			var i = Index;

			Debug.Assert(i >= 0 && i < Record.Categories.Length);

			var category = Record.GetCategory(i);

			if (category != null && category.Type != ArtifactType.None)
			{
				var fieldProperty = category.GetType().GetProperty(string.Format("Field{0}", fieldNumber));

				var fieldValue = fieldProperty != null ? Convert.ToInt64(fieldProperty.GetValue(category)) : 0L;

				switch (fieldNumber)
				{
					case 1:

						if (category.Type == ArtifactType.InContainer)
						{
							if (fieldValue > 0 && gADB[fieldValue] == null)
							{
								result = false;

								Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "Artifact", fieldValue, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IArtifact);

								NewRecordUid = fieldValue;
							}
						}
						else if (category.Type == ArtifactType.Readable)
						{
							if (fieldValue > 0 && gEDB[fieldValue] == null)
							{
								result = false;

								Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "Effect", fieldValue, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IEffect);

								NewRecordUid = fieldValue;
							}
						}
						else if (category.Type == ArtifactType.DoorGate)
						{
							if (fieldValue > 0 && gRDB[fieldValue] == null)
							{
								result = false;

								Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "Room", fieldValue, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IRoom);

								NewRecordUid = fieldValue;
							}
						}
						else if (category.Type == ArtifactType.BoundMonster || category.Type == ArtifactType.DisguisedMonster)
						{
							if (fieldValue > 0 && gMDB[fieldValue] == null)
							{
								result = false;

								Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "Monster", fieldValue, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IMonster);

								NewRecordUid = fieldValue;
							}
						}

						break;

					case 2:

						if (category.Type == ArtifactType.Readable)
						{
							var effectUid = category.Field1;

							if (effectUid > 0)
							{
								effectUid++;

								for (var j = 1; j < fieldValue; j++, effectUid++)
								{
									if (gEDB[effectUid] == null)
									{
										result = false;

										Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "Effect", effectUid, "which doesn't exist");

										ErrorMessage = Buf.ToString();

										RecordType = typeof(IEffect);

										NewRecordUid = effectUid;

										goto Cleanup;
									}
								}
							}
						}
						else if (category.Type == ArtifactType.DoorGate || category.Type == ArtifactType.BoundMonster)
						{
							if (fieldValue > 0 && gADB[fieldValue] == null)
							{
								result = false;

								Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "Artifact", fieldValue, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IArtifact);

								NewRecordUid = fieldValue;
							}
						}
						else if (category.Type == ArtifactType.DisguisedMonster)
						{
							if (fieldValue > 0 && gEDB[fieldValue] == null)
							{
								result = false;

								Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "Effect", fieldValue, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IEffect);

								NewRecordUid = fieldValue;
							}
						}

						break;

					case 3:

						if (category.Type == ArtifactType.BoundMonster)
						{
							if (fieldValue > 0 && gMDB[fieldValue] == null)
							{
								result = false;

								Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("CategoriesField3"), "Monster", fieldValue, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IMonster);

								NewRecordUid = fieldValue;
							}
						}
						else if (category.Type == ArtifactType.DisguisedMonster)
						{
							var effectUid = category.Field2;

							if (effectUid > 0)
							{
								effectUid++;

								for (var j = 1; j < fieldValue; j++, effectUid++)
								{
									if (gEDB[effectUid] == null)
									{
										result = false;

										Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("CategoriesField3"), "Effect", effectUid, "which doesn't exist");

										ErrorMessage = Buf.ToString();

										RecordType = typeof(IEffect);

										NewRecordUid = effectUid;

										goto Cleanup;
									}
								}
							}
						}

						break;

					default:

						// Do nothing

						break;
				}
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField1()
		{
			return ValidateInterdependenciesCategoriesField(1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField2()
		{
			return ValidateInterdependenciesCategoriesField(2);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField3()
		{
			return ValidateInterdependenciesCategoriesField(3);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField4()
		{
			return ValidateInterdependenciesCategoriesField(4);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField5()
		{
			return ValidateInterdependenciesCategoriesField(5);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField6()
		{
			return ValidateInterdependenciesCategoriesField(6);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField7()
		{
			return ValidateInterdependenciesCategoriesField(7);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField8()
		{
			return ValidateInterdependenciesCategoriesField(8);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField9()
		{
			return ValidateInterdependenciesCategoriesField(9);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField10()
		{
			return ValidateInterdependenciesCategoriesField(10);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField11()
		{
			return ValidateInterdependenciesCategoriesField(11);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField12()
		{
			return ValidateInterdependenciesCategoriesField(12);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField13()
		{
			return ValidateInterdependenciesCategoriesField(13);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField14()
		{
			return ValidateInterdependenciesCategoriesField(14);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField15()
		{
			return ValidateInterdependenciesCategoriesField(15);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField16()
		{
			return ValidateInterdependenciesCategoriesField(16);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField17()
		{
			return ValidateInterdependenciesCategoriesField(17);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField18()
		{
			return ValidateInterdependenciesCategoriesField(18);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField19()
		{
			return ValidateInterdependenciesCategoriesField(19);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField20()
		{
			return ValidateInterdependenciesCategoriesField(20);
		}

		#endregion

		#region PrintDesc Methods

		/// <summary></summary>
		public virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the Artifact." + Environment.NewLine + Environment.NewLine + "Artifact names should always be in singular form and capitalized when appropriate.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescStateDesc()
		{
			var fullDesc = "Enter the state description of the Artifact (will typically be empty).";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescDesc()
		{
			var fullDesc = "Enter a detailed description of the Artifact.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescSeen()
		{
			var fullDesc = "Enter whether the player has seen the Artifact.";

			var briefDesc = "0=Not seen; 1=Seen";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescIsCharOwned()
		{
			var fullDesc = "Enter whether the player owns the Artifact.";

			var briefDesc = "0=Not char owned; 1=Char owned";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescIsPlural()
		{
			var fullDesc = "Enter whether the Artifact is singular or plural.";

			var briefDesc = "0=Singular; 1=Plural";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescIsListed()
		{
			var fullDesc = "Enter whether the Artifact is included in all appropriate content lists (Room, inventory, etc.)" + Environment.NewLine + Environment.NewLine + "Artifacts are typically included in content lists except in unusual circumstances (invisibility, vision impairment, mentioned in Room description, etc.); excluding them requires occasional support from special (user-programmed) events.";

			var briefDesc = "0=Not listed; 1=Listed";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescPluralType()
		{
			var fullDesc = "Enter the plural type that converts the Artifact's name from singular to plural.";

			var briefDesc = "0=No change; 1=Use 's'; 2=Use 'es'; 3=Use 'y' to 'ies'; (1000 + N)=Use Effect Uid N as plural name";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescArticleType()
		{
			var fullDesc = "Enter the article type that prefixes the Artifact's name with an article.";

			var briefDesc = "0=No article; 1=Use 'a'; 2=Use 'an'; 3=Use 'some'; 4=Use 'the'";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescValue()
		{
			var fullDesc = "Enter the value of the Artifact in gold pieces.";

			var briefDesc = string.Format("{0}-{1}=Valid value", gEngine.MinGoldValue, gEngine.MaxGoldValue);

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescWeight()
		{
			var fullDesc = "Enter the weight of the Artifact in gronds." + Environment.NewLine + Environment.NewLine + "Be sure to factor bulk and encumberance into weight values.";

			var briefDesc = "-999=Fixtures, doors, buildings, structures, etc; 1-5=Handheld object; 6-10=Medium sized items; 15-35=Weapons, equipment, etc; 999=Heavy furniture, giant objects, etc";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescLocation()
		{
			var fullDesc = "Enter the location of the Artifact.";

			var briefDesc = gDatabase.ArtifactTableType == ArtifactTableType.CharArt ? "(-3000 - N)=Worn by Character Uid N; (-N - 2000)=Carried by Character Uid N; 0=Limbo" : "(-1000 - N)=Worn by Monster Uid N; -999=Worn by player; (-N - 1)=Carried by Monster Uid N; -1=Carried by player; 0=Limbo; 1-1000=Room Uid; (1000 + N)=Inside Artifact Uid N; (2000 + N)=On Artifact Uid N; (3000 + N)=Under Artifact Uid N; (4000 + N)=Behind Artifact Uid N; (5000 + N)=Embedded in Room Uid N";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescUseExtendedAttributes()
		{
			var fullDesc = "Enter whether the Artifact is using extended attributes." + Environment.NewLine + Environment.NewLine + "This advanced feature allows a set of bit flags to determine various Artifact characteristics, previously settable only through programming.";

			var briefDesc = "0=Not using Extended Attributes; 1=Using Extended Attributes";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescExtendedAttributes()
		{
			var fullDesc = "Enter the extended attributes of the Artifact." + Environment.NewLine + Environment.NewLine + "When disabled, the game engine uses default behavior. See the EXTENDED_ATTRIBUTES.html file for more details.";

			var briefDesc = string.Format("0{0}=Extended Attributes bit flags", Record.UseExtendedAttributes ? "-511" : "");

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesType()
		{
			var i = Index;

			var fullDesc = new StringBuilder(gEngine.BufSize);

			var briefDesc = new StringBuilder(gEngine.BufSize);

			fullDesc.Append("Enter the type of the Artifact.");

			var artTypeValues = gDatabase.ArtifactTableType == ArtifactTableType.CharArt ? new List<ArtifactType>() { ArtifactType.Weapon, ArtifactType.MagicWeapon, ArtifactType.Wearable } : EnumUtil.GetValues<ArtifactType>(at => at != ArtifactType.None);

			for (var j = 0; j < artTypeValues.Count; j++)
			{
				var artType = gEngine.GetArtifactType(artTypeValues[j]);

				Debug.Assert(artType != null);

				briefDesc.AppendFormat("{0}{1}={2}", i > 0 && j == 0 ? "-1=None; " : j != 0 ? "; " : "", (long)artTypeValues[j], artType.Name);
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		/// <param name="fieldNumber"></param>
		public virtual void PrintDescCategoriesField(long fieldNumber)
		{
			var i = Index;

			var category = Record.GetCategory(i);

			if (category == null)
			{
				goto Cleanup;
			}

			var fullDesc = new StringBuilder(gEngine.BufSize);

			var briefDesc = new StringBuilder(gEngine.BufSize);

			Action field3Func = () =>
			{
				var containerType = gEngine.GetContainerType(category.Type);

				fullDesc.AppendFormat("Enter the maximum weight allowed in the Artifact's container content list.{0}{0}This is the total combined weight of all Artifacts immediately {1} the container (not including their contents).", Environment.NewLine, gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"));

				briefDesc.Append("(GE 0)=Valid value");
			};

			Action field3Func02 = () =>
			{
				fullDesc.Append("Enter whether the Artifact is open/closed.");

				briefDesc.Append("0=Closed; 1=Open");
			};

			Action field4Func = () =>
			{
				var containerType = gEngine.GetContainerType(category.Type);

				fullDesc.AppendFormat("Enter the maximum number of items allowed in the Artifact's container content list.{0}{0}Additionally, you can specify that the player can't put anything {1} the container.", Environment.NewLine, gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"));

				briefDesc.AppendFormat("0=Player can't put anything {0}; (GT 0)=Valid value", gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"));
			};

			Action field5Func = () =>
			{
				fullDesc.Append("Enter the display code that describes how to show the Artifact's container content list.");

				var containerDisplayCodeValues = EnumUtil.GetValues<ContainerDisplayCode>();

				for (var j = 0; j < containerDisplayCodeValues.Count; j++)
				{
					briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)containerDisplayCodeValues[j], gEngine.GetContainerDisplayCodeDesc(containerDisplayCodeValues[j]));
				}
			};

			switch (category.Type)
			{
				case ArtifactType.Weapon:
				case ArtifactType.MagicWeapon:

					if (fieldNumber == 1)
					{
						fullDesc.Append("Enter the Artifact's weapon complexity.");

						briefDesc.Append("-50-50=Valid value");
					}
					else if (fieldNumber == 2)
					{
						fullDesc.Append("Enter the Artifact's weapon type.");

						var weaponValues = EnumUtil.GetValues<Weapon>();

						for (var j = 0; j < weaponValues.Count; j++)
						{
							var weapon = gEngine.GetWeapon(weaponValues[j]);

							Debug.Assert(weapon != null);

							briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)weaponValues[j], weapon.Name);
						}
					}
					else if (fieldNumber == 3)
					{
						fullDesc.Append("Enter the Artifact's weapon hit dice.");

						briefDesc.Append("1-25=Valid value");
					}
					else if (fieldNumber == 4)
					{
						fullDesc.Append("Enter the Artifact's weapon hit dice sides.");

						briefDesc.Append("1-25=Valid value");
					}
					else if (fieldNumber == 5)
					{
						fullDesc.Append("Enter the Artifact's weapon number of hands required.");

						briefDesc.Append("1-2=Valid value");
					}
					else if (fieldNumber >= 6 && fieldNumber <= 13)
					{
						// Do nothing
					}

					break;

				case ArtifactType.InContainer:

					if (fieldNumber == 1)
					{
						fullDesc.Append("Enter the Artifact Uid of the key used to lock/unlock the container.");

						briefDesc.AppendFormat("-1=Can't be unlocked/opened normally; 0=No key; 1-{0}=Key Artifact Uid", gEngine.NumRecords);
					}
					else if (fieldNumber == 2)
					{
						fullDesc.AppendFormat("Enter whether the Artifact is open/closed.{0}{0}Additionally, you can specify that the container must be forced open.", Environment.NewLine);

						briefDesc.Append("0=Closed; 1=Open; (1000 + N)=Forced open with N hits damage");
					}
					else if (fieldNumber == 3)
					{
						field3Func();
					}
					else if (fieldNumber == 4)
					{
						field4Func();
					}
					else if (fieldNumber == 5)
					{
						field5Func();
					}

					break;

				case ArtifactType.OnContainer:
				case ArtifactType.UnderContainer:
				case ArtifactType.BehindContainer:

					if (fieldNumber == 3)
					{
						field3Func();
					}
					else if (fieldNumber == 4)
					{
						field4Func();
					}
					else if (fieldNumber == 5)
					{
						field5Func();
					}

					break;

				case ArtifactType.LightSource:

					if (fieldNumber == 1)
					{
						fullDesc.Append("Enter the number of rounds before the light source is exhausted/goes out.");

						briefDesc.Append("-1=Never runs out; (GE 0)=Valid value");
					}

					break;

				case ArtifactType.Drinkable:
				case ArtifactType.Edible:

					if (fieldNumber == 1)
					{
						fullDesc.Append("Enter the number of hits healed (or inflicted, if negative) for the Artifact.");
					}
					else if (fieldNumber == 2)
					{
						fullDesc.Append("Enter the number of times the Artifact can be used.");

						briefDesc.Append("(GTE 0)=Valid value");
					}
					else if (fieldNumber == 3)
					{
						field3Func02();
					}

					break;

				case ArtifactType.Readable:

					if (fieldNumber == 1)
					{
						fullDesc.Append("Enter the Uid of the first Effect displayed when the Artifact is read.");

						briefDesc.AppendFormat("1-{0}=Effect Uid", gEngine.NumRecords);
					}
					else if (fieldNumber == 2)
					{
						fullDesc.Append("Enter the number of sequential Effects displayed when the Artifact is read.");

						briefDesc.Append("(GT 0)=Valid value");
					}
					else if (fieldNumber == 3)
					{
						field3Func02();
					}

					break;

				case ArtifactType.DoorGate:

					if (fieldNumber == 1)
					{
						fullDesc.Append("Enter the Uid of the Room on the opposite side of the door/gate.");
					}
					else if (fieldNumber == 2)
					{
						fullDesc.Append("Enter the Artifact Uid of the key used to lock/unlock the door/gate.");

						briefDesc.AppendFormat("-1=Can't be unlocked/opened normally; 0=No key; 1-{0}=Key Artifact Uid", gEngine.NumRecords);
					}
					else if (fieldNumber == 3)
					{
						fullDesc.AppendFormat("Enter whether the Artifact is open/closed.{0}{0}Additionally, you can specify that the door/gate must be forced open.", Environment.NewLine);

						briefDesc.Append("0=Open; 1=Closed; (1000 + N)=Forced open with N hits damage");
					}
					else if (fieldNumber == 4)
					{
						fullDesc.Append("Enter whether the Artifact is normal/hidden.");

						briefDesc.Append("0=Normal; 1=Hidden");
					}

					break;

				case ArtifactType.BoundMonster:

					if (fieldNumber == 1)
					{
						fullDesc.Append("Enter the Uid of the Monster that is bound.");

						briefDesc.AppendFormat("1-{0}=Bound Monster Uid", gEngine.NumRecords);
					}
					else if (fieldNumber == 2)
					{
						fullDesc.Append("Enter the Artifact Uid of the key used to lock/unlock the bound Monster.");

						briefDesc.AppendFormat("-1=Can't be unlocked/opened normally; 0=No key; 1-{0}=Key Artifact Uid", gEngine.NumRecords);
					}
					else if (fieldNumber == 3)
					{
						fullDesc.Append("Enter the Uid of the Monster that is guarding the bound Monster.");

						briefDesc.AppendFormat("0=No guard; 1-{0}=Guard Monster Uid", gEngine.NumRecords);
					}

					break;

				case ArtifactType.Wearable:

					if (fieldNumber == 1)
					{
						fullDesc.Append("Enter the armor class of the Artifact.");

						var armorValues = EnumUtil.GetValues<Armor>(a => (a == Armor.ClothesShield || ((long)a) % 2 == 0) && (gDatabase.ArtifactTableType == ArtifactTableType.Default || a != Armor.SkinClothes));

						for (var j = 0; j < armorValues.Count; j++)
						{
							var armor = gEngine.GetArmor(armorValues[j]);

							Debug.Assert(armor != null);

							briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)armorValues[j], armor.Name);
						}
					}
					else if (fieldNumber == 2)
					{
						fullDesc.Append("Enter the clothing type of the Artifact.");

						var clothingValues = gDatabase.ArtifactTableType == ArtifactTableType.CharArt ? new List<Clothing>() { Clothing.ArmorShields } : EnumUtil.GetValues<Clothing>();

						for (var j = 0; j < clothingValues.Count; j++)
						{
							briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)clothingValues[j], gEngine.GetClothingName(clothingValues[j]));
						}
					}

					break;

				case ArtifactType.DisguisedMonster:

					if (fieldNumber == 1)
					{
						fullDesc.Append("Enter the Uid of the Monster that is disguised.");

						briefDesc.AppendFormat("1-{0}=Disguised Monster Uid", gEngine.NumRecords);
					}
					else if (fieldNumber == 2)
					{
						fullDesc.Append("Enter the Uid of the first Effect displayed when the disguised Monster is revealed.");

						briefDesc.AppendFormat("1-{0}=Effect Uid", gEngine.NumRecords);
					}
					else if (fieldNumber == 3)
					{
						fullDesc.Append("Enter the number of sequential Effects displayed when the disguised Monster is revealed.");

						briefDesc.Append("(GT 0)=Valid value");
					}

					break;

				case ArtifactType.DeadBody:

					if (fieldNumber == 1)
					{
						fullDesc.AppendFormat("Enter whether the Artifact is takeable.{0}{0}Typically, dead bodies should not be takeable unless it serves some useful purpose.", Environment.NewLine);

						briefDesc.Append("0=Not takeable; 1=Takeable");
					}

					break;

				default:

					break;
			}

			if (fullDesc.Length > 0)
			{
				gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.Length > 0 ? briefDesc : null);
			}

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField1()
		{
			PrintDescCategoriesField(1);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField2()
		{
			PrintDescCategoriesField(2);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField3()
		{
			PrintDescCategoriesField(3);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField4()
		{
			PrintDescCategoriesField(4);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField5()
		{
			PrintDescCategoriesField(5);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField6()
		{
			PrintDescCategoriesField(6);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField7()
		{
			PrintDescCategoriesField(7);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField8()
		{
			PrintDescCategoriesField(8);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField9()
		{
			PrintDescCategoriesField(9);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField10()
		{
			PrintDescCategoriesField(10);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField11()
		{
			PrintDescCategoriesField(11);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField12()
		{
			PrintDescCategoriesField(12);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField13()
		{
			PrintDescCategoriesField(13);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField14()
		{
			PrintDescCategoriesField(14);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField15()
		{
			PrintDescCategoriesField(15);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField16()
		{
			PrintDescCategoriesField(16);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField17()
		{
			PrintDescCategoriesField(17);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField18()
		{
			PrintDescCategoriesField(18);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField19()
		{
			PrintDescCategoriesField(19);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField20()
		{
			PrintDescCategoriesField(20);
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
		public virtual void ListStateDesc()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (!string.IsNullOrWhiteSpace(Record.StateDesc))
				{
					Buf.Clear();

					Buf.Append(Record.StateDesc);

					gOut.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("StateDesc"), null), Buf);
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("StateDesc"), null), Record.StateDesc);
				}
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
		public virtual void ListIsCharOwned()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("IsCharOwned"), null), Convert.ToInt64(Record.IsCharOwned));
			}
		}

		/// <summary></summary>
		public virtual void ListIsPlural()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("IsPlural"), null), Convert.ToInt64(Record.IsPlural));
			}
		}

		/// <summary></summary>
		public virtual void ListIsListed()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("IsListed"), null), Convert.ToInt64(Record.IsListed));
			}
		}

		/// <summary></summary>
		public virtual void ListPluralType()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					Buf.Clear();

					Buf01.Clear();

					var effectUid = gEngine.GetPluralTypeEffectUid(Record.PluralType);

					var effect = gEDB[effectUid];

					if (effect != null)
					{
						Buf01.Append(effect.Desc.Length > gEngine.ArtNameLen - 6 ? effect.Desc.Substring(0, gEngine.ArtNameLen - 9) + "..." : effect.Desc);

						Buf.AppendFormat("Use '{0}'", Buf01.ToString());
					}
					else
					{
						Buf.AppendFormat("Use Effect Uid {0}", effectUid);
					}

					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("PluralType"), null),
						gEngine.BuildValue(51, ' ', 8, (long)Record.PluralType, null,
						Record.PluralType == PluralType.None ? "No change" :
						Record.PluralType == PluralType.S ? "Use 's'" :
						Record.PluralType == PluralType.Es ? "Use 'es'" :
						Record.PluralType == PluralType.YIes ? "Use 'y' to 'ies'" :
						Buf.ToString()));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("PluralType"), null), (long)Record.PluralType);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListArticleType()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ArticleType"), null),
						gEngine.BuildValue(51, ' ', 8, (long)Record.ArticleType, null,
						Record.ArticleType == ArticleType.None ? "No article" :
						Record.ArticleType == ArticleType.A ? "Use 'a'" :
						Record.ArticleType == ArticleType.An ? "Use 'an'" :
						Record.ArticleType == ArticleType.Some ? "Use 'some'" :
						"Use 'the'"));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ArticleType"), null), (long)Record.ArticleType);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListValue()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Value"), null), Record.Value);
			}
		}

		/// <summary></summary>
		public virtual void ListWeight()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Weight"), null),
						BuildValue(51, ' ', 8, "Weight"));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Weight"), null), Record.Weight);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListLocation()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Location"), null),
						BuildValue(51, ' ', 8, "Location"));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Location"), null), Record.Location);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListUseExtendedAttributes()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("UseExtendedAttributes"), null), Convert.ToInt64(Record.UseExtendedAttributes));
			}
		}

		/// <summary></summary>
		public virtual void ListExtendedAttributes()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg && Record.UseExtendedAttributes)
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ExtendedAttributes"), null),
						gEngine.BuildValue(51, ' ', 8, (long)Record.ExtendedAttributes, null, string.Join(",", BitFlags.GetSetBits(Record.ExtendedAttributes))));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ExtendedAttributes"), null), Record.ExtendedAttributes);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListCategories()
		{
			var artTypes = new ArtifactType[] { /* ArtifactType.Weapon, ArtifactType.MagicWeapon */ };		// TODO: uncomment when needed

			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				ListField("CategoriesType");
				ListField("CategoriesField1");
				ListField("CategoriesField2");
				ListField("CategoriesField3");
				ListField("CategoriesField4");
				ListField("CategoriesField5");

				var category = Record.GetCategory(Index);

				if (gEngine.EnableEnhancedCombat && category != null && artTypes.Contains(category.Type))
				{
					ListField("CategoriesField6");
					ListField("CategoriesField7");
					ListField("CategoriesField8");
					ListField("CategoriesField9");
					ListField("CategoriesField10");
					ListField("CategoriesField11");
					ListField("CategoriesField12");
					ListField("CategoriesField13");
				}

				/*
				ListField("CategoriesField14");
				ListField("CategoriesField15");
				ListField("CategoriesField16");
				ListField("CategoriesField17");
				ListField("CategoriesField18");
				ListField("CategoriesField19");
				ListField("CategoriesField20");
				*/
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		public virtual void ListCategoriesType()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || i == 0 || Record.GetCategory(i - 1).Type != ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesType"), null),
							BuildValue(51, ' ', 8, "CategoriesType"));
					}
					else
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesType"), null), (long)Record.GetCategory(i).Type);
					}
				}
			}
		}

		/// <summary></summary>
		/// <param name="fieldNumber"></param>
		public virtual void ListCategoriesField(long fieldNumber)
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategory(i).Type != ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					var fieldName = string.Format("CategoriesField{0}", fieldNumber);

					if (LookupMsg)
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName(fieldName), null),
							BuildValue(51, ' ', 8, fieldName));
					}
					else
					{
						var category = Record.GetCategory(i);

						var property = category != null ? category.GetType().GetProperty(string.Format("Field{0}", fieldNumber)) : null;

						var fieldValue = property != null ? Convert.ToInt64(property.GetValue(category)) : 0L;

						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName(fieldName), null),
							fieldValue);
					}
				}
			}
		}

		/// <summary></summary>
		public virtual void ListCategoriesField1()
		{
			ListCategoriesField(1);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField2()
		{
			ListCategoriesField(2);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField3()
		{
			ListCategoriesField(3);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField4()
		{
			ListCategoriesField(4);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField5()
		{
			ListCategoriesField(5);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField6()
		{
			ListCategoriesField(6);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField7()
		{
			ListCategoriesField(7);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField8()
		{
			ListCategoriesField(8);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField9()
		{
			ListCategoriesField(9);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField10()
		{
			ListCategoriesField(10);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField11()
		{
			ListCategoriesField(11);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField12()
		{
			ListCategoriesField(12);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField13()
		{
			ListCategoriesField(13);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField14()
		{
			ListCategoriesField(14);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField15()
		{
			ListCategoriesField(15);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField16()
		{
			ListCategoriesField(16);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField17()
		{
			ListCategoriesField(17);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField18()
		{
			ListCategoriesField(18);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField19()
		{
			ListCategoriesField(19);
		}

		/// <summary></summary>
		public virtual void ListCategoriesField20()
		{
			ListCategoriesField(20);
		}

		#endregion

		#region Input Methods

		/// <summary></summary>
		public virtual void InputUid()
		{
			gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			gOut.Print("{0}", gEngine.LineSep);
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

				var rc = gEngine.In.ReadField(Buf, gEngine.ArtNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Name = Buf.ToString();

				if (ValidateField("Name"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputStateDesc()
		{
			var fieldDesc = FieldDesc;

			var stateDesc = Record.StateDesc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", stateDesc);

				PrintFieldDesc("StateDesc", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("StateDesc"), null));

				gOut.WordWrap = false;

				var rc = gEngine.In.ReadField(Buf, gEngine.ArtStateDescLen, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				Record.StateDesc = Buf.TrimEnd().ToString();

				if (ValidateField("StateDesc"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
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

				var rc = gEngine.In.ReadField(Buf, gEngine.ArtDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				Record.Desc = Buf.Trim().ToString();

				if (ValidateField("Desc"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
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

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Seen = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("Seen"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputIsCharOwned()
		{
			var fieldDesc = FieldDesc;

			var isCharOwned = Record.IsCharOwned;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isCharOwned));

				PrintFieldDesc("IsCharOwned", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("IsCharOwned"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.IsCharOwned = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsCharOwned"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputIsPlural()
		{
			var fieldDesc = FieldDesc;

			var isPlural = Record.IsPlural;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isPlural));

				PrintFieldDesc("IsPlural", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("IsPlural"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.IsPlural = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsPlural"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputIsListed()
		{
			var fieldDesc = FieldDesc;

			var isListed = Record.IsListed;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isListed));

				PrintFieldDesc("IsListed", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("IsListed"), "1"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.IsListed = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsListed"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputPluralType()
		{
			var fieldDesc = FieldDesc;

			var pluralType = Record.PluralType;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)pluralType);

				PrintFieldDesc("PluralType", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("PluralType"), "1"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.PluralType = (PluralType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("PluralType"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputArticleType()
		{
			var fieldDesc = FieldDesc;

			var articleType = Record.ArticleType;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)articleType);

				PrintFieldDesc("ArticleType", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("ArticleType"), "1"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ArticleType = (ArticleType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("ArticleType"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputValue()
		{
			var fieldDesc = FieldDesc;

			var value = Record.Value;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", value);

				PrintFieldDesc("Value", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Value"), "25"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "25", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Value = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Value"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputWeight()
		{
			var artType = EditRec ? gEngine.GetArtifactType(Record.GetCategory(0).Type) : null;

			Debug.Assert(!EditRec || artType != null);

			var fieldDesc = FieldDesc;

			var weight = Record.Weight;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", weight);

				PrintFieldDesc("Weight", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Weight"), artType != null ? artType.WeightEmptyVal : "15"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, artType != null ? artType.WeightEmptyVal : "15", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Weight = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Weight"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputLocation()
		{
			var artType = EditRec ? gEngine.GetArtifactType(Record.GetCategory(0).Type) : null;

			Debug.Assert(!EditRec || artType != null);

			var fieldDesc = FieldDesc;

			var location = Record.Location;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", location);

				PrintFieldDesc("Location", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Location"), artType != null ? artType.LocationEmptyVal : "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, artType != null ? artType.LocationEmptyVal : "0", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Location = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Location"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputUseExtendedAttributes()
		{
			var fieldDesc = FieldDesc;

			var useExtendedAttributes = Record.UseExtendedAttributes;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(useExtendedAttributes));

				PrintFieldDesc("UseExtendedAttributes", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("UseExtendedAttributes"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.UseExtendedAttributes = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("UseExtendedAttributes"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Record.ExtendedAttributes = Record.UseExtendedAttributes ? gEngine.DefaultArtExtAttributes : 0;

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputExtendedAttributes()
		{
			var fieldDesc = FieldDesc;

			var extendedAttributes = Record.ExtendedAttributes;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", extendedAttributes);

				PrintFieldDesc("ExtendedAttributes", EditRec, EditField, fieldDesc);

				var defaultExtAttributes = Record.UseExtendedAttributes ? gEngine.DefaultArtExtAttributes.ToString() : "0";

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("ExtendedAttributes"), defaultExtAttributes));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, defaultExtAttributes, null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ExtendedAttributes = Convert.ToUInt64(Buf.Trim().ToString());

				if (ValidateField("ExtendedAttributes"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputCategories()
		{
			var artTypes = new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon };

			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				InputField("CategoriesType");
				InputField("CategoriesField1");
				InputField("CategoriesField2");
				InputField("CategoriesField3");
				InputField("CategoriesField4");
				InputField("CategoriesField5");

				var category = Record.GetCategory(Index);

				if (gEngine.EnableEnhancedCombat && category != null && artTypes.Contains(category.Type))
				{
					InputField("CategoriesField6");
					InputField("CategoriesField7");
					InputField("CategoriesField8");
					InputField("CategoriesField9");
					InputField("CategoriesField10");
					InputField("CategoriesField11");
					InputField("CategoriesField12");
					InputField("CategoriesField13");
				}

				/*
				InputField("CategoriesField14");
				InputField("CategoriesField15");
				InputField("CategoriesField16");
				InputField("CategoriesField17");
				InputField("CategoriesField18");
				InputField("CategoriesField19");
				InputField("CategoriesField20");
				*/
			}
		}

		/// <summary></summary>
		public virtual void InputCategoriesType()
		{
			var i = Index;

			if (i == 0 || Record.GetCategory(i - 1).Type != ArtifactType.None)
			{
				var fieldDesc = FieldDesc;

				var type = Record.GetCategory(i).Type;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", (long)type);

					PrintFieldDesc("CategoriesType", EditRec, EditField, fieldDesc);

					var defaultArtType = gDatabase.ArtifactTableType == ArtifactTableType.CharArt ? "2" : i == 0 ? "1" : "-1";

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesType"), defaultArtType));

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, defaultArtType, null, i == 0 ? (Func<char, bool>)gEngine.IsCharDigit : gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategory(i).Type = (ArtifactType)Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesType"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetCategory(i).Type != ArtifactType.None)
				{
					if (EditRec && Record.GetCategory(i).Type != type)
					{
						var artType = gEngine.GetArtifactType(Record.GetCategory(i).Type);

						Debug.Assert(artType != null);

						for (var m = 1; m <= gEngine.NumArtifactCategoryFields; m++)
						{
							var destProperty = Record.GetCategory(i).GetType().GetProperty(string.Format("Field{0}", m));

							var srcProperty = artType.GetType().GetProperty(string.Format("Field{0}EmptyVal", m));

							if (destProperty != null && srcProperty != null && srcProperty.CanRead && destProperty.CanWrite)
							{
								destProperty.SetValue(Record.GetCategory(i), Convert.ToInt64(srcProperty.GetValue(artType)));
							}
						}
					}
				}
				else
				{
					for (var k = i; k < Record.Categories.Length; k++)
					{
						Record.GetCategory(k).Type = ArtifactType.None;

						Record.GetCategory(k).SetFieldsValue(1, gEngine.NumArtifactCategoryFields, 0);
					}
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				Record.GetCategory(i).Type = ArtifactType.None;
			}
		}

		/// <summary></summary>
		/// <param name="fieldNumber"></param>
		public virtual void InputCategoriesField(long fieldNumber)
		{
			var i = Index;

			var category = Record.GetCategory(i);

			var fieldProperty = category != null ? category.GetType().GetProperty(string.Format("Field{0}", fieldNumber)) : null;

			if (category != null && category.Type != ArtifactType.None)
			{
				var artType = gEngine.GetArtifactType(category.Type);

				var fieldDesc = FieldDesc;

				var fieldValue = fieldProperty != null ? Convert.ToInt64(fieldProperty.GetValue(category)) : 0L;

				var artTypeFieldEmptyValProperty = artType != null ? artType.GetType().GetProperty(string.Format("Field{0}EmptyVal", fieldNumber)) : null;

				var fieldEmptyVal = artTypeFieldEmptyValProperty != null ? (artTypeFieldEmptyValProperty.GetValue(artType)?.ToString() ?? "0") : "0";

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", fieldValue);

					var fieldName = string.Format("CategoriesField{0}", fieldNumber);

					PrintFieldDesc(fieldName, EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName(fieldName), fieldEmptyVal));

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, fieldEmptyVal, null, gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						var newValue = Convert.ToInt64(Buf.Trim().ToString());

						if (fieldProperty != null)
						{
							fieldProperty.SetValue(category, newValue);
						}
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField(fieldName))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				if (fieldProperty != null)
				{
					fieldProperty.SetValue(category, 0L);
				}
			}
		}

		/// <summary></summary>
		public virtual void InputCategoriesField1()
		{
			InputCategoriesField(1);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField2()
		{
			InputCategoriesField(2);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField3()
		{
			InputCategoriesField(3);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField4()
		{
			InputCategoriesField(4);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField5()
		{
			InputCategoriesField(5);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField6()
		{
			InputCategoriesField(6);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField7()
		{
			InputCategoriesField(7);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField8()
		{
			InputCategoriesField(8);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField9()
		{
			InputCategoriesField(9);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField10()
		{
			InputCategoriesField(10);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField11()
		{
			InputCategoriesField(11);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField12()
		{
			InputCategoriesField(12);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField13()
		{
			InputCategoriesField(13);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField14()
		{
			InputCategoriesField(14);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField15()
		{
			InputCategoriesField(15);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField16()
		{
			InputCategoriesField(16);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField17()
		{
			InputCategoriesField(17);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField18()
		{
			InputCategoriesField(18);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField19()
		{
			InputCategoriesField(19);
		}

		/// <summary></summary>
		public virtual void InputCategoriesField20()
		{
			InputCategoriesField(20);
		}

		#endregion

		#region BuildValue Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueWeight()
		{
			Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.Weight, null, Record.IsUnmovable() ? "Unmovable" : null));

			return Buf01.ToString();
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueLocation()
		{
			string lookupMsg;

			if (Record.IsCarriedByCharacter())
			{
				var character = Record.GetCarriedByCharacter();

				lookupMsg = string.Format("Carried by {0}",
					character != null ? gEngine.Capitalize(character.Name.Length > 29 ? character.Name.Substring(0, 26) + "..." : character.Name) : gEngine.UnknownName);
			}
			else if (Record.IsWornByCharacter())
			{
				var character = Record.GetWornByCharacter();

				lookupMsg = string.Format("Worn by {0}",
					character != null ? gEngine.Capitalize(character.Name.Length > 32 ? character.Name.Substring(0, 29) + "..." : character.Name) : gEngine.UnknownName);
			}
			else if (Record.IsCarriedByMonster(MonsterType.CharMonster))
			{
				lookupMsg = "Carried by Player Character";
			}
			else if (Record.IsWornByMonster(MonsterType.CharMonster))
			{
				lookupMsg = "Worn by Player Character";
			}
			else if (Record.IsCarriedByMonster())
			{
				var monster = Record.GetCarriedByMonster();

				lookupMsg = string.Format("Carried by {0}",
					monster != null ? gEngine.Capitalize(monster.Name.Length > 29 ? monster.Name.Substring(0, 26) + "..." : monster.Name) : gEngine.UnknownName);
			}
			else if (Record.IsWornByMonster())
			{
				var monster = Record.GetWornByMonster();

				lookupMsg = string.Format("Worn by {0}",
					monster != null ? gEngine.Capitalize(monster.Name.Length > 32 ? monster.Name.Substring(0, 29) + "..." : monster.Name) : gEngine.UnknownName);
			}
			else if (Record.IsCarriedByContainer())
			{
				var containerType = Record.GetCarriedByContainerContainerType();

				var artifact = Record.GetCarriedByContainer();

				lookupMsg = string.Format("{0} {1}",
					gEngine.EvalContainerType(containerType, "Inside", "On", "Under", "Behind"),
					artifact != null ? gEngine.Capitalize(artifact.Name.Length > 33 ? artifact.Name.Substring(0, 30) + "..." : artifact.Name) : gEngine.UnknownName);
			}
			else if (Record.IsEmbeddedInRoom())
			{
				var room = Record.GetEmbeddedInRoom();

				lookupMsg = string.Format("Embedded in {0}",
					room != null ? gEngine.Capitalize(room.Name.Length > 28 ? room.Name.Substring(0, 25) + "..." : room.Name) : gEngine.UnknownName);
			}
			else if (Record.IsInRoom())
			{
				var room = Record.GetInRoom();

				lookupMsg = room != null ? gEngine.Capitalize(room.Name) : gEngine.UnknownName;
			}
			else
			{
				lookupMsg = null;
			}

			Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.Location, null, lookupMsg));

			return Buf01.ToString();
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesType()
		{
			var i = Index;

			var artType = gEngine.GetArtifactType(Record.GetCategory(i).Type);

			Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, (long)Record.GetCategory(i).Type, null, artType != null ? artType.Name : "None"));

			return Buf01.ToString();
		}

		/// <summary></summary>
		/// <param name="fieldNumber"></param>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField(long fieldNumber)
		{
			var i = Index;

			var category = Record.GetCategory(i);

			if (category == null)
			{
				goto Cleanup;
			}

			var fieldProperty = category.GetType().GetProperty(string.Format("Field{0}", fieldNumber));

			var fieldValue = fieldProperty != null ? Convert.ToInt64(fieldProperty.GetValue(category)) : 0L;

			string stringVal = null;

			string lookupMsg = null;

			Action artLookupFunc = () =>
			{
				var artifact = gADB[fieldValue];

				lookupMsg = artifact != null ? gEngine.Capitalize(artifact.Name) : gEngine.UnknownName;
			};

			Action monLookupFunc = () =>
			{
				var monster = gMDB[fieldValue];

				lookupMsg = monster != null ? gEngine.Capitalize(monster.Name) : gEngine.UnknownName;
			};

			switch (category.Type)
			{
				case ArtifactType.Weapon:
				case ArtifactType.MagicWeapon:

					if (fieldNumber == 1)
					{
						stringVal = string.Format("{0}%", fieldValue);

						fieldValue = 0;
					}
					else if (fieldNumber == 2)
					{
						var weapon = gEngine.GetWeapon((Weapon)fieldValue);

						Debug.Assert(weapon != null);

						lookupMsg = weapon.Name;
					}

					break;

				case ArtifactType.InContainer:

					if (fieldNumber == 1)
					{
						if (fieldValue > 0)
						{
							artLookupFunc();
						}
					}
					else if (fieldNumber == 2)
					{
						if (Record.IsFieldStrength(fieldValue))
						{
							lookupMsg = string.Format("Strength of {0}", Record.GetFieldStrength(fieldValue));
						}
						else
						{
							lookupMsg = fieldValue == 1 ? "Open" : "Closed";
						}
					}
					else if (fieldNumber == 5)
					{
						lookupMsg = gEngine.GetContainerDisplayCodeDesc((ContainerDisplayCode)fieldValue);
					}

					break;

				case ArtifactType.OnContainer:
				case ArtifactType.UnderContainer:
				case ArtifactType.BehindContainer:

					if (fieldNumber == 5)
					{
						lookupMsg = gEngine.GetContainerDisplayCodeDesc((ContainerDisplayCode)fieldValue);
					}

					break;

				case ArtifactType.Drinkable:
				case ArtifactType.Readable:
				case ArtifactType.Edible:

					if (fieldNumber == 3)
					{
						lookupMsg = category.IsOpen() ? "Open" : "Closed";
					}

					break;

				case ArtifactType.DoorGate:

					if (fieldNumber == 1)
					{
						if (fieldValue > 0)
						{
							var room = gRDB[fieldValue];

							lookupMsg = room != null ? gEngine.Capitalize(room.Name) : gEngine.UnknownName;
						}
					}
					else if (fieldNumber == 2)
					{
						if (fieldValue > 0)
						{
							artLookupFunc();
						}
					}
					else if (fieldNumber == 3)
					{
						if (Record.IsFieldStrength(fieldValue))
						{
							lookupMsg = string.Format("Strength of {0}", Record.GetFieldStrength(fieldValue));
						}
						else
						{
							lookupMsg = category.IsOpen() ? "Open" : "Closed";
						}
					}
					else if (fieldNumber == 4)
					{
						lookupMsg = fieldValue == 1 ? "Hidden" : "Normal";
					}

					break;

				case ArtifactType.BoundMonster:

					if (fieldNumber == 1)
					{
						monLookupFunc();
					}
					else if (fieldNumber == 2)
					{
						if (fieldValue > 0)
						{
							artLookupFunc();
						}
					}
					else if (fieldNumber == 3)
					{
						if (fieldValue > 0)
						{
							monLookupFunc();
						}
					}

					break;

				case ArtifactType.Wearable:

					if (fieldNumber == 1)
					{
						var armor = gEngine.GetArmor((Armor)fieldValue);

						Debug.Assert(armor != null);

						lookupMsg = armor.Name;
					}
					else if (fieldNumber == 2)
					{
						lookupMsg = gEngine.GetClothingName((Clothing)fieldValue);
					}

					break;

				case ArtifactType.DisguisedMonster:

					if (fieldNumber == 1)
					{
						monLookupFunc();
					}

					break;

				case ArtifactType.DeadBody:

					if (fieldNumber == 1)
					{
						lookupMsg = fieldValue == 1 ? "Takeable" : "Not Takeable";
					}

					break;

				default:

					break;
			}

			Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, fieldValue, stringVal, lookupMsg));

		Cleanup:

			return Buf01.ToString();
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField1()
		{
			return BuildValueCategoriesField(1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField2()
		{
			return BuildValueCategoriesField(2);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField3()
		{
			return BuildValueCategoriesField(3);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField4()
		{
			return BuildValueCategoriesField(4);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField5()
		{
			return BuildValueCategoriesField(5);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField6()
		{
			return BuildValueCategoriesField(6);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField7()
		{
			return BuildValueCategoriesField(7);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField8()
		{
			return BuildValueCategoriesField(8);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField9()
		{
			return BuildValueCategoriesField(9);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField10()
		{
			return BuildValueCategoriesField(10);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField11()
		{
			return BuildValueCategoriesField(11);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField12()
		{
			return BuildValueCategoriesField(12);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField13()
		{
			return BuildValueCategoriesField(13);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField14()
		{
			return BuildValueCategoriesField(14);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField15()
		{
			return BuildValueCategoriesField(15);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField16()
		{
			return BuildValueCategoriesField(16);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField17()
		{
			return BuildValueCategoriesField(17);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField18()
		{
			return BuildValueCategoriesField(18);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField19()
		{
			return BuildValueCategoriesField(19);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField20()
		{
			return BuildValueCategoriesField(20);
		}

		/// <summary></summary>
		/// <param name="bufSize"></param>
		/// <param name="fillChar"></param>
		/// <param name="offset"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public virtual string BuildValue(long bufSize, char fillChar, long offset, string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var origBufSize = BufSize;

			var origFillChar = FillChar;

			var origOffset = Offset;

			BufSize = bufSize;

			FillChar = fillChar;

			Offset = offset;

			var result = BuildValue(fieldName);

			BufSize = origBufSize;

			FillChar = origFillChar;

			Offset = origOffset;

			return result;
		}

		#endregion

		#endregion

		#region Class ArtifactHelper

		public override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = gDatabase.GetArtifactUid();
			}
		}

		public ArtifactHelper()
		{

		}

		#endregion

		#endregion
	}
}
