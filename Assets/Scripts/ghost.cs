using UnityEngine;
using System.Collections;

public class ghost : MonoBehaviour 
{
	private block _block;
	private tetrisMain _main;
	private Transform _ghost;
	private int _size;
    private Material _material;
	void Start () 
	{
		_main = tetrisMain.Instance;
		_block = GameObject.FindGameObjectWithTag("block").GetComponent<block>();
		_size = _block.brick.Length;
        _material = new Material(Shader.Find("Custom/transparent"));
        _material.SetColor("_MainColor11", _block.color);
		for (int j=0; j < _size; j++)
			for (int i=0; i < _size; i++)
				if (_block._brickMatrix[i, j])
				{
					_ghost = Instantiate(_main.ghostCube, new Vector3(i - _size*0.5f, (_size - j) + _size*0.5f - _size, 0.0f), Quaternion.identity) as Transform;
					_ghost.parent = transform;
                    _ghost.GetComponent<Renderer>().material = _material;
				}
		transform.position = new Vector3 (10, 10, 0);		//что-бы в первое мгновение после создания position.x не был равен нулю
        transform.tag = "ghost";
	}

	void Update () 
	{
        for (int i= _main.fieldHeight; i>0; i--)
			if ((gameField.Instance.checkBrick (_block._brickMatrix, (int)transform.position.x - (int)(_size * 0.5f), i)) && (_block.transform.position.y > i-_size * 0.5f))
			{
				transform.position = new Vector3 (_block.transform.position.x, i - _size * 0.5f + 1, 0);
				break;
			}
		transform.rotation = _block.transform.rotation;
	}
}
