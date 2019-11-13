using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class DrawingTool : MonoBehaviour
	{
		public enum ToolModes { Pen, Eraser, None }
		
		public VRTK.Prefabs.CameraRig.TrackedAlias.TrackedAliasFacade trackedAlias;
		public VRTK.Prefabs.CameraRig.UnityXRCameraRig.Input.UnityButtonAction
			leftTrigger,
			rightTrigger;
		[HideInInspector] public ToolModes mode = ToolModes.None;
		[HideInInspector] public Transform
			rightHand,
			leftHand,
			toolPosRight,
			toolPosLeft,
			activeHand;
		private Draw _tool;
		private DrawingButton[] buttons;


		private void Start()
		{
			//dependancy check
			if (trackedAlias == null || leftTrigger == null || rightTrigger == null)
			{
				Debug.LogError("Drawing Tool Component requires references to a VRTK Tracked Atlas and both comtroller input prefab trigger button components!");
			}
			//initialization
			else
			{
				rightHand = trackedAlias.transform.Find("Aliases/RightControllerAlias");
				leftHand = trackedAlias.transform.Find("Aliases/LeftControllerAlias");
				toolPosRight = rightHand.Find("controller model/tool pos");
				toolPosLeft = leftHand.Find("controller model/tool pos");
				_tool = GetComponentInChildren<Draw>();
				buttons = GetComponentsInChildren<DrawingButton>();
			}
		}

		private void Update()
		{
			if(activeHand != null)
			{
				_tool.transform.position = activeHand.position;
				_tool.transform.rotation = activeHand.rotation;
			}
		}

		/*
		/// <summary></summary>
		/// <param name="hand">must equal "right" or "left"</param>
		public void SetActiveHand(string hand)
		{
			if(hand == "right") { activeHand = toolPosRight; }
			else if (hand == "left") { activeHand = toolPosLeft; }
		}
		*/


		public void LeftTrigerDown()
		{
			HandleTriggerDown(ref toolPosLeft);
		}
		public void RightTrigerDown()
		{
			HandleTriggerDown(ref toolPosRight);
		}
		public void LeftTrigerUp()
		{
			HandleTriggerUp();
		}
		public void RightTrigerUp()
		{
			HandleTriggerUp();
		}

		void HandleTriggerDown(ref Transform hand)
		{
			//check btns
			foreach (var b in buttons)
			{
				//perform button action if in proximity
				if(b.Check(hand.position))
				{
					activeHand = hand;
					b.ButtonAction();
					return;//end click here because we don't want to do tool actions till next click
				}
			}

			//check if a hand is active
			if(activeHand != null)
			{
				//handle tool mode
				if(mode == ToolModes.Pen)
				{
					StartLine();
				}
			}
		}

		void HandleTriggerUp()
		{
			if (activeHand != null)
			{
				//handle tool mode
				if (mode == ToolModes.Pen)
				{
					EndLine();
				}
			}
		}


		void StartLine()
		{
			_tool.StartLine();
		}
		 void EndLine()
		{
			_tool.EndLine();
		}
		void Undo()
		{
			_tool.Undo();
		}
		void Redo()
		{
			_tool.Redo();
		}
	}
}