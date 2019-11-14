using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Charlie.DrawingTool
{
	public class DrawingTool : MonoBehaviour
	{
		public enum ToolModes { Pen, Eraser, None }
		public Color lineHighlightColor;//for showing which line will be erased
		public VRTK.Prefabs.CameraRig.TrackedAlias.TrackedAliasFacade trackedAlias;
		public delegate void OnModeChange(ToolModes m);
		public OnModeChange onModeChange;
		public float
			lineWidthMin = 0.01f,
			lineWidthRange = 0.2f;
		public ToolModes mode {
			get {
				return _mode;
			} set {
				_mode = value;
				if (onModeChange != null) { onModeChange(value); }//fire mode change event whenever it changes
			}
		}
		[HideInInspector] public bool dragging = false;//<- unforseen side-effect of original design
		[HideInInspector] public float widthSliderValue = 0;//<- this too
		[HideInInspector] public List<Color> specialColorsFromTheInternet = new List<Color>();
		[HideInInspector] public Transform
			rightHand,
			leftHand,
			toolPosRight,
			toolPosLeft,
			activeHand; //active hand is the transform of the controller currently holding a tool
		private ToolModes _mode = ToolModes.None;
		private LineRenderer _targetLine;
		private Draw _toolController;
		private DrawingButton[] buttons;
		private GameObject
			_pen,
			_eraser;


		private void Start()
		{
			//dependancy check
			if (trackedAlias == null) { 
				Debug.LogError("Drawing Tool requires a reference to a VRTK Tracked Atlas component!");
			//initialization
			} else {
				rightHand = trackedAlias.transform.Find("Aliases/RightControllerAlias");
				leftHand = trackedAlias.transform.Find("Aliases/LeftControllerAlias");
				toolPosRight = rightHand.Find("controller_model/tool pos");
				toolPosLeft = leftHand.Find("controller_model/tool pos");
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

				if (req.isNetworkError || req.isHttpError) {
					print(req.error);
				} else {
					//not the most exciting json handling but I wanted to implement something that at least somewhat relied of getting the web colors
					//see the JSONResponse class for more
					string json = req.downloadHandler.text;
					JSONResponse res = JsonUtility.FromJson<JSONResponse>(json);
					print("colors from the web: " + res);
					specialColorsFromTheInternet = res.InterpretColors();
				}
			}
		}
		
		private void Update()
		{
			if(activeHand != null)//if a hand has selected one of the tools
			{
				//get the drawing component in the right place
				_toolController.transform.position = activeHand.position;
				_toolController.transform.rotation = activeHand.rotation;

				//line selection and highlighting for eraser
				if(mode == ToolModes.Eraser) {
					_targetLine = _toolController.SelectClostsLine(activeHand.position, lineHighlightColor);
				}

				//set width for future lines
				_toolController.lineWidth = lineWidthMin + (widthSliderValue * lineWidthRange);
			}
		}
		
		public void SetColor(Color c)
		{
			_toolController.lineColor = c;
			_pen.transform.Find("tip").GetComponent<Renderer>().material.color = c;
			mode = ToolModes.Pen; //this is a dirty hack to get the OnModeChange to trigger on the color picker and turn off the color buttons
		}

		//trigger handler method to hook up to Unity Button Actions on OpenVR Controller prefabs
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
				if(b.InClickZone(hand.position))
				{
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
				if(mode == ToolModes.Pen) {
					StartLine();
				} else if (mode == ToolModes.Eraser) {
					_toolController.DeleteLine(_targetLine);
				}
			}
		}

		void HandleTriggerUp()
		{
			dragging = false;//I'd really rather have this in the SliderHandle class but for now it has to live here

			if (activeHand != null) {
				//handle tool mode
				if (mode == ToolModes.Pen) {
					EndLine();
				}
			}
		}

		//mode setting methods...
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
		
		//tool method wrappers...
		private void StartLine()
		{
			_toolController.StartLine();
		}

		private void EndLine()
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