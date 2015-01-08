using UnityEngine;
using System.Collections;

public class button : MonoBehaviour {

	private int move = 0;

	// Use this for initialization
	void Start () 
	{
	}
	// Update is called once per frame
	void Update () 
	{
		if ((move == 1) && (transform.position.y < -5.6f))
			transform.position = new Vector3 (transform.position.x, transform.position.y + 1 * Time.deltaTime, transform.position.z);
		else if (move == -1)
			transform.position = new Vector3 (transform.position.x, transform.position.y - 3 * Time.deltaTime, transform.position.z);
		else
			move = 0;
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("working");
		move = -1;
	}

	void OnTriggerExit(Collider other)
	{
		move = 1;
	}

}
