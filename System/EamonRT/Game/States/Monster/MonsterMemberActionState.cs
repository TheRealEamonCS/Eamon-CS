
// MonsterMemberActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterMemberActionState : State, IMonsterMemberActionState
	{
		/// <summary></summary>
		public Enums.Spell _spellCast;

		/// <summary></summary>
		public IGameBase _spellTarget;

		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual IArtifact WeaponArtifact { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> WeaponArtifactList { get; set; }

		/// <summary></summary>
		public virtual IMonsterSpell MonsterSpell { get; set; }

		/// <summary></summary>
		public virtual ICommand ActionCommand { get; set; }

		/// <summary></summary>
		public virtual long WeaponArtifactListIndex { get; set; }

		/// <summary></summary>
		public virtual ContainerType WeaponContainerType { get; set; }

		/// <summary></summary>
		public virtual string ContainerPrepName { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			MonsterMemberReadiesWeaponCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberReadiesNaturalWeaponCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberCastsSpellCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberAttacksEnemyCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterMemberLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public virtual void MonsterMemberReadiesWeaponCheck()
		{
			if (LoopMonster.ShouldReadyWeapon() && ((LoopMonster.CombatCode == CombatCode.NaturalWeapons && LoopMonster.Weapon <= 0) || ((LoopMonster.CombatCode == CombatCode.Weapons || LoopMonster.CombatCode == CombatCode.Attacks) && LoopMonster.Weapon < 0)))
			{
				WeaponArtifactList = gEngine.BuildLoopWeaponArtifactList(LoopMonster);

				if (WeaponArtifactList != null && WeaponArtifactList.Count > 0)
				{
					for (WeaponArtifactListIndex = 0; WeaponArtifactListIndex < WeaponArtifactList.Count; WeaponArtifactListIndex++)
					{
						WeaponArtifact = WeaponArtifactList[(int)WeaponArtifactListIndex];

						Debug.Assert(WeaponArtifact != null);

						if (!WeaponArtifact.IsCarriedByMonster(LoopMonster))
						{
							WeaponContainerType = WeaponArtifact.GetCarriedByContainerContainerType();

							if (Enum.IsDefined(typeof(ContainerType), WeaponContainerType))
							{
								ContainerPrepName = gEngine.EvalContainerType(WeaponContainerType, "in", "on", "under", "behind");

								ActionCommand = Globals.CreateInstance<IMonsterRemoveCommand>(x =>
								{
									x.ActorMonster = LoopMonster;

									x.ActorRoom = LoopMonsterRoom;

									x.Dobj = WeaponArtifact;

									x.Iobj = WeaponArtifact.GetCarriedByContainer();

									x.Prep = gEngine.Preps.FirstOrDefault(prep => prep.Name.Equals(ContainerPrepName, StringComparison.OrdinalIgnoreCase));
								});
							}
							else
							{
								ActionCommand = Globals.CreateInstance<IMonsterGetCommand>(x =>
								{
									x.ActorMonster = LoopMonster;

									x.ActorRoom = LoopMonsterRoom;

									x.Dobj = WeaponArtifact;
								});
							}

							ActionCommand.Execute();

							Globals.UseRevealContentMonsterTheName = true;

							gEngine.CheckRevealContainerContents();

							Globals.UseRevealContentMonsterTheName = false;
						}

						if (WeaponArtifact.IsCarriedByMonster(LoopMonster))
						{
							ActionCommand = Globals.CreateInstance<IMonsterReadyCommand>(x =>
							{
								x.ActorMonster = LoopMonster;

								x.ActorRoom = LoopMonsterRoom;

								x.Dobj = WeaponArtifact;
							});

							ActionCommand.Execute();
						}

						if (LoopMonster.Weapon > 0)
						{
							GotoCleanup = true;

							break;
						}
					}
				}
			}
		}

		public virtual void MonsterMemberReadiesNaturalWeaponCheck()
		{
			if (LoopMonster.CombatCode == CombatCode.NaturalWeapons && LoopMonster.Weapon < 0)
			{
				LoopMonster.Weapon = 0;
			}
		}

		public virtual void MonsterMemberCastsSpellCheck()
		{
			if (LoopMonster.ShouldCastSpell(ref _spellCast, ref _spellTarget))
			{
				MonsterSpell = LoopMonster.GetMonsterSpell(_spellCast);

				if (MonsterSpell != null)
				{
					ActionCommand = null;

					switch (_spellCast)
					{
						case Spell.Blast:

							if (LoopMonster.CombatCode != CombatCode.NeverFights)
							{
								Debug.Assert(_spellTarget != null);

								ActionCommand = Globals.CreateInstance<IMonsterBlastCommand>(x =>
								{
									x.CheckAttack = true;
								});
							}

							break;

						case Spell.Heal:

							ActionCommand = Globals.CreateInstance<IMonsterHealCommand>();

							break;

						case Spell.Speed:

							Debug.Assert(_spellTarget == null);

							ActionCommand = Globals.CreateInstance<IMonsterSpeedCommand>();

							break;

						case Spell.Power:

							Debug.Assert(_spellTarget == null);

							ActionCommand = Globals.CreateInstance<IMonsterPowerCommand>();

							break;
					}

					if (ActionCommand != null)
					{
						ActionCommand.NextState = Globals.CreateInstance<IMonsterMemberLoopIncrementState>();

						ActionCommand.ActorMonster = LoopMonster;

						ActionCommand.ActorRoom = LoopMonsterRoom;

						ActionCommand.Dobj = _spellTarget;

						ActionCommand.Execute();

						NextState = ActionCommand.NextState;

						GotoCleanup = true;
					}
				}
			}
		}

		public virtual void MonsterMemberAttacksEnemyCheck()
		{
			if (LoopMonster.CombatCode != CombatCode.NeverFights && LoopMonster.CheckNBTLHostility() && LoopMonster.Weapon >= 0)
			{
				NextState = Globals.CreateInstance<IMonsterAttackLoopInitializeState>();

				GotoCleanup = true;
			}
		}

		public MonsterMemberActionState()
		{
			Name = "MonsterMemberActionState";
		}
	}
}
