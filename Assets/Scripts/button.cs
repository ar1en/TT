using UnityEngine;
using System.Collections;

public class button : MonoBehaviour {

	private int move = 0;
	private bool flag;

	// Use this for initialization
	void Start () 
	{
	}
	// Update is called once per frame
	void Update () 
	{
		if ((move == 1) && (transform.position.y < -5.6f))
			transform.position = new Vector3 (transform.position.x, transform.position.y + 20f * Time.deltaTime, transform.position.z);
		else if ((move == -1) && (transform.position.y > -8.6f))
			transform.position = new Vector3 (transform.position.x, transform.position.y - 20f * Time.deltaTime, transform.position.z);
		else
			move = 0;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (flag)
		{
			Debug.Log ("working");
			move = -1;
			flag = false;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		move = 1;
	}

}
