using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace HarvestSync
{
	class Command_SetHarvestMode : Command_SetHarvestSetting
	{
		public override SoundDef CurActivateSound => SoundDefOf.Tick_High;

		public Command_SetHarvestMode(Zone zone, HarvestManager_MapComponent manager) : base(zone, manager)
		{
			defaultLabel = "SetHarvestSettingRoot".Translate(ZoneSetting.Translate());
			defaultDesc = "SetHarvestSettingDescFirstLine".Translate();
			defaultDesc += Environment.NewLine;
			defaultDesc += Environment.NewLine;
			defaultDesc += ZoneSetting.GetDescription(ProportionAsPercent, ProportionNumber, numIntendedPlants);
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
