using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class UndoButton : DrawingButton
	{
		public override void OnModeChange(DrawingTool.ToolModes mode)
		{
			//change this button's visibility based on tool mode
			if (mode == DrawingTool.ToolModes.Pen) {
				Show();
			} else {
				Hide();
			}
		}

		public override void ButtonAction()
		{
			_parent.Undo();
		}
	}
}