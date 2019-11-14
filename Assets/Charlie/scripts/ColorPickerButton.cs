using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class ColorPickerButton : DrawingButton
	{
		public ColorButton
			color0,
			color1,
			color2;

		public override void OnModeChange(DrawingTool.ToolModes mode)
		{
			if(mode == DrawingTool.ToolModes.Pen)
			{
				Show();
			}
			else
			{
				Hide();
			}
		}

		public override void ButtonAction()
		{
			color0.Hide();
			color1.Hide(); 
			color2.Hide();
		}
	}
}