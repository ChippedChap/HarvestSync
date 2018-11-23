using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace HarvestSync
{
	class Command_SetHarvestSetting : Command
	{
		private Zone growingZone;
		private HarvestManager_MapComponent harvestManager;
		private bool currentlyCanHarvest;

		private HarvestSetting ZoneSetting => harvestManager.GetHarvestSetting(growingZone);

		public Command_SetHarvestSetting(Zone zone, HarvestManager_MapComponent manager)
		{
			growingZone = zone;
			harvestManager = manager;
			currentlyCanHarvest = manager.CanHarvestZone(zone);
			icon = TexCommand.ForbidOff;
			defaultLabel = "SetHarvestSettingRoot".Translate(ZoneSetting.Translate());
			defaultDesc = ZoneSetting.GetDescription();
		}

		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			harvestManager.SetHarvestSetting(growingZone, ZoneSetting.Next());
		}

		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth);
			Rect drawRect = new Rect(topLeft.x + GetWidth(maxWidth) - 24f, topLeft.y, 24f, 24f);
			Texture2D image = (!currentlyCanHarvest) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
			GUI.DrawTexture(drawRect, image);
			return result;
		}

		private List<FloatMenuOption> GetFloatMenuOptions()
		{
			List<FloatMenuOption> options = new List<FloatMenuOption>();
			foreach (HarvestSetting setting in Enum.GetValues(typeof(HarvestSetting)))
			{
				if (ZoneSetting == setting) continue;
				FloatMenuOption option = new FloatMenuOption(setting.Translate(), delegate ()
				{
					harvestManager.SetHarvestSetting(growingZone, setting);
				});
				options.Add(option);
			}
			return options;
		}
	}
}