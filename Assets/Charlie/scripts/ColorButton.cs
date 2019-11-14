using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class ColorButton : DrawingButton
	{
		//more code pretending to do something fancy with colors from web
		public int webColorIndex = 0;

		public override void OnModeChange(DrawingTool.ToolModes mode)
		{
			try //this is here because the mode switches once at load before the colors com back from the web
			{
				normalColor = _parent.specialColorsFromTheInternet[webColorIndex];
				GetComponent<Renderer>().material.color = normalColor;
			} catch(System.Exception e) { }
		}

		public override void ButtonAction()
		{
			_parent.SetColor(normalColor);
		}
	}
}