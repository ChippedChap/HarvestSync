﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HarvestSync
{
	class HarvestManager_MapComponent : MapComponent
	{
		private HashSet<Zone> fullyGrownZones = new HashSet<Zone>();

		private Dictionary<Zone, HarvestSetting> harvestSettings = new Dictionary<Zone, HarvestSetting>();
		private List<Zone> areasForSaving = new List<Zone>();
		private List<HarvestSetting> settingsForSaving = new List<HarvestSetting>();

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

		public HarvestSetting GetHarvestSetting(Zone zone)
		{
			return (harvestSettings.ContainsKey(zone)) ? harvestSettings[zone] : HarvestSetting.AlwaysHarvest;
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
				if (zoneAndSetting.Value == HarvestSetting.SyncHarvest && zoneAndSetting.Key.PlantsMatureInZone())
				{
					if (zoneAndSetting.Key.PlantsMatureInZone())
					{
						fullyGrownZones.Add(zoneAndSetting.Key);
					}
				}
			}
		}

		private void StripNullKeys()
		{
			foreach(Zone zoneKey in new List<Zone>(harvestSettings.Keys))
			{
				if (zoneKey == null) harvestSettings.Remove(zoneKey);
			}
		}
	}
}
