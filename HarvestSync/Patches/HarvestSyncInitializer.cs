using HarmonyLib;
using System.Reflection;
using Verse;

namespace HarvestSync
{
	[StaticConstructorOnStartup]
	class HarvestSyncInitializer
	{
		static HarvestSyncInitializer()
		{
			var harmonyInstance = new Harmony("ChippedChap.HarvestSync");
			harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
