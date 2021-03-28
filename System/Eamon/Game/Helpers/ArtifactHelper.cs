
// ArtifactHelper.cs

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
		public virtual string GetPrintedNameCategoriesType()
		{
			var i = Index;

			return string.Format("Cat #{0} Type", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField1()
		{
			var i = Index;

			var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

			return string.Format("Cat #{0} {1}", i + 1, artType != null ? artType.Field1Name : "Field1");
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField2()
		{
			var i = Index;

			var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

			return string.Format("Cat #{0} {1}", i + 1, artType != null ? artType.Field2Name : "Field2");
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField3()
		{
			var i = Index;

			var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

			return string.Format("Cat #{0} {1}", i + 1, artType != null ? artType.Field3Name : "Field3");
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField4()
		{
			var i = Index;

			var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

			return string.Format("Cat #{0} {1}", i + 1, artType != null ? artType.Field4Name : "Field4");
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCategoriesField5()
		{
			var i = Index;

			var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

			return string.Format("Cat #{0} {1}", i + 1, artType != null ? artType.Field5Name : "Field5");
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
			}

			return "Categories";
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
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField1(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field1", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField2(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field2", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField3(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field3", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField4(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field4", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameCategoriesField5(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field5", i);

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
		public virtual object GetValueCategoriesType()
		{
			var i = Index;

			return Record.GetCategories(i).Type;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField1()
		{
			var i = Index;

			return Record.GetCategories(i).Field1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField2()
		{
			var i = Index;

			return Record.GetCategories(i).Field2;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField3()
		{
			var i = Index;

			return Record.GetCategories(i).Field3;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField4()
		{
			var i = Index;

			return Record.GetCategories(i).Field4;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueCategoriesField5()
		{
			var i = Index;

			return Record.GetCategories(i).Field5;
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

			var result = !string.IsNullOrWhiteSpace(Record.Name);

			if (result && Record.Name.Length > Constants.ArtNameLen)
			{
				for (var i = Constants.ArtNameLen; i < Record.Name.Length; i++)
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

				result = Array.FindIndex(Constants.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? recordName.IndexOf(" " + token + " ") >= 0 : recordName.IndexOf(token) >= 0) < 0;

				if (result)
				{
					result = Array.FindIndex(Constants.PronounTokens, token => recordName.IndexOf(" " + token + " ") >= 0) < 0;
				}

				// TODO: might need to disallow verb name matches as well
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateStateDesc()
		{
			return Record.StateDesc != null && Record.StateDesc.Length <= Constants.ArtStateDescLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateDesc()
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.ArtDescLen;
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
			return Record.Value >= Constants.MinGoldValue && Record.Value <= Constants.MaxGoldValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategories()
		{
			var result = true;

			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				result = ValidateField("CategoriesType") &&
								ValidateField("CategoriesField1") &&
								ValidateField("CategoriesField2") &&
								ValidateField("CategoriesField3") &&
								ValidateField("CategoriesField4") &&
								ValidateField("CategoriesField5");

				if (result == false)
				{
					break;
				}
			}

			return result;
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
				if (Record.GetCategories(h).Type == ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				result = gEngine.IsValidArtifactType(Record.GetCategories(i).Type);

				if (result)
				{
					for (var h = 0; h < Record.Categories.Length; h++)
					{
						if (h != i && Record.GetCategories(h).Type != ArtifactType.None)
						{
							if ((Record.GetCategories(h).Type == Record.GetCategories(i).Type) ||
									(Record.GetCategories(h).Type == ArtifactType.Gold && Record.GetCategories(i).Type == ArtifactType.Treasure) ||
									(Record.GetCategories(h).Type == ArtifactType.Treasure && Record.GetCategories(i).Type == ArtifactType.Gold) ||
									(Record.GetCategories(h).Type == ArtifactType.Weapon && Record.GetCategories(i).Type == ArtifactType.MagicWeapon) ||
									(Record.GetCategories(h).Type == ArtifactType.MagicWeapon && Record.GetCategories(i).Type == ArtifactType.Weapon) ||
									((Record.GetCategories(h).Type == ArtifactType.InContainer || Record.GetCategories(h).Type == ArtifactType.OnContainer || Record.GetCategories(h).Type == ArtifactType.UnderContainer || Record.GetCategories(h).Type == ArtifactType.BehindContainer) && Record.GetCategories(i).Type == ArtifactType.DoorGate) ||
									(Record.GetCategories(h).Type == ArtifactType.DoorGate && (Record.GetCategories(i).Type == ArtifactType.InContainer || Record.GetCategories(i).Type == ArtifactType.OnContainer || Record.GetCategories(i).Type == ArtifactType.UnderContainer || Record.GetCategories(i).Type == ArtifactType.BehindContainer)) ||
									(Record.GetCategories(h).Type == ArtifactType.BoundMonster && Record.GetCategories(i).Type == ArtifactType.DisguisedMonster) ||
									(Record.GetCategories(h).Type == ArtifactType.DisguisedMonster && Record.GetCategories(i).Type == ArtifactType.BoundMonster))
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
				result = Record.GetCategories(i).Type == ArtifactType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField1()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				switch (Record.GetCategories(i).Type)
				{
					case ArtifactType.Weapon:
					case ArtifactType.MagicWeapon:

						result = Record.GetCategories(i).Field1 >= -50 && Record.GetCategories(i).Field1 <= 50;

						break;

					case ArtifactType.InContainer:

						result = Record.GetCategories(i).Field1 >= -2;         // -2=Broken

						break;

					case ArtifactType.LightSource:

						result = Record.GetCategories(i).Field1 >= -1;

						break;

					case ArtifactType.Readable:
					case ArtifactType.BoundMonster:
					case ArtifactType.DisguisedMonster:

						result = Record.GetCategories(i).Field1 > 0;

						break;

					case ArtifactType.Wearable:

						result = gEngine.IsValidArtifactArmor(Record.GetCategories(i).Field1);

						break;

					case ArtifactType.DeadBody:

						result = Record.GetCategories(i).Field1 >= 0 && Record.GetCategories(i).Field1 <= 1;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetCategories(i).Field1 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField2()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				switch (Record.GetCategories(i).Type)
				{
					case ArtifactType.Weapon:
					case ArtifactType.MagicWeapon:

						result = Enum.IsDefined(typeof(Weapon), Record.GetCategories(i).Field2);

						break;

					case ArtifactType.InContainer:

						result = (Record.GetCategories(i).Field2 >= 0 && Record.GetCategories(i).Field2 <= 1) || gEngine.IsArtifactFieldStrength(Record.GetCategories(i).Field2);

						break;

					case ArtifactType.Drinkable:
					case ArtifactType.Edible:

						result = Record.GetCategories(i).Field2 >= 0;

						break;

					case ArtifactType.Readable:
					case ArtifactType.DisguisedMonster:

						result = Record.GetCategories(i).Field2 > 0;

						break;

					case ArtifactType.DoorGate:

						result = Record.GetCategories(i).Field2 >= -2;         // -2=Broken

						break;

					case ArtifactType.BoundMonster:

						result = Record.GetCategories(i).Field2 >= -1;

						break;

					case ArtifactType.Wearable:

						result = Enum.IsDefined(typeof(Clothing), Record.GetCategories(i).Field2);

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetCategories(i).Field2 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField3()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				switch (Record.GetCategories(i).Type)
				{
					case ArtifactType.Weapon:
					case ArtifactType.MagicWeapon:

						result = Record.GetCategories(i).Field3 >= 1 && Record.GetCategories(i).Field3 <= 25;

						break;

					case ArtifactType.InContainer:
					case ArtifactType.OnContainer:
					case ArtifactType.UnderContainer:
					case ArtifactType.BehindContainer:
					case ArtifactType.BoundMonster:

						result = Record.GetCategories(i).Field3 >= 0;

						break;

					case ArtifactType.Drinkable:
					case ArtifactType.Edible:
					case ArtifactType.Readable:

						result = Record.GetCategories(i).Field3 >= 0 && Record.GetCategories(i).Field3 <= 1;

						break;

					case ArtifactType.DoorGate:

						result = (Record.GetCategories(i).Field3 >= 0 && Record.GetCategories(i).Field3 <= 1) || gEngine.IsArtifactFieldStrength(Record.GetCategories(i).Field3);

						break;

					case ArtifactType.DisguisedMonster:

						result = Record.GetCategories(i).Field3 > 0;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetCategories(i).Field3 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField4()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				switch (Record.GetCategories(i).Type)
				{
					case ArtifactType.Weapon:
					case ArtifactType.MagicWeapon:

						result = Record.GetCategories(i).Field4 >= 1 && Record.GetCategories(i).Field4 <= 25;

						break;

					case ArtifactType.InContainer:
					case ArtifactType.OnContainer:
					case ArtifactType.UnderContainer:
					case ArtifactType.BehindContainer:

						result = Record.GetCategories(i).Field4 >= 0;

						break;

					case ArtifactType.DoorGate:

						result = Record.GetCategories(i).Field4 >= 0 && Record.GetCategories(i).Field4 <= 1;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetCategories(i).Field4 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCategoriesField5()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				switch (Record.GetCategories(i).Type)
				{
					case ArtifactType.Weapon:
					case ArtifactType.MagicWeapon:

						if (Record.GetCategories(i).Field5 == 0)	// auto-upgrade old weapons
						{
							Record.GetCategories(i).Field5 = Record.GetCategories(i).Field2 == (long)Weapon.Bow ? 2 : 1;
						}

						result = Record.GetCategories(i).Field5 >= 1 && Record.GetCategories(i).Field5 <= 2;

						break;

					case ArtifactType.InContainer:
					case ArtifactType.OnContainer:
					case ArtifactType.UnderContainer:
					case ArtifactType.BehindContainer:

						result = Enum.IsDefined(typeof(ContainerDisplayCode), Record.GetCategories(i).Field5);

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetCategories(i).Field5 == 0;
			}

			return result;
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
		public virtual bool ValidateInterdependenciesPluralType()
		{
			var result = true;

			var effectUid = gEngine.GetPluralTypeEffectUid(Record.PluralType);

			if (effectUid > 0)
			{
				var effect = gEDB[effectUid];

				if (effect == null)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("PluralType"), "Effect", effectUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IEffect);

					NewRecordUid = effectUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesLocation()
		{
			var result = true;

			var monUid = Record.GetWornByMonsterUid();

			if (monUid > 0)
			{
				var monster = gMDB[monUid];

				if (monster == null)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "Monster", monUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IMonster);

					NewRecordUid = monUid;

					goto Cleanup;
				}
			}
			else
			{
				monUid = Record.GetCarriedByMonsterUid();

				if (monUid > 0)
				{
					var monster = gMDB[monUid];

					if (monster == null)
					{
						result = false;

						Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "Monster", monUid, "which doesn't exist");

						ErrorMessage = Buf.ToString();

						RecordType = typeof(IMonster);

						NewRecordUid = monUid;

						goto Cleanup;
					}
				}
				else
				{
					var roomUid = Record.GetInRoomUid();

					if (roomUid > 0)
					{
						var room = gRDB[roomUid];

						if (room == null)
						{
							result = false;

							Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "Room", roomUid, "which doesn't exist");

							ErrorMessage = Buf.ToString();

							RecordType = typeof(IRoom);

							NewRecordUid = roomUid;

							goto Cleanup;
						}
					}
					else
					{
						roomUid = Record.GetEmbeddedInRoomUid();

						if (roomUid > 0)
						{
							var room = gRDB[roomUid];

							if (room == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "Room", roomUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IRoom);

								NewRecordUid = roomUid;

								goto Cleanup;
							}
						}
						else
						{
							var containerType = Record.GetCarriedByContainerContainerType();

							var artUid = Enum.IsDefined(typeof(ContainerType), containerType) ? Record.GetCarriedByContainerUid() : 0;

							if (artUid > 0)
							{
								var artifact = gADB[artUid];

								if (artifact == null)
								{
									result = false;

									Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "Artifact", artUid, "which doesn't exist");

									ErrorMessage = Buf.ToString();

									RecordType = typeof(IArtifact);

									NewRecordUid = artUid;

									goto Cleanup;
								}
								else if (artifact.GetArtifactCategory(gEngine.EvalContainerType(containerType, ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer)) == null)
								{
									result = false;

									Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "Artifact", artUid, string.Format("which should be {0}, but isn't", gEngine.EvalContainerType(containerType, "an In container", "an On container", "an Under container", "a Behind container")));

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

										Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "Artifact", artUid, "which produces a cyclic graph");

										ErrorMessage = Buf.ToString();

										RecordType = typeof(IArtifact);

										EditRecord = artifact;

										goto Cleanup;
									}
								}
							}
						}
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
								ValidateFieldInterdependencies("CategoriesField5");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField1()
		{
			var result = true;

			var i = Index;

			Debug.Assert(i >= 0 && i < Record.Categories.Length);

			if (Record.GetCategories(i).Type != ArtifactType.None)
			{
				switch (Record.GetCategories(i).Type)
				{
					case ArtifactType.InContainer:
					{
						var artUid = Record.GetCategories(i).Field1;

						if (artUid > 0)
						{
							var artifact = gADB[artUid];

							if (artifact == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "Artifact", artUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IArtifact);

								NewRecordUid = artUid;

								goto Cleanup;
							}
						}

						break;
					}

					case ArtifactType.Readable:
					{
						var effectUid = Record.GetCategories(i).Field1;

						if (effectUid > 0)
						{
							var effect = gEDB[effectUid];

							if (effect == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "Effect", effectUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IEffect);

								NewRecordUid = effectUid;

								goto Cleanup;
							}
						}

						break;
					}

					case ArtifactType.DoorGate:
					{
						var roomUid = Record.GetCategories(i).Field1;

						if (roomUid > 0)
						{
							var room = gRDB[roomUid];

							if (room == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "Room", roomUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IRoom);

								NewRecordUid = roomUid;

								goto Cleanup;
							}
						}

						break;
					}

					case ArtifactType.BoundMonster:
					{
						var monUid = Record.GetCategories(i).Field1;

						if (monUid > 0)
						{
							var monster = gMDB[monUid];

							if (monster == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "Monster", monUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IMonster);

								NewRecordUid = monUid;

								goto Cleanup;
							}
						}

						break;
					}

					case ArtifactType.DisguisedMonster:
					{
						var monUid = Record.GetCategories(i).Field1;

						if (monUid > 0)
						{
							var monster = gMDB[monUid];

							if (monster == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "Monster", monUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IMonster);

								NewRecordUid = monUid;

								goto Cleanup;
							}
						}

						break;
					}

					default:
					{
						// do nothing

						break;
					}
				}
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField2()
		{
			var result = true;

			var i = Index;

			Debug.Assert(i >= 0 && i < Record.Categories.Length);

			if (Record.GetCategories(i).Type != ArtifactType.None)
			{
				switch (Record.GetCategories(i).Type)
				{
					case ArtifactType.Readable:
					{
						var effectUid = Record.GetCategories(i).Field1;

						if (effectUid > 0)
						{
							effectUid++;

							for (var j = 1; j < Record.GetCategories(i).Field2; j++, effectUid++)
							{
								var effect = gEDB[effectUid];

								if (effect == null)
								{
									result = false;

									Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "Effect", effectUid, "which doesn't exist");

									ErrorMessage = Buf.ToString();

									RecordType = typeof(IEffect);

									NewRecordUid = effectUid;

									goto Cleanup;
								}
							}
						}

						break;
					}

					case ArtifactType.DoorGate:
					{
						var artUid = Record.GetCategories(i).Field2;

						if (artUid > 0)
						{
							var artifact = gADB[artUid];

							if (artifact == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "Artifact", artUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IArtifact);

								NewRecordUid = artUid;

								goto Cleanup;
							}
						}

						break;
					}

					case ArtifactType.BoundMonster:
					{
						var artUid = Record.GetCategories(i).Field2;

						if (artUid > 0)
						{
							var artifact = gADB[artUid];

							if (artifact == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "Artifact", artUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IArtifact);

								NewRecordUid = artUid;

								goto Cleanup;
							}
						}

						break;
					}

					case ArtifactType.DisguisedMonster:
					{
						var effectUid = Record.GetCategories(i).Field2;

						if (effectUid > 0)
						{
							var effect = gEDB[effectUid];

							if (effect == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "Effect", effectUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IEffect);

								NewRecordUid = effectUid;

								goto Cleanup;
							}
						}

						break;
					}

					default:
					{
						// do nothing

						break;
					}
				}
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesCategoriesField3()
		{
			var result = true;

			var i = Index;

			Debug.Assert(i >= 0 && i < Record.Categories.Length);

			if (Record.GetCategories(i).Type != ArtifactType.None)
			{
				switch (Record.GetCategories(i).Type)
				{
					case ArtifactType.BoundMonster:
					{
						var monUid = Record.GetCategories(i).Field3;

						if (monUid > 0)
						{
							var monster = gMDB[monUid];

							if (monster == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField3"), "Monster", monUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IMonster);

								NewRecordUid = monUid;

								goto Cleanup;
							}
						}

						break;
					}

					case ArtifactType.DisguisedMonster:
					{
						var effectUid = Record.GetCategories(i).Field2;

						if (effectUid > 0)
						{
							effectUid++;

							for (var j = 1; j < Record.GetCategories(i).Field3; j++, effectUid++)
							{
								var effect = gEDB[effectUid];

								if (effect == null)
								{
									result = false;

									Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField3"), "Effect", effectUid, "which doesn't exist");

									ErrorMessage = Buf.ToString();

									RecordType = typeof(IEffect);

									NewRecordUid = effectUid;

									goto Cleanup;
								}
							}
						}

						break;
					}

					default:
					{
						// do nothing

						break;
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

			var briefDesc = string.Format("{0}-{1}=Valid value", Constants.MinGoldValue, Constants.MaxGoldValue);

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

			var briefDesc = "(-1000 - N)=Worn by Monster Uid N; -999=Worn by player; (-N - 1)=Carried by Monster Uid N; -1=Carried by player; 0=Limbo; 1-1000=Room Uid; (1000 + N)=Inside Artifact Uid N; (2000 + N)=On Artifact Uid N; (3000 + N)=Under Artifact Uid N; (4000 + N)=Behind Artifact Uid N; (5000 + N)=Embedded in Room Uid N";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesType()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			fullDesc.Append("Enter the type of the Artifact.");

			var artTypeValues = EnumUtil.GetValues<ArtifactType>(at => at != ArtifactType.None);

			for (var j = 0; j < artTypeValues.Count; j++)
			{
				var artType = gEngine.GetArtifactTypes(artTypeValues[j]);

				Debug.Assert(artType != null);

				briefDesc.AppendFormat("{0}{1}={2}", i > 0 && j == 0 ? "-1=None; " : j != 0 ? "; " : "", (long)artTypeValues[j], artType.Name);
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField1()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetCategories(i).Type)
			{
				case ArtifactType.Weapon:
				case ArtifactType.MagicWeapon:

					fullDesc.Append("Enter the Artifact's weapon complexity.");

					briefDesc.Append("-50-50=Valid value");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.InContainer:

					fullDesc.Append("Enter the Artifact Uid of the key used to lock/unlock the container.");

					briefDesc.Append("-1=Can't be unlocked/opened normally; 0=No key; (GT 0)=Key Artifact Uid");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.LightSource:

					fullDesc.Append("Enter the number of rounds before the light source is exhausted/goes out.");

					briefDesc.Append("-1=Never runs out; (GE 0)=Valid value");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.Drinkable:
				case ArtifactType.Edible:

					fullDesc.Append("Enter the number of hits healed (or inflicted, if negative) for the Artifact.");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);

					break;

				case ArtifactType.Readable:

					fullDesc.Append("Enter the Uid of the first Effect displayed when the Artifact is read.");

					briefDesc.Append("(GT 0)=Effect Uid");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.DoorGate:

					fullDesc.Append("Enter the Uid of the Room on the opposite side of the door/gate.");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);

					break;

				case ArtifactType.BoundMonster:

					fullDesc.Append("Enter the Uid of the Monster that is bound.");

					briefDesc.Append("(GT 0)=Bound Monster Uid");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.Wearable:

					fullDesc.Append("Enter the armor class of the Artifact.");

					var armorValues = EnumUtil.GetValues<Armor>(a => a == Armor.ClothesShield || ((long)a) % 2 == 0);

					for (var j = 0; j < armorValues.Count; j++)
					{
						var armor = gEngine.GetArmors(armorValues[j]);

						Debug.Assert(armor != null);

						briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)armorValues[j], armor.Name);
					}

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.DisguisedMonster:

					fullDesc.Append("Enter the Uid of the Monster that is disguised.");

					briefDesc.Append("(GT 0)=Disguised Monster Uid");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.DeadBody:

					fullDesc.AppendFormat("Enter whether the Artifact is takeable.{0}{0}Typically, dead bodies should not be takeable unless it serves some useful purpose.", Environment.NewLine);

					briefDesc.Append("0=Not takeable; 1=Takeable");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField2()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetCategories(i).Type)
			{
				case ArtifactType.Weapon:
				case ArtifactType.MagicWeapon:

					fullDesc.Append("Enter the Artifact's weapon type.");

					var weaponValues = EnumUtil.GetValues<Weapon>();

					for (var j = 0; j < weaponValues.Count; j++)
					{
						var weapon = gEngine.GetWeapons(weaponValues[j]);

						Debug.Assert(weapon != null);

						briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)weaponValues[j], weapon.Name);
					}

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.InContainer:

					fullDesc.AppendFormat("Enter whether the Artifact is open/closed.{0}{0}Additionally, you can specify that the container must be forced open.", Environment.NewLine);

					briefDesc.Append("0=Closed; 1=Open; (1000 + N)=Forced open with N hits damage");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.Drinkable:
				case ArtifactType.Edible:

					fullDesc.Append("Enter the number of times the Artifact can be used.");

					briefDesc.Append("(GTE 0)=Valid value");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.Readable:

					fullDesc.Append("Enter the number of sequential Effects displayed when the Artifact is read.");

					briefDesc.Append("(GT 0)=Valid value");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.DoorGate:

					fullDesc.Append("Enter the Artifact Uid of the key used to lock/unlock the door/gate.");

					briefDesc.Append("-1=Can't be unlocked/opened normally; 0=No key; (GT 0)=Key Artifact Uid");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.BoundMonster:

					fullDesc.Append("Enter the Artifact Uid of the key used to lock/unlock the bound Monster.");

					briefDesc.Append("-1=Can't be unlocked/opened normally; 0=No key; (GT 0)=Key Artifact Uid");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.Wearable:

					fullDesc.Append("Enter the clothing type of the Artifact.");

					var clothingValues = EnumUtil.GetValues<Clothing>();

					for (var j = 0; j < clothingValues.Count; j++)
					{
						briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)clothingValues[j], gEngine.GetClothingNames(clothingValues[j]));
					}

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.DisguisedMonster:

					fullDesc.Append("Enter the Uid of the first Effect displayed when the disguised Monster is revealed.");

					briefDesc.Append("(GT 0)=Effect Uid");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField3()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetCategories(i).Type)
			{
				case ArtifactType.Weapon:
				case ArtifactType.MagicWeapon:

					fullDesc.Append("Enter the Artifact's weapon hit dice.");

					briefDesc.Append("1-25=Valid value");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.InContainer:
				case ArtifactType.OnContainer:
				case ArtifactType.UnderContainer:
				case ArtifactType.BehindContainer:

					var containerType = gEngine.GetContainerType(Record.GetCategories(i).Type);

					fullDesc.AppendFormat("Enter the maximum weight allowed in the Artifact's container content list.{0}{0}This is the total combined weight of all Artifacts immediately {1} the container (not including their contents).", Environment.NewLine, gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"));

					briefDesc.Append("(GE 0)=Valid value");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.Drinkable:
				case ArtifactType.Edible:
				case ArtifactType.Readable:

					fullDesc.Append("Enter whether the Artifact is open/closed.");

					briefDesc.Append("0=Closed; 1=Open");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.DoorGate:

					fullDesc.AppendFormat("Enter whether the Artifact is open/closed.{0}{0}Additionally, you can specify that the door/gate must be forced open.", Environment.NewLine);

					briefDesc.Append("0=Open; 1=Closed; (1000 + N)=Forced open with N hits damage");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.BoundMonster:

					fullDesc.Append("Enter the Uid of the Monster that is guarding the bound Monster.");

					briefDesc.Append("0=No guard; (GT 0)=Guard Monster Uid");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.DisguisedMonster:

					fullDesc.Append("Enter the number of sequential Effects displayed when the disguised Monster is revealed.");

					briefDesc.Append("(GT 0)=Valid value");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField4()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetCategories(i).Type)
			{
				case ArtifactType.Weapon:
				case ArtifactType.MagicWeapon:

					fullDesc.Append("Enter the Artifact's weapon hit dice sides.");

					briefDesc.Append("1-25=Valid value");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.InContainer:
				case ArtifactType.OnContainer:
				case ArtifactType.UnderContainer:
				case ArtifactType.BehindContainer:

					var containerType = gEngine.GetContainerType(Record.GetCategories(i).Type);

					fullDesc.AppendFormat("Enter the maximum number of items allowed in the Artifact's container content list.{0}{0}Additionally, you can specify that the player can't put anything {1} the container.", Environment.NewLine, gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"));

					briefDesc.AppendFormat("0=Player can't put anything {0}; (GT 0)=Valid value", gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"));

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.DoorGate:

					fullDesc.Append("Enter whether the Artifact is normal/hidden.");

					briefDesc.Append("0=Normal; 1=Hidden");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		/// <summary></summary>
		public virtual void PrintDescCategoriesField5()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetCategories(i).Type)
			{
				case ArtifactType.Weapon:
				case ArtifactType.MagicWeapon:

					fullDesc.Append("Enter the Artifact's weapon number of hands required.");

					briefDesc.Append("1-2=Valid value");

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case ArtifactType.InContainer:
				case ArtifactType.OnContainer:
				case ArtifactType.UnderContainer:
				case ArtifactType.BehindContainer:

					fullDesc.Append("Enter the display code that describes how to show the Artifact's container content list.");

					var containerDisplayCodeValues = EnumUtil.GetValues<ContainerDisplayCode>();

					for (var j = 0; j < containerDisplayCodeValues.Count; j++)
					{
						briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)containerDisplayCodeValues[j], gEngine.GetContainerDisplayCodeDescs(containerDisplayCodeValues[j]));
					}

					gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
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
						Buf01.Append(effect.Desc.Length > Constants.ArtNameLen - 6 ? effect.Desc.Substring(0, Constants.ArtNameLen - 9) + "..." : effect.Desc);

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
		public virtual void ListCategories()
		{
			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				ListField("CategoriesType");
				ListField("CategoriesField1");
				ListField("CategoriesField2");
				ListField("CategoriesField3");
				ListField("CategoriesField4");
				ListField("CategoriesField5");
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		public virtual void ListCategoriesType()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || i == 0 || Record.GetCategories(i - 1).Type != ArtifactType.None)
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
						gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesType"), null), (long)Record.GetCategories(i).Type);
					}
				}
			}
		}

		/// <summary></summary>
		public virtual void ListCategoriesField1()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategories(i).Type != ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField1"), null),
							BuildValue(51, ' ', 8, "CategoriesField1"));
					}
					else
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField1"), null), Record.GetCategories(i).Field1);
					}
				}
			}
		}

		/// <summary></summary>
		public virtual void ListCategoriesField2()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategories(i).Type != ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField2"), null),
							BuildValue(51, ' ', 8, "CategoriesField2"));
					}
					else
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField2"), null), Record.GetCategories(i).Field2);
					}
				}
			}
		}

		/// <summary></summary>
		public virtual void ListCategoriesField3()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategories(i).Type != ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField3"), null),
							BuildValue(51, ' ', 8, "CategoriesField3"));
					}
					else
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField3"), null), Record.GetCategories(i).Field3);
					}
				}
			}
		}

		/// <summary></summary>
		public virtual void ListCategoriesField4()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategories(i).Type != ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField4"), null),
							BuildValue(51, ' ', 8, "CategoriesField4"));
					}
					else
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField4"), null), Record.GetCategories(i).Field4);
					}
				}
			}
		}

		/// <summary></summary>
		public virtual void ListCategoriesField5()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategories(i).Type != ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField5"), null),
							BuildValue(51, ' ', 8, "CategoriesField5"));
					}
					else
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField5"), null), Record.GetCategories(i).Field5);
					}
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

				var rc = Globals.In.ReadField(Buf, Constants.ArtNameLen, null, '_', '\0', false, null, null, null, null);

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

				var rc = Globals.In.ReadField(Buf, Constants.ArtStateDescLen, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				Record.StateDesc = Buf.Trim().ToString();

				if (ValidateField("StateDesc"))
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

				var rc = Globals.In.ReadField(Buf, Constants.ArtDescLen, null, '_', '\0', false, null, null, null, null);

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
		public virtual void InputIsCharOwned()
		{
			var fieldDesc = FieldDesc;

			var isCharOwned = Record.IsCharOwned;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isCharOwned));

				PrintFieldDesc("IsCharOwned", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("IsCharOwned"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.IsCharOwned = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsCharOwned"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
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

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.IsPlural = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsPlural"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
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

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.IsListed = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsListed"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
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

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.PluralType = (PluralType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("PluralType"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
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

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ArticleType = (ArticleType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("ArticleType"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", Globals.LineSep);
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

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "25", null, gEngine.IsCharPlusMinusDigit, null);

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

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputWeight()
		{
			var artType = EditRec ? gEngine.GetArtifactTypes(Record.GetCategories(0).Type) : null;

			Debug.Assert(!EditRec || artType != null);

			var fieldDesc = FieldDesc;

			var weight = Record.Weight;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", weight);

				PrintFieldDesc("Weight", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Weight"), artType != null ? artType.WeightEmptyVal : "15"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType != null ? artType.WeightEmptyVal : "15", null, gEngine.IsCharPlusMinusDigit, null);

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

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputLocation()
		{
			var artType = EditRec ? gEngine.GetArtifactTypes(Record.GetCategories(0).Type) : null;

			Debug.Assert(!EditRec || artType != null);

			var fieldDesc = FieldDesc;

			var location = Record.Location;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", location);

				PrintFieldDesc("Location", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Location"), artType != null ? artType.LocationEmptyVal : "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType != null ? artType.LocationEmptyVal : "0", null, gEngine.IsCharPlusMinusDigit, null);

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

			gOut.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		public virtual void InputCategories()
		{
			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				InputField("CategoriesType");
				InputField("CategoriesField1");
				InputField("CategoriesField2");
				InputField("CategoriesField3");
				InputField("CategoriesField4");
				InputField("CategoriesField5");
			}
		}

		/// <summary></summary>
		public virtual void InputCategoriesType()
		{
			var i = Index;

			if (i == 0 || Record.GetCategories(i - 1).Type != ArtifactType.None)
			{
				var fieldDesc = FieldDesc;

				var type = Record.GetCategories(i).Type;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", (long)type);

					PrintFieldDesc("CategoriesType", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesType"), i == 0 ? "1" : "-1"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, i == 0 ? "1" : "-1", null, i == 0 ? (Func<char, bool>)gEngine.IsCharDigit : gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Type = (ArtifactType)Convert.ToInt64(Buf.Trim().ToString());
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

				if (Record.GetCategories(i).Type != ArtifactType.None)
				{
					if (EditRec && Record.GetCategories(i).Type != type)
					{
						var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

						Debug.Assert(artType != null);

						Record.GetCategories(i).Field1 = Convert.ToInt64(artType.Field1EmptyVal);

						Record.GetCategories(i).Field2 = Convert.ToInt64(artType.Field2EmptyVal);

						Record.GetCategories(i).Field3 = Convert.ToInt64(artType.Field3EmptyVal);

						Record.GetCategories(i).Field4 = Convert.ToInt64(artType.Field4EmptyVal);

						Record.GetCategories(i).Field5 = Convert.ToInt64(artType.Field5EmptyVal);
					}
				}
				else
				{
					for (var k = i; k < Record.Categories.Length; k++)
					{
						Record.GetCategories(k).Type = ArtifactType.None;

						Record.GetCategories(k).Field1 = 0;

						Record.GetCategories(k).Field2 = 0;

						Record.GetCategories(k).Field3 = 0;

						Record.GetCategories(k).Field4 = 0;

						Record.GetCategories(k).Field5 = 0;
					}
				}

				gOut.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Type = ArtifactType.None;
			}
		}

		/// <summary></summary>
		public virtual void InputCategoriesField1()
		{
			var i = Index;

			if (Record.GetCategories(i).Type != ArtifactType.None)
			{
				var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = FieldDesc;

				var field1 = Record.GetCategories(i).Field1;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field1);

					PrintFieldDesc("CategoriesField1", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesField1"), artType.Field1EmptyVal));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field1EmptyVal, null, gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Field1 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesField1"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				gOut.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Field1 = 0;
			}
		}

		/// <summary></summary>
		public virtual void InputCategoriesField2()
		{
			var i = Index;

			if (Record.GetCategories(i).Type != ArtifactType.None)
			{
				var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = FieldDesc;

				var field2 = Record.GetCategories(i).Field2;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field2);

					PrintFieldDesc("CategoriesField2", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesField2"), artType.Field2EmptyVal));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field2EmptyVal, null, gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Field2 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesField2"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				gOut.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Field2 = 0;
			}
		}

		/// <summary></summary>
		public virtual void InputCategoriesField3()
		{
			var i = Index;

			if (Record.GetCategories(i).Type != ArtifactType.None)
			{
				var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = FieldDesc;

				var field3 = Record.GetCategories(i).Field3;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field3);

					PrintFieldDesc("CategoriesField3", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesField3"), artType.Field3EmptyVal));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field3EmptyVal, null, gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Field3 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesField3"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				gOut.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Field3 = 0;
			}
		}

		/// <summary></summary>
		public virtual void InputCategoriesField4()
		{
			var i = Index;

			if (Record.GetCategories(i).Type != ArtifactType.None)
			{
				var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = FieldDesc;

				var field4 = Record.GetCategories(i).Field4;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field4);

					PrintFieldDesc("CategoriesField4", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesField4"), artType.Field4EmptyVal));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field4EmptyVal, null, gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Field4 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesField4"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				gOut.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Field4 = 0;
			}
		}

		/// <summary></summary>
		public virtual void InputCategoriesField5()
		{
			var i = Index;

			if (Record.GetCategories(i).Type != ArtifactType.None)
			{
				var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = FieldDesc;

				var field5 = Record.GetCategories(i).Field5;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field5);

					PrintFieldDesc("CategoriesField5", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesField5"), artType.Field5EmptyVal));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field5EmptyVal, null, gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Field5 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesField5"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				gOut.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Field5 = 0;
			}
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
				lookupMsg = "Carried by Player Character";
			}
			else if (Record.IsWornByCharacter())
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

			var artType = gEngine.GetArtifactTypes(Record.GetCategories(i).Type);

			Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, (long)Record.GetCategories(i).Type, null, artType != null ? artType.Name : "None"));

			return Buf01.ToString();
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField1()
		{
			var i = Index;

			switch (Record.GetCategories(i).Type)
			{
				case ArtifactType.Weapon:
				case ArtifactType.MagicWeapon:

					var stringVal = string.Format("{0}%", Record.GetCategories(i).Field1);

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, 0, stringVal, null));

					break;

				case ArtifactType.InContainer:

					if (Record.GetCategories(i).Field1 > 0)
					{
						var artifact = gADB[Record.GetCategories(i).Field1];

						Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, artifact != null ? gEngine.Capitalize(artifact.Name) : gEngine.UnknownName));
					}
					else
					{
						Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, null));
					}

					break;

				case ArtifactType.BoundMonster:
				case ArtifactType.DisguisedMonster:

					var monster = gMDB[Record.GetCategories(i).Field1];

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, monster != null ? gEngine.Capitalize(monster.Name) : gEngine.UnknownName));

					break;

				case ArtifactType.DoorGate:

					if (Record.GetCategories(i).Field1 > 0)
					{
						var room = gRDB[Record.GetCategories(i).Field1];

						Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, room != null ? gEngine.Capitalize(room.Name) : gEngine.UnknownName));
					}
					else
					{
						Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, null));
					}

					break;

				case ArtifactType.Wearable:

					var armor = gEngine.GetArmors((Armor)Record.GetCategories(i).Field1);

					Debug.Assert(armor != null);

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, armor.Name));

					break;

				case ArtifactType.DeadBody:

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, Record.GetCategories(i).Field1 == 1 ? "Takeable" : "Not Takeable"));

					break;

				default:

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, null));

					break;
			}

			return Buf01.ToString();
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField2()
		{
			var i = Index;

			switch (Record.GetCategories(i).Type)
			{
				case ArtifactType.Weapon:
				case ArtifactType.MagicWeapon:

					var weapon = gEngine.GetWeapons((Weapon)Record.GetCategories(i).Field2);

					Debug.Assert(weapon != null);

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, weapon.Name));

					break;

				case ArtifactType.InContainer:

					var lookupMsg = string.Empty;

					if (Record.IsFieldStrength(Record.GetCategories(i).Field2))
					{
						lookupMsg = string.Format("Strength of {0}", Record.GetFieldStrength(Record.GetCategories(i).Field2));
					}

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, Record.IsFieldStrength(Record.GetCategories(i).Field2) ? lookupMsg : Record.GetCategories(i).Field2 == 1 ? "Open" : "Closed"));

					break;

				case ArtifactType.BoundMonster:
				case ArtifactType.DoorGate:

					if (Record.GetCategories(i).Field2 > 0)
					{
						var artifact = gADB[Record.GetCategories(i).Field2];

						Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, artifact != null ? gEngine.Capitalize(artifact.Name) : gEngine.UnknownName));
					}
					else
					{
						Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, null));
					}

					break;

				case ArtifactType.Wearable:

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, gEngine.GetClothingNames((Clothing)Record.GetCategories(i).Field2)));

					break;

				default:

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, null));

					break;
			}

			return Buf01.ToString();
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField3()
		{
			var i = Index;

			switch (Record.GetCategories(i).Type)
			{
				case ArtifactType.Drinkable:
				case ArtifactType.Readable:
				case ArtifactType.Edible:

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field3, null, Record.GetCategories(i).IsOpen() ? "Open" : "Closed"));

					break;

				case ArtifactType.BoundMonster:

					if (Record.GetCategories(i).Field3 > 0)
					{
						var monster = gMDB[Record.GetCategories(i).Field3];

						Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field3, null, monster != null ? gEngine.Capitalize(monster.Name) : gEngine.UnknownName));
					}
					else
					{
						Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field3, null, null));
					}

					break;

				case ArtifactType.DoorGate:

					var lookupMsg = string.Empty;

					if (Record.IsFieldStrength(Record.GetCategories(i).Field3))
					{
						lookupMsg = string.Format("Strength of {0}", Record.GetFieldStrength(Record.GetCategories(i).Field3));
					}

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field3, null, Record.IsFieldStrength(Record.GetCategories(i).Field3) ? lookupMsg : Record.GetCategories(i).IsOpen() ? "Open" : "Closed"));

					break;

				default:

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field3, null, null));

					break;
			}

			return Buf01.ToString();
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField4()
		{
			var i = Index;

			switch (Record.GetCategories(i).Type)
			{
				case ArtifactType.DoorGate:

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field4, null, Record.GetCategories(i).Field4 == 1 ? "Hidden" : "Normal"));

					break;

				default:

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field4, null, null));

					break;
			}

			return Buf01.ToString();
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string BuildValueCategoriesField5()
		{
			var i = Index;

			switch (Record.GetCategories(i).Type)
			{
				case ArtifactType.InContainer:
				case ArtifactType.OnContainer:
				case ArtifactType.UnderContainer:
				case ArtifactType.BehindContainer:

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field5, null, gEngine.GetContainerDisplayCodeDescs((ContainerDisplayCode)Record.GetCategories(i).Field5)));

					break;

				default:

					Buf01.Append(gEngine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field5, null, null));

					break;
			}

			return Buf01.ToString();
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
				Record.Uid = Globals.Database.GetArtifactUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		public ArtifactHelper()
		{
			FieldNameList = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Name",
				"StateDesc",
				"Desc",
				"Seen",
				"IsCharOwned",
				"IsPlural",
				"IsListed",
				"PluralType",
				"ArticleType",
				"Value",
				"Weight",
				"Location",
				"Categories",
			};
		}

		#endregion

		#endregion
	}
}
