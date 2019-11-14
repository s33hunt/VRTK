using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class ColorPickerButton : DrawingButton
	{
		//control color buttons in this class
		public ColorButton
			color0,
			color1,
			color2;
		private bool _showColors = true;


		public override void OnModeChange(DrawingTool.ToolModes mode)
		{
			//change this button's visibility based on tool mode
			if(mode == DrawingTool.ToolModes.Pen) {
				Show();
			} else {
				Hide();
			}
			
			HideTheChildren();//hide the children if they are visible on mode change
		}

		public override void ButtonAction()
		{
			if (_showColors) {
				ShowChildren();
				buttonActive = true;//if the color picker is opening, highlight it
			} else {
				HideTheChildren();
			}
			_showColors = !_showColors;//toggle state for next button press
		}

		void ShowChildren()
		{
			color0.Show();
			color1.Show();
			color2.Show();
			_showColors = false;
		}

		void HideTheChildren()
		{
			color0.Hide();
			color1.Hide();
			color2.Hide();
			_showColors = true;
		}
	}
}