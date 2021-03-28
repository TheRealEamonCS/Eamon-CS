
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
		public virtual bool GriffinAngered { get; set; }

		public virtual bool GiantCrayfishKilled { get; set; }

		public virtual bool WaterWeirdKilled { get; set; }

		public virtual bool EfreetiKilled { get; set; }

		public virtual bool AmoebaAppeared { get; set; }

		public virtual bool ShowCombatDamage { get; set; }

		public virtual bool ExitDirNames { get; set; }

		public virtual bool[] SecretDoors { get; set; }

		public virtual long WeatherFreqPct { get; set; }

		public virtual long EncounterFreqPct { get; set; }

		public virtual long FlavorFreqPct { get; set; }

		public virtual long FoggyRoomWeatherIntensity { get; set; }

		public virtual long PlayerResurrections { get; set; }

		public virtual long PlayerHardinessPointsDrained { get; set; }

		public virtual long BloodnettleVictimUid { get; set; }

		public virtual long EfreetiSummons { get; set; }

		public virtual long LightningBolts { get; set; }

		public virtual long IceBolts { get; set; }

		public virtual long MentalBlasts { get; set; }

		public virtual long MysticMissiles { get; set; }

		public virtual long FireBalls { get; set; }

		public virtual long ClumsySpells { get; set; }

		public virtual long TorchRounds { get; set; }

		public virtual long Minute { get; set; }

		public virtual long Hour { get; set; }

		public virtual long Day { get; set; }

		public virtual long WeatherIntensity { get; set; }

		public virtual long WeatherDuration { get; set; }

		public virtual WeatherType WeatherType { get; set; }

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

		public virtual bool GetSecretDoors(long index)
		{
			return SecretDoors[index];
		}

		public virtual void SetSecretDoors(long index, bool value)
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
			SecretDoors = new bool[13];

			WeatherFreqPct = 100;

			EncounterFreqPct = 100;

			FlavorFreqPct = 100;

			Hour = Constants.StartHour;

			Minute = Constants.StartMinute;

			ParalyzedTargets = new Dictionary<long, long>();

			ClumsyTargets = new Dictionary<long, IList<long>>();
		}
	}
}
