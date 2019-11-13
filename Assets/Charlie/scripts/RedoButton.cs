using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class RedoButton : DrawingButton
	{
		public override void ButtonAction()
		{
			print("redo");
			_parent.Redo();
		}
	}
}