
// BitFlags.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;

namespace Eamon.Game.Utilities
{
	/// <summary>
	/// Utility class for manipulating bit flags stored in a ulong.
	/// Provides Get, Set, Clear, and Toggle operations for individual bits or bit masks.
	/// </summary>
	public static class BitFlags
	{
		/// <summary>
		/// Checks if the specified bit is set in the flags.
		/// </summary>
		/// <param name="flags">The current bitflags value.</param>
		/// <param name="bit">The bit position (0-63) to check.</param>
		/// <returns>True if the bit is set; otherwise, false.</returns>
		public static bool Get(ulong flags, int bit)
		{
			Debug.Assert(bit >= 0 && bit <= 63);

			return (flags & (1UL << bit)) != 0;
		}

		/// <summary>
		/// Checks if all bits in the mask are set in the flags.
		/// </summary>
		/// <param name="flags">The current bitflags value.</param>
		/// <param name="mask">The bit mask to check.</param>
		/// <returns>True if all bits in the mask are set; otherwise, false.</returns>
		public static bool Get(ulong flags, ulong mask)
		{
			return (flags & mask) == mask;
		}

		/// <summary>
		/// Sets the specified bit to 1.
		/// </summary>
		/// <param name="flags">The current bitflags value (passed by ref to modify).</param>
		/// <param name="bit">The bit position (0-63) to set.</param>
		public static void Set(ref ulong flags, int bit)
		{
			Debug.Assert(bit >= 0 && bit <= 63);

			flags |= (1UL << bit);
		}

		/// <summary>
		/// Sets all bits in the mask to 1.
		/// </summary>
		/// <param name="flags">The current bitflags value (passed by ref).</param>
		/// <param name="mask">The bit mask to set.</param>
		public static void Set(ref ulong flags, ulong mask)
		{
			flags |= mask;
		}

		/// <summary>
		/// Clears the specified bit (sets it to 0).
		/// </summary>
		/// <param name="flags">The current bitflags value (passed by ref).</param>
		/// <param name="bit">The bit position (0-63) to clear.</param>
		public static void Clear(ref ulong flags, int bit)
		{
			Debug.Assert(bit >= 0 && bit <= 63);

			flags &= ~(1UL << bit);
		}

		/// <summary>
		/// Clears all bits in the mask (sets them to 0).
		/// </summary>
		/// <param name="flags">The current bitflags value (passed by ref).</param>
		/// <param name="mask">The bit mask to clear.</param>
		public static void Clear(ref ulong flags, ulong mask)
		{
			flags &= ~mask;
		}

		/// <summary>
		/// Toggles the specified bit (0 becomes 1, 1 becomes 0).
		/// </summary>
		/// <param name="flags">The current bitflags value (passed by ref).</param>
		/// <param name="bit">The bit position (0-63) to toggle.</param>
		public static void Toggle(ref ulong flags, int bit)
		{
			Debug.Assert(bit >= 0 && bit <= 63);

			flags ^= (1UL << bit);
		}

		/// <summary>
		/// Toggles all bits in the mask.
		/// </summary>
		/// <param name="flags">The current bitflags value (passed by ref).</param>
		/// <param name="mask">The bit mask to toggle.</param>
		public static void Toggle(ref ulong flags, ulong mask)
		{
			flags ^= mask;
		}

		/// <summary>
		/// Returns a new flags value with the specified bit set.
		/// </summary>
		public static ulong WithSet(ulong flags, int bit)
		{
			Set(ref flags, bit);

			return flags;
		}

		/// <summary>
		/// Returns a new flags value with the specified mask set.
		/// </summary>
		public static ulong WithSet(ulong flags, ulong mask)
		{
			return flags | mask;
		}

		/// <summary>
		/// Returns a new flags value with the specified bit cleared.
		/// </summary>
		public static ulong WithClear(ulong flags, int bit)
		{
			Clear(ref flags, bit);

			return flags;
		}

		/// <summary>
		/// Returns a new flags value with the specified mask cleared.
		/// </summary>
		public static ulong WithClear(ulong flags, ulong mask)
		{
			return flags & ~mask;
		}

		/// <summary>
		/// Returns a new flags value with the specified bit toggled.
		/// </summary>
		public static ulong WithToggle(ulong flags, int bit)
		{
			Toggle(ref flags, bit);

			return flags;
		}

		/// <summary>
		/// Returns a new flags value with the specified mask toggled.
		/// </summary>
		public static ulong WithToggle(ulong flags, ulong mask)
		{
			return flags ^ mask;
		}

		/// <summary>
		/// Gets all set bit positions (0-63) as an enumerable.
		/// </summary>
		public static IEnumerable<int> GetSetBits(ulong flags)
		{
			for (int i = 0; i < 64; i++)
			{
				if ((flags & (1UL << i)) != 0)
					yield return i;
			}
		}
	}
}
