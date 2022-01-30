
// IRoom.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;

namespace WrenholdsSecretVigil.Framework
{
	/// <summary></summary>
	public interface IRoom : Eamon.Framework.IRoom
	{
		/// <summary></summary>
		/// <returns></returns>
		bool IsDigCommandAllowedInRoom();

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionEffect(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionEffect(Direction dir);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		long GetDirectionEffectUid(Direction dir);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		IEffect GetDirectionEffect(Direction dir);
	}
}
