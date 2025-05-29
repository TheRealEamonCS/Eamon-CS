
// ObjectExtensions.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.Extensions
{
	public static class ObjectExtensions
	{
		private class ReferenceEqualityComparer : IEqualityComparer<object>
		{
			public static readonly ReferenceEqualityComparer Instance = new ReferenceEqualityComparer();

			public new bool Equals(object x, object y) => ReferenceEquals(x, y);

			public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
		}

		private static readonly BindingFlags PropertyBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

		private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> _propertyCache = new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

		private static Dictionary<string, PropertyInfo> GetCachedProperties(Type type)
		{
			return _propertyCache.GetOrAdd(type, t =>
			{
				var props = t.GetProperties(PropertyBindingFlags).Where(p => p.CanRead || p.CanWrite).ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

				return props;
			});
		}

		private static object CopyValue(object source, Type targetType, bool recurse, CopyContext copyContext)
		{
			if (source == null)
			{
				return null;
			}

			var sourceType = source.GetType();

			if (copyContext.TryGetCopiedObject(source, out var existingCopy))
			{
				return existingCopy;
			}

			if (sourceType.IsPrimitive || sourceType == typeof(string) || sourceType.IsValueType)
			{
				return source;
			}

			if (sourceType.IsArray)
			{
				var sourceArray = (Array)source;

				var elementType = sourceType.GetElementType();

				var newArray = Array.CreateInstance(elementType, sourceArray.Length);

				copyContext.RegisterCopy(source, newArray);

				for (int i = 0; i < sourceArray.Length; i++)
				{
					var element = sourceArray.GetValue(i);

					var copiedElement = recurse ? CopyValue(element, elementType, true, copyContext) : element;

					newArray.SetValue(copiedElement, i);
				}

				return newArray;
			}

			if (typeof(IList).IsAssignableFrom(sourceType))
			{
				var sourceList = (IList)source;

				var sourceGenericArgs = sourceType.GetGenericArguments();

				var elementType = sourceGenericArgs.Length > 0 ? sourceGenericArgs[0] : typeof(object);

				// Prefer the source's concrete type if it's a generic list and compatible with targetType
				Type targetListType;

				if (sourceType.IsGenericType && typeof(IList).IsAssignableFrom(sourceType) && (targetType.IsInterface || targetType.IsAssignableFrom(sourceType)))
				{
					targetListType = sourceType; // Preserve List, ObservableCollection, LinkedList, etc.
				}
				else
				{
					// Use targetType if concrete and compatible, otherwise fall back to List<>
					targetListType = targetType.IsInterface || !typeof(IList).IsAssignableFrom(targetType) ? typeof(List<>).MakeGenericType(elementType) : targetType;
				}

				var newList = (IList)Activator.CreateInstance(targetListType);

				copyContext.RegisterCopy(source, newList);

				foreach (var item in sourceList)
				{
					var copiedItem = recurse ? CopyValue(item, item?.GetType() ?? typeof(object), true, copyContext) : item;

					newList.Add(copiedItem);
				}

				return newList;
			}

			if (typeof(IDictionary).IsAssignableFrom(sourceType))
			{
				var sourceDict = (IDictionary)source;

				var sourceGenericArgs = sourceType.GetGenericArguments();

				var keyType = sourceGenericArgs.Length > 0 ? sourceGenericArgs[0] : typeof(object);

				var valueType = sourceGenericArgs.Length > 1 ? sourceGenericArgs[1] : typeof(object);

				// Prefer the source's concrete type if it's a generic dictionary and compatible with targetType
				Type targetDictType;

				if (sourceType.IsGenericType && typeof(IDictionary).IsAssignableFrom(sourceType) && (targetType.IsInterface || targetType.IsAssignableFrom(sourceType)))
				{
					targetDictType = sourceType; // Preserve SortedDictionary, Dictionary, etc.
				}
				else
				{
					// Use targetType if concrete and compatible, otherwise fall back to Dictionary<,>
					targetDictType = targetType.IsInterface || !typeof(IDictionary).IsAssignableFrom(targetType) ? typeof(Dictionary<,>).MakeGenericType(keyType, valueType) : targetType;
				}

				// Handle SortedDictionary with custom comparer if present
				IDictionary newDict;

				if (sourceType.GetGenericTypeDefinition() == typeof(SortedDictionary<,>))
				{
					var comparerProp = sourceType.GetProperty("Comparer", BindingFlags.NonPublic | BindingFlags.Instance);

					var comparer = comparerProp?.GetValue(sourceDict);

					if (comparer != null)
					{
						var constructor = targetDictType.GetConstructor(new[] { typeof(IComparer<>).MakeGenericType(keyType) });

						newDict = (IDictionary)constructor?.Invoke(new[] { comparer }) ?? (IDictionary)Activator.CreateInstance(targetDictType);
					}
					else
					{
						newDict = (IDictionary)Activator.CreateInstance(targetDictType);
					}
				}
				else
				{
					newDict = (IDictionary)Activator.CreateInstance(targetDictType);
				}

				copyContext.RegisterCopy(source, newDict);

				foreach (DictionaryEntry entry in sourceDict)
				{
					var copiedKey = recurse ? CopyValue(entry.Key, entry.Key?.GetType() ?? typeof(object), true, copyContext) : entry.Key;

					var copiedValue = recurse ? CopyValue(entry.Value, entry.Value?.GetType() ?? typeof(object), true, copyContext) : entry.Value;

					newDict.Add(copiedKey, copiedValue);
				}

				return newDict;
			}

			if (recurse)
			{
				object newObj = null;

				if (targetType.IsInterface)
				{
					newObj = gEngine.CreateInstance<object>(targetType);
				}

				if (newObj == null)
				{
					newObj = Activator.CreateInstance(targetType);
				}

				copyContext.RegisterCopy(source, newObj);

				newObj.CopyPropertiesFrom(source, null, true, copyContext); // Pass copyContext

				return newObj;
			}

			return source;
		}

		public class CopyContext
		{
			private readonly Dictionary<object, object> _copiedObjects = new Dictionary<object, object>(ReferenceEqualityComparer.Instance);

			public bool TryGetCopiedObject(object source, out object copied)
			{
				return _copiedObjects.TryGetValue(source, out copied);
			}

			public void RegisterCopy(object source, object copied)
			{
				if (source != null && copied != null)
				{
					_copiedObjects[source] = copied;
				}
			}
		}

		public static T Cast<T>(this object obj) where T : class
		{
			Debug.Assert(obj != null);

			return obj as T;
		}

		/// <remarks>
		/// Full credit:  https://stackoverflow.com/questions/2683442/where-can-i-find-the-clamp-function-in-net
		/// </remarks>
		public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0) return min;
			else if (val.CompareTo(max) > 0) return max;
			else return val;
		}

		public static bool CopyPropertiesFrom(this object destObj, object sourceObj, string propertyName = null, bool recurse = false, CopyContext copyContext = null)
		{
			if (destObj == null || sourceObj == null)
			{
				return false;
			}

			try
			{
				if (copyContext == null)
				{
					copyContext = new CopyContext();
				}

				if (!string.IsNullOrWhiteSpace(propertyName))
				{
					var sourceProps = GetCachedProperties(sourceObj.GetType());

					if (!sourceProps.TryGetValue(propertyName, out var sourceProp) || !sourceProp.CanRead)
					{
						return false;
					}

					if (sourceProp.GetCustomAttribute<ExcludeFromDeepCopyAttribute>() != null)
					{
						return true;
					}

					var destProps = GetCachedProperties(destObj.GetType());

					if (!destProps.TryGetValue(propertyName, out var destProp) || !destProp.CanWrite)
					{
						return false;
					}

					var value = sourceProp.GetValue(sourceObj);

					return destObj.SetPropertyValue(propertyName, value, recurse, copyContext);
				}
				else
				{
					var sourceProps = GetCachedProperties(sourceObj.GetType());

					var destProps = GetCachedProperties(destObj.GetType());

					var result = true;

					foreach (var sourceProp in sourceProps.Values.Where(p => p.CanRead))
					{
						if (sourceProp.GetCustomAttribute<ExcludeFromDeepCopyAttribute>() != null)
						{
							continue;
						}

						if (!destProps.TryGetValue(sourceProp.Name, out var destProp) || !destProp.CanWrite)
						{
							continue;
						}

						var value = sourceProp.GetValue(sourceObj);

						result &= destObj.SetPropertyValue(sourceProp.Name, value, recurse, copyContext);
					}

					return result;
				}
			}
			catch
			{
				return false;
			}
		}

		public static bool SetPropertyValue(this object destObj, string propertyName, object value, bool recurse = false, CopyContext copyContext = null)
		{
			if (destObj == null || string.IsNullOrWhiteSpace(propertyName))
			{
				return false;
			}

			try
			{
				var destProps = GetCachedProperties(destObj.GetType());

				if (!destProps.TryGetValue(propertyName, out var destProp) || !destProp.CanWrite)
				{
					return false;
				}

				if (copyContext == null)
				{
					copyContext = new CopyContext();
				}
				
				var copiedValue = CopyValue(value, destProp.PropertyType, recurse, copyContext);

				destProp.SetValue(destObj, copiedValue);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool CopyFieldsFrom(this object destObj, object sourceObj, long startField, long endField)
		{
			if (destObj == null || sourceObj == null || startField < 1 || endField < startField)
			{
				return false;
			}

			for (var i = startField; i <= endField; i++)
			{
				if (!destObj.CopyPropertiesFrom(sourceObj, string.Format("Field{0}", i), true))
				{
					return false;
				}
			}

			return true;
		}

		public static bool SetFieldsValue(this object destObj, long startField, long endField, object value)
		{
			if (destObj == null || startField < 1 || endField < startField)
			{
				return false;
			}

			for (var i = startField; i <= endField; i++)
			{
				if (!destObj.SetPropertyValue(string.Format("Field{0}", i), value, true))
				{
					return false;
				}
			}

			return true;
		}
	}
}
