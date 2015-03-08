using UnityEngine;
//using System.Collections;

public class borderShaderManager : MonoBehaviour 
{
	public bool colorIsSend = true;
	public float colorChangeCounter;
	
	public float coord = 0;
	public float coord2 = 0;
	
	public float brightnessCentral;
	public float brightnessLampInside;
	public float brightnessLampOutside;
	public float brightnessLampGradient;
	public float brightnessReflectorGradient;

	private float _downCounter;
	private tetrisMain _main;

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
			//передача в шейдер рамки значений из префаба блока
			gameObject.renderer.material.SetFloat ("_Power1", brightnessCentral);
			gameObject.renderer.material.SetFloat ("_Power2", brightnessLampInside);
			gameObject.renderer.material.SetFloat ("_Power3", brightnessLampOutside);
			gameObject.renderer.material.SetFloat ("_Step", brightnessLampGradient);
			gameObject.renderer.material.SetFloat ("_Step2", brightnessReflectorGradient);
			//\передача в шейдер рамки значений из префаба блока
						Debug.Log (1);
		}

		if (_downCounter > 0)
		{
			_downCounter -= 0.25f;
			gameObject.renderer.material.SetFloat ("_Coord", _downCounter);
		}
		else
			gameObject.renderer.material.SetFloat ("_Coord", coord);
		//\изменение координаты

		if ((colorChangeCounter < _main.colorAnimationChangeSpeed) && !_main.blockDown)
		{
			colorChangeCounter++;
			gameObject.renderer.material.SetFloat ("_ColorChangeCounter", colorChangeCounter * (1/_main.colorAnimationChangeSpeed));
		}
		else if ((colorChangeCounter >= _main.colorAnimationChangeSpeed) && !_main.blockDown)
		{
			if (!colorIsSend)
			{
				gameObject.renderer.material.SetColor("_CurrentColor", _main.currentBrickColor);
				gameObject.renderer.material.SetColor("_NextColor", _main.nextBrickColor);
				gameObject.renderer.material.SetInt("_ColorChangeCounter", 0);
				colorIsSend = true;
			}
		}
		else if (_main.blockDown && (colorChangeCounter < _main.colorAnimationChangeSpeed))
		{
			gameObject.renderer.material.SetColor("_CurrentColor", _main.currentBrickColor2);
			gameObject.renderer.material.SetColor("_NextColor", _main.nextBrickColor2);
			gameObject.renderer.material.SetInt("_ColorChangeCounter", 0);
			_main.blockDown = false;
		}
	}
}
