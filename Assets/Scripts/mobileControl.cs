using UnityEngine;
using System.Collections;

public class mobileControl : MonoBehaviour {

	private block _block;
	private tetrisMain _main;
	
	void Update () 
	{
		_block = GameObject.FindGameObjectWithTag("block").GetComponent<block>();
		_main = GameObject.Find ("main").GetComponent<tetrisMain>();
	}
}
