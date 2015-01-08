using UnityEngine;
using System.Collections;

public class planeAction : MonoBehaviour 
{
	public float height = 0;
	public float speed = 5;

	private bool flag = true;
	void Start () 
	{
	
	}

	void Update () 
	{
		if (transform.position.y > height - 0.03)
			flag = false;

		if (flag)
		{
			transform.position = new Vector3 (transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
		}
		else if (!flag && (transform.position.y > 0 + 0.03))
			transform.position = new Vector3 (transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
	}
}
