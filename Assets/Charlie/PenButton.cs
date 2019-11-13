﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class PenButton : DrawingButton
	{
		public override void ButtonAction()
		{
			_parent.EnterPenMode();
		}
	}
}