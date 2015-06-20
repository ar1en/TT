using UnityEngine;

public class borderShaderManager : MonoBehaviour 
{
	public bool colorIsSend = true;
	public float colorChangeCounter;
	
	public float coord = 0;
	public float coord2 = 0;

	private float _downCounter;
	private tetrisMain _main;

	public void setCustomBrightness (string parametr, float value)
	{
		switch (parametr)
		{
			case "1":
				gameObject.GetComponent<Renderer>().material.SetFloat ("_Power1", value);
				break;
			case "2":
				gameObject.GetComponent<Renderer>().material.SetFloat ("_Power2", value);
				break;
			case "3":
				gameObject.GetComponent<Renderer>().material.SetFloat ("_Power3", value);
				break;
			case "4":
				gameObject.GetComponent<Renderer>().material.SetFloat ("_Step", value);
				break;
			case "5":
				gameObject.GetComponent<Renderer>().material.SetFloat ("_Step2", value);
				break;
		}
	}

	void Start () 
	{
		_main = GameObject.Find ("main").GetComponent<tetrisMain>();
	}
	
	void Update () 
	{
		//изменение координаты
		if (_main.blockDown)
		{
			_downCounter = coord2;
		}

		if (_downCounter > 0)
		{
			_downCounter -= 0.25f;
			gameObject.GetComponent<Renderer>().material.SetFloat ("_Coord", _downCounter);
		}
		else
			gameObject.GetComponent<Renderer>().material.SetFloat ("_Coord", coord);
		//\изменение координаты

		if ((colorChangeCounter < _main.colorAnimationChangeSpeed) && !_main.blockDown)
		{
			colorChangeCounter++;
			gameObject.GetComponent<Renderer>().material.SetFloat ("_ColorChangeCounter", colorChangeCounter * (1/_main.colorAnimationChangeSpeed));
		}
		else if ((colorChangeCounter >= _main.colorAnimationChangeSpeed) && !_main.blockDown)
		{
			if (!colorIsSend)
			{
				gameObject.GetComponent<Renderer>().material.SetColor("_CurrentColor", _main.currentBrickColor);
				gameObject.GetComponent<Renderer>().material.SetColor("_NextColor", _main.nextBrickColor);
				gameObject.GetComponent<Renderer>().material.SetInt("_ColorChangeCounter", 0);
				colorIsSend = true;
			}
		}
		else if (_main.blockDown && (colorChangeCounter < _main.colorAnimationChangeSpeed))
		{
			gameObject.GetComponent<Renderer>().material.SetColor("_CurrentColor", _main.currentBrickColor2);
			gameObject.GetComponent<Renderer>().material.SetColor("_NextColor", _main.nextBrickColor2);
			gameObject.GetComponent<Renderer>().material.SetInt("_ColorChangeCounter", 0);
			_main.blockDown = false;
		}
	}
}
