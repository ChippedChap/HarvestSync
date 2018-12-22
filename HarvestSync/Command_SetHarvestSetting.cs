using Verse;

namespace HarvestSync
{
	abstract class Command_SetHarvestSetting : Command
	{
		protected readonly Zone growingZone;
		protected readonly HarvestManager_MapComponent harvestManager;
		protected readonly int immatureIntended;

		protected bool ZoneHarvestableNow => harvestManager.CanHarvestZone(growingZone);

		protected HarvestSetting ZoneSetting => harvestManager.GetHarvestSetting(growingZone);

		protected float Proportion => harvestManager.GetProportion(growingZone);

		protected int ProportionAsPercent => (int)(Proportion * 100f);

		protected int ProportionNumber => PlantsInZoneUtility.GetProportionNumber(growingZone.Cells.Count, Proportion);

		public Command_SetHarvestSetting(Zone zone, HarvestManager_MapComponent manager)
		{
			growingZone = zone;
			harvestManager = manager;
			immatureIntended = zone.GetNumberImmaturePlants();
		}
	}
}