
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class GameStateHelper : Helper<IGameState>, IGameStateHelper
	{
		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

		#region GetPrintedName Methods

		// do nothing

		#endregion

		#region GetName Methods

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameNBTL(bool addToNameList)
		{
			var friendlinessValues = EnumUtil.GetValues<Friendliness>();

			foreach (var fv in friendlinessValues)
			{
				Index = (long)fv;

				GetName("NBTLElement", addToNameList);
			}

			return "NBTL";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameNBTLElement(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("NBTL[{0}].Element", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameDTTL(bool addToNameList)
		{
			if (Globals.IsRulesetVersion(5))
			{
				var friendlinessValues = EnumUtil.GetValues<Friendliness>();

				foreach (var fv in friendlinessValues)
				{
					Index = (long)fv;

					GetName("DTTLElement", addToNameList);
				}
			}

			return "DTTL";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameDTTLElement(bool addToNameList)
		{
			string result = string.Empty;

			if (Globals.IsRulesetVersion(5))
			{
				var i = Index;

				result = string.Format("DTTL[{0}].Element", i);

				if (addToNameList)
				{
					NameList.Add(result);
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameSa(bool addToNameList)
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				GetName("SaElement", addToNameList);
			}

			return "Sa";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameSaElement(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Sa[{0}].Element", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameImportedArtUids(bool addToNameList)
		{
			for (Index = 0; Index < Record.ImportedArtUids.Length; Index++)
			{
				GetName("ImportedArtUidsElement", addToNameList);
			}

			return "ImportedArtUids";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameImportedArtUidsElement(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("ImportedArtUids[{0}].Element", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameHeldWpnUids(bool addToNameList)
		{
			for (Index = 0; Index < Record.HeldWpnUids.Length; Index++)
			{
				GetName("HeldWpnUidsElement", addToNameList);
			}

			return "HeldWpnUids";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameHeldWpnUidsElement(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("HeldWpnUids[{0}].Element", i);

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
		public virtual object GetValueNBTLElement()
		{
			var i = Index;

			return Record.GetNBTL(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueDTTLElement()
		{
			var i = Index;

			return Record.GetDTTL(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueSaElement()
		{
			var i = Index;

			return Record.GetSa(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueImportedArtUidsElement()
		{
			var i = Index;

			return Record.GetImportedArtUids(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueHeldWpnUidsElement()
		{
			var i = Index;

			return Record.GetHeldWpnUids(i);
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
		public virtual bool ValidateAr()
		{
			return Record.Ar >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCm()
		{
			return Record.Cm > 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateLs()
		{
			return Record.Ls >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSh()
		{
			return Record.Sh >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSpeed()
		{
			return Record.Speed >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCurrTurn()
		{
			return Record.CurrTurn >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidatePauseCombatMs()
		{
			return Record.PauseCombatMs >= 0 && Record.PauseCombatMs <= 10000;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateImportedArtUidsIdx()
		{
			return Record.ImportedArtUidsIdx >= 0 && Record.ImportedArtUidsIdx <= Record.ImportedArtUids.Length;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateUsedWpnIdx()
		{
			return Record.UsedWpnIdx >= 0 && Record.UsedWpnIdx < Record.HeldWpnUids.Length;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNBTL()
		{
			var result = true;

			var friendlinessValues = EnumUtil.GetValues<Friendliness>();

			foreach (var fv in friendlinessValues)
			{
				Index = (long)fv;

				result = ValidateField("NBTLElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNBTLElement()
		{
			var i = Index;

			return Record.GetNBTL(i) >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateDTTL()
		{
			var result = true;

			if (Globals.IsRulesetVersion(5))
			{
				var friendlinessValues = EnumUtil.GetValues<Friendliness>();

				foreach (var fv in friendlinessValues)
				{
					Index = (long)fv;

					result = ValidateField("DTTLElement");

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
		public virtual bool ValidateDTTLElement()
		{
			var i = Index;

			return !Globals.IsRulesetVersion(5) || Record.GetDTTL(i) >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSa()
		{
			var result = true;

			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				result = ValidateField("SaElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSaElement()
		{
			var i = Index;

			var spell = gEngine.GetSpells((Spell)i);

			Debug.Assert(spell != null);

			return Record.GetSa(i) >= spell.MinValue && Record.GetSa(i) <= spell.MaxValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateImportedArtUids()
		{
			var result = true;

			for (Index = 0; Index < Record.ImportedArtUids.Length; Index++)
			{
				result = ValidateField("ImportedArtUidsElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateImportedArtUidsElement()
		{
			var i = Index;

			return Record.GetImportedArtUids(i) >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateHeldWpnUids()
		{
			var result = true;

			for (Index = 0; Index < Record.HeldWpnUids.Length; Index++)
			{
				result = ValidateField("HeldWpnUidsElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateHeldWpnUidsElement()
		{
			var i = Index;

			return Record.GetHeldWpnUids(i) >= 0;
		}

		#endregion

		#region ValidateInterdependencies Methods

		// do nothing

		#endregion

		#region PrintDesc Methods

		// do nothing

		#endregion

		#region List Methods

		// do nothing

		#endregion

		#region Input Methods

		// do nothing

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class GameStateHelper

		public override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetGameStateUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		public GameStateHelper()
		{
			FieldNameList = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Ar",
				"Cm",
				"Ls",
				"Ro",
				"R2",
				"R3",
				"Sh",
				"Af",
				"Die",
				"Speed",
				"Vr",
				"Vm",
				"Va",
				"MatureContent",
				"EnhancedParser",
				"ShowPronounChanges",
				"ShowFulfillMessages",
				"CurrTurn",
				"PauseCombatMs",
				"ImportedArtUidsIdx",
				"UsedWpnIdx",
				"NBTL",
				"DTTL",
				"Sa",
				"ImportedArtUids",
				"HeldWpnUids",
			};
		}

		#endregion

		#endregion
	}
}
