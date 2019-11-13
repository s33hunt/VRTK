﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Charlie.DrawingTool
{
	public class DrawingButton : MonoBehaviour
	{
		public bool isToggle = true;
		public Color
			normalColor = new Color(0.3f, 0.3f, 0.3f, 1),
			hoverColor = new Color(0.4f, 0.4f, 0.8f, 1),
			activeColor = new Color(0.6f, 0.9f, 0.6f, 1);
		[HideInInspector] public bool buttonActive = false;
		protected DrawingTool _parent;
		private float _radius;
		private float _leftDist { get { return Vector3.Distance(_parent.toolPosLeft.position, transform.position); } }
		private float _rightDist { get { return Vector3.Distance(_parent.toolPosRight.position, transform.position); } }
		private Material buttonMat;

		void Start()
		{
			_parent = transform.parent.GetComponent<DrawingTool>();
			_radius = GetComponent<MeshRenderer>().bounds.extents.x;
			//_parent.leftTrigger.Activated.AddListener(OnLeftTrigger);
			//_parent.rightTrigger.Activated.AddListener(OnRightTrigger);
			buttonMat = GetComponent<Renderer>().material;
		}
		
		void Update()
		{
			//highlight if controller in range
			if((_leftDist < _radius || _rightDist < _radius) && !buttonActive)
			{
				buttonMat.color = hoverColor;
			}
			//set resting color...
			else if(buttonActive && isToggle)
			{
				buttonMat.color = activeColor;
			}
			else
			{
				buttonMat.color = normalColor;
			}
		}

		public bool Check(Vector3 toolPos)
		{
			if (Vector3.Distance(toolPos, transform.position) < _radius)
			{
				if (isToggle) { buttonActive = true; }
				ButtonAction();
				return true;
			}
			return false;
		}

		public virtual void ButtonAction()
		{

		}

		/*void OnLeftTrigger(bool b)
		{
			if (_leftDist < _radius)
			{
				buttonActive = true;
				_parent.SetActiveHand("left");
				ButtonAction();
			}
		}
		void OnRightTrigger(bool b)
		{
			if (_rightDist < _radius)
			{
				buttonActive = true;
				_parent.SetActiveHand("right");
				ButtonAction();
			}
		}*/
	}
}