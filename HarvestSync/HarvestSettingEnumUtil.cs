using System;
using Verse;

namespace HarvestSync
{
	enum HarvestSetting : int
	{
		AlwaysHarvest = 0,
		NeverHarvest = 1,
		SyncHarvest = 2
	}

	static class HarvestSettingEnumUtil
	{
		private static readonly string harvestSettingPrefix = "Setting";
		private static readonly string harvestDescriptionSuffix = "Desc";

		public static string Translate(this HarvestSetting setting)
		{
			return (harvestSettingPrefix + setting.ToString()).Translate();
		}

		public static string GetDescription(this HarvestSetting setting, int ignore)
		{
			string baseString = harvestSettingPrefix + setting.ToString() + harvestDescriptionSuffix;
			if (setting == HarvestSetting.SyncHarvest)
			{
				return baseString.Translate(ignore);
			}
			return baseString.Translate();
		}

		public static HarvestSetting Next(this HarvestSetting setting)
		{
			int index = ((int)setting + 1) % Enum.GetValues(typeof(HarvestSetting)).Length;
			return (HarvestSetting)index;
		}
	}
}
