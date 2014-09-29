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
		//if (functions.checkBrick(_block._brickMatrix, (int) transform.position.x, (int) transform.position.y, _main._field))
		//		Debug.Log("done");
		//var _halfsize = _size * 0.5f;
		var _c = (int)transform.position.x - (int)(_size * 0.5f);
		//Debug.Log ((int)transform.position.y);
		for (int i= _main.fieldHeight; i>1; i--)
		{
			if (functions.checkBrick (_block._brickMatrix, _c, i, _main._field))
			{
				Debug.Log("done  " + i);
				transform.position = new Vector3 (_block.transform.position.x, i - 2 + _size*0.5f, 0);
				break;
			}
		}
			//{
		//	Debug.Log("working");
		//	transform.position = new Vector3 (transform.position.x, transform.position.y + 20, 0);
		//}
			//{
			//Debug.Log(i);
			//transform.position = new Vector3 (_block.transform.position.x, i + _size * 0.5f, 0);
		//}
		//transform.position = new Vector3 (_block.transform.position.x, transform.position.y, 0);
		transform.rotation = _block.transform.rotation;
		//Debug.Log ("x=" + transform.position.x + "   y=" + transform.position.y);
		//if (Input.GetKeyDown (KeyCode.Space))
		//	transform.Rotate(Vector3.forward * - 90.0f );
	}
}
