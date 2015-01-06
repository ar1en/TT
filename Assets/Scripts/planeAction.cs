using UnityEngine;
using System.Collections;

public class planeAction : MonoBehaviour 
{
	void Start () 
	{
	
	}

	void Update () 
	{
		transform.position = new Vector3 (transform.position.x, transform.position.y + 1 * Time.deltaTime, transform.position.z);
	}
}
