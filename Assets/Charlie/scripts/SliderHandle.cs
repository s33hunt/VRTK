using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class SliderHandle : DrawingButton
	{
		public Transform thicknessTrack;
		public float sliderValue = 0;
		private float _trackLength = -1;
		private Transform 
			_zero, 
			_one;


		protected override void Start()
		{
			base.Start();

			//initialize variables
			if (_zero == null) {
				_zero = thicknessTrack.Find("zero");
				_one = thicknessTrack.Find("one");
				_trackLength = Vector3.Distance(_zero.position, _one.position);
			}
		}

		public override void OnModeChange(DrawingTool.ToolModes mode)
		{
			//change visibility based on tool mode
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
			//I would much rather have all dragging functionality in this class but
			//the button classes weren't set up to recieve TriggerUp evenets so I
			//moved some dragging finctionality to the Drawing Tool parent class
			_parent.dragging = true;
		}

		protected override void Update()
		{
			base.Update();

			if(_parent.dragging) {
				Vector3 handLocalOffset = _zero.InverseTransformPoint(_parent.activeHand.position);//get local controller's vector relative to the slider's zero transform
				sliderValue = Mathf.Clamp(handLocalOffset.z, 0, _trackLength);//use the relative z distance but clamp it to the slider bounds
				transform.position = _zero.position + _zero.forward * sliderValue;//add the forward vector of the zero point scaled by the slider value to the zero point's world position
				_parent.widthSliderValue = sliderValue;//do this in order to have the Drawing Tool set the Draw class's new lineWidth property
			}
		}
	}
}