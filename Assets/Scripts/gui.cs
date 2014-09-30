using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour 
{
	void OnGUI()
	{
		tetrisMain _main = this.GetComponent<tetrisMain>();
		GUI.Label (new Rect (Screen.width-Screen.width/4, 25, 100, 30), "Score: "+ _main.score);
		if (Time.timeScale == 0)
			GUI.Label (new Rect (Screen.width/2, Screen.height/2, 100, 30), "Pause");
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (Time.timeScale == 1)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}
	}
}
