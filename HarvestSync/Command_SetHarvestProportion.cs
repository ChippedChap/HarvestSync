using RimWorld;
using UnityEngine;
using Verse;

namespace HarvestSync
{
	class Command_SetHarvestProportion : Command_SetHarvestSetting
	{
		public override bool Visible => ZoneSetting == HarvestSetting.SyncHarvest;

		public Command_SetHarvestProportion(Zone zone, HarvestManager_MapComponent manager) : base(zone, manager)
		{
			defaultLabel = "SetHarvestProportionLabel".Translate(ProportionAsPercent);
			defaultDesc = "SetHarvestProportionDesc".Translate(ProportionAsPercent, ProportionNumber, numIntendedPlants);
			icon = TexCommand.ForbidOff;
		}

		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			Dialog_Slider slider = new Dialog_Slider(GetSliderText,
				0,
				100,
				NewSetting,
				ProportionAsPercent);
			Find.WindowStack.Add(slider);
		}

		private string GetSliderText(int percent)
		{
			int currentNumber = PlantsInZoneUtility.GetProportionNumber(numIntendedPlants, percent / 100f);
			return "SetHarvestProportionSliderText".Translate(percent, currentNumber, numIntendedPlants);
		}

		private void NewSetting(int newPercent)
		{
			harvestManager.SetHarvestProportion(growingZone, newPercent / 100f);
		}
	}
}
