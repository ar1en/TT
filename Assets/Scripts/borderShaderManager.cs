using UnityEngine;
using System.Collections;

public class borderShaderManager : MonoBehaviour 
{
	public bool newBlock = true;

	private bool _colorIsSend = true;
	private tetrisMain _main;
	public int _colorChangeCounter;

	void Start () 
	{
		_main = GameObject.Find ("main").GetComponent<tetrisMain>();
	}

	void Update () 
	{
		if (newBlock)													//если появляется новый блок сбрасываем счетчик
		{
			_colorChangeCounter = 0;
			newBlock = false;
			//_colorIsSend = false;
		}

		if (_colorChangeCounter < _main.colorAnimationChangeSpeed) 
		{
			_colorChangeCounter++;
			gameObject.renderer.material.SetInt ("_ColorChangeCounter", _colorChangeCounter);
			//Debug.Log(_colorChangeCounter);
		}
		else
		{
			//if (!_colorIsSend)
			//{
				gameObject.renderer.material.SetColor("_CurrentColor", _main.currentBrickColor);
				gameObject.renderer.material.SetColor("_NextColor", _main.nextBrickColor);
				gameObject.renderer.material.SetInt("_ColorChangeCounter", 0);
				//_colorChangeCounter = 0;
				//_colorIsSend = true;
			//}
		}

		if (_main.blockDown)
		//if ((_colorChangeCounter < _main.colorAnimationChangeSpeed)&&(_main.blockDown))	
		{
			//Debug.Log("Done");
			//_colorChangeCounter++;
			//gameObject.renderer.material.SetInt("_ColorChangeCounter", _colorChangeCounter);
			//Debug.Log(_colorChangeCounter);
		}

		/*if ((_main.blockDown) && (_colorChangeCounter < _main.colorAnimationChangeSpeed))
		{
			gameObject.renderer.material.SetColor("_CurrentColor", _main.currentBrickColor);
			gameObject.renderer.material.SetColor("_NextColor", _main.nextBrickColor);
		}*/
	}
}
