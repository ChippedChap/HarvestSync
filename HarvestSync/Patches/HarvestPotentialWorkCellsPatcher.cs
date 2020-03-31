using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace HarvestSync
{
	[HarmonyPatch(typeof(WorkGiver_Grower))]
	[HarmonyPatch("ExtraRequirements")]
	class HarvestPotentialWorkCellsPatcher
	{
		static void Postfix(ref bool __result, ref WorkGiver_Grower __instance, IPlantToGrowSettable settable)
		{
			if (__instance is WorkGiver_GrowerHarvest && settable is Zone_Growing zone)
			{
				__result = zone.Map.GetComponent<HarvestManager_MapComponent>().CanHarvestZone(zone);
			}
		}
	}
}
