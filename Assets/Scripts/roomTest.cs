using UnityEngine;
using System.Collections;

public class roomTest : MonoBehaviour {

	public GameObject cube;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay(Collision collision)
	{
		Debug.Log("1");
		foreach (ContactPoint contact in collision.contacts) 
		{
			Instantiate(cube, new Vector3(contact.point.x, contact.point.y + 1, contact.point.z), Quaternion.identity);
			Debug.DrawRay(contact.point, contact.normal, Color.green);
			Debug.Log("2");
		}
	}
	/*void OnCollisionEnter(Collision collision) 
	{
		int i = 0;
		foreach (ContactPoint contact2 in collision.contacts) 
		{
			i++;
		}
		Debug.Log (i);
		ContactPoint contact = collision.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		Instantiate (cube, pos, rot);
	}*/
}
