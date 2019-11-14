using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Charlie.DrawingTool
{
	public class EraserButton : DrawingButton
	{
		public override void OnModeChange(DrawingTool.ToolModes mode)
		{
			//highlight this button only in the correct mode
			buttonActive = mode == DrawingTool.ToolModes.Eraser;
		}

		public override void ButtonAction()
		{
			_parent.EnterEraserMode();
		}
	}
}