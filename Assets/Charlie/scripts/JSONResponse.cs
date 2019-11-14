using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is somewhat superfluous after getting the json once. I wanted to use the actual 
/// json color names and do some reflection to set the colors but that would have been quite a tangent. 
/// I'm just putting this here to do SOMETHING with the json from the heroku server.
/// </summary>
class JSONResponse
{
	public string
		color0,
		color1,
		color2;
	

	public List<Color> InterpretColors()
	{
		List<Color> colors = new List<Color>();
		if (color0 == "blue") { colors.Add(Color.blue); }//pretty goofy...
		if (color1 == "red") { colors.Add(Color.red); }
		if (color2 == "purple") { colors.Add(new Color(.8f, 0, .7f, 1)); }
		return colors;
	}

	public override string ToString()
	{
		return color0 + ", " + color1 + ", " + color2;
	}
}