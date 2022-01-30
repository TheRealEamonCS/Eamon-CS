
// Trigger.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Trigger : GameBase, ITrigger
	{
		[FieldName(620)]
		public virtual long Occurrences { get; set; }

		[FieldName(640)]
		public virtual long SortOrder { get; set; }

		[FieldName(660)]
		public virtual TriggerType Type { get; set; }

		[FieldName(680)]
		public virtual long Field1 { get; set; }

		[FieldName(700)]
		public virtual long Field2 { get; set; }

		[FieldName(720)]
		public virtual long Field3 { get; set; }

		[FieldName(740)]
		public virtual long Field4 { get; set; }

		[FieldName(760)]
		public virtual long Field5 { get; set; }

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeTriggerUid(Uid);

				Uid = 0;
			}
		}

		public virtual int CompareTo(ITrigger trigger)
		{
			return this.Uid.CompareTo(trigger.Uid);
		}

		public virtual bool IsActive()
		{
			var result = Occurrences > 0 || Occurrences == -1;

			if (result)
			{
				var gameState = Globals?.Engine?.GetGameState();

				result = gameState != null;

				switch (Type)
				{
					case TriggerType.Interval:
					{
						if (result)
						{
							result = gameState.CurrTurn == Field1 || (gameState.CurrTurn > Field1 && ((gameState.CurrTurn - Field1) % Field2) == 0);
						}

						if (result)
						{
							var rl = gEngine.RollDice(1, 100, 0);

							result = rl <= Field3;
						}

						break;
					}
				}
			}

			return result;
		}

		public virtual IList<IScript> GetScriptList()
		{
			var scriptList = gSDB.Records.Where(s => s.TriggerUid == Uid).OrderBy(s02 => s02.SortOrder).ToList();

			return scriptList;
		}

		public Trigger()
		{
			
		}
	}
}
