
// IHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Helpers.Generic
{
	/// <summary></summary>
	public interface IHelper<T> where T : class, IGameBase
	{
		#region Properties

		/// <summary></summary>
		T Record { get; set; }

		/// <summary></summary>
		long Index { get; set; }

		/// <summary></summary>
		StringBuilder Buf { get; set; }

		/// <summary></summary>
		StringBuilder Buf01 { get; set; }

		/// <summary></summary>
		bool EditRec { get; set; }

		/// <summary></summary>
		bool EditField { get; set; }

		/// <summary></summary>
		bool ShowDesc { get; set; }

		/// <summary></summary>
		FieldDesc FieldDesc { get; set; }

		/// <summary></summary>
		long BufSize { get; set; }

		/// <summary></summary>
		char FillChar { get; set; }

		/// <summary></summary>
		long Offset { get; set; }

		/// <summary></summary>
		string ErrorFieldName { get; set; }

		/// <summary></summary>
		string ErrorMessage { get; set; }

		/// <summary></summary>
		Type RecordType { get; set; }

		/// <summary></summary>
		IGameBase EditRecord { get; set; }

		/// <summary></summary>
		long NewRecordUid { get; set; }

		/// <summary></summary>
		bool FullDetail { get; set; }

		/// <summary></summary>
		bool ResolveEffects { get; set; }

		/// <summary></summary>
		bool LookupMsg { get; set; }

		/// <summary></summary>
		bool NumberFields { get; set; }

		/// <summary></summary>
		bool ExcludeROFields { get; set; }

		/// <summary></summary>
		bool AddToListedNames { get; set; }

		/// <summary></summary>
		long ListNum { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="name"></param>
		/// <returns></returns>
		string GetFieldName(string name);

		/// <summary></summary>
		/// <param name="listNum"></param>
		/// <returns></returns>
		string GetFieldName(long listNum);

		/// <summary></summary>
		/// <param name="matchFunc"></param>
		/// <returns></returns>
		IList<string> GetNameList(Func<string, bool> matchFunc = null);

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		string GetPrintedName(string fieldName);

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		string GetName(string fieldName, bool addToNameList = false);

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		object GetValue(string fieldName);

		/// <summary></summary>
		/// <returns></returns>
		bool ValidateRecord();

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		bool ValidateField(string fieldName);

		/// <summary></summary>
		/// <returns></returns>
		bool ValidateRecordAfterDatabaseLoaded();

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		bool ValidateFieldAfterDatabaseLoaded(string fieldName);

		/// <summary></summary>
		/// <returns></returns>
		bool ValidateRecordInterdependencies();

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		bool ValidateFieldInterdependencies(string fieldName);

		/// <summary></summary>
		/// <param name="fieldName"></param>
		/// <param name="editRec"></param>
		/// <param name="editField"></param>
		/// <param name="fieldDesc"></param>
		void PrintFieldDesc(string fieldName, bool editRec, bool editField, FieldDesc fieldDesc);

		/// <summary></summary>
		/// <param name="fieldName"></param>
		void PrintFieldDesc(string fieldName);

		/// <summary></summary>
		/// <param name="fullDetail"></param>
		/// <param name="showDesc"></param>
		/// <param name="resolveEffects"></param>
		/// <param name="lookupMsg"></param>
		/// <param name="numberFields"></param>
		/// <param name="excludeROFields"></param>
		void ListRecord(bool fullDetail, bool showDesc, bool resolveEffects, bool lookupMsg, bool numberFields, bool excludeROFields);

		/// <summary></summary>
		/// <param name="callClear"></param>
		void ListRecord(bool callClear = true);

		/// <summary></summary>
		/// <param name="fieldName"></param>
		void ListField(string fieldName);

		/// <summary></summary>
		void ListErrorField();

		/// <summary></summary>
		/// <param name="editRec"></param>
		/// <param name="fieldDesc"></param>
		void InputRecord(bool editRec, FieldDesc fieldDesc);

		/// <summary></summary>
		/// <param name="callClear"></param>
		void InputRecord(bool callClear = true);

		/// <summary></summary>
		/// <param name="fieldName"></param>
		void InputField(string fieldName);

		/// <summary></summary>
		void Clear();

		#endregion
	}
}
