using UnityEngine;
using System.Collections;

public class block : MonoBehaviour 
{
	public string[] brick;
	public Color color;

	/*public float mainColorCorrection = 0;
	
	public float brightnessCentral = 0;					//1
	public float brightnessLampInside = 0;				//2
	public float brightnessLampOutside = 0;				//3
	public float brightnessLampGradient = 0;			//4
	public float brightnessReflectorGradient = 0;		//5*/

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
	private tetrisMain _main;
	private borderShaderManager _shaderManager;
	//private int _count = 1;

	//private float _brightnes = 1.5f;
	private bool _stopFall = false;
	

	void Start () 
	{
		_shaderManager = GameObject.FindGameObjectWithTag("border").GetComponent<borderShaderManager>();
		_main = tetrisMain.Instance;

		/*if (brightnessCentral != 0)
			_shaderManager.setCustomBrightness("1", brightnessCentral);
		if (brightnessLampInside != 0)
			_shaderManager.setCustomBrightness("2", brightnessLampInside);
		if (brightnessLampOutside != 0)
			_shaderManager.setCustomBrightness("3", brightnessLampOutside);
		if (brightnessLampGradient != 0)
			_shaderManager.setCustomBrightness("4", brightnessLampGradient);
		if (brightnessReflectorGradient != 0)
			_shaderManager.setCustomBrightness("5",brightnessReflectorGradient);*/
		
		_main.currentBrickColor2 = _main.currentBrickColor;
		_main.currentBrickColor = color;
		_fallSpeed = _main.fallSpeed;
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
		if (_main.useGhost)
			Instantiate (_main.ghost);
		StartCoroutine (Fall ());
	}
	
	void Update ()
	{
		_main.currentFallSpeed = _fallSpeed;
		
		if (firstFrame == 0)								//задержка в 1 кадр для  смены цвета
			firstFrame++;
		if(firstFrame == 1)
		{
			_shaderManager.colorChangeCounter = 0;
			_shaderManager.colorIsSend = false;
			tetrisMain.Instance.blockDown = false;//_main.blockDown = false;
			firstFrame++;
		}
		/*if (special == 1) 
		{
			
		}*/
		/*if (special == 1)
		{
			if (_count == true)
			{
				_brightnes = _brightnes + 0.05f;
				if (_brightnes > 2.5f)
					_count = false;
			} 
			else 
			{
				_brightnes = _brightnes - 0.05f;
				if (_brightnes < 1f)
					_count = true;
			}
			gameObject.GetComponentInChildren<Renderer> ().material.SetFloat ("_Power2", _brightnes);
		}*/
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
				//_shaderManager.coord2 = _yPosition;
				setBrick(_brickMatrix, _xPosition, _yPosition + 1, color, cubeOnFieldMatherial);

				foreach(Transform cube in gameObject.GetComponentsInChildren<Transform>())
				{
					poolManager.Instance.returnCubeToPool(cube);//functions.hideCube(cube);
				}
				Destroy(gameObject);
				if (_main.useGhost)
					Destroy(GameObject.FindGameObjectWithTag("ghost"));
					//Destroy(GameObject.Find("ghost(Clone)"));
				break;	
			}
			for (float i = _yPosition + 1; i > _yPosition; i -= Time.deltaTime * _fallSpeed) //физика
			{
				transform.position = new Vector3 (transform.position.x, i - _halfSizeFloat, 0);
				//_shaderManager.coord = i;
				yield return 0;
			}
		}
	}

	public void setBrick(bool[,] brickMatrix, int xPosition, int yPosition, Color color, Material material)
	{
		if (_main.currentFallSpeed > 30)
			_main.colorAnimationChangeSpeed = 30;
		else
			_main.colorAnimationChangeSpeed = 120;
		_main.blockDown = true;
		int size = brickMatrix.GetLength (0);
		for (var y = 0; y < size; y++)
			for (var x = 0; x < size; x++)
				if (brickMatrix[x, y])
				{
					//var cubeOnField = Instantiate(cube, new Vector3(xPosition + x, yPosition - y, 0), Quaternion.identity) as Transform;
					//pool
					var cubeOnField = poolManager.Instance.getCubeFromPool();
					//cubeOnField.gameObject.SetActive(true);
					cubeOnField.position = new Vector3(xPosition + x, yPosition - y, 0);
					//\pool
					cubeOnField.gameObject.isStatic = true; //оптимизация (?)
					cubeOnField.GetComponent<Renderer>().material = material;
					//cubeOnField.GetComponent<Renderer>().material.SetColor("_Color1", color);
					//cubeOnField.GetComponent<Renderer>().material.SetFloat("_Power1", power1 * mainColorCorrection);
					//cubeOnField.GetComponent<Renderer>().material.SetFloat("_Power2", power2 * mainColorCorrection);
					//cubeOnField.GetComponent<Renderer>().material.SetFloat("_Power5", cubeOnField.GetComponent<Renderer>().material.GetFloat("_Power5") + 0.2f);
					cubeOnField.tag = "Cube";
					gameField.Instance.field[(int) xPosition + x, (int) yPosition - y] = true;		
				}
		StartCoroutine (_main.checkRows());
		_main.endGameCheck();
		_main.spawnBrick (false);
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
			//Debug.Log("Rotate start! " + xShift);
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