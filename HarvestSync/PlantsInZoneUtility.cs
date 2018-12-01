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

		public static ThingDef[] GetIntendedPlants(this Zone zone)
		{
			if (zone is Zone_Growing growingZone)
			{
				return new ThingDef[] { growingZone.GetPlantDefToGrow() };
			}
			return new ThingDef[] { };
		}

		public static bool PlantsMatureInZone(this Zone zone, float proportion)
		{
			ThingDef[] plantDefsGrowing = GetIntendedPlants(zone);
			int intended = 0;
			int harvestableIntended = 0;
			for (int i = 0; i < zone.Cells.Count; i++)
			{
				Plant plant = zone.Cells[i].GetPlant(zone.Map);
				if (plant == null) continue;
				if (plantDefsGrowing.Contains(plant.def))
				{
					intended++;
					if (plant.HarvestableNow) harvestableIntended++;
				}
			}
			return harvestableIntended >= GetProportionNumber(intended, proportion);
		}
	}
}
