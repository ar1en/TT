using UnityEngine;
using System.Collections;

public class pcControl : MonoBehaviour 
{
	private block _block;
	private tetrisMain _main;
	
	void Update () 
	{
		_block = GameObject.FindGameObjectWithTag("block").GetComponent<block>();
		_main = GameObject.Find ("main").GetComponent<tetrisMain>();

		if (Input.GetKeyDown (KeyCode.RightArrow))
			_block.horizontalMove (1);
		else if (Input.GetKeyDown (KeyCode.LeftArrow))
			_block.horizontalMove (-1);
		
		if (Input.GetKeyDown (KeyCode.Space))
			_block.Rotate ();

		if (Input.GetKey (KeyCode.DownArrow))
			_block._fallSpeed = _main.fallSpeedUltra;
		else
			_block._fallSpeed = _main.fallSpeed;

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (_main.pause == true)
				_main.pause = false;
			else
				_main.pause = true;

		}
	}
}
