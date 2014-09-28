using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour 
{
	void OnGUI()
	{
		tetrisMain _main = this.GetComponent<tetrisMain>();
		GUI.Label (new Rect (Screen.width-Screen.width/4, 25, 100, 30), "Score: "+ _main.score + "ololo");
		Debug.Log("1");
	}
}
