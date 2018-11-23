using Harmony;
using Verse;

namespace HarvestSync
{
	[HarmonyPatch(typeof(Zone))]
	[HarmonyPatch("Delete")]
	class ZoneDeleteNotifier
	{
		static void Prefix(Zone __instance)
		{
			__instance.Map.GetComponent<HarvestManager_MapComponent>().OnAreaRemoved(__instance);
		}
	}
}
