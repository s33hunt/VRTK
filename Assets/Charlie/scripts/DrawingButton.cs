using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Charlie.DrawingTool
{
	public class DrawingButton : MonoBehaviour
	{
		public bool 
			isToggle = true,
			visible = true;
		public Color
			normalColor = new Color(0.3f, 0.3f, 0.3f, 1),
			hoverColor = new Color(0.4f, 0.4f, 0.8f, 1),
			activeColor = new Color(0.6f, 0.9f, 0.6f, 1);
		[HideInInspector] public bool
			buttonActive = false;
		protected DrawingTool _parent;
		private float _radius;
		private float _leftDist { get { return Vector3.Distance(_parent.toolPosLeft.position, transform.position); } }
		private float _rightDist { get { return Vector3.Distance(_parent.toolPosRight.position, transform.position); } }
		private Material buttonMat;

		void Start()
		{
			_parent = transform.parent.GetComponent<DrawingTool>();
			_radius = GetComponent<MeshRenderer>().bounds.extents.x;
			buttonMat = GetComponent<Renderer>().material;
			ToggleMeshesRecursive(transform, visible);
			_parent.onModeChange += OnModeChange;
		}
		
		void Update()
		{
			if (!visible) { return; }

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

		public void Show()
		{
			ToggleMeshesRecursive(transform, true);
			visible = true;
		}
		public void Hide()
		{
			ToggleMeshesRecursive(transform, false);
			visible = false;
		}

		void ToggleMeshesRecursive(Transform t, bool show)
		{
			t.GetComponent<MeshRenderer>().enabled = show;
			foreach (Transform child in t.transform)
			{
				ToggleMeshesRecursive(child, show);
			}
		}
		
		public bool InClickZone(Vector3 handPos)
		{
			if (!visible) { return false; }
			return Vector3.Distance(handPos, transform.position) < _radius;
		}

		public void ShowActive()
		{
			if (isToggle) { buttonActive = true; }
		}
		
		//virtual methods
		public virtual void OnModeChange(DrawingTool.ToolModes mode) { }
		public virtual void ButtonAction() { }
	}
}