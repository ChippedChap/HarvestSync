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
			HarvestManager_MapComponent manager = __instance.Map.GetComponent<HarvestManager_MapComponent>();
			Gizmo setMode = new Command_SetHarvestMode(__instance, manager);
			Gizmo setProportion = new Command_SetHarvestProportion(__instance, manager);
			__result = AppendGizmoToEnumerable(__result, setMode);
			__result = AppendGizmoToEnumerable(__result, setProportion);
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
