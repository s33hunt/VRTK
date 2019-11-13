using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Charlie.DrawingTool
{
	public class DrawingTool : MonoBehaviour
	{
		public enum ToolModes { Pen, Eraser, None }
		//public Color lineHighlightColor;
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
		//private Color _targetLineColor;
		private LineRenderer
			_targetLine;
			//_prevLineSelection;
		private Draw _toolController;
		private DrawingButton[] buttons;
		private GameObject
			_pen,
			_eraser;


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
				_toolController = GetComponentInChildren<Draw>();
				buttons = GetComponentsInChildren<DrawingButton>();
				_pen = transform.Find("Tool Controller/pen").gameObject;
				_eraser = transform.Find("Tool Controller/eraser").gameObject;
			}
		}

		private void Update()
		{
			if(activeHand != null)
			{
				_toolController.transform.position = activeHand.position;
				_toolController.transform.rotation = activeHand.rotation;

				if(mode == ToolModes.Eraser)
				{
					_targetLine = _toolController.SelectClostsLine(activeHand.position);
					/*if(_targetLine != null && _prevLineSelection == null)
					{
						_targetLineColor = _targetLine.startColor;

					}*/
				}
			}
		}

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

		void ResetButtons()
		{
			foreach(var b in buttons)
			{
				b.buttonActive = false;
			}
		}

		void HandleTriggerDown(ref Transform hand)
		{
			//check btns
			foreach (var b in buttons)
			{
				//perform button action if in proximity
				if(b.InClickZone(hand.position))
				{
					ResetButtons();
					b.ShowActive();
					activeHand = hand;
					b.ButtonAction();
					return;//end click here because we don't want to do tool actions till next click
				}
			}

			//check if a hand is active
			if(activeHand != null)
			{
				//handle tool modes
				if(mode == ToolModes.Pen)
				{
					StartLine();
				}
				else if (mode == ToolModes.Eraser)
				{
					_toolController.DeleteLine(_targetLine);
				}
			}
		}

		public void EnterPenMode()
		{
			mode = DrawingTool.ToolModes.Pen;
			_pen.SetActive(true);
			_eraser.SetActive(false);
		}
		public void EnterEraserMode()
		{
			mode = DrawingTool.ToolModes.Eraser;
			_eraser.SetActive(true);
			_pen.SetActive(false);
		}
		public void ExitDrawingTool()
		{
			mode = DrawingTool.ToolModes.None;
			_pen.SetActive(false);
			_eraser.SetActive(false);
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
			_toolController.StartLine();
		}
		 void EndLine()
		{
			_toolController.EndLine();
		}
		public void Undo()
		{
			_toolController.Undo();
		}
		public void Redo()
		{
			_toolController.Redo();
		}
	}
}