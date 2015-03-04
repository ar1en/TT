using UnityEngine;
using System.Collections;

public class borderShaderManager : MonoBehaviour 
{

	public bool colorIsSend = true;
	private tetrisMain _main;
	public int colorChangeCounter;

	void Start () 
	{
		_main = GameObject.Find ("main").GetComponent<tetrisMain>();
	}

	void Update () 
	{
		if ((colorChangeCounter < _main.colorAnimationChangeSpeed) && !_main.blockDown)
		{
			colorChangeCounter++;
			gameObject.renderer.material.SetInt ("_ColorChangeCounter", colorChangeCounter);
		}
		else if ((colorChangeCounter >= _main.colorAnimationChangeSpeed) && !_main.blockDown)
		{
			if (!colorIsSend)
			{
				Debug.Log ("done3");
				gameObject.renderer.material.SetColor("_CurrentColor", _main.currentBrickColor);
				gameObject.renderer.material.SetColor("_NextColor", _main.nextBrickColor);
				gameObject.renderer.material.SetInt("_ColorChangeCounter", 0);
				
				colorIsSend = true;
			}
		}
		else if (_main.blockDown && (colorChangeCounter < _main.colorAnimationChangeSpeed))
		{
			Debug.Log("Done2");
			gameObject.renderer.material.SetColor("_CurrentColor", _main.currentBrickColor2);
			gameObject.renderer.material.SetColor("_NextColor", _main.nextBrickColor2);
			gameObject.renderer.material.SetInt("_ColorChangeCounter", 0);
			_main.blockDown = false;
		}

	}
}
