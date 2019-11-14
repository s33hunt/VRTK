using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
		public delegate void OnModeChange(ToolModes m);
		public OnModeChange onModeChange;
		public ToolModes mode {
			get {
				return _mode;
			} set {
				_mode = value;
				if (onModeChange != null) { onModeChange(value); }
			}
		}
		[HideInInspector] public List<Color> specialColorsFromTheInternet = new List<Color>();
		[HideInInspector] public Transform
			rightHand,
			leftHand,
			toolPosRight,
			toolPosLeft,
			activeHand;
		//private Color _targetLineColor;
		private ToolModes _mode = ToolModes.None;
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
				StartCoroutine("GetColorsFromWeb");
				ExitDrawingTool();
			}
		}

		IEnumerator GetColorsFromWeb()
		{
			using (UnityWebRequest req = UnityWebRequest.Get("http://codetestvrh.herokuapp.com/view/test"))
			{
				req.SetRequestHeader("Authorization", "steve");
				yield return req.SendWebRequest();

				if (req.isNetworkError || req.isHttpError)
				{
					print(req.error);
				}
				else
				{
					string json = req.downloadHandler.text;
					JSONResponse res = JsonUtility.FromJson<JSONResponse>(json);
					print(res);
					specialColorsFromTheInternet = res.InterpretColors();
				}
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


		public void SetColor(Color c)
		{
			_toolController.lineColor = c;
			_pen.transform.Find("tip").GetComponent<Renderer>().material.color = c;
			mode = ToolModes.Pen; //this is a dirty hack to get the OnModeChange to trigger on the color picker and turn off the color buttons
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

		/*void ResetButtons()
		{
			foreach(var b in buttons)
			{
				b.buttonActive = false;
			}
		}*/

		void HandleTriggerDown(ref Transform hand)
		{
			//check btns
			foreach (var b in buttons)
			{
				//perform button action if in proximity
				if(b.InClickZone(hand.position))
				{
					//ResetButtons();
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
			_pen.SetActive(true);
			_eraser.SetActive(false);
			mode = DrawingTool.ToolModes.Pen;
		}
		public void EnterEraserMode()
		{
			_eraser.SetActive(true);
			_pen.SetActive(false);
			mode = DrawingTool.ToolModes.Eraser;
		}
		public void ExitDrawingTool()
		{
			_pen.SetActive(false);
			_eraser.SetActive(false);
			mode = DrawingTool.ToolModes.None;
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

class JSONResponse
{
	public string
		color0,
		color1,
		color2;
	
	/// <summary>
	/// This method is pretty much for humor purposes. 
	/// I wanted to use the actual color names and do some reflection but that would be quite a tangent. 
	/// I'm just putting this here to do SOMETHING with the json from the heroku server.
	/// </summary>
	public List<Color> InterpretColors()
	{
		List<Color> colors = new List<Color>();
		if (color0 == "blue") { colors.Add(Color.blue); }
		if (color1 == "red") { colors.Add(Color.red); }
		if (color2 == "purple") { colors.Add(new Color(.8f,0,.7f,1)); }
		return colors;
	}

	public override string ToString()
	{
		return color0 + ", " + color1 + ", " + color2;
	}
}