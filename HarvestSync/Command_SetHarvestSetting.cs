using Verse;

namespace HarvestSync
{
	abstract class Command_SetHarvestSetting : Command
	{
		protected readonly Zone growingZone;
		protected readonly HarvestManager_MapComponent harvestManager;
		protected int numIntendedPlants;

		protected bool ZoneHarvestableNow => harvestManager.CanHarvestZone(growingZone);

		protected HarvestSetting ZoneSetting => harvestManager.GetHarvestSetting(growingZone);

		protected float Proportion => harvestManager.GetProportion(growingZone);

		protected int ProportionAsPercent => (int)(Proportion * 100f);

		protected int ProportionNumber => PlantsInZoneUtility.GetProportionNumber(numIntendedPlants, Proportion);

		public Command_SetHarvestSetting(Zone zone, HarvestManager_MapComponent manager)
		{
			growingZone = zone;
			harvestManager = manager;
			numIntendedPlants = zone.GetNumberOfIntendedPlants();
		}
	}
}