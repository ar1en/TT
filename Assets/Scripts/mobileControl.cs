﻿using UnityEngine;
using System.Collections;

public class mobileControl : MonoBehaviour 
{

	private block _block;// = GameObject.FindGameObjectWithTag("block").GetComponent<block>();
	private tetrisMain _main;// = GameObject.Find ("main").GetComponent<tetrisMain>();
	//private bool inSwipe = false;
	//private Touch initialTouch = new Touch();
	//private bool hasSwiped = false;
	private Vector2 startPos;
	public float swipeValue1;
	//void Start()
	//{

	//}

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
					startPos = touch.position;
					break;
///////////////////////////
				case TouchPhase.Moved:
					//float distY1 = (new Vector3(0, touch.position.y, 0) - new Vector3(0,startPos.y, 0)).magnitude;
					float distY1 = new Vector3(0, touch.position.y - startPos.y, 0).magnitude;
					float distX1 = new Vector3(0, touch.position.x - startPos.x, 0).magnitude;
					//swipeValue1 = Mathf.Sign (touch.position.x - startPos.x);
					//magnitude = distX1;
					//float distX1 = (new Vector3(0, touch.position.x, 0) - new Vector3(0,startPos.x, 0)).magnitude;
					//Mathf.Sign (touch.position.x - startPos.x);
					if (distX1 > distY1)
					{
						float swipeValue1 = Mathf.Sign (touch.position.x - startPos.x);
						if ((swipeValue1 > 0)&&(distX1 > 200))
							StartCoroutine(move(1));
							//_block.horizontalMove(1);
						else if ((swipeValue1 < 0)&&(distX1 > 200))
							StartCoroutine(move(-1));
					}
					break;
//////////////////////////
				//case TouchPhase.Ended:
				case TouchPhase.Ended:
					float distY = (new Vector3(0, touch.position.y, 0) - new Vector3(0,startPos.y, 0)).magnitude;
					float distX = (new Vector3(0, touch.position.x, 0) - new Vector3(0,startPos.x, 0)).magnitude;
					if (distY > distX)
					{
						//if (distY > 100f)
						//{
							float swipeValue = Mathf.Sign (touch.position.y - startPos.y);
							if (swipeValue > 0)
								_block.Rotate();
							else if (swipeValue < 0)
								_block._fallSpeed = _main.fallSpeedUltra;
						//}
					}
					else if (distX > distY)
					//while(distX > distY)
					{
						//if (distX > 50f)
						//{
							float swipeValue = Mathf.Sign (touch.position.x - startPos.x);
							if (swipeValue > 0)
								_block.horizontalMove(1);
							else if (swipeValue < 0)
								_block.horizontalMove(-1);
						//}
					}
					else
					{
						//_block.Rotate();
						_block._fallSpeed = _main.fallSpeed;
					}
					break;
				//case TouchPhase.Stationary:
				//	_block.Rotate();
				//	break;
			}
		}

		/*foreach (Touch t in Input.touches)
		{
			if (t.phase == TouchPhase.Began)
			{
				initialTouch = t;
			}
			else if (t.phase == TouchPhase.Stationary && !hasSwiped)
			{
				_block.Rotate ();
				//hasSwiped = true;
			}
			else if (t.phase == TouchPhase.Moved && !hasSwiped)
			{
				float deltaX = initialTouch.position.x - t.position.x;
				float deltaY = initialTouch.position.y - t.position.y;
				//float distance = Mathf.Sqrt((deltaX*deltaX)+(deltaY*deltaY));
				bool swipedsideways = Mathf.Abs(deltaX) > Mathf.Abs(deltaY);
				//if (distance > 20f)
				//{
					_block._fallSpeed = _main.fallSpeed;
					if (swipedsideways && deltaX > 0)
						_block.horizontalMove (-1);
					else if (swipedsideways && deltaX <= 0)
						_block.horizontalMove (1);
					else if (!swipedsideways && deltaY > 0)
						_block._fallSpeed = _main.fallSpeedUltra;
					else if (!swipedsideways && deltaY <= 0)
					//else if (!swipedsideways)
						_block.Rotate ();
				//}
				hasSwiped = true;
			}
			//else if (t.phase == TouchPhase.Stationary && !hasSwiped)
			//{
			//	_block.Rotate ();
			//	hasSwiped = true;
			//}
			else if (t.phase == TouchPhase.Ended)
			{
				initialTouch = new Touch();
				hasSwiped = false;
			}
		}*/

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (Time.timeScale == 1)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}

		//if (inSwipe)
			//_block.horizontalMove(-1);
	}

	IEnumerator move(int pos)
	{
		yield return new WaitForSeconds(1f);
		_block.horizontalMove (pos);
	}
}
