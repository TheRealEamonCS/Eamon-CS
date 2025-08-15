
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game
{
	[ClassMappings]
	public class Artifact : GameBase, IArtifact
	{
		#region Public Fields

		public long _location;

		public IArtifactCategory _lastArtifactCategory;

		#endregion

		#region Public Properties

		#region Interface IArtifact

		[FieldName(320)]
		public virtual string StateDesc { get; set; }

		[FieldName(520)]
		public virtual bool IsCharOwned { get; set; }

		[FieldName(540)]
		public virtual bool IsPlural { get; set; }

		[FieldName(560)]
		public virtual bool IsListed { get; set; }

		[FieldName(580)]
		public virtual PluralType PluralType { get; set; }

		[FieldName(620)]
		public virtual long Value { get; set; }

		[FieldName(640)]
		public virtual long Weight { get; set; }

		[ExcludeFromSerialization]
		public virtual long RecursiveWeight 
		{ 
			get
			{
				RetCode rc;

				long c = 0;

				long w = Weight;

				if (!gEngine.IsUnmovable01(w) && GeneralContainer != null)
				{
					rc = GetContainerInfo(ref c, ref w, (ContainerType)(-1), true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				return w;
			}
		}

		[FieldName(660)]
		public virtual long Location
		{
			get
			{
				return _location;
			}

			set
			{
				if (gEngine.EnableMutateProperties && _location != value)
				{
					if (gEngine.RevealContentCounter > 0 && GeneralContainer != null && !gEngine.RevealContentArtifactList.Contains(this))
					{
						var room = gEngine.RevealContentRoom;

						var monster = gEngine.RevealContentMonster;

						var origLocation = _location;

						gEngine.RevealContentArtifactList.Add(this);

						gEngine.RevealContentFuncList.Add(() =>
						{
							if (gEngine.RevealContainerContentsFunc != null && room != null)
							{
								gEngine.RevealContainerContentsFunc(room, monster, this, origLocation, true);
							}
						});
					}

					if (HasMoved(_location, value))
					{
						Moved = true;
					}
				}

				_location = value;
			}
		}

		[ExcludeFromSerialization]
		public virtual ArtifactType Type
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Type : ArtifactType.None;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Type = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field1
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field1 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field1 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field2
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field2 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field2 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field3
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field3 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field3 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field4
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field4 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field4 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field5
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field5 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field5 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field6
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field6 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field6 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field7
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field7 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field7 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field8
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field8 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field8 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field9
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field9 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field9 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field10
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field10 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field10 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field11
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field11 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field11 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field12
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field12 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field12 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field13
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field13 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field13 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field14
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field14 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field14 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field15
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field15 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field15 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field16
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field16 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field16 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field17
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field17 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field17 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field18
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field18 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field18 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field19
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field19 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field19 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field20
		{
			get
			{
				var ac = GetCategory(0);

				return ac != null ? ac.Field20 : 0;
			}

			set
			{
				var ac = GetCategory(0);

				if (ac != null)
				{
					ac.Field20 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Gold
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Gold);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Treasure
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Treasure);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Weapon
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Weapon);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory MagicWeapon
		{
			get
			{
				return GetArtifactCategory(ArtifactType.MagicWeapon);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory GeneralWeapon
		{
			get
			{
				var artTypes = new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon };

				return GetArtifactCategory(artTypes);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory InContainer
		{
			get
			{
				return GetArtifactCategory(ArtifactType.InContainer);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory OnContainer
		{
			get
			{
				return GetArtifactCategory(ArtifactType.OnContainer);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory UnderContainer
		{
			get
			{
				return GetArtifactCategory(ArtifactType.UnderContainer);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory BehindContainer
		{
			get
			{
				return GetArtifactCategory(ArtifactType.BehindContainer);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory GeneralContainer
		{
			get
			{
				var artTypes = new ArtifactType[] { ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer };

				return GetArtifactCategory(artTypes);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory LightSource
		{
			get
			{
				return GetArtifactCategory(ArtifactType.LightSource);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Drinkable
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Drinkable);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Readable
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Readable);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory DoorGate
		{
			get
			{
				return GetArtifactCategory(ArtifactType.DoorGate);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Edible
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Edible);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory BoundMonster
		{
			get
			{
				return GetArtifactCategory(ArtifactType.BoundMonster);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Wearable
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Wearable);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory DisguisedMonster
		{
			get
			{
				return GetArtifactCategory(ArtifactType.DisguisedMonster);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory DeadBody
		{
			get
			{
				return GetArtifactCategory(ArtifactType.DeadBody);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory User1
		{
			get
			{
				return GetArtifactCategory(ArtifactType.User1);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory User2
		{
			get
			{
				return GetArtifactCategory(ArtifactType.User2);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory User3
		{
			get
			{
				return GetArtifactCategory(ArtifactType.User3);
			}
		}

		[FieldName(680)]
		public virtual IArtifactCategory[] Categories { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Get rid of managed resources
			}

			if (Uid > 0)
			{
				gDatabase.FreeArtifactUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IGameBase

		public override void SetParentReferences()
		{
			// Backpoint ArtifactCategory objects to this Artifact to allow easy access

			foreach (var ac in Categories)
			{
				if (ac != null)
				{
					ac.Parent = this;
				}
			}

			// Ensure ArtifactCategory objects are in sync for this Artifact

			foreach (var ac in Categories)
			{
				if (ac != null && ac.Type != ArtifactType.None)
				{
					SyncArtifactCategories(ac);
				}
			}
		}

		public override string GetPluralName(string fieldName)
		{
			IEffect effect;
			long effectUid;
			string result;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			var buf = new StringBuilder(gEngine.BufSize);

			effectUid = gEngine.GetPluralTypeEffectUid(PluralType);

			effect = gEDB[effectUid];

			if (effect != null)
			{
				buf.Append(effect.Desc.Substring(0, Math.Min(gEngine.ArtNameLen, effect.Desc.Length)).Trim());
			}
			else
			{
				buf.Append(Name);

				if (buf.Length > 0 && PluralType == PluralType.YIes)
				{
					buf.Length--;
				}

				buf.Append(PluralType == PluralType.None ? "" :
						PluralType == PluralType.Es ? "es" :
						PluralType == PluralType.YIes ? "ies" :
						"s");
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override string GetDecoratedName(string fieldName, ArticleType articleType, bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool showContents = false, bool groupCountOne = false)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			var buf = new StringBuilder(gEngine.BufSize);

			switch (articleType)
			{
				case ArticleType.None:

					buf.AppendFormat
					(
						"{0}{1}{2}",
						EvalPlural(Name, GetPluralName(fieldName)),
						showContents && GeneralContainer != null ? GetContainerContentsDesc() : "",
						showStateDesc ? StateDesc : ""
					);

					break;

				case ArticleType.The:

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						ArticleType == ArticleType.None ? "" :
						ArticleType == ArticleType.The ? "the " :
						IsCharOwned && showCharOwned ? "your " :
						"the ",
						EvalPlural(Name, GetPluralName(fieldName)),
						showContents && GeneralContainer != null ? GetContainerContentsDesc() : "",
						showStateDesc ? StateDesc : ""
					);

					break;

				default:

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						ArticleType == ArticleType.None ? "" :
						ArticleType == ArticleType.The ? "the " :
						IsCharOwned && showCharOwned ? "your " :
						ArticleType == ArticleType.Some ? "some " :
						ArticleType == ArticleType.An ? "an " :
						"a ",
						EvalPlural(Name, GetPluralName(fieldName)),
						showContents && GeneralContainer != null ? GetContainerContentsDesc() : "",
						showStateDesc ? StateDesc : ""
					);

					break;
			}

			if (buf.Length > 0 && upshift)
			{
				buf[0] = Char.ToUpper(buf[0]);
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName, bool showVerboseName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (showName || showVerboseName)
			{
				buf.AppendFormat("{0}[{1}]",
					Environment.NewLine,
					GetArticleName
					(
						true, 
						true, 
						showVerboseName && !string.IsNullOrWhiteSpace(StateDesc) && ShouldShowVerboseNameStateDesc(), 
						showVerboseName && GeneralContainer != null && ShouldShowVerboseNameContentsNameList(),
						false
					)
				);
			}

			if (!string.IsNullOrWhiteSpace(Desc))
			{
				buf.AppendFormat("{0}{1}", Environment.NewLine, Desc);
			}

			if (showName || showVerboseName || !string.IsNullOrWhiteSpace(Desc))
			{
				buf.Append(Environment.NewLine);
			}

		Cleanup:

			return rc;
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IArtifact artifact)
		{
			return this.Uid.CompareTo(artifact.Uid);
		}

		#endregion

		#region Interface IArtifact

		public virtual IArtifactCategory GetCategory(long index)
		{
			_lastArtifactCategory = Categories[index];

			return _lastArtifactCategory;
		}

		public virtual string GetSynonym(long index)
		{
			return Synonyms[index];
		}

		public virtual void SetCategory(long index, IArtifactCategory value)
		{
			Categories[index] = value;
		}

		public virtual void SetSynonym(long index, string value)
		{
			Synonyms[index] = value;
		}

		public virtual bool HasMoved(long oldLocation, long newLocation)
		{
			return oldLocation != gEngine.LimboLocation && newLocation < 0 && newLocation > -999;
		}

		public virtual bool IsCarriedByCharacter()
		{
			return Location < -2000 && Location > -3001;
		}

		public virtual bool IsCarriedByMonster(MonsterType monsterType = MonsterType.NonCharMonster, bool recurse = false)
		{
			var result = false;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				result = artifact.IsCarriedByMonster(monsterType);
			}
			else
			{
				if (monsterType == MonsterType.Any)
				{
					result = (Location < -1 && Location > -999) || Location == -1;
				}
				else if (monsterType == MonsterType.NonCharMonster)
				{
					result = Location < -1 && Location > -999;
				}
				else
				{
					result = Location == -1;
				}
			}

			return result;
		}

		public virtual bool IsCarriedByContainer()
		{
			return Location > 1000 && Location < 5001;
		}

		public virtual bool IsWornByCharacter()
		{
			return Location < -3000 && Location > -4001;
		}

		public virtual bool IsWornByMonster(MonsterType monsterType = MonsterType.NonCharMonster, bool recurse = false)
		{
			var result = false;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				result = artifact.IsWornByMonster(monsterType);
			}
			else
			{
				if (monsterType == MonsterType.Any)
				{
					result = (Location < -1000 && Location > -2001) || Location == -999;
				}
				else if (monsterType == MonsterType.NonCharMonster)
				{
					result = Location < -1000 && Location > -2001;
				}
				else
				{
					result = Location == -999;
				}
			}

			return result;
		}

		public virtual bool IsInRoom(bool recurse = false)
		{
			var result = false;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				var monster = gMDB[artifact.GetCarriedByMonsterUid()];

				if (monster == null)
				{
					monster = gMDB[artifact.GetWornByMonsterUid()];
				}

				if (monster != null)
				{
					result = monster.IsInRoom();
				}
				else
				{
					result = artifact.IsInRoom();
				}
			}
			else
			{
				result = Location > 0 && Location < 1001;
			}

			return result;
		}

		public virtual bool IsEmbeddedInRoom(bool recurse = false)
		{
			var result = false;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				result = artifact.IsEmbeddedInRoom();
			}
			else
			{
				result = Location > 5000 && Location < 6001;
			}

			return result;
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToMonster(MonsterType monsterType = MonsterType.NonCharMonster, bool recurse = false)
		{
			var result = false;

			var artifact = gADB[GetCarriedByContainerUid()];

			var containerType = GetCarriedByContainerContainerType();

			if (artifact != null)
			{
				if (artifact.ShouldExposeContentsToMonster(monsterType, containerType))
				{
					if (containerType != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed())
					{
						if (!recurse || gADB[artifact.GetCarriedByContainerUid()] == null || artifact.IsCarriedByContainerContainerTypeExposedToMonster(monsterType, recurse))
						{
							result = true;
						}
					}
				}
			}

			return result;
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToRoom(bool recurse = false)
		{
			var result = false;

			var artifact = gADB[GetCarriedByContainerUid()];

			var containerType = GetCarriedByContainerContainerType();

			if (artifact != null)
			{
				if (artifact.ShouldExposeContentsToRoom(containerType))
				{
					if (containerType != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed())
					{
						if (!recurse || gADB[artifact.GetCarriedByContainerUid()] == null || artifact.IsCarriedByContainerContainerTypeExposedToRoom(recurse))
						{
							result = true;
						}
					}
				}
			}

			return result;
		}

		public virtual bool IsInLimbo(bool recurse = false)
		{
			var result = false;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				var monster = gMDB[artifact.GetCarriedByMonsterUid()];

				if (monster == null)
				{
					monster = gMDB[artifact.GetWornByMonsterUid()];
				}

				if (monster != null)
				{
					result = monster.IsInLimbo();
				}
				else
				{
					result = artifact.IsInLimbo();
				}
			}
			else
			{
				result = Location == gEngine.LimboLocation;
			}

			return result;
		}

		public virtual bool IsCarriedByCharacterUid(long characterUid)
		{
			return Location == (-characterUid - 2000);
		}

		public virtual bool IsCarriedByMonsterUid(long monsterUid, bool recurse = false)
		{
			var result = false;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				result = artifact.IsCarriedByMonsterUid(monsterUid);
			}
			else
			{
				var gameState = gEngine.GetGameState();

				if (monsterUid == long.MaxValue || (gameState != null && monsterUid == gameState.Cm))
				{
					result = Location == -1;
				}
				else
				{
					result = Location == (-monsterUid - 1);
				}
			}

			return result;
		}

		public virtual bool IsCarriedByContainerUid(long containerUid, bool recurse = false)
		{
			var result = false;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid()];

				while (artifact != null && artifact.Uid != containerUid)
				{
					var artifact01 = gADB[artifact.GetCarriedByContainerUid()];

					if (artifact01 != null)
					{
						artifact = artifact01;
					}
					else
					{
						break;
					}
				}

				if (artifact != null)
				{
					result = artifact.Uid == containerUid;
				}
			}
			else
			{
				var containerType = GetCarriedByContainerContainerType();

				if (Enum.IsDefined(typeof(ContainerType), containerType))
				{
					result = Location == (containerUid + (((long)containerType * 1000) + 1000));
				}
			}

			return result;
		}

		public virtual bool IsWornByCharacterUid(long characterUid)
		{
			return Location == (-characterUid - 3000);
		}

		public virtual bool IsWornByMonsterUid(long monsterUid, bool recurse = false)
		{
			var result = false;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				result = artifact.IsWornByMonsterUid(monsterUid);
			}
			else
			{
				var gameState = gEngine.GetGameState();

				if (monsterUid == long.MaxValue || (gameState != null && monsterUid == gameState.Cm))
				{
					result = Location == -999;
				}
				else
				{
					result = Location == (-monsterUid - 1000);
				}
			}

			return result;
		}

		public virtual bool IsReadyableByMonsterUid(long monsterUid)
		{
			return GeneralWeapon != null;
		}

		public virtual bool IsInRoomUid(long roomUid, bool recurse = false)
		{
			var result = false;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				var monster = gMDB[artifact.GetCarriedByMonsterUid()];

				if (monster == null)
				{
					monster = gMDB[artifact.GetWornByMonsterUid()];
				}

				if (monster != null)
				{
					result = monster.IsInRoomUid(roomUid);
				}
				else
				{
					result = artifact.IsInRoomUid(roomUid);
				}
			}
			else
			{
				result = Location == roomUid;
			}

			return result;
		}

		public virtual bool IsEmbeddedInRoomUid(long roomUid, bool recurse = false)
		{
			var result = false;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				result = artifact.IsEmbeddedInRoomUid(roomUid);
			}
			else
			{
				result = Location == (roomUid + 5000);
			}

			return result;
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToMonsterUid(long monsterUid, bool recurse = false)
		{
			var result = false;

			var gameState = gEngine.GetGameState();

			var artifact = gADB[GetCarriedByContainerUid()];

			var monsterType = monsterUid == long.MaxValue || (gameState != null && monsterUid == gameState.Cm) ? MonsterType.CharMonster : MonsterType.NonCharMonster;

			var containerType = GetCarriedByContainerContainerType();

			if (artifact != null)
			{
				if (artifact.ShouldExposeContentsToMonster(monsterType, containerType))
				{
					if (containerType != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed())
					{
						if (artifact.IsCarriedByMonsterUid(monsterUid) || (recurse && gADB[artifact.GetCarriedByContainerUid()] != null && artifact.IsCarriedByContainerContainerTypeExposedToMonsterUid(monsterUid, recurse)))
						{
							result = true;
						}
					}
				}
			}

			return result;
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToRoomUid(long roomUid, bool recurse = false)
		{
			var result = false;

			var artifact = gADB[GetCarriedByContainerUid()];

			var containerType = GetCarriedByContainerContainerType();

			if (artifact != null)
			{
				if (artifact.ShouldExposeContentsToRoom(containerType))
				{
					if (containerType != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed())
					{
						if (artifact.IsInRoomUid(roomUid) || artifact.IsEmbeddedInRoomUid(roomUid) || (recurse && gADB[artifact.GetCarriedByContainerUid()] != null && artifact.IsCarriedByContainerContainerTypeExposedToRoomUid(roomUid, recurse)))
						{
							result = true;
						}
					}
				}
			}

			return result;
		}

		public virtual bool IsCarriedByCharacter(ICharacter character)
		{
			Debug.Assert(character != null);

			return IsCarriedByCharacterUid(character.Uid);
		}

		public virtual bool IsCarriedByMonster(IMonster monster, bool recurse = false)
		{
			return IsCarriedByMonsterUid(monster != null ? monster.Uid : long.MaxValue, recurse);
		}

		public virtual bool IsCarriedByContainer(IArtifact container, bool recurse = false)
		{
			Debug.Assert(container != null);

			return IsCarriedByContainerUid(container.Uid, recurse);
		}

		public virtual bool IsWornByCharacter(ICharacter character)
		{
			Debug.Assert(character != null);

			return IsWornByCharacterUid(character.Uid);
		}

		public virtual bool IsWornByMonster(IMonster monster, bool recurse = false)
		{
			return IsWornByMonsterUid(monster != null ? monster.Uid : long.MaxValue, recurse);
		}

		public virtual bool IsReadyableByMonster(IMonster monster)
		{
			return IsReadyableByMonsterUid(monster != null ? monster.Uid : long.MaxValue);
		}

		public virtual bool IsInRoom(IRoom room, bool recurse = false)
		{
			Debug.Assert(room != null);

			return IsInRoomUid(room.Uid, recurse);
		}

		public virtual bool IsEmbeddedInRoom(IRoom room, bool recurse = false)
		{
			Debug.Assert(room != null);

			return IsEmbeddedInRoomUid(room.Uid, recurse);
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToMonster(IMonster monster, bool recurse = false)
		{
			return IsCarriedByContainerContainerTypeExposedToMonsterUid(monster != null ? monster.Uid : long.MaxValue, recurse);
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToRoom(IRoom room, bool recurse = false)
		{
			Debug.Assert(room != null);

			return IsCarriedByContainerContainerTypeExposedToRoomUid(room.Uid, recurse);
		}

		public virtual long GetCarriedByCharacterUid()
		{
			return IsCarriedByCharacter() ? -Location - 2000 : 0L;
		}

		public virtual long GetCarriedByMonsterUid(bool recurse = false)
		{
			var monsterUid = 0L;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				monsterUid = artifact.GetCarriedByMonsterUid();
			}
			else
			{
				var gameState = gEngine.GetGameState();

				if (IsCarriedByMonster(MonsterType.CharMonster) && gameState != null)
				{
					monsterUid = gameState.Cm;
				}
				else if (IsCarriedByMonster())
				{
					monsterUid = -Location - 1;
				}
			}

			return monsterUid;
		}

		public virtual long GetCarriedByContainerUid(bool recurse = false)
		{
			var artifactUid = 0L;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid()];

				while (artifact != null)
				{
					var artifact01 = gADB[artifact.GetCarriedByContainerUid()];

					if (artifact01 != null)
					{
						artifact = artifact01;
					}
					else
					{
						break;
					}
				}

				if (artifact != null)
				{
					artifactUid = artifact.Uid;
				}
			}
			else
			{
				var containerType = GetCarriedByContainerContainerType();

				if (Enum.IsDefined(typeof(ContainerType), containerType))
				{
					artifactUid = Location - (((long)containerType * 1000) + 1000);
				}
			}

			return artifactUid;
		}

		public virtual long GetWornByCharacterUid()
		{
			return IsWornByCharacter() ? -Location - 3000 : 0L;
		}

		public virtual long GetWornByMonsterUid(bool recurse = false)
		{
			var monsterUid = 0L;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				monsterUid = artifact.GetWornByMonsterUid();
			}
			else
			{
				var gameState = gEngine.GetGameState();

				if (IsWornByMonster(MonsterType.CharMonster) && gameState != null)
				{
					monsterUid = gameState.Cm;
				}
				else if (IsWornByMonster())
				{
					monsterUid = -Location - 1000;
				}
			}

			return monsterUid;
		}

		public virtual long GetInRoomUid(bool recurse = false)
		{
			var roomUid = 0L;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				var monster = gMDB[artifact.GetCarriedByMonsterUid()];

				if (monster == null)
				{
					monster = gMDB[artifact.GetWornByMonsterUid()];
				}

				if (monster != null)
				{
					roomUid = monster.GetInRoomUid();
				}
				else
				{
					roomUid = artifact.GetInRoomUid();
				}
			}
			else
			{
				if (IsInRoom())
				{
					roomUid = Location;
				}
			}

			return roomUid;
		}

		public virtual long GetEmbeddedInRoomUid(bool recurse = false)
		{
			var roomUid = 0L;

			if (recurse)
			{
				var artifact = gADB[GetCarriedByContainerUid(recurse)];

				if (artifact == null)
				{
					artifact = this;
				}

				roomUid = artifact.GetEmbeddedInRoomUid();
			}
			else
			{
				if (IsEmbeddedInRoom())
				{
					roomUid = Location - 5000;
				}
			}

			return roomUid;
		}

		public virtual ICharacter GetCarriedByCharacter()
		{
			var characterUid = GetCarriedByCharacterUid();

			return gCHRDB[characterUid];
		}

		public virtual IMonster GetCarriedByMonster(bool recurse = false)
		{
			var monsterUid = GetCarriedByMonsterUid(recurse);

			return gMDB[monsterUid];
		}

		public virtual IArtifact GetCarriedByContainer(bool recurse = false)
		{
			var artifactUid = GetCarriedByContainerUid(recurse);

			return gADB[artifactUid];
		}

		public virtual ICharacter GetWornByCharacter()
		{
			var characterUid = GetWornByCharacterUid();

			return gCHRDB[characterUid];
		}

		public virtual IMonster GetWornByMonster(bool recurse = false)
		{
			var monsterUid = GetWornByMonsterUid(recurse);

			return gMDB[monsterUid];
		}

		public virtual IRoom GetInRoom(bool recurse = false)
		{
			var roomUid = GetInRoomUid(recurse);

			return gRDB[roomUid];
		}

		public virtual IRoom GetEmbeddedInRoom(bool recurse = false)
		{
			var roomUid = GetEmbeddedInRoomUid(recurse);

			return gRDB[roomUid];
		}

		public virtual ContainerType GetCarriedByContainerContainerType()
		{
			return Location > 1000 && Location < 2001 ? ContainerType.In :
						Location > 2000 && Location < 3001 ? ContainerType.On :
						Location > 3000 && Location < 4001 ? ContainerType.Under :
						Location > 4000 && Location < 5001 ? ContainerType.Behind : 
						(ContainerType)(-1);
		}

		public virtual void SetCarriedByCharacterUid(long characterUid)
		{
			Location = -characterUid - 2000;
		}

		public virtual void SetCarriedByMonsterUid(long monsterUid)
		{
			var gameState = gEngine.GetGameState();

			Location = monsterUid == long.MaxValue || (gameState != null && monsterUid == gameState.Cm) ? -1 : (-monsterUid - 1);
		}

		public virtual void SetCarriedByContainerUid(long containerUid, ContainerType containerType = ContainerType.In)
		{
			if (!Enum.IsDefined(typeof(ContainerType), containerType))
			{
				containerType = ContainerType.In;
			}

			Location = (containerUid + (((long)containerType * 1000) + 1000));
		}

		public virtual void SetWornByCharacterUid(long characterUid)
		{
			Location = -characterUid - 3000;
		}

		public virtual void SetWornByMonsterUid(long monsterUid)
		{
			var gameState = gEngine.GetGameState();

			Location = monsterUid == long.MaxValue || (gameState != null && monsterUid == gameState.Cm) ? -999 : (-monsterUid - 1000);
		}

		public virtual void SetInRoomUid(long roomUid)
		{
			Location = roomUid;
		}

		public virtual void SetEmbeddedInRoomUid(long roomUid)
		{
			Location = (roomUid + 5000);
		}

		public virtual void SetInLimbo()
		{
			Location = gEngine.LimboLocation;
		}

		public virtual void SetCarriedByCharacter(ICharacter character)
		{
			Debug.Assert(character != null);

			SetCarriedByCharacterUid(character.Uid);
		}

		public virtual void SetCarriedByMonster(IMonster monster)
		{
			SetCarriedByMonsterUid(monster != null ? monster.Uid : long.MaxValue);
		}

		public virtual void SetCarriedByContainer(IArtifact container, ContainerType containerType = ContainerType.In)
		{
			Debug.Assert(container != null);

			SetCarriedByContainerUid(container.Uid, containerType);
		}

		public virtual void SetWornByCharacter(ICharacter character)
		{
			Debug.Assert(character != null);

			SetWornByCharacterUid(character.Uid);
		}

		public virtual void SetWornByMonster(IMonster monster)
		{
			SetWornByMonsterUid(monster != null ? monster.Uid : long.MaxValue);
		}

		public virtual void SetInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			SetInRoomUid(room.Uid);
		}

		public virtual void SetEmbeddedInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			SetEmbeddedInRoomUid(room.Uid);
		}

		public virtual bool IsInRoomLit()
		{
			var room = GetInRoom();

			return room != null && room.IsLit();
		}

		public virtual bool IsInRoomViewable()
		{
			var room = GetInRoom();

			return room != null && room.IsViewable();
		}

		public virtual bool IsEmbeddedInRoomLit()
		{
			var room = GetEmbeddedInRoom();

			return room != null && room.IsLit();
		}

		public virtual bool IsEmbeddedInRoomViewable()
		{
			var room = GetEmbeddedInRoom();

			return room != null && room.IsViewable();
		}

		public virtual bool IsFieldStrength(long value)
		{
			return gEngine.IsArtifactFieldStrength(value);
		}

		public virtual long GetFieldStrength(long value)
		{
			return gEngine.GetArtifactFieldStrength(value);
		}

		public virtual bool IsWeapon(Weapon weapon)
		{
			var ac = GeneralWeapon;

			return ac != null && ac.IsWeapon(weapon);
		}

		public virtual bool IsAttackable(ref IArtifactCategory ac)
		{
			var artTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.InContainer, ArtifactType.DoorGate };

			ac = GetArtifactCategory(artTypes, false);

			return !gEngine.IsRulesetVersion(5) && ac != null;
		}

		public virtual bool IsRequestable()
		{
			return true;
		}

		public virtual bool IsUnmovable()
		{
			return gEngine.IsUnmovable(Weight);
		}

		public virtual bool IsUnmovable01()
		{
			return gEngine.IsUnmovable01(Weight);
		}

		public virtual bool IsArmor()
		{
			var ac = GetArtifactCategory(ArtifactType.Wearable);

			return ac != null && ac.Field1 > 1;
		}

		public virtual bool IsShield()
		{
			var ac = GetArtifactCategory(ArtifactType.Wearable);

			return ac != null && ac.Field1 == 1;
		}

		public virtual bool IsDisguisedMonster()
		{
			return DisguisedMonster != null;
		}

		public virtual bool IsStateDescSideNotes()
		{
			if (!string.IsNullOrWhiteSpace(StateDesc))
			{
				var regex = new Regex(@"\(.+\)");

				return regex.IsMatch(StateDesc);
			}
			else
			{
				return false;
			}
		}

		public virtual bool IsInContainerOpenedFromTop()
		{
			return true;
		}

		public virtual bool IsDoorGateInObviousExitsList()
		{
			return IsInRoom();
		}

		public virtual bool ShouldAllowBlastSkillGains()
		{
			var artTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.InContainer, ArtifactType.DoorGate };

			var ac = GetArtifactCategory(artTypes);		// TODO: should this mirror IsAttackable ???

			return ac != null && (ac.Type == ArtifactType.DisguisedMonster || ac.Type == ArtifactType.DeadBody || ac.GetBreakageStrength() >= 1000);
		}

		public virtual bool ShouldExposeInContentsWhenClosed()
		{
			return false;
		}

		public virtual bool ShouldExposeContentsToMonster(MonsterType monsterType = MonsterType.NonCharMonster, ContainerType containerType = ContainerType.In)
		{
			return containerType == ContainerType.On;
		}

		public virtual bool ShouldExposeContentsToRoom(ContainerType containerType = ContainerType.In)
		{
			return containerType == ContainerType.On;
		}

		public virtual bool ShouldAddContentsWhenCarried(ContainerType containerType = ContainerType.In)
		{
			return containerType == ContainerType.In || containerType == ContainerType.On;
		}

		public virtual bool ShouldAddContentsWhenWorn(ContainerType containerType = ContainerType.In)
		{
			return containerType == ContainerType.In || containerType == ContainerType.On;
		}

		public virtual bool ShouldAddContents(IArtifact artifact, ContainerType containerType = ContainerType.In)
		{
			Debug.Assert(artifact != null);

			return true;
		}

		public virtual bool ShouldAddToHeldWpnUids()
		{
			return false;
		}

		public virtual bool ShouldRevealContentsWhenMoved(ContainerType containerType = ContainerType.In)
		{
			return containerType == ContainerType.Under || containerType == ContainerType.Behind;
		}

		public virtual bool ShouldRevealContentsWhenMovedIntoLimbo(ContainerType containerType = ContainerType.In)
		{
			return false;
		}

		public virtual bool ShouldShowContentsWhenExamined()
		{
			return false;
		}

		public virtual bool ShouldShowContentsWhenOpened()
		{
			var room = GetInRoom(true);

			if (room == null)
			{
				room = GetEmbeddedInRoom(true);
			}

			return room != null ? room.IsViewable() : true;
		}

		public virtual bool ShouldShowVerboseNameContentsNameList()
		{
			return true;
		}

		public virtual bool ShouldShowVerboseNameStateDesc()
		{
			return true;
		}

		public virtual long GetMaxContentsNameListCount(ContainerType containerType = ContainerType.In)
		{
			return long.MaxValue;
		}

		public virtual string GetContainerSomethingDesc()
		{
			return "something";
		}

		public virtual string GetContainerSomeStuffDesc()
		{
			return "some stuff";
		}

		public virtual string GetDoorGateFleeDesc()
		{
			return "";
		}

		public virtual string GetProvidingLightDesc()
		{
			return " (providing light)";
		}

		public virtual string GetReadyWeaponDesc()
		{
			return " (ready weapon)";
		}

		public virtual string GetBrokenDesc()
		{
			return " (broken)";
		}

		public virtual string GetEmptyDesc()
		{
			return " (empty)";
		}

		public virtual T EvalPlural<T>(T singularValue, T pluralValue)
		{
			return gEngine.EvalPlural(IsPlural, singularValue, pluralValue);
		}

		public virtual T EvalInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsInRoomLit() ? lightValue : darkValue;
		}

		public virtual T EvalInRoomViewability<T>(T nonviewableValue, T viewableValue)
		{
			return IsInRoomViewable() ? viewableValue : nonviewableValue;
		}

		public virtual T EvalEmbeddedInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsEmbeddedInRoomLit() ? lightValue : darkValue;
		}

		public virtual T EvalEmbeddedInRoomViewability<T>(T nonviewableValue, T viewableValue)
		{
			return IsEmbeddedInRoomViewable() ? viewableValue : nonviewableValue;
		}

		public virtual IArtifactCategory GetArtifactCategory(ArtifactType artifactType)
		{
			IArtifactCategory result = null;

			if (_lastArtifactCategory != null && _lastArtifactCategory.Type == artifactType)
			{
				result = _lastArtifactCategory;
			}
			else if (GetCategory(0) != null && GetCategory(0).Type != ArtifactType.None)
			{
				result = Categories.FirstOrDefault(ac => ac != null && ac.Type == artifactType);
			}
			else
			{
				result = null;
			}

			_lastArtifactCategory = result;

			return result;
		}

		public virtual IArtifactCategory GetArtifactCategory(ArtifactType[] artifactTypes, bool categoryArrayPrecedence = true)
		{
			IArtifactCategory result = null;

			if (artifactTypes == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (GetCategory(0) != null && GetCategory(0).Type != ArtifactType.None)
			{
				if (categoryArrayPrecedence)
				{
					result = Categories.FirstOrDefault(ac => ac != null && artifactTypes.Contains(ac.Type));
				}
				else
				{
					foreach (var at in artifactTypes)
					{
						result = Categories.FirstOrDefault(ac => ac != null && ac.Type == at);

						if (result != null)
						{
							break;
						}
					}
				}
			}
			else
			{
				result = null;
			}

		Cleanup:

			_lastArtifactCategory = result;

			return result;
		}

		public virtual IList<IArtifactCategory> GetArtifactCategoryList(ArtifactType[] artifactTypes)
		{
			IList<IArtifactCategory> result = null;

			if (artifactTypes == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (GetCategory(0) != null && GetCategory(0).Type != ArtifactType.None)
			{
				result = Categories.Where(ac => ac != null && artifactTypes.Contains(ac.Type)).ToList();
			}
			else
			{
				result = new List<IArtifactCategory>() { null };
			}

		Cleanup:

			return result;
		}

		public virtual RetCode SetArtifactCategoryCount(long count)
		{
			RetCode rc;

			if (count < 1 || count > gEngine.NumArtifactCategories)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var categories01 = new IArtifactCategory[count];

			var i = 0L;

			if (categories01.Length < Categories.Length)
			{
				while (i < categories01.Length)
				{
					categories01[i] = GetCategory(i);

					i++;
				}
			}
			else
			{
				while (i < Categories.Length)
				{
					categories01[i] = GetCategory(i);

					i++;
				}

				while (i < categories01.Length)
				{
					categories01[i] = gEngine.CreateInstance<IArtifactCategory>(x =>
					{
						x.Parent = this;
					});

					i++;
				}
			}

			Categories = categories01;

		Cleanup:

			_lastArtifactCategory = null;

			return rc;
		}

		public virtual RetCode AddStateDesc(string stateDesc, bool dupAllowed = false)
		{
			StringBuilder buf;
			RetCode rc;
			int p;

			if (string.IsNullOrWhiteSpace(stateDesc))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			p = StateDesc.IndexOf(stateDesc, StringComparison.OrdinalIgnoreCase);

			if (dupAllowed || p == -1)
			{
				buf = new StringBuilder(gEngine.BufSize);

				buf.AppendFormat
				(
					"{0}{1}",
					StateDesc,
					stateDesc
				);

				StateDesc = buf.ToString();
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode RemoveStateDesc(string stateDesc)
		{
			StringBuilder buf;
			RetCode rc;
			int p, q;

			if (string.IsNullOrWhiteSpace(stateDesc))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			p = StateDesc.IndexOf(stateDesc, StringComparison.OrdinalIgnoreCase);

			if (p != -1)
			{
				buf = new StringBuilder(gEngine.BufSize);

				buf.Append(StateDesc);

				q = p + stateDesc.Length;

				if (!Char.IsWhiteSpace(buf[p]))
				{
					while (q < buf.Length && Char.IsWhiteSpace(buf[q]))
					{
						q++;
					}
				}

				buf.Remove(p, q - p);

				StateDesc = buf.TrimEnd().ToString();
			}

		Cleanup:

			return rc;
		}

		public virtual IList<IArtifact> GetContainedList(Func<IArtifact, bool> artifactFindFunc = null, ContainerType containerType = ContainerType.In, bool recurse = false)
		{
			var artifactList = new List<IArtifact>();

			if (GeneralContainer != null)
			{ 
				GetContainedList01(artifactList, artifactFindFunc, containerType, recurse);
			}

			return artifactList;
		}

		public virtual void GetContainedList01(IList<IArtifact> artifactList, Func<IArtifact, bool> artifactFindFunc = null, ContainerType containerType = ContainerType.In, bool recurse = false)
		{
			Debug.Assert(artifactList != null);

			var origArtifactFindFunc = artifactFindFunc;

			var allContainerTypes = !Enum.IsDefined(typeof(ContainerType), containerType);

			if (artifactFindFunc == null)
			{
				artifactFindFunc = a => a.IsCarriedByContainer(this) && (allContainerTypes || a.GetCarriedByContainerContainerType() == containerType);
			}

			var artifactList01 = gEngine.GetArtifactList(a => artifactFindFunc(a) && !artifactList.Contains(a));

			artifactList.AddRange(artifactList01);

			if (recurse && artifactList01.Count > 0)
			{
				foreach (var a in artifactList01)
				{
					if (a.GeneralContainer != null)
					{
						a.GetContainedList01(artifactList, origArtifactFindFunc, (ContainerType)(-1), recurse);
					}
				}
			}
		}

		public virtual RetCode GetContainerInfo(ref long count, ref long weight, ContainerType containerType = ContainerType.In, bool recurse = false)
		{
			RetCode rc;

			rc = RetCode.Success;

			if (GeneralContainer != null)
			{
				var queue = new Queue<IArtifact>();

				queue.AddRange(GetContainedList(null, containerType, recurse));

				while (queue.Any())
				{
					count++;

					var a = queue.Dequeue();

					if (!a.IsUnmovable01())
					{
						weight += a.Weight;
					}
				}
			}

			return rc;
		}

		public virtual string GetContainerContentsDesc(IRecordNameListArgs recordNameListArgs = null)
		{
			var result = "";

			if (GeneralContainer != null)
			{ 
				var artTypes = new ArtifactType[] { ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer };

				var buf = new StringBuilder(gEngine.BufSize);

				var ac = Categories.FirstOrDefault(ac01 => ac01 != null && artTypes.Contains(ac01.Type) && ac01.Field5 == (long)ContainerDisplayCode.ArtifactNameList && (ac01.Type != ArtifactType.InContainer || ac01.IsOpen() || ShouldExposeInContentsWhenClosed()) && GetContainedList(containerType: gEngine.GetContainerType(ac01.Type)).Count > 0);

				if (ac == null)
				{
					ac = Categories.FirstOrDefault(ac01 => ac01 != null && artTypes.Contains(ac01.Type) && ac01.Field5 == (long)ContainerDisplayCode.ArtifactNameSomeStuff && (ac01.Type != ArtifactType.InContainer || ac01.IsOpen() || ShouldExposeInContentsWhenClosed()) && GetContainedList(containerType: gEngine.GetContainerType(ac01.Type)).Count > 0);
				}

				if (ac == null)
				{
					ac = Categories.FirstOrDefault(ac01 => ac01 != null && artTypes.Contains(ac01.Type) && ac01.Field5 == (long)ContainerDisplayCode.SomethingSomeStuff && (ac01.Type != ArtifactType.InContainer || ac01.IsOpen() || ShouldExposeInContentsWhenClosed()) && GetContainedList(containerType: gEngine.GetContainerType(ac01.Type)).Count > 0);
				}

				if (ac != null)
				{
					var containerType = gEngine.GetContainerType(ac.Type);

					var contentsList = GetContainedList(containerType: containerType);

					var maxContentsNameListCount = GetMaxContentsNameListCount(containerType);

					if (contentsList.Count > 0)
					{
						if (recordNameListArgs == null)
						{
							recordNameListArgs = gEngine.CreateInstance<IRecordNameListArgs>(x =>
							{
								x.ArticleType = ArticleType.A;

								x.ShowCharOwned = !IsCarriedByMonster(MonsterType.CharMonster) && !IsWornByMonster(MonsterType.CharMonster);

								x.StateDescCode = StateDescDisplayCode.None;

								x.ShowContents = false;

								x.GroupCountOne = false;
							});
						}

						if (ac.Field5 == (long)ContainerDisplayCode.ArtifactNameList && contentsList.Count <= maxContentsNameListCount)
						{
							gEngine.GetRecordNameList(contentsList.Cast<IGameBase>().ToList(), recordNameListArgs, buf);
						}

						result = string.Format
						(
							" with {0} {1} {2}",
							ac.Field5 == (long)ContainerDisplayCode.ArtifactNameList && contentsList.Count <= maxContentsNameListCount ? buf.ToString() : contentsList.Count > 1 || contentsList[0].IsPlural ? GetContainerSomeStuffDesc() : ac.Field5 == (long)ContainerDisplayCode.ArtifactNameSomeStuff ? contentsList[0].GetDecoratedName("Name", recordNameListArgs.ArticleType, false, recordNameListArgs.ShowCharOwned, recordNameListArgs.StateDescCode == StateDescDisplayCode.AllStateDescs || (recordNameListArgs.StateDescCode == StateDescDisplayCode.SideNotesOnly && contentsList[0].IsStateDescSideNotes()), recordNameListArgs.ShowContents, recordNameListArgs.GroupCountOne) : GetContainerSomethingDesc(),
							gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"),
							EvalPlural("it", "them")
						);
					}
				}
			}

			return result;
		}

		public virtual void SyncArtifactCategories(IArtifactCategory modifiedAc)
		{
			Debug.Assert(modifiedAc != null);

			if (Categories != null)
			{
				switch (modifiedAc.Type)
				{
					case ArtifactType.InContainer:
					case ArtifactType.DoorGate:
					{
						foreach (var ac in Categories)
						{
							if (ac != null && ac != modifiedAc && ac.Type != ArtifactType.None)
							{
								ac.SyncFields = false;

								if (ac.IsLockable())
								{
									ac.SetKeyUid(modifiedAc.GetKeyUid());
								}

								if (ac.IsOpenable())
								{
									if (modifiedAc.GetBreakageStrength() > 0)
									{
										if (ac.Type == ArtifactType.InContainer || ac.Type == ArtifactType.DoorGate)
										{
											ac.SetBreakageStrength(modifiedAc.GetBreakageStrength());
										}
										else
										{
											ac.SetOpen(modifiedAc.IsOpen());
										}
									}
									else if (ac.GetBreakageStrength() <= 0 || modifiedAc.IsOpen())
									{
										ac.SetOpen(modifiedAc.IsOpen());
									}
								}

								ac.SyncFields = true;
							}
						}

						break;
					}

					case ArtifactType.Drinkable:
					case ArtifactType.Edible:
					{
						foreach (var ac in Categories)
						{
							if (ac != null && ac != modifiedAc && ac.Type != ArtifactType.None)
							{
								ac.SyncFields = false;

								if (ac.Type == ArtifactType.Drinkable || ac.Type == ArtifactType.Edible)
								{
									ac.Field1 = modifiedAc.Field1;

									ac.Field2 = modifiedAc.Field2;
								}

								if (ac.IsOpenable() && (ac.GetBreakageStrength() <= 0 || modifiedAc.IsOpen()))
								{
									ac.SetOpen(modifiedAc.IsOpen());
								}

								ac.SyncFields = true;
							}
						}

						break;
					}

					case ArtifactType.Readable:
					{
						foreach (var ac in Categories)
						{
							if (ac != null && ac != modifiedAc && ac.Type != ArtifactType.None)
							{
								ac.SyncFields = false;

								if (ac.IsOpenable() && (ac.GetBreakageStrength() <= 0 || modifiedAc.IsOpen()))
								{
									ac.SetOpen(modifiedAc.IsOpen());
								}

								ac.SyncFields = true;
							}
						}

						break;
					}

					case ArtifactType.BoundMonster:
					{
						foreach (var ac in Categories)
						{
							if (ac != null && ac != modifiedAc && ac.Type != ArtifactType.None)
							{
								ac.SyncFields = false;

								if (ac.Type == ArtifactType.DisguisedMonster)
								{
									ac.Field1 = modifiedAc.Field1;
								}

								if (ac.IsLockable())
								{
									ac.SetKeyUid(modifiedAc.GetKeyUid());
								}

								ac.SyncFields = true;
							}
						}

						break;
					}

					case ArtifactType.DisguisedMonster:
					{
						foreach (var ac in Categories)
						{
							if (ac != null && ac != modifiedAc && ac.Type != ArtifactType.None)
							{
								ac.SyncFields = false;

								if (ac.Type == ArtifactType.BoundMonster)
								{
									ac.Field1 = modifiedAc.Field1;
								}

								ac.SyncFields = true;
							}
						}

						break;
					}
				}
			}
		}

		#endregion

		#region Class Artifact

		public Artifact()
		{
			StateDesc = "";

			Categories = new IArtifactCategory[]
			{
				gEngine.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				gEngine.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				gEngine.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				gEngine.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				})
			};
		}

		#endregion

		#endregion
	}
}
