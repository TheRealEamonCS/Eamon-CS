
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using TheVileGrimoireOfJaldial.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		[FieldName(1121)]
		public virtual long FoggyRoomWeatherIntensity { get; set; }

		[FieldName(1122)]
		public virtual long PlayerResurrections { get; set; }

		[FieldName(1123)]
		public virtual long PlayerHardinessPointsDrained { get; set; }

		[FieldName(1124)]
		public virtual long BloodnettleVictimUid { get; set; }

		[FieldName(1125)]
		public virtual long EfreetiSummons { get; set; }

		[FieldName(1126)]
		public virtual long LightningBolts { get; set; }

		[FieldName(1127)]
		public virtual long IceBolts { get; set; }

		[FieldName(1128)]
		public virtual long MentalBlasts { get; set; }

		[FieldName(1129)]
		public virtual long MysticMissiles { get; set; }

		[FieldName(1130)]
		public virtual long FireBalls { get; set; }

		[FieldName(1131)]
		public virtual long ClumsySpells { get; set; }

		[FieldName(1132)]
		public virtual long TorchRounds { get; set; }

		[FieldName(1133)]
		public virtual long Minute { get; set; }

		[FieldName(1134)]
		public virtual long Hour { get; set; }

		[FieldName(1135)]
		public virtual long Day { get; set; }

		[FieldName(1136)]
		public virtual long WeatherIntensity { get; set; }

		[FieldName(1137)]
		public virtual long WeatherDuration { get; set; }

		public virtual WeatherType WeatherType { get; set; }

		public virtual long WeatherFreqPct { get; set; }

		public virtual long EncounterFreqPct { get; set; }

		public virtual long FlavorFreqPct { get; set; }

		public virtual bool GriffinAngered { get; set; }

		public virtual bool GiantCrayfishKilled { get; set; }

		public virtual bool WaterWeirdKilled { get; set; }

		public virtual bool EfreetiKilled { get; set; }

		public virtual bool AmoebaAppeared { get; set; }

		public virtual bool ShowCombatDamage { get; set; }

		public virtual bool ExitDirNames { get; set; }

		public virtual bool[] SecretDoors { get; set; }

		public virtual IDictionary<long, long> ParalyzedTargets { get; set; }

		public virtual IDictionary<long, IList<long>> ClumsyTargets { get; set; }

		public virtual bool IsNightTime()
		{
			return !IsDayTime();
		}

		public virtual bool IsDayTime()
		{
			return Hour > 6 && Hour < 19;
		}

		public virtual bool IsRaining()
		{
			return WeatherType == WeatherType.Rain;
		}

		public virtual bool IsFoggy()
		{
			return WeatherType == WeatherType.Fog;
		}

		public virtual bool IsFoggyHours()
		{
			return Hour < 10 || Hour > 20;
		}

		public virtual bool GetSecretDoor(long index)
		{
			return SecretDoors[index];
		}

		public virtual void SetSecretDoor(long index, bool value)
		{
			SecretDoors[index] = value;
		}

		public virtual void SetFoggyRoomWeatherIntensity(Framework.IRoom room)
		{
			Debug.Assert(room != null);

			var rl = gEngine.RollDice(1, 100, 0);

			if (room.IsFoggyRoom())
			{
				if (FoggyRoomWeatherIntensity == 0)
				{
					FoggyRoomWeatherIntensity++;
				}
				else if (FoggyRoomWeatherIntensity == 1)
				{
					if (FoggyRoomWeatherIntensity < WeatherIntensity && rl > 25)
					{
						FoggyRoomWeatherIntensity++;
					}
				}
				else if (FoggyRoomWeatherIntensity < WeatherIntensity)
				{
					if (rl > 63)
					{
						FoggyRoomWeatherIntensity++;
					}
					else if (rl > 38)
					{
						FoggyRoomWeatherIntensity--;
					}
				}
				else
				{
					if (rl > 75)
					{
						FoggyRoomWeatherIntensity--;
					}
				}
			}
			else
			{
				FoggyRoomWeatherIntensity = 0;
			}
		}

		public GameState()
		{
			Minute = Constants.StartMinute;

			Hour = Constants.StartHour;

			WeatherFreqPct = 100;

			EncounterFreqPct = 100;

			FlavorFreqPct = 100;

			SecretDoors = new bool[13];

			ParalyzedTargets = new Dictionary<long, long>();

			ClumsyTargets = new Dictionary<long, IList<long>>();
		}
	}
}
