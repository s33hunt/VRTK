using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Charlie.DrawingTool
{
	public class CloseButton : DrawingButton
	{
		public override void ButtonAction()
		{
			_parent.ExitDrawingTool();
		}
	}
}