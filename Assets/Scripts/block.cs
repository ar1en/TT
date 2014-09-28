using UnityEngine;
using System.Collections;

public class block : MonoBehaviour 
{
	public string[] brick;
	public Color color;

	[HideInInspector]
	public bool[,] _brickMatrix;					//матрица для кирпичика
	//[HideInInspector]
	//public bool isset = true;
	private int _yPosition;
	private int _xPosition;
	private int _size;								//размер матрицы кирпичика
	private Transform _brick;
	//private Transform _ghostcube;
	private Transform _ghost;
	private float _fallSpeed;
	private tetrisMain _main;
	private float _halfSizeFloat;
	private Color _color;
	//private ghost _ghost;

	void Start () 
	{
		//_ghost = GameObject.Find ("ghost").GetComponent<ghost> ();
		_main = GameObject.Find ("main").GetComponent<tetrisMain>();
		_fallSpeed = _main.fallSpeed;
		_size = brick.Length;						//число элементов текстового массива
		_halfSizeFloat = _size * 0.5f;
		_brickMatrix = new bool[_size, _size];		//создание логической матрицы заданной размерности
		//_color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
		for (int y = 0; y < _size; y++)
			for (int x = 0; x < _size; x++)
				if (brick[y][x] == "1"[0])				
				{
					_brickMatrix[x, y] = true;
					_brick = Instantiate(_main.cube, new Vector3(x - _halfSizeFloat, (_size - y) + _halfSizeFloat - _size, 0.0f), Quaternion.identity) as Transform;
					_brick.renderer.material.color = color;
					_brick.parent = transform;		//делаем созданные кубики дочерними
					transform.tag = "block";
					
					//_ghost = Instantiate(_main.ghostCube, new Vector3(x - _halfSizeFloat, (_size - y) + _halfSizeFloat - _size, 0.0f), Quaternion.identity) as Transform;
				}
		transform.position = new Vector3 (_main._fieldWidth / 2 + (_size%2 == 0? 0.0f : 0.5f), _main._fieldHeight - _halfSizeFloat, 0);	//выставляем кирпичик сверху и по центру
		_yPosition = _main._fieldHeight - 1;
		_xPosition = (int)transform.position.x - (int) _halfSizeFloat;
		if (_main.useGhost)
			Instantiate (_main.ghost);
		StartCoroutine (Fall ());
	}

	void Update () 
	{
		//горизонтальное смещение
		if (Input.GetKeyDown (KeyCode.RightArrow))
			StartCoroutine (horizontalMove (1));
		else if (Input.GetKeyDown (KeyCode.LeftArrow))
			StartCoroutine (horizontalMove (-1));
		//\горизонтальное смещение

		if (Input.GetKeyDown (KeyCode.Space))
			Rotate ();

		if (Input.GetKey (KeyCode.DownArrow))
			_fallSpeed = _main.fallSpeedUltra;
		else
			_fallSpeed = _main.fallSpeed;
	}

	void Rotate()
	{
		bool[,] tempMatrix = new bool[_size, _size];
		for (int y = 0; y < _size; y++)
			for (int x = 0; x < _size; x++)
				tempMatrix[y, x] = _brickMatrix[x, (_size-1) - y];

		if (!functions.checkBrick(_brickMatrix, _xPosition, _yPosition, _main._field))
		{
			System.Array.Copy (tempMatrix, _brickMatrix, _size * _size);
			transform.Rotate(Vector3.forward * - 90.0f );
		}
	}

	IEnumerator  Fall ()
	{
		while (true) 
		{
			_yPosition --;
			if (functions.checkBrick(_brickMatrix, _xPosition, _yPosition, _main._field))
			{
				_main.setBrick(_brickMatrix, _xPosition, _yPosition + 1, color);
				Destroy(gameObject);
				if (_main.useGhost)
					Destroy(GameObject.Find("ghost(Clone)"));
				break;	
			}
			for (float i = _yPosition + 1; i > _yPosition; i -= Time.deltaTime * _fallSpeed)
			{
				transform.position = new Vector3 (transform.position.x, i - _halfSizeFloat, 0);
				yield return 0;
			}
		}
	}

	IEnumerator horizontalMove (int dir)
	{
		if (!functions.checkBrick(_brickMatrix, _xPosition + dir, _yPosition,_main._field))
		{
			transform.position = new Vector3 (transform.position.x + dir, _brick.transform.position.y, 0);
			_xPosition += dir;
			yield return 0;
		}
	}	
}