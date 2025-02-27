
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;

namespace TheSubAquanLaboratory.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		/// <summary></summary>
		new StringBuilder Buf01 { get; set; }

		/// <summary></summary>
		IArtifact WallArtifact { get; set; }

		/// <summary></summary>
		IArtifactCategory WallZeroAc { get; set; }

		/// <summary></summary>
		long WallDamage { get; set; }

		/// <summary></summary>
		bool AttackingWall { get; set; }

		/// <summary></summary>
		/// <param name="actorRoom"></param>
		/// <param name="actorMonster"></param>
		/// <param name="dobjArtifact"></param>
		/// <param name="blastSpell"></param>
		void ProcessWallAttack(IRoom actorRoom, Eamon.Framework.IMonster actorMonster, IArtifact dobjArtifact, bool blastSpell);

		bool ApplyWallDamage(IRoom actorRoom);
	}
}
