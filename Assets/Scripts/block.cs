using UnityEngine;
using System.Collections;

public class block : MonoBehaviour 
{
	public string[] brick;
	public Color color;
	public int special = 0;
	public int rate = 1;

	[HideInInspector]
	public bool[,] _brickMatrix;							//матрица для кирпичика
	[HideInInspector]
	public float _fallSpeed;
	[HideInInspector]
	public Material cubeMatherial;
	[HideInInspector]
	public Material cubeOnFieldMatherial;

	private int _yPosition;
	private int _xPosition;
	private int _size; 									//размер матрицы кирпичика
	private float _halfSizeFloat;
	private byte firstFrame = 0;
	private Transform _brick;
	private borderShaderManager _shaderManager;
	private bool _stopFall = false;
	
	
	
	void Start () 
	{
		blockObserver BlockObserver = new blockObserver();
		tetrisMain.Instance.addObserver(BlockObserver);

		_shaderManager = GameObject.FindGameObjectWithTag("border").GetComponent<borderShaderManager>();
		
		tetrisMain.Instance.currentBrickColor2 = tetrisMain.Instance.currentBrickColor;
		tetrisMain.Instance.currentBrickColor = color;
		_fallSpeed = tetrisMain.Instance.fallSpeed;
		_size = brick.Length;						//число элементов текстового массива
		_halfSizeFloat = _size * 0.5f;
		_brickMatrix = new bool[_size, _size];		//создание логической матрицы заданной размерности
		for (int y = 0; y < _size; y++)
			for (int x = 0; x < _size; x++)
				if (brick[y][x] == "1"[0])				
				{
					_brickMatrix[x, y] = true;
					//pool
					_brick = poolManager.Instance.getCubeFromPool();//_main.getCubeFromPool();
					_brick.transform.position = new Vector3(x - _halfSizeFloat, (_size - y) + _halfSizeFloat - _size, 0.0f);
					_brick.GetComponent<Renderer>().material = cubeMatherial;
					//\pool
					_brick.parent = transform;		//делаем созданные кубики дочерними
					transform.tag = "block";
				}
		transform.position = new Vector3 (gameField.Instance.width / 2 + (_size%2 == 0? 0.0f : 0.5f), gameField.Instance.height - _halfSizeFloat, 0);	//выставляем кирпичик сверху и по центру
		_yPosition = gameField.Instance.height - 1;
		_xPosition = (int)transform.position.x - (int) _halfSizeFloat;
		if (tetrisMain.Instance.useGhost)
			Instantiate (tetrisMain.Instance.ghost);
		StartCoroutine (Fall ());
	}
	
	void Update ()
	{
		tetrisMain.Instance.currentFallSpeed = _fallSpeed;
		
		if (firstFrame < 3)								//задержка в 3 кадрa для  смены цвета
			firstFrame++;
		if(firstFrame == 3)
		{
			_shaderManager.colorChangeCounter = 0;
			_shaderManager.colorIsSend = false;
			tetrisMain.Instance.blockDown = false;
			firstFrame++;
		}
	}

	IEnumerator  Fall ()
	{	
		while (true) 
		{
			if (!_stopFall)
				_yPosition --;

			if (!gameField.Instance.checkBrick(_brickMatrix, _xPosition, _yPosition) && gameField.Instance.checkBrick(_brickMatrix, _xPosition, _yPosition - 1))
				Destroy(GameObject.FindGameObjectWithTag("ghost"));

			if (((special == 0) && (gameField.Instance.checkBrick(_brickMatrix, _xPosition, _yPosition))) || (((special == 1) && (gameField.Instance.checkBrickSpecial(_brickMatrix, _xPosition, _yPosition)))))
			{
				tetrisMain.Instance.blockDown = true;
				setBrick(_brickMatrix, _xPosition, _yPosition + 1, color, cubeOnFieldMatherial);

				foreach(Transform cube in gameObject.GetComponentsInChildren<Transform>())
				{
					poolManager.Instance.returnCubeToPool(cube);
				}
				Destroy(gameObject);
				if (tetrisMain.Instance.useGhost)
					Destroy(GameObject.FindGameObjectWithTag("ghost"));
				break;	
			}
			for (float i = _yPosition + 1; i > _yPosition; i -= Time.deltaTime * _fallSpeed) //физика
			{
				transform.position = new Vector3 (transform.position.x, i - _halfSizeFloat, 0);
				yield return 0;
			}
		}
	}

	public void setBrick(bool[,] brickMatrix, int xPosition, int yPosition, Color color, Material material)
	{
		if (tetrisMain.Instance.currentFallSpeed > 30)
			tetrisMain.Instance.colorAnimationChangeSpeed = 30;
		else
			tetrisMain.Instance.colorAnimationChangeSpeed = 120;
		tetrisMain.Instance.blockDown = true;
		int size = brickMatrix.GetLength (0);
		for (var y = 0; y < size; y++)
			for (var x = 0; x < size; x++)
				if (brickMatrix[x, y])
				{
					var cubeOnField = poolManager.Instance.getCubeFromPool();
					cubeOnField.position = new Vector3(xPosition + x, yPosition - y, 0);
					cubeOnField.gameObject.isStatic = true; //оптимизация (?)
					cubeOnField.GetComponent<Renderer>().material = material;
					cubeOnField.tag = "Cube";
					gameField.Instance.field[(int) xPosition + x, (int) yPosition - y] = true;		
				}
		tetrisMain.Instance.notify();
		tetrisMain.Instance.endGameCheck();
		tetrisMain.Instance.spawnBrick (false);
	}

	public void horizontalMove (int dir)
	{
		if (!gameField.Instance.checkBrick(_brickMatrix, _xPosition + dir, _yPosition))
		{
			transform.position = new Vector3 (transform.position.x + dir, _brick.transform.position.y, 0);
			_xPosition += dir;
		}
	}

	public void Rotate()
	{

		_stopFall = true;

		bool[,] tempMatrix = new bool[_size, _size];
		int xShift = 0;

		for (int y = 0; y < _size; y++)      //поворачиваем матрицу падающего блока на +90 градусов
			for (int x = 0; x < _size; x++)
				tempMatrix[y, x] = _brickMatrix[x, (_size - 1) - y];

		if (gameField.Instance.checkBrick(tempMatrix, _xPosition, _yPosition)) //расчитываем необходимый сдвиг, если блок не может повернуться.
		{
			while (gameField.Instance.checkBrick(tempMatrix, _xPosition + xShift, _yPosition))
			{
				if (_xPosition <=9)
					xShift++;
				else if (_xPosition > 9)
					xShift--;
				if ((xShift>4) || (xShift < -4)) //Если для разворота требуется сдвиг больше чем на 4, заблокировтаь разваорот
				{
					xShift = 0;
					 break;
				}
			}
		}
	  
		if ((!gameField.Instance.checkBrick(_brickMatrix, _xPosition+xShift, _yPosition)) && (!gameField.Instance.checkBrick(tempMatrix, _xPosition + xShift, _yPosition)))
		{
			_xPosition += xShift;
			System.Array.Copy (tempMatrix, _brickMatrix, _size * _size);
			transform.Rotate(Vector3.forward * - 90.0f );
			transform.position = new Vector3(transform.position.x + xShift, _brick.transform.position.y, 0);
			for (int i=0;  i< transform.childCount; i++)
			{
				transform.GetChild(i).Rotate(Vector3.forward * + 90.0f );
				transform.GetChild(i).position = new Vector3 (transform.GetChild(i).position.x - 1, transform.GetChild(i).position.y, transform.GetChild(i).position.z);
			}
		}
		_stopFall = false;
	}
}