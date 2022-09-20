
// IDdMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonDD.Framework.Menus
{
	/// <summary></summary>
	public interface IDdMenu
	{
		/// <summary></summary>
		void PrintMainMenuSubtitle();

		/// <summary></summary>
		void PrintConfigMenuSubtitle();

		/// <summary></summary>
		void PrintFilesetMenuSubtitle();

		/// <summary></summary>
		void PrintCharacterMenuSubtitle();

		/// <summary></summary>
		void PrintModuleMenuSubtitle();

		/// <summary></summary>
		void PrintRoomMenuSubtitle();

		/// <summary></summary>
		void PrintArtifactMenuSubtitle();

		/// <summary></summary>
		void PrintEffectMenuSubtitle();

		/// <summary></summary>
		void PrintMonsterMenuSubtitle();

		/// <summary></summary>
		void PrintHintMenuSubtitle();
	};
}
