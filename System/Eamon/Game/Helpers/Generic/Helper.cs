
// Helper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers.Generic
{
	public abstract class Helper<T> : IHelper<T> where T : class, IGameBase
	{
		#region Public Fields

		/// <summary></summary>
		public T _record;

		#endregion

		#region Public Properties

		public virtual T Record
		{
			get
			{
				return _record;
			}

			set
			{
				if (_record != value)
				{
					Clear();

					_record = value;
				}
			}
		}

		public virtual long Index { get; set; }

		public virtual StringBuilder Buf { get; set; }

		public virtual StringBuilder Buf01 { get; set; }

		public virtual bool EditRec { get; set; }

		public virtual bool EditField { get; set; }

		public virtual bool ShowDesc { get; set; }

		public virtual FieldDesc FieldDesc { get; set; }

		public virtual long BufSize { get; set; }

		public virtual char FillChar { get; set; }

		public virtual long Offset { get; set; }

		public virtual string ErrorFieldName { get; set; }

		public virtual string ErrorMessage { get; set; }

		public virtual Type RecordType { get; set; }

		public virtual IGameBase EditRecord { get; set; }

		public virtual long NewRecordUid { get; set; }

		public virtual bool FullDetail { get; set; }

		public virtual bool ResolveEffects { get; set; }

		public virtual bool LookupMsg { get; set; }

		public virtual bool NumberFields { get; set; }

		public virtual bool ExcludeROFields { get; set; }

		public virtual bool AddToListedNames { get; set; }

		public virtual long ListNum { get; set; }

		/// <summary></summary>
		public virtual IList<string> FieldNameList { get; set; }

		/// <summary></summary>
		public virtual IList<string> ListedNameList { get; set; }

		/// <summary></summary>
		public virtual IList<string> NameList { get; set; }

		#endregion

		#region Public Methods

		#region Interface IHelper

		public virtual string GetFieldName(string name)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(name));

			var result = string.Empty;

			var delims = new char[] { '[', ']', '.' };

			var tokens = name.Split(delims);

			if (tokens.Length > 1)
			{
				Index = long.Parse(tokens[1]);

				result = tokens[0] + tokens[3];
			}
			else
			{
				Index = -1;

				result = name;
			}

			return result;
		}

		public virtual string GetFieldName(long listNum)
		{
			return listNum >= 1 && listNum <= ListedNameList.Count ? GetFieldName(ListedNameList[(int)listNum - 1]) : null;
		}

		public virtual IList<string> GetNameList(Func<string, bool> matchFunc = null)
		{
			if (matchFunc == null)
			{
				matchFunc = n => true;
			}

			NameList.Clear();

			foreach (var fieldName in FieldNameList)
			{
				GetName(fieldName, true);
			}

			return NameList.Where(matchFunc).ToList();
		}

		public virtual string GetPrintedName(string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var result = fieldName;

			var methodName = string.Format("GetPrintedName{0}", fieldName);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				result = (string)methodInfo.Invoke(this, null);
			}

			return result;
		}

		public virtual string GetName(string fieldName, bool addToNameList = false)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var result = fieldName;

			var methodName = string.Format("GetName{0}", fieldName);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				result = (string)methodInfo.Invoke(this, new object[] { addToNameList });
			}
			else if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		public virtual object GetValue(string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var result = default(object);

			var methodName = string.Format("GetValue{0}", fieldName);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				result = methodInfo.Invoke(this, null);
			}
			else
			{
				var propInfo = Record.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

				if (propInfo != null)
				{
					result = propInfo.GetValue(Record);
				}
			}

			return result;
		}

		public virtual bool ValidateRecord()
		{
			Clear();

			var result = true;

			foreach (var fieldName in FieldNameList)
			{
				result = ValidateField(fieldName);

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		public virtual bool ValidateField(string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var result = true;

			var methodName = string.Format("Validate{0}", fieldName);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				result = (bool)methodInfo.Invoke(this, null);

				if (result == false && string.IsNullOrWhiteSpace(ErrorFieldName))
				{
					ErrorFieldName = fieldName;
				}
			}

			return result;
		}

		public virtual bool ValidateRecordAfterDatabaseLoaded()
		{
			Clear();

			var result = true;

			foreach (var fieldName in FieldNameList)
			{
				result = ValidateFieldAfterDatabaseLoaded(fieldName);

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		public virtual bool ValidateFieldAfterDatabaseLoaded(string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var result = true;

			var methodName = string.Format("ValidateAfterDatabaseLoaded{0}", fieldName);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				result = (bool)methodInfo.Invoke(this, null);

				if (result == false && string.IsNullOrWhiteSpace(ErrorFieldName))
				{
					ErrorFieldName = fieldName;
				}
			}

			return result;
		}

		public virtual bool ValidateRecordInterdependencies()
		{
			Clear();

			var result = true;

			foreach (var fieldName in FieldNameList)
			{
				result = ValidateFieldInterdependencies(fieldName);

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		public virtual bool ValidateFieldInterdependencies(string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var result = true;

			var methodName = string.Format("ValidateInterdependencies{0}", fieldName);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				result = (bool)methodInfo.Invoke(this, null);

				if (result == false && string.IsNullOrWhiteSpace(ErrorFieldName))
				{
					ErrorFieldName = fieldName;
				}
			}

			return result;
		}

		public virtual void PrintFieldDesc(string fieldName, bool editRec, bool editField, FieldDesc fieldDesc)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			Debug.Assert(Enum.IsDefined(typeof(FieldDesc), fieldDesc));

			var origEditRec = EditRec;

			var origEditField = EditField;

			var origFieldDesc = FieldDesc;

			EditRec = editRec;

			EditField = editField;

			FieldDesc = fieldDesc;

			PrintFieldDesc(fieldName);

			EditRec = origEditRec;

			EditField = origEditField;

			FieldDesc = origFieldDesc;
		}

		public virtual void PrintFieldDesc(string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var methodName = string.Format("PrintDesc{0}", fieldName);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				Buf01.Clear();

				methodInfo.Invoke(this, null);

				if (Buf01.Length > 0)
				{
					gOut.Write("{0}", Buf01);
				}
			}
		}

		public virtual void ListRecord(bool fullDetail, bool showDesc, bool resolveEffects, bool lookupMsg, bool numberFields, bool excludeROFields)
		{
			Clear();

			FullDetail = fullDetail;

			ShowDesc = showDesc;

			ResolveEffects = resolveEffects;

			LookupMsg = lookupMsg;

			NumberFields = numberFields;

			ExcludeROFields = excludeROFields;

			ListRecord(false);
		}

		public virtual void ListRecord(bool callClear = true)
		{
			if (callClear)
			{
				Clear();
			}

			foreach (var fieldName in FieldNameList)
			{
				ListField(fieldName);
			}
		}

		public virtual void ListField(string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var methodName = string.Format("List{0}", fieldName);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				var origAddToListedNames = AddToListedNames;

				var origListNum = ListNum;

				AddToListedNames = true;

				methodInfo.Invoke(this, null);

				if (AddToListedNames && origListNum + 1 == ListNum)
				{
					ListedNameList.Add(GetName(fieldName));
				}

				AddToListedNames = origAddToListedNames;
			}
		}

		public virtual void ListErrorField()
		{

		}

		public virtual void InputRecord(bool editRec, FieldDesc fieldDesc)
		{
			Debug.Assert(Enum.IsDefined(typeof(FieldDesc), fieldDesc));

			Clear();

			EditRec = editRec;

			FieldDesc = fieldDesc;

			InputRecord(false);
		}

		public virtual void InputRecord(bool callClear = true)
		{
			if (callClear)
			{
				Clear();
			}

			SetUidIfInvalid();

			foreach (var fieldName in FieldNameList)
			{
				InputField(fieldName);
			}
		}

		public virtual void InputField(string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var methodName = string.Format("Input{0}", fieldName);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				methodInfo.Invoke(this, null);
			}
		}

		public virtual void Clear()
		{
			ListedNameList.Clear();

			NameList.Clear();

			Index = -1;

			Buf.Clear();

			Buf01.Clear();

			EditRec = false;

			EditField = false;

			ShowDesc = false;

			FieldDesc = FieldDesc.None;

			BufSize = 0;

			FillChar = '\0';

			Offset = 0;

			ErrorFieldName = null;

			ErrorMessage = "";

			RecordType = null;

			EditRecord = null;

			NewRecordUid = 0;

			FullDetail = false;

			ResolveEffects = false;

			LookupMsg = false;

			NumberFields = false;

			ExcludeROFields = false;

			ListNum = 1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameIsUidRecycled()
		{
			return "Is Uid Recycled";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameDesc()
		{
			return "Description";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameArticleType()
		{
			return "Article Type";
		}

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public virtual string BuildValue(string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var result = "";

			var methodName = string.Format("BuildValue{0}", fieldName);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				Buf01.Clear();

				result = (string)methodInfo.Invoke(this, null);
			}

			return result;
		}

		#endregion

		#region Class Helper

		/// <summary></summary>
		public virtual void SetUidIfInvalid()
		{

		}

		public Helper()
		{
			Buf = new StringBuilder(Constants.BufSize);

			Buf01 = new StringBuilder(Constants.BufSize);

			FieldNameList = new List<string>();

			ListedNameList = new List<string>();

			NameList = new List<string>();

			Clear();
		}

		#endregion

		#endregion
	}
}
