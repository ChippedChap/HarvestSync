using UnityEngine;
using Verse;

namespace HarvestSync
{
	[StaticConstructorOnStartup]
	class TextureLoader
	{
		public static Texture2D setHarvestProportionIcon = ContentFinder<Texture2D>.Get("setHarvestProportion");
	}
}
