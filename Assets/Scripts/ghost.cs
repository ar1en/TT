using UnityEngine;
using System.Collections;

public class ghost : MonoBehaviour 
{
	private block _block;
	private tetrisMain _main;
	private Transform _ghost;
	private int _size;
	void Start () 
	{
		_main = GameObject.Find ("main").GetComponent<tetrisMain>();
		_block = GameObject.FindGameObjectWithTag("block").GetComponent<block>();
		_size = _block.brick.Length;
		for (int j=0; j < _size; j++)
			for (int i=0; i < _size; i++)
				if (_block._brickMatrix[i, j])
				{
					_ghost = Instantiate(_main.ghostCube, new Vector3(i - _size*0.5f, j, 0.0f), Quaternion.identity) as Transform;
					_ghost.parent = transform;	
				}
	}


	void Update () 
	{
		transform.position = new Vector3 (_block.transform.position.x, transform.position.y, 0);
		//if (_block.transform.position.y < 2)
		//	Destroy (gameObject);
	}
}
