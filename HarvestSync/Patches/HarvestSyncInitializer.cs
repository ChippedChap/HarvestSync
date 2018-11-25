using Harmony;
using System.Reflection;
using Verse;

namespace HarvestSync
{
	[StaticConstructorOnStartup]
	class HarvestSyncInitializer
	{
		static HarvestSyncInitializer()
		{
			var harmonyInstance = HarmonyInstance.Create("com.chippedchap.harvestsync");
			harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
