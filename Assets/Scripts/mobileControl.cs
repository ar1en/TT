using UnityEngine;
using System.Collections;

public class mobileControl : MonoBehaviour {

	private block _block;
	private tetrisMain _main;
	private Touch initialTouch = new Touch();
	private bool hasSwiped = false;
	
	void Update () 
	{
		_block = GameObject.FindGameObjectWithTag("block").GetComponent<block>();
		_main = GameObject.Find ("main").GetComponent<tetrisMain>();

		foreach (Touch t in Input.touches)
		{
			if (t.phase == TouchPhase.Began)
				initialTouch = t;
			else if (t.phase == TouchPhase.Moved && !hasSwiped)
			{
				float deltaX = initialTouch.position.x - t.position.x;
				float deltaY = initialTouch.position.y - t.position.y;
				//distance = Mathf.Sqrt((deltaX*deltaX)+(deltaY*deltaY));
				bool swipedsideways = Mathf.Abs(deltaX) > Mathf.Abs(deltaY);
				//if (distance > 50f)
				//{
				_block._fallSpeed = _main.fallSpeed;
				if (swipedsideways && deltaX > 0)
					_block.horizontalMove (-1);
				else if (swipedsideways && deltaX <= 0)
					_block.horizontalMove (1);
				else if (!swipedsideways && deltaY > 0)
					_block._fallSpeed = _main.fallSpeedUltra;
				else if (!swipedsideways && deltaY <= 0)
					_block.Rotate ();
				hasSwiped = true;
				//}
			}
			else if (t.phase == TouchPhase.Ended)
			{
				initialTouch = new Touch();
				hasSwiped = false;
			}
		}
	}
}
