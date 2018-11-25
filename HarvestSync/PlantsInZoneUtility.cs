using RimWorld;
using System.Linq;
using Verse;

namespace HarvestSync
{
	static class PlantsInZoneUtility
	{
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

		public static bool PlantsMatureInZone(this Zone zone)
		{
			ThingDef[] plantDefsGrowing = GetIntendedPlants(zone);
			for (int i = 0; i < zone.Cells.Count; i++)
			{
				Plant plant = zone.Cells[i].GetPlant(zone.Map);
				if (plant == null) continue;
				if (!plant.HarvestableNow && plantDefsGrowing.Contains(plant.def)) return false;
			}
			return true;
		}
	}
}
