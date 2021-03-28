
// Script.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Script : GameBase, IScript
	{
		public virtual long SortOrder { get; set; }

		public virtual long TriggerUid { get; set; }

		public virtual ScriptType Type { get; set; }

		public virtual long Field1 { get; set; }

		public virtual long Field2 { get; set; }

		public virtual long Field3 { get; set; }

		public virtual long Field4 { get; set; }

		public virtual long Field5 { get; set; }

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeScriptUid(Uid);

				Uid = 0;
			}
		}

		public virtual int CompareTo(IScript script)
		{
			return this.Uid.CompareTo(script.Uid);
		}

		public virtual void Execute()
		{
			switch(Type)
			{
				case ScriptType.PrintRandomEffect:
				{
					var effectUidList = new List<long>();

					if (Field1 > 0)
					{
						effectUidList.Add(Field1);
					}

					if (Field2 > 0)
					{
						effectUidList.Add(Field2);
					}

					if (Field3 > 0)
					{
						effectUidList.Add(Field3);
					}

					if (Field4 > 0)
					{
						effectUidList.Add(Field4);
					}

					if (Field5 > 0)
					{
						effectUidList.Add(Field5);
					}

					if (effectUidList.Count > 0)
					{
						var idx = gEngine.RollDice(1, effectUidList.Count, -1);

						gEngine.PrintEffectDesc(effectUidList[(int)idx]);
					}

					break;
				}
			}
		}

		public Script()
		{
			
		}
	}
}
