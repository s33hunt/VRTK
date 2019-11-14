using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Draw : MonoBehaviour
{
    public List<Vector3> linePositions = new List<Vector3>();
    public GameObject paintPrefab;
    public float lineDist = 0.01f;
    public bool isDrawing = false;
	[HideInInspector] public float lineWidth = 0.1f;
	public Color lineColor;

    private GameObject _go;
    private LineRenderer _line;
    private int _num = 1;
    private Coroutine _co = null;
    private Vector3 _oldPosition;
    private LineRenderer _oldLine;
	private List<LineRenderer> _lines = new List<LineRenderer>();


	//Starts Drawing
	public void StartLine()
    {
        if (isDrawing == false)
        {
            linePositions.Clear();

            _go = Instantiate(paintPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _line = _go.GetComponent<LineRenderer>();
			_line.startWidth = _line.endWidth = lineWidth;
			_line.material.color = lineColor;
			_lines.Add(_line);
            _co = StartCoroutine(DrawLine());

            isDrawing = true;
        }
    }

    //Ends Drawing
    public void EndLine()
    {
        if (isDrawing == true)
        {
            StopCoroutine(_co);

            _go = null;
            _oldLine = _line;
            _line = null;
            _num = 1;

            isDrawing = false;
        }
    }

    //On press, removes the last line position and stores it in a list
    public void Undo()
    {
        if (_oldLine != null && !isDrawing && _oldLine.positionCount > 0)
        {
            linePositions.Add(_oldLine.GetPosition(_oldLine.positionCount - 1));
            _oldLine.positionCount = _oldLine.positionCount - 1;
        }
    }

    public void Redo()
    {
        if (_oldLine != null && !isDrawing && linePositions.Count > 0)
        {
            _oldLine.positionCount = _oldLine.positionCount + 1;
            _oldLine.SetPosition(_oldLine.positionCount - 1, linePositions[linePositions.Count - 1]);
            linePositions.RemoveAt(linePositions.Count - 1);
        }
    }

    private IEnumerator DrawLine()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, _oldPosition) > lineDist)
            {
                //Ends line when it hits 995 points
                if (_num > 995)
                {
                    EndLine();
                }
                else
                {
                    _oldPosition = transform.position;
                    _line.positionCount = _num;
                    _line.SetPosition(_num - 1, transform.position);
                    _num++;
                }
            }

            yield return null;
        }
    }

	LineRenderer _selected;
	Color _selectedPrevColor;

	public LineRenderer SelectClostsLine(Vector3 targetPos, Color highlightColor)
	{
		if (_selected != null) {
			_selected.material.color = _selectedPrevColor;
		}

		foreach (LineRenderer line in _lines)
		{
			Vector3[] positions = new Vector3[line.positionCount];
			line.GetPositions(positions);
			foreach (var point in positions)
			{
				if(Vector3.Distance(point, targetPos) < 0.1f)
				{
					_selected = line;
					_selectedPrevColor = _selected.material.color;
					_selected.material.color = highlightColor;
					return line;
				}
			}
		}
		return null;
	}

	public void DeleteLine(LineRenderer line)
	{
		if (line != null)
		{
			_lines.Remove(line);
			Destroy(line.gameObject);
		}
	}
}
