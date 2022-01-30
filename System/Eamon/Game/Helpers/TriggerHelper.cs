﻿
// TriggerHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Helpers.Generic;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class TriggerHelper : Helper<ITrigger>, ITriggerHelper
	{
		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

		public override void ListErrorField()
		{
			// TODO: implement
		}

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameSortOrder()
		{
			return "Sort Order";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameField1()
		{
			var triggerType = gEngine.GetTriggerTypes(Record.Type);

			return string.Format("{0}", triggerType != null ? triggerType.Field1Name : "Field #1");
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameField2()
		{
			var triggerType = gEngine.GetTriggerTypes(Record.Type);

			return string.Format("{0}", triggerType != null ? triggerType.Field2Name : "Field #2");
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameField3()
		{
			var triggerType = gEngine.GetTriggerTypes(Record.Type);

			return string.Format("{0}", triggerType != null ? triggerType.Field3Name : "Field #3");
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameField4()
		{
			var triggerType = gEngine.GetTriggerTypes(Record.Type);

			return string.Format("{0}", triggerType != null ? triggerType.Field4Name : "Field #4");
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameField5()
		{
			var triggerType = gEngine.GetTriggerTypes(Record.Type);

			return string.Format("{0}", triggerType != null ? triggerType.Field5Name : "Field #5");
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
		public virtual bool ValidateOccurrences()
		{
			return Record.Occurrences >= -1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSortOrder()
		{
			return Record.SortOrder >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateType()
		{
			return gEngine.IsValidTriggerType(Record.Type);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateField1()
		{
			var result = true;

			switch (Record.Type)
			{
				case TriggerType.Interval:

					result = Record.Field1 >= 0;

					break;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateField2()
		{
			var result = true;

			switch (Record.Type)
			{
				case TriggerType.Interval:

					result = Record.Field2 >= 1;

					break;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateField3()
		{
			var result = true;

			switch (Record.Type)
			{
				case TriggerType.Interval:

					result = Record.Field3 >= 0 && Record.Field3 <= 100;

					break;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateField4()
		{
			var result = true;

			switch (Record.Type)
			{
				case TriggerType.Interval:

					// do nothing

					break;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateField5()
		{
			var result = true;

			switch (Record.Type)
			{
				case TriggerType.Interval:

					// do nothing

					break;
			}

			return result;
		}

		#endregion

		#region ValidateInterdependencies Methods

		#endregion

		#region PrintDesc Methods

		#endregion

		#region List Methods

		#endregion

		#region Input Methods

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class TriggerHelper

		public override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetTriggerUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		public TriggerHelper()
		{

		}

		#endregion

		#endregion
	}
}
