using UnityEngine;
using System.Collections;

public class planeTest : MonoBehaviour 
{
	public float height = 0;
	public GameObject plane;
	public float height1 = 1;
	public float height2 = 2;
	public float height3 = 3;

	void Start () 
	{
	
	}

	void Update () 
	{
		if (this.transform.position.y < height)
		{
			Debug.Log ("Working");
			Instantiate(plane);
		}
	}
}
