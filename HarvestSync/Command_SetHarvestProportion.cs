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
			defaultLabel = "SetHarvestProportionLabel".Translate(ProportionNumber);
			defaultDesc = "SetHarvestProportionDesc".Translate(ProportionNumber, ProportionAsPercent);
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
			int currentNumber = PlantsInZoneUtility.GetProportionNumber(growingZone.Cells.Count, percent / 100f);

			string futureStatusMessage = "";
			if (percent == 100)
			{
				futureStatusMessage = "HarvestProportionResultRedundant".Translate();
			}
			else if (immatureIntended <= currentNumber)
			{
				futureStatusMessage = "HarvestProportionResultHarvestable".Translate();
			}
			else
			{
				futureStatusMessage = "HarvestProportionResultNotHarvestable".Translate();
			}

			return "SetHarvestProportionSliderText".Translate(currentNumber, percent, futureStatusMessage);
		}

		private void NewSetting(int newPercent)
		{
			harvestManager.SetHarvestProportion(growingZone, newPercent / 100f);
		}
	}
}
