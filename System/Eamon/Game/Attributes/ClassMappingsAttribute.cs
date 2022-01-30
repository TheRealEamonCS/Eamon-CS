
// ClassMappingsAttribute.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework.Plugin;

namespace Eamon.Game.Attributes
{
	/// <summary>
	/// An attribute that decorates concrete classes in Eamon CS, which corresponding interfaces resolve to via
	/// dependency injection.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ClassMappingsAttribute : Attribute
	{
		public Type InterfaceType { get; set; }

		/// <summary>
		/// An attribute that indicates this concrete class is the Value in a Key/Value pair stored in the
		/// <see cref="IPluginClassMappings.ClassMappingsDictionary">ClassMappingsDictionary</see>;
		/// the Key is the interface provided as a parameter.
		/// </summary>
		public ClassMappingsAttribute(Type interfaceType)
		{
			InterfaceType = interfaceType;
		}

		/// <summary>
		/// An attribute that indicates this concrete class is the Value in a Key/Value pair stored in the
		/// <see cref="IPluginClassMappings.ClassMappingsDictionary">ClassMappingsDictionary</see>;
		/// the Key is a corresponding interface with the same name but prefixed with "I".
		/// </summary>
		public ClassMappingsAttribute()
		{

		}
	}
}
