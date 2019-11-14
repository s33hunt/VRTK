using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class ColorButton : DrawingButton
	{
		public int webColorIndex = 0;//more code pretending to do something fancy with colors from web

		public override void OnModeChange(DrawingTool.ToolModes mode)
		{
			try
			{
				normalColor = _parent.specialColorsFromTheInternet[webColorIndex];
				GetComponent<Renderer>().material.color = normalColor;
			}
			catch(System.Exception e)
			{
				//this is here because the mode switches once at load before the colors com back from the web
			}
		}

		public override void ButtonAction()
		{
			_parent.SetColor(normalColor);
		}
	}
}