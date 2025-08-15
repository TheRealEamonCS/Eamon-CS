
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
using static EamonRT.Game.Plugin.Globals;

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
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			MonsterMemberMiscActionCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberReadiesWeaponCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberMiscActionCheck01();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberReadiesNaturalWeaponCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberMiscActionCheck02();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberCastsSpellCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberMiscActionCheck03();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberAttacksEnemyCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberMiscActionCheck04();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterMemberLoopIncrementState>();
			}

			gEngine.NextState = NextState;
		}

		public virtual void MonsterMemberMiscActionCheck()
		{
			// Do nothing
		}

		public virtual void MonsterMemberReadiesWeaponCheck()
		{
			if (LoopMonster.ShouldReadyWeapon() && (((LoopMonster.CombatCode == CombatCode.NaturalWeapons || LoopMonster.CombatCode == CombatCode.NaturalAttacks) && LoopMonster.Weapon <= 0) || ((LoopMonster.CombatCode == CombatCode.Weapons || LoopMonster.CombatCode == CombatCode.Attacks) && LoopMonster.Weapon < 0)))
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

								ActionCommand = gEngine.CreateInstance<IRemoveCommand>(x =>
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
								ActionCommand = gEngine.CreateInstance<IGetCommand>(x =>
								{
									x.ActorMonster = LoopMonster;

									x.ActorRoom = LoopMonsterRoom;

									x.Dobj = WeaponArtifact;
								});
							}

							ActionCommand.Execute();

							try
							{
								gEngine.UseRevealContentMonsterTheName = true;

								gEngine.CheckRevealContainerContents();
							}
							finally
							{
								gEngine.UseRevealContentMonsterTheName = false;
							}
						}

						if (WeaponArtifact.IsCarriedByMonster(LoopMonster))
						{
							ActionCommand = gEngine.CreateInstance<IReadyCommand>(x =>
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

		public virtual void MonsterMemberMiscActionCheck01()
		{
			// Do nothing
		}

		public virtual void MonsterMemberReadiesNaturalWeaponCheck()
		{
			if ((LoopMonster.CombatCode == CombatCode.NaturalWeapons || LoopMonster.CombatCode == CombatCode.NaturalAttacks) && LoopMonster.Weapon < 0)
			{
				LoopMonster.Weapon = 0;
			}
		}

		public virtual void MonsterMemberMiscActionCheck02()
		{
			// Do nothing
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

								ActionCommand = gEngine.CreateInstance<IBlastCommand>();
							}

							break;

						case Spell.Heal:

							ActionCommand = gEngine.CreateInstance<IHealCommand>();

							break;

						case Spell.Speed:

							Debug.Assert(_spellTarget == null);

							ActionCommand = gEngine.CreateInstance<ISpeedCommand>();

							break;

						case Spell.Power:

							Debug.Assert(_spellTarget == null);

							ActionCommand = gEngine.CreateInstance<IPowerCommand>();

							break;
					}

					if (ActionCommand != null)
					{
						ActionCommand.NextState = gEngine.CreateInstance<IMonsterMemberLoopIncrementState>();

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

		public virtual void MonsterMemberMiscActionCheck03()
		{
			// Do nothing
		}

		public virtual void MonsterMemberAttacksEnemyCheck()
		{
			if (LoopMonster.CombatCode != CombatCode.NeverFights && LoopMonster.CheckNBTLHostility() && LoopMonster.Weapon >= 0)
			{
				NextState = gEngine.CreateInstance<IMonsterAttackLoopInitializeState>();

				GotoCleanup = true;
			}
		}

		public virtual void MonsterMemberMiscActionCheck04()
		{
			// Do nothing
		}

		public MonsterMemberActionState()
		{
			Name = "MonsterMemberActionState";
		}
	}
}
