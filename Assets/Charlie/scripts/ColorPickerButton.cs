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
		private bool _showColors = true;

		public override void OnModeChange(DrawingTool.ToolModes mode)
		{
			if(mode == DrawingTool.ToolModes.Pen)
			{
				Show();
				HideTheChildren();
			}
			else
			{
				Hide();
				HideTheChildren();
			}
		}

		public override void ButtonAction()
		{
			if (_showColors)
			{
				ShowChildren();
			}
			else
			{
				HideTheChildren();
			}
			_showColors = !_showColors;
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