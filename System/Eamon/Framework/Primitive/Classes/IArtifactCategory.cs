
// IArtifactCategory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Classes
{
	/// <summary></summary>
	public interface IArtifactCategory
	{
		/// <summary></summary>
		IArtifact Parent { get; set; }

		/// <summary></summary>
		bool SyncFields { get; set; }

		/// <summary></summary>
		Enums.ArtifactType Type { get; set; }

		/// <summary></summary>
		long Field1 { get; set; }

		/// <summary></summary>
		long Field2 { get; set; }

		/// <summary></summary>
		long Field3 { get; set; }

		/// <summary></summary>
		long Field4 { get; set; }

		/// <summary></summary>
		long Field5 { get; set; }

		/// <summary></summary>
		/// <returns></returns>
		bool IsOpenable();

		/// <summary></summary>
		/// <returns></returns>
		bool IsLockable();

		/// <summary></summary>
		/// <returns></returns>
		bool IsBreakable();

		/// <summary></summary>
		/// <returns></returns>
		bool IsEffectExposer();

		/// <summary></summary>
		/// <returns></returns>
		bool IsMonsterExposer();

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <returns></returns>
		bool IsWeapon(Enums.Weapon weapon);

		/// <summary></summary>
		/// <returns></returns>
		bool IsWeapon01();

		/// <summary></summary>
		/// <returns></returns>
		bool IsOpen();

		/// <summary></summary>
		/// <param name="open"></param>
		void SetOpen(bool open);

		/// <summary></summary>
		/// <param name="artifactUid"></param>
		void SetKeyUid(long artifactUid);

		/// <summary></summary>
		/// <param name="strength"></param>
		void SetBreakageStrength(long strength);

		/// <summary></summary>
		/// <param name="effectUid"></param>
		void SetFirstEffect(long effectUid);

		/// <summary></summary>
		/// <param name="numEffects"></param>
		void SetNumEffects(long numEffects);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		void SetMonsterUid(long monsterUid);

		/// <summary></summary>
		/// <returns></returns>
		long GetKeyUid();

		/// <summary></summary>
		/// <returns></returns>
		long GetBreakageStrength();

		/// <summary></summary>
		/// <returns></returns>
		long GetFirstEffect();

		/// <summary></summary>
		/// <returns></returns>
		long GetNumEffects();

		/// <summary></summary>
		/// <returns></returns>
		long GetMonsterUid();

		/// <summary></summary>
		void SyncArtifactCategories();
	}
}
