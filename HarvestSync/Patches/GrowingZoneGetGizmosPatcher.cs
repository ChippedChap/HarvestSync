using Harmony;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace HarvestSync
{
	[HarmonyPatch(typeof(Zone_Growing))]
	[HarmonyPatch("GetGizmos")]
	class GrowingZoneGetGizmosPatcher
	{
		static void Postfix(ref IEnumerable<Gizmo> __result, Zone_Growing __instance)
		{
			Command_SetHarvestSetting harvestSync = new Command_SetHarvestSetting(__instance, __instance.Map.GetComponent<HarvestManager_MapComponent>());
			__result = AppendGizmoToEnumerable(__result, harvestSync);
		}

		static IEnumerable<Gizmo> AppendGizmoToEnumerable(IEnumerable<Gizmo> gizmoEnumerable, Gizmo gizmoToAppend)
		{
			foreach (Gizmo gizmo in gizmoEnumerable)
			{
				yield return gizmo;
			}
			yield return gizmoToAppend;
		}
	}
}
