﻿using RimWorld;
using UnityEngine;
using Verse;

namespace HarvestSync
{
	class Command_SetHarvestMode : Command_SetHarvestSetting
	{
		public override SoundDef CurActivateSound => SoundDefOf.Tick_High;

		public Command_SetHarvestMode(Zone zone, HarvestManager_MapComponent manager) : base(zone, manager)
		{
			if (ZoneSetting == HarvestSetting.SyncHarvest)
			{
				defaultLabel = "SetHarvestSettingRootSync".Translate(ZoneSetting.Translate(), ProportionAsPercent);
			}
			else
			{
				defaultLabel = "SetHarvestSettingRoot".Translate(ZoneSetting.Translate());
			}
			
			defaultDesc = ZoneSetting.GetDescription(ProportionAsPercent, ProportionNumber, numIntendedPlants);
			icon = TexCommand.ForbidOff;
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
			Texture2D image = (!ZoneHarvestableNow) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
			GUI.DrawTexture(drawRect, image);
			return result;
		}
	}
}
