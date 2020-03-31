using HarmonyLib;
using RimWorld;

namespace HarvestSync
{
	[HarmonyPatch(typeof(WorkGiver_GrowerHarvest))]
	[HarmonyPatch("ExtraRequirements")]
	class HarvestPotentialWorkCellsPatcher
	{
		static void Postfix(ref bool __result, IPlantToGrowSettable settable)
		{
			if (settable is Zone_Growing zone)
			{
				__result = zone.Map.GetComponent<HarvestManager_MapComponent>().CanHarvestZone(zone);
			}
		}
	}
}
