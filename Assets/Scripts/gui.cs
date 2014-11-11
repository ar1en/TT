using UnityEngine;
using System.Collections;
public class gui : MonoBehaviour 
{
	private float accum;
	private int frames;

	void OnGUI()
	{
		tetrisMain _main = this.GetComponent<tetrisMain> ();
		mobileControl _mobC = this.GetComponent<mobileControl> ();

		accum += Time.timeScale/Time.deltaTime;
		++frames;
		float fps = accum / frames;

		GUI.Label (new Rect (Screen.width-Screen.width/4-20, 5, 100, 50), "Score: "+ _main.score + " FPS " + " X " + _mobC.swipeValue1);
		if (Time.timeScale == 0)
			GUI.Label (new Rect (Screen.width/2, Screen.height/2, 100, 30), "Pause");
	}
}
