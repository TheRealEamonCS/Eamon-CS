
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
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.PluginContext;

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

				if (gEngine != null && !gEngine.IsUnmovable01(w) && GeneralContainer != null)
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
				if (Globals.EnableGameOverrides && Globals.RevealContentCounter > 0 && _location != value && GeneralContainer != null && !Globals.RevealContentArtifactList.Contains(this))
				{
					var origLocation = _location;

					Globals.RevealContentArtifactList.Add(this);

					Globals.RevealContentFuncList.Add(() =>
					{
						if (gEngine != null && gEngine.RevealContainerContentsFunc != null)
						{ 
							gEngine.RevealContainerContentsFunc(this, origLocation, true);
						}
					});
				}

				_location = value;
			}
		}

		[ExcludeFromSerialization]
		public virtual ArtifactType Type
		{
			get
			{
				var ac = GetCategories(0);

				return ac != null ? ac.Type : ArtifactType.None;
			}

			set
			{
				var ac = GetCategories(0);

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
				var ac = GetCategories(0);

				return ac != null ? ac.Field1 : 0;
			}

			set
			{
				var ac = GetCategories(0);

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
				var ac = GetCategories(0);

				return ac != null ? ac.Field2 : 0;
			}

			set
			{
				var ac = GetCategories(0);

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
				var ac = GetCategories(0);

				return ac != null ? ac.Field3 : 0;
			}

			set
			{
				var ac = GetCategories(0);

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
				var ac = GetCategories(0);

				return ac != null ? ac.Field4 : 0;
			}

			set
			{
				var ac = GetCategories(0);

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
				var ac = GetCategories(0);

				return ac != null ? ac.Field5 : 0;
			}

			set
			{
				var ac = GetCategories(0);

				if (ac != null)
				{
					ac.Field5 = value;
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
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeArtifactUid(Uid);

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
					ac.SyncArtifactCategories();
				}
			}
		}

		public override string GetPluralName(string fieldName, StringBuilder buf = null)
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

			if (buf == null)
			{
				buf = Globals.Buf;
			}

			buf.Clear();

			effectUid = gEngine.GetPluralTypeEffectUid(PluralType);

			effect = gEDB[effectUid];

			if (effect != null)
			{
				buf.Append(effect.Desc.Substring(0, Math.Min(Constants.ArtNameLen, effect.Desc.Length)).Trim());
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

		public override string GetDecoratedName(string fieldName, ArticleType articleType, bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool groupCountOne = false, StringBuilder buf = null)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			if (buf == null)
			{
				buf = Globals.Buf;
			}

			buf.Clear();

			switch (articleType)
			{
				case ArticleType.None:

					buf.AppendFormat
					(
						"{0}{1}{2}",
						EvalPlural(Name, GetPluralName(fieldName, new StringBuilder(Constants.BufSize))),
						showStateDesc && StateDesc.Length > 0 && !StateDesc.OmitStateDescSpace() ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
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
						EvalPlural(Name, GetPluralName(fieldName, new StringBuilder(Constants.BufSize))),
						showStateDesc && StateDesc.Length > 0 && !StateDesc.OmitStateDescSpace() ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
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
						EvalPlural(Name, GetPluralName(fieldName, new StringBuilder(Constants.BufSize))),
						showStateDesc && StateDesc.Length > 0 && !StateDesc.OmitStateDescSpace() ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
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

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (showName)
			{
				buf.AppendFormat("{0}[{1}]",
					Environment.NewLine,
					GetArticleName(true, buf: new StringBuilder(Constants.BufSize)));
			}

			if (!string.IsNullOrWhiteSpace(Desc))
			{
				buf.AppendFormat("{0}{1}", Environment.NewLine, Desc);
			}

			if (showName || !string.IsNullOrWhiteSpace(Desc))
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

		public virtual IArtifactCategory GetCategories(long index)
		{
			_lastArtifactCategory = Categories[index];

			return _lastArtifactCategory;
		}

		public virtual string GetSynonyms(long index)
		{
			return Synonyms[index];
		}

		public virtual void SetCategories(long index, IArtifactCategory value)
		{
			Categories[index] = value;
		}

		public virtual void SetSynonyms(long index, string value)
		{
			Synonyms[index] = value;
		}

		public virtual bool IsCarriedByCharacter(bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.IsCarriedByCharacter(recurse) : Location == -1;
		}

		public virtual bool IsCarriedByMonster(bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.IsCarriedByMonster(recurse) : Location < -1 && Location > -999;
		}

		public virtual bool IsCarriedByContainer()
		{
			return Location > 1000 && Location < 5001;
		}

		public virtual bool IsWornByCharacter(bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.IsWornByCharacter(recurse) : Location == -999;
		}

		public virtual bool IsWornByMonster(bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.IsWornByMonster(recurse) : Location < -1000;
		}

		public virtual bool IsReadyableByCharacter()
		{
			return GeneralWeapon != null;
		}

		public virtual bool IsInRoom(bool recurse = false)
		{
			var gameState = recurse ? Globals?.Engine?.GetGameState() : null;

			var charMonster = recurse && gameState != null ? gMDB[gameState.Cm] : null;

			return recurse && (IsCarriedByCharacter(recurse) || IsWornByCharacter(recurse)) && charMonster != null ? charMonster.IsInRoom() :
						recurse && GetCarriedByMonster(recurse) != null ? GetCarriedByMonster(recurse).IsInRoom() : 
						recurse && GetWornByMonster(recurse) != null ? GetWornByMonster(recurse).IsInRoom() : 
						recurse && GetCarriedByContainer(recurse) != null ? GetCarriedByContainer(recurse).IsInRoom() :
						Location > 0 && Location < 1001;
		}

		public virtual bool IsEmbeddedInRoom(bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.IsEmbeddedInRoom(recurse) : Location > 5000 && Location < 6001;
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToMonster(bool recurse = false)
		{
			var artifact = GetCarriedByContainer();

			var containerType = GetCarriedByContainerContainerType();

			return artifact != null && artifact.ShouldExposeContentsToMonster(containerType) && (containerType != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed()) && (!recurse || artifact.GetCarriedByContainer() == null || artifact.IsCarriedByContainerContainerTypeExposedToMonster(recurse));
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToRoom(bool recurse = false)
		{
			var artifact = GetCarriedByContainer();

			var containerType = GetCarriedByContainerContainerType();

			return artifact != null && artifact.ShouldExposeContentsToRoom(containerType) && (containerType != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed()) && (!recurse || artifact.GetCarriedByContainer() == null || artifact.IsCarriedByContainerContainerTypeExposedToRoom(recurse));
		}

		public virtual bool IsInLimbo(bool recurse = false)
		{
			var gameState = recurse ? Globals?.Engine?.GetGameState() : null;

			var charMonster = recurse && gameState != null ? gMDB[gameState.Cm] : null;

			return recurse && (IsCarriedByCharacter(recurse) || IsWornByCharacter(recurse)) && charMonster != null ? charMonster.IsInLimbo() :
						recurse && GetCarriedByMonster(recurse) != null ? GetCarriedByMonster(recurse).IsInLimbo() :
						recurse && GetWornByMonster(recurse) != null ? GetWornByMonster(recurse).IsInLimbo() :
						recurse && GetCarriedByContainer(recurse) != null ? GetCarriedByContainer(recurse).IsInLimbo() :
						Location == Constants.LimboLocation;
		}

		public virtual bool IsCarriedByMonsterUid(long monsterUid, bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.IsCarriedByMonsterUid(monsterUid, recurse) : Location == (-monsterUid - 1);
		}

		public virtual bool IsCarriedByContainerUid(long containerUid, bool recurse = false)
		{
			if (recurse)
			{
				var artifact = GetCarriedByContainer();

				while (artifact != null && artifact.Uid != containerUid)
				{
					var artifact01 = artifact.GetCarriedByContainer();

					if (artifact01 != null)
					{
						artifact = artifact01;
					}
					else
					{
						break;
					}
				}

				return artifact != null && artifact.Uid == containerUid;
			}
			else
			{
				var containerType = GetCarriedByContainerContainerType();

				return Enum.IsDefined(typeof(ContainerType), containerType) && Location == (containerUid + (((long)containerType * 1000) + 1000));
			}
		}

		public virtual bool IsWornByMonsterUid(long monsterUid, bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.IsWornByMonsterUid(monsterUid, recurse) : Location == (-monsterUid - 1000);
		}

		public virtual bool IsReadyableByMonsterUid(long monsterUid)
		{
			return GeneralWeapon != null;
		}

		public virtual bool IsInRoomUid(long roomUid, bool recurse = false)
		{
			var gameState = recurse ? Globals?.Engine?.GetGameState() : null;

			var charMonster = recurse && gameState != null ? gMDB[gameState.Cm] : null;

			return recurse && (IsCarriedByCharacter(recurse) || IsWornByCharacter(recurse)) && charMonster != null ? charMonster.IsInRoomUid(roomUid) :
						recurse && GetCarriedByMonster(recurse) != null ? GetCarriedByMonster(recurse).IsInRoomUid(roomUid) :
						recurse && GetWornByMonster(recurse) != null ? GetWornByMonster(recurse).IsInRoomUid(roomUid) :
						recurse && GetCarriedByContainer(recurse) != null ? GetCarriedByContainer(recurse).IsInRoomUid(roomUid) :
						Location == roomUid;
		}

		public virtual bool IsEmbeddedInRoomUid(long roomUid, bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.IsEmbeddedInRoomUid(roomUid, recurse) : Location == (roomUid + 5000);
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToCharacter(bool recurse = false)
		{
			var artifact = GetCarriedByContainer();

			var containerType = GetCarriedByContainerContainerType();

			return artifact != null && artifact.ShouldExposeContentsToCharacter(containerType) && (containerType != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed()) && (artifact.IsCarriedByCharacter() || (recurse && artifact.GetCarriedByContainer() != null && artifact.IsCarriedByContainerContainerTypeExposedToCharacter(recurse)));
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToMonsterUid(long monsterUid, bool recurse = false)
		{
			var artifact = GetCarriedByContainer();

			var containerType = GetCarriedByContainerContainerType();

			return artifact != null && artifact.ShouldExposeContentsToMonster(containerType) && (containerType != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed()) && (artifact.IsCarriedByMonsterUid(monsterUid) || (recurse && artifact.GetCarriedByContainer() != null && artifact.IsCarriedByContainerContainerTypeExposedToMonsterUid(monsterUid, recurse)));
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToRoomUid(long roomUid, bool recurse = false)
		{
			var artifact = GetCarriedByContainer();

			var containerType = GetCarriedByContainerContainerType();

			return artifact != null && artifact.ShouldExposeContentsToRoom(containerType) && (containerType != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed()) && (artifact.IsInRoomUid(roomUid) || (recurse && artifact.GetCarriedByContainer() != null && artifact.IsCarriedByContainerContainerTypeExposedToRoomUid(roomUid, recurse)));
		}

		public virtual bool IsCarriedByMonster(IMonster monster, bool recurse = false)
		{
			Debug.Assert(monster != null);

			return IsCarriedByMonsterUid(monster.Uid, recurse);
		}

		public virtual bool IsCarriedByContainer(IArtifact container, bool recurse = false)
		{
			Debug.Assert(container != null);

			return IsCarriedByContainerUid(container.Uid, recurse);
		}

		public virtual bool IsWornByMonster(IMonster monster, bool recurse = false)
		{
			Debug.Assert(monster != null);

			return IsWornByMonsterUid(monster.Uid, recurse);
		}

		public virtual bool IsReadyableByMonster(IMonster monster)
		{
			Debug.Assert(monster != null);

			return IsReadyableByMonsterUid(monster.Uid);
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
			Debug.Assert(monster != null);

			return IsCarriedByContainerContainerTypeExposedToMonsterUid(monster.Uid, recurse);
		}

		public virtual bool IsCarriedByContainerContainerTypeExposedToRoom(IRoom room, bool recurse = false)
		{
			Debug.Assert(room != null);

			return IsCarriedByContainerContainerTypeExposedToRoomUid(room.Uid, recurse);
		}

		public virtual long GetCarriedByMonsterUid(bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.GetCarriedByMonsterUid(recurse) : IsCarriedByMonster() ? -Location - 1 : 0;
		}

		public virtual long GetCarriedByContainerUid(bool recurse = false)
		{
			if (recurse)
			{
				var artifact = GetCarriedByContainer();

				while (artifact != null)
				{
					var artifact01 = artifact.GetCarriedByContainer();

					if (artifact01 != null)
					{
						artifact = artifact01;
					}
					else
					{
						break;
					}
				}

				return artifact != null ? artifact.Uid : 0;
			}
			else
			{
				var containerType = GetCarriedByContainerContainerType();

				return IsCarriedByContainer() && Enum.IsDefined(typeof(ContainerType), containerType) ? Location - (((long)containerType * 1000) + 1000) : 0;
			}
		}

		public virtual long GetWornByMonsterUid(bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.GetWornByMonsterUid(recurse) : IsWornByMonster() ? -Location - 1000 : 0;
		}

		public virtual long GetInRoomUid(bool recurse = false)
		{
			var gameState = recurse ? Globals?.Engine?.GetGameState() : null;

			var charMonster = recurse && gameState != null ? gMDB[gameState.Cm] : null;

			return recurse && (IsCarriedByCharacter(recurse) || IsWornByCharacter(recurse)) && charMonster != null ? charMonster.GetInRoomUid() :
						recurse && GetCarriedByMonster(recurse) != null ? GetCarriedByMonster(recurse).GetInRoomUid() :
						recurse && GetWornByMonster(recurse) != null ? GetWornByMonster(recurse).GetInRoomUid() :
						recurse && GetCarriedByContainer(recurse) != null ? GetCarriedByContainer(recurse).GetInRoomUid() :
						IsInRoom() ? Location : 0;
		}

		public virtual long GetEmbeddedInRoomUid(bool recurse = false)
		{
			var artifact = recurse ? GetCarriedByContainer() : null;

			return artifact != null ? artifact.GetEmbeddedInRoomUid(recurse) : IsEmbeddedInRoom() ? Location - 5000 : 0;
		}

		public virtual IMonster GetCarriedByMonster(bool recurse = false)
		{
			var uid = GetCarriedByMonsterUid(recurse);

			return gMDB[uid];
		}

		public virtual IArtifact GetCarriedByContainer(bool recurse = false)
		{
			var uid = GetCarriedByContainerUid(recurse);

			return gADB[uid];
		}

		public virtual IMonster GetWornByMonster(bool recurse = false)
		{
			var uid = GetWornByMonsterUid(recurse);

			return gMDB[uid];
		}

		public virtual IRoom GetInRoom(bool recurse = false)
		{
			var uid = GetInRoomUid(recurse);

			return gRDB[uid];
		}

		public virtual IRoom GetEmbeddedInRoom(bool recurse = false)
		{
			var uid = GetEmbeddedInRoomUid(recurse);

			return gRDB[uid];
		}

		public virtual ContainerType GetCarriedByContainerContainerType()
		{
			return Location > 1000 && Location < 2001 ? ContainerType.In :
						Location > 2000 && Location < 3001 ? ContainerType.On :
						Location > 3000 && Location < 4001 ? ContainerType.Under :
						Location > 4000 && Location < 5001 ? ContainerType.Behind : 
						(ContainerType)(-1);
		}

		public virtual void SetCarriedByCharacter()
		{
			Location = -1;
		}

		public virtual void SetCarriedByMonsterUid(long monsterUid)
		{
			Location = (-monsterUid - 1);
		}

		public virtual void SetCarriedByContainerUid(long containerUid, ContainerType containerType = ContainerType.In)
		{
			if (!Enum.IsDefined(typeof(ContainerType), containerType))
			{
				containerType = ContainerType.In;
			}

			Location = (containerUid + (((long)containerType * 1000) + 1000));
		}

		public virtual void SetWornByCharacter()
		{
			Location = -999;
		}

		public virtual void SetWornByMonsterUid(long monsterUid)
		{
			Location = (-monsterUid - 1000);
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
			Location = Constants.LimboLocation;
		}

		public virtual void SetCarriedByMonster(IMonster monster)
		{
			Debug.Assert(monster != null);

			SetCarriedByMonsterUid(monster.Uid);
		}

		public virtual void SetCarriedByContainer(IArtifact container, ContainerType containerType = ContainerType.In)
		{
			Debug.Assert(container != null);

			SetCarriedByContainerUid(container.Uid, containerType);
		}

		public virtual void SetWornByMonster(IMonster monster)
		{
			Debug.Assert(monster != null);

			SetWornByMonsterUid(monster.Uid);
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

		public virtual bool IsEmbeddedInRoomLit()
		{
			var room = GetEmbeddedInRoom();

			return room != null && room.IsLit();
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

		public virtual bool IsAttackable()
		{
			var artTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.InContainer, ArtifactType.DoorGate };

			var ac = GetArtifactCategory(artTypes);

			return ac != null && (ac.Type == ArtifactType.DisguisedMonster || ac.Type == ArtifactType.DeadBody || ac.GetBreakageStrength() >= 1000);
		}

		public virtual bool IsAttackable01(ref IArtifactCategory ac)
		{
			var artTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.InContainer, ArtifactType.DoorGate };

			ac = GetArtifactCategory(artTypes, false);

			return !Globals.IsRulesetVersion(5) && ac != null;
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
				var regex = new Regex(@".*\(.+\)");

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

		public virtual bool ShouldExposeInContentsWhenClosed()
		{
			return false;
		}

		public virtual bool ShouldExposeContentsToCharacter(ContainerType containerType = ContainerType.In)
		{
			return containerType == ContainerType.On;
		}

		public virtual bool ShouldExposeContentsToMonster(ContainerType containerType = ContainerType.In)
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

		public virtual bool ShouldRevealContentsWhenMoved(ContainerType containerType = ContainerType.In)
		{
			return containerType == ContainerType.Under || containerType == ContainerType.Behind;
		}

		public virtual bool ShouldRevealContentsWhenMovedIntoLimbo(ContainerType containerType = ContainerType.In)
		{
			return containerType != ContainerType.In;
		}

		public virtual bool ShouldShowContentsWhenExamined()
		{
			return false;
		}

		public virtual bool ShouldShowContentsWhenOpened()
		{
			return true;
		}

		public virtual string GetDoorGateFleeDesc()
		{
			return "";
		}

		public virtual string GetProvidingLightDesc()
		{
			return "(providing light)";
		}

		public virtual string GetReadyWeaponDesc()
		{
			return "(ready weapon)";
		}

		public virtual string GetBrokenDesc()
		{
			return "(broken)";
		}

		public virtual string GetEmptyDesc()
		{
			return "(empty)";
		}

		public virtual T EvalPlural<T>(T singularValue, T pluralValue)
		{
			return gEngine.EvalPlural(IsPlural, singularValue, pluralValue);
		}

		public virtual T EvalInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsInRoomLit() ? lightValue : darkValue;
		}

		public virtual T EvalEmbeddedInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsEmbeddedInRoomLit() ? lightValue : darkValue;
		}

		public virtual IArtifactCategory GetArtifactCategory(ArtifactType artifactType)
		{
			IArtifactCategory result = null;

			if (_lastArtifactCategory != null && _lastArtifactCategory.Type == artifactType)
			{
				result = _lastArtifactCategory;
			}
			else if (GetCategories(0) != null && GetCategories(0).Type != ArtifactType.None)
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

			if (GetCategories(0) != null && GetCategories(0).Type != ArtifactType.None)
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

			if (GetCategories(0) != null && GetCategories(0).Type != ArtifactType.None)
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

			if (count < 1 || count > Constants.NumArtifactCategories)
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
					categories01[i] = GetCategories(i);

					i++;
				}
			}
			else
			{
				while (i < Categories.Length)
				{
					categories01[i] = GetCategories(i);

					i++;
				}

				while (i < categories01.Length)
				{
					categories01[i] = Globals.CreateInstance<IArtifactCategory>(x =>
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
				buf = new StringBuilder(Constants.BufSize);

				buf.AppendFormat
				(
					"{0}{1}{2}",
					StateDesc,
					StateDesc.Length > 0 && !stateDesc.OmitStateDescSpace() ? " " : "",
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
				buf = new StringBuilder(Constants.BufSize);

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

				StateDesc = buf.ToString().Trim();
			}

		Cleanup:

			return rc;
		}

		public virtual IList<IArtifact> GetContainedList(Func<IArtifact, bool> artifactFindFunc = null, ContainerType containerType = ContainerType.In, bool recurse = false)
		{
			var artifactList = new List<IArtifact>();

			gEngine.ArtifactContainedList.Clear();

			GetContainedList01(artifactFindFunc, containerType, recurse);

			artifactList.AddRange(gEngine.ArtifactContainedList);

			return artifactList;
		}

		public virtual RetCode GetContainerInfo(ref long count, ref long weight, ContainerType containerType = ContainerType.In, bool recurse = false)
		{
			RetCode rc;

			rc = RetCode.Success;

			var queue = new Queue<IArtifact>();

			if (GeneralContainer != null)
			{
				queue.AddRange(GetContainedList(null, containerType, recurse));
			}

			while (queue.Any())
			{
				count++;

				var a = queue.Dequeue();

				if (!a.IsUnmovable01())
				{
					weight += a.Weight;
				}
			}

			return rc;
		}

		#endregion

		#region Class Artifact

		public virtual void GetContainedList01(Func<IArtifact, bool> artifactFindFunc = null, ContainerType containerType = ContainerType.In, bool recurse = false)
		{
			var origArtifactFindFunc = artifactFindFunc;

			var allContainerTypes = !Enum.IsDefined(typeof(ContainerType), containerType);

			if (artifactFindFunc == null)
			{
				artifactFindFunc = a => a.IsCarriedByContainer(this) && (allContainerTypes || a.GetCarriedByContainerContainerType() == containerType);
			}

			var artifactList = gEngine.GetArtifactList(a => artifactFindFunc(a) && !gEngine.ArtifactContainedList.Contains(a));

			gEngine.ArtifactContainedList.AddRange(artifactList);

			if (recurse && artifactList.Count > 0)
			{
				foreach (var a in artifactList)
				{
					var a01 = a as Artifact;

					if (a.GeneralContainer != null && a01 != null)
					{
						a01.GetContainedList01(origArtifactFindFunc, (ContainerType)(-1), recurse);
					}
				}
			}
		}

		public Artifact()
		{
			StateDesc = "";

			Categories = new IArtifactCategory[]
			{
				Globals.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				})
			};
		}

		#endregion

		#endregion
	}
}
