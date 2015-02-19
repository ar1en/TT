using UnityEngine;
using System.Collections;

public class block : MonoBehaviour 
{
	public string[] brick;
	public Color color;
	public float mainColorCorrection = 0;
	//public Texture texture;

	[HideInInspector]
	public bool[,] _brickMatrix;						//матрица для кирпичика
	private int _yPosition;
	private int _xPosition;
	private int _size; 									//размер матрицы кирпичика
	//private int _count;
	private float _fallFrames;
	private float _fallspcount;
	private Transform _brick;
	private Transform _ghost;
	[HideInInspector]
	public float _fallSpeed;
	private tetrisMain _main;
	private float _halfSizeFloat;
	private GameObject _border;

	void Start () 
	{
		_border = GameObject.FindGameObjectWithTag("border");
		//_count = 1;
		_fallspcount = _fallSpeed;
		//_border.renderer.material.SetFloat ("_Counter", _count);
		_border.renderer.material.SetFloat ("_fallspcount", _fallspcount);
		_border.renderer.material.SetColor("_Color1", color);
		_main = GameObject.Find ("main").GetComponent<tetrisMain>();
		_fallSpeed = _main.fallSpeed;
		_size = brick.Length;						//число элементов текстового массива
		_halfSizeFloat = _size * 0.5f;
		_brickMatrix = new bool[_size, _size];		//создание логической матрицы заданной размерности
		for (int y = 0; y < _size; y++)
			for (int x = 0; x < _size; x++)
				if (brick[y][x] == "1"[0])				
				{
					_brickMatrix[x, y] = true;
					_brick = Instantiate(_main.cube, new Vector3(x - _halfSizeFloat, (_size - y) + _halfSizeFloat - _size, 0.0f), Quaternion.identity) as Transform;
					_brick.renderer.material.SetColor("_Color1", color);
					_brick.renderer.material.SetFloat("_Power1", _brick.renderer.material.GetFloat("_Power1") * mainColorCorrection);
					_brick.renderer.material.SetFloat("_Power2", _brick.renderer.material.GetFloat("_Power2") * mainColorCorrection);
					//_brick.renderer.material.mainTexture = texture;
					_brick.parent = transform;		//делаем созданные кубики дочерними
					transform.tag = "block";
				}
		transform.position = new Vector3 (_main._fieldWidth / 2 + (_size%2 == 0? 0.0f : 0.5f), _main._fieldHeight - _halfSizeFloat, 0);	//выставляем кирпичик сверху и по центру
		_yPosition = _main._fieldHeight - 1;
		_xPosition = (int)transform.position.x - (int) _halfSizeFloat;
		if (_main.useGhost)
			Instantiate (_main.ghost);
		StartCoroutine (Fall ());
	}

	/*void Update ()
	{
		if (_count < 60 * _fallSpeed)
		{
			_count++;
			_border.renderer.material.SetFloat ("_Counter", _count);
		}
	}*/

	void Update ()
	{
		_main.blocksPerCount++;
		_main.fallFramesMean++;
		//_fallFrames++;
		//_fallTime += Time.deltaTime;
		//Debug.Log (_fallFrames);
	}

	IEnumerator  Fall ()
	{
		while (true) 
		{
			_yPosition --;
			if (functions.checkBrick(_brickMatrix, _xPosition, _yPosition, _main._field))
			{
				_main.setBrick(_brickMatrix, _xPosition, _yPosition + 1, color, mainColorCorrection);
				Destroy(gameObject);
				if (_main.useGhost)
					Destroy(GameObject.Find("ghost(Clone)"));
				break;	
			}
			for (float i = _yPosition + 1; i > _yPosition; i -= Time.deltaTime * _fallSpeed)
			{
				transform.position = new Vector3 (transform.position.x, i - _halfSizeFloat, 0);
				_border.renderer.material.SetFloat("_Coord", i);
				//_border.renderer.material.SetColor("_Color2", color);
				yield return 0;
			}
		}
	}

	public void horizontalMove (int dir)
	{
		if (!functions.checkBrick(_brickMatrix, _xPosition + dir, _yPosition,_main._field))
		{
			transform.position = new Vector3 (transform.position.x + dir, _brick.transform.position.y, 0);
			_xPosition += dir;
		}
	}

	public void Rotate()
	{
		bool[,] tempMatrix = new bool[_size, _size];
		for (int y = 0; y < _size; y++)
			for (int x = 0; x < _size; x++)
				tempMatrix[y, x] = _brickMatrix[x, (_size-1) - y];
		
		if ((!functions.checkBrick(_brickMatrix, _xPosition, _yPosition, _main._field)) && (!functions.checkBrick(tempMatrix, _xPosition, _yPosition, _main._field)))
		{
			System.Array.Copy (tempMatrix, _brickMatrix, _size * _size);
			transform.Rotate(Vector3.forward * - 90.0f );
			for (int i=0;  i< transform.childCount; i++)
			{
				transform.GetChild(i).Rotate(Vector3.forward * + 90.0f );
				transform.GetChild(i).position = new Vector3 (transform.GetChild(i).position.x - 1, transform.GetChild(i).position.y, transform.GetChild(i).position.z);
			}
		}
	}
}