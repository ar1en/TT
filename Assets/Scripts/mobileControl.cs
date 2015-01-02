using UnityEngine;
using System.Collections;

public class mobileControl : MonoBehaviour 
{

	private block _block;// = GameObject.FindGameObjectWithTag("block").GetComponent<block>();
	private tetrisMain _main;// = GameObject.Find ("main").GetComponent<tetrisMain>();
	//private bool inSwipe = false;
	//private Touch initialTouch = new Touch();
	//private bool hasSwiped = false;
	private Vector2 startPos;
	private bool inSwipe = false;
	private float lengthOnPreviousFrame = 0;
	private float buffer = 0;

	public float test;
	void Update () 
	{
		_block = GameObject.FindGameObjectWithTag("block").GetComponent<block>();
		_main = GameObject.Find ("main").GetComponent<tetrisMain>();
		if (Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			_block._fallSpeed = _main.fallSpeed;
			switch (touch.phase)
			{
				case TouchPhase.Began:
					inSwipe = false;
					startPos = touch.position;
					break;
				case TouchPhase.Moved:
					float distY1 = new Vector3(0, touch.position.y - startPos.y, 0).magnitude;
					float distX1 = new Vector3(0, touch.position.x - startPos.x, 0).magnitude;
					float swipeValue1 = Mathf.Sign (touch.position.x - startPos.x);
					if (distX1 > distY1)
					{
						if (swipeValue1 > 0)
							buffer += distX1 - lengthOnPreviousFrame;
						else
							buffer -= distX1 - lengthOnPreviousFrame;
						
						if (buffer > _main.sensivity)
						{
							_block.horizontalMove(1);
							buffer = 0;
							inSwipe = true;
						}
						else if (buffer < -_main.sensivity)
						{
							_block.horizontalMove(-1);
							inSwipe = true;
							buffer = 0;
						}
					}
					else if (distY1 > distX1)
					{
						test = distY1;
						if ((swipeValue1 > 0) && (distY1 > 220))
						{
							Time.timeScale = 0.45f;
							inSwipe = true;
						}
						else
							Time.timeScale = 1;
					}
					lengthOnPreviousFrame = distX1;
					break;
				case TouchPhase.Ended:
					Time.timeScale = 1;
					buffer = 0;
					lengthOnPreviousFrame = 0;
					float distY = new Vector3(0, touch.position.y - startPos.y, 0).magnitude;
					float distX = new Vector3(0, touch.position.x - startPos.x, 0).magnitude;
					if (distY > distX)
					{
						float swipeValue = Mathf.Sign (touch.position.y - startPos.y);
						//if ((swipeValue > 0)  && !inSwipe)
						if (swipeValue > 0)
							_block.Rotate();
						else if (swipeValue < 0)
							_block._fallSpeed = _main.fallSpeedUltra;
					}
					else if (distY == distX)
					{
						_block._fallSpeed = _main.fallSpeed;
					}
					else if ((distX > distY) && !inSwipe)
					{
						float swipeValue = Mathf.Sign (touch.position.x - startPos.x);
						if (swipeValue > 0)
							_block.horizontalMove(1);
						else if (swipeValue < 0)
							_block.horizontalMove(-1);
					}
					break;
			}
		}
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (Time.timeScale == 1)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}
	}
}
