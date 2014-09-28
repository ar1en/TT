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
					_ghost = Instantiate(_main.ghostCube, new Vector3(i - _size*0.5f, (_size - j) + _size*0.5f - _size, 0.0f), Quaternion.identity) as Transform;
					_ghost.parent = transform;	
				}
	}

	void Update () 
	{
		if (functions.checkBrick(_block._brickMatrix, (int) transform.position.x, (int) transform.position.y, _main._field))
				Debug.Log("done");
		//if (functions.checkBrick(_block._brickMatrix, 0, i, _main._field))
		//{
			//Debug.Log(i);
			//transform.position = new Vector3 (_block.transform.position.x, i + _size * 0.5f, 0);
		//}
		transform.position = new Vector3 (_block.transform.position.x, 3 - _size * 0.5f, 0);
		transform.rotation = _block.transform.rotation;
		//if (Input.GetKeyDown (KeyCode.Space))
		//	transform.Rotate(Vector3.forward * - 90.0f );
	}
}
