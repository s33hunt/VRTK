using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class DrawingTool : MonoBehaviour
	{
		public VRTK.Prefabs.CameraRig.TrackedAlias.TrackedAliasFacade trackedAlias;
		Draw _draw;
		Transform
			_rightHand,
			_leftHand,
			_toolPosRight,
			_toolPosLeft;

		private void Start()
		{
			//dependancy check
			if (trackedAlias == null)
			{
				Debug.LogError("Drawing Tool Component requires a reference to a VRTK Tracked Atlas");
			}
			//initialization
			else
			{
				_rightHand = trackedAlias.transform.Find("Aliases/RightControllerAlias");
				_leftHand = trackedAlias.transform.Find("Aliases/LeftControllerAlias");
				_toolPosRight = _rightHand.Find("controller model/tool pos");
				_toolPosLeft = _leftHand.Find("controller model/tool pos");
				_draw = GetComponentInChildren<Draw>();
			}
		}

		private void Update()
		{
			transform.position = _toolPosRight.position;
			transform.rotation = _toolPosRight.rotation;
		}

		public void StartLine()
		{
			_draw.StartLine();
		}
		public void EndLine()
		{
			_draw.EndLine();
		}
		public void Undo()
		{
			_draw.Undo();
		}
		public void Redo()
		{
			_draw.Redo();
		}
	}
}