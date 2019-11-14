using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class SliderHandle : DrawingButton
	{
		public Transform thicknessTrack;
		public float sliderValue = 0;
		private float trackLength = -1;
		private Transform 
			_zero, 
			_one;


		public override void OnModeChange(DrawingTool.ToolModes mode)
		{
			if (mode == DrawingTool.ToolModes.Pen) {
				Show();
				thicknessTrack.gameObject.SetActive(true);
			} else {
				Hide();
				thicknessTrack.gameObject.SetActive(false);
			}
		}

		public override void ButtonAction()
		{
			if(_zero == null)
			{
				_zero = thicknessTrack.Find("zero");
				_one = thicknessTrack.Find("one");
				trackLength = Vector3.Distance(_zero.position, _one.position);
			}

			_parent.dragging = true;
		}

		protected override void Update()
		{
			base.Update();

			if(_parent.dragging)
			{
				Vector3 handLocalOffset = _zero.InverseTransformPoint(_parent.activeHand.position);
				sliderValue = Mathf.Clamp(handLocalOffset.z, 0, trackLength);
				transform.position = _zero.position + _zero.forward * sliderValue;
				_parent.widthSliderValue = sliderValue;
			}
		}
	}
}