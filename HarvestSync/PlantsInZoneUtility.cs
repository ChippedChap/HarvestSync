using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace HarvestSync
{
	static class PlantsInZoneUtility
	{
		public static int GetProportionNumber(int total, float proportion)
		{
			return (int)(total * Mathf.Clamp01(proportion));
		}

		public static bool HasHarvestableIntendedPlants(this Zone zone)
		{
			ThingDef[] intendedPlants = GetIntendedPlants(zone);
			for (int i = 0; i < zone.Cells.Count; i++)
			{
				Plant plant = zone.Cells[i].GetPlant(zone.Map);
				if (plant == null) continue;
				if (intendedPlants.Contains(plant.def) && plant.HarvestableNow) return true;
			}
			return false;
		}

		public static int GetNumberImmaturePlants(this Zone zone)
		{
			ThingDef[] intendedPlants = GetIntendedPlants(zone);
			int numberHarvestable = 0;
			for (int i = 0; i < zone.Cells.Count; i++)
			{
				Plant plant = zone.Cells[i].GetPlant(zone.Map);
				if (plant == null) continue;
				if (intendedPlants.Contains(plant.def) && !plant.HarvestableNow) numberHarvestable++;
			}
			return numberHarvestable;
		}

		public static ThingDef[] GetIntendedPlants(this Zone zone)
		{
			if (zone is Zone_Growing growingZone)
			{
				return new ThingDef[] { growingZone.GetPlantDefToGrow() };
			}
			return new ThingDef[] { };
		}

		public static int GetNumberOfIntendedPlants(this Zone zone)
		{
			ThingDef[] intendedDefs = zone.GetIntendedPlants();
			int number = 0;
			for (int i = 0; i < zone.Cells.Count; i++)
			{
				Plant plant = zone.Cells[i].GetPlant(zone.Map);
				if (plant == null) continue;
				if (intendedDefs.Contains(plant.def)) number++;
			}
			return number;
		}

		public static bool PlantsMatureInZone(this Zone zone, int ignoreNumber = 0)
		{
			ThingDef[] plantDefsGrowing = GetIntendedPlants(zone);
			int immatureIntendedPlants = 0;
			for (int i = 0; i < zone.Cells.Count; i++)
			{
				Plant plant = zone.Cells[i].GetPlant(zone.Map);
				if (plant == null) continue;
				if (plantDefsGrowing.Contains(plant.def) && !plant.HarvestableNow) immatureIntendedPlants++;
			}
			return immatureIntendedPlants <= ignoreNumber;
		}

		public static bool PlantsMatureInZone(this Zone zone, float proportionNeeded)
		{
			int ignore = GetProportionNumber(zone.Cells.Count, proportionNeeded);
			return PlantsMatureInZone(zone, ignore);
		}
	}
}
