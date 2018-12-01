using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace HarvestSync
{
	class HarvestManager_MapComponent : MapComponent
	{
		private HashSet<Zone> fullyGrownZones = new HashSet<Zone>();

		private Dictionary<Zone, HarvestSetting> harvestSettings = new Dictionary<Zone, HarvestSetting>();
		private List<Zone> areasForSaving = new List<Zone>();
		private List<HarvestSetting> settingsForSaving = new List<HarvestSetting>();

		private Dictionary<Zone, float> harvestProportions = new Dictionary<Zone, float>();
		private List<Zone> areasWithProportionsForSaving = new List<Zone>();
		private List<float> proportionsForSaving = new List<float>();

		public HarvestManager_MapComponent(Map map) : base(map)
		{
		}

		public override void FinalizeInit()
		{
			RecalculateFullyGrownZones();
		}

		public override void ExposeData()
		{
			Scribe_Collections.Look(ref harvestSettings, "harvestSyncHarvestSettings", LookMode.Reference, LookMode.Value, ref areasForSaving, ref settingsForSaving);
			Scribe_Collections.Look(ref harvestProportions, "harvestSyncHarvestProportyions", LookMode.Reference, LookMode.Value, ref areasWithProportionsForSaving, ref proportionsForSaving);
			StripNullKeys();
		}

		public override void MapComponentTick()
		{
			if (Find.TickManager.TicksGame % GenTicks.TickLongInterval == 0) RecalculateFullyGrownZones();
		}

		public void OnAreaRemoved(Zone zone)
		{
			if (harvestSettings.ContainsKey(zone)) harvestSettings.Remove(zone);
			if (fullyGrownZones.Contains(zone)) fullyGrownZones.Remove(zone);
		}

		public void SetHarvestSetting(Zone zone, HarvestSetting setting)
		{
			if (harvestSettings.ContainsKey(zone))
			{
				if (setting == HarvestSetting.AlwaysHarvest)
				{
					harvestSettings.Remove(zone);
				}
				else
				{
					harvestSettings[zone] = setting;
					if (setting == HarvestSetting.SyncHarvest) RecalculateFullyGrownZones();
				}
			}
			else
			{
				if (setting != HarvestSetting.AlwaysHarvest)
				{
					harvestSettings.Add(zone, setting);
				}

			}
		}

		public void SetHarvestProportion(Zone zone, float proportion)
		{
			float clamped = Mathf.Clamp01(proportion);
			if (harvestProportions.ContainsKey(zone))
			{
				if (clamped == 0)
				{
					harvestProportions.Remove(zone);
				}
				else
				{
					harvestProportions[zone] = proportion;
				}
			}
			else if (clamped != 0)
			{
				harvestProportions.Add(zone, proportion);
			}
			RecalculateFullyGrownZones();
		}

		public HarvestSetting GetHarvestSetting(Zone zone)
		{
			return (harvestSettings.ContainsKey(zone)) ? harvestSettings[zone] : HarvestSetting.AlwaysHarvest;
		}

		public float GetProportion(Zone zone)
		{
			return (harvestProportions.ContainsKey(zone)) ? harvestProportions[zone] : 1f;
		}

		public bool CanHarvestZone(Zone zone)
		{
			switch (GetHarvestSetting(zone))
			{
				case HarvestSetting.NeverHarvest:
					return false;
				case HarvestSetting.SyncHarvest:
					return fullyGrownZones.Contains(zone);
				default:
					return true;
			}
		}

		private void RecalculateFullyGrownZones()
		{
			foreach (Zone zone in new HashSet<Zone>(fullyGrownZones))
			{
				if (!zone.HasHarvestableIntendedPlants())
				{
					fullyGrownZones.Remove(zone);
				}
			}
			foreach(KeyValuePair<Zone, HarvestSetting> zoneAndSetting in harvestSettings)
			{
				float proportion = GetProportion(zoneAndSetting.Key);
				if (zoneAndSetting.Value == HarvestSetting.SyncHarvest && zoneAndSetting.Key.PlantsMatureInZone(proportion))
				{
					fullyGrownZones.Add(zoneAndSetting.Key);
				}
			}
		}

		private void StripNullKeys()
		{
			foreach(Zone zoneKey in new List<Zone>(harvestSettings.Keys))
			{
				if (zoneKey == null) harvestSettings.Remove(zoneKey);
			}
			foreach (Zone zoneKey in new List<Zone>(harvestProportions.Keys))
			{
				if (zoneKey == null) harvestProportions.Remove(zoneKey);
			}
		}
	}
}
