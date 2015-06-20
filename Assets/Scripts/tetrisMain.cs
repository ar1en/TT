using UnityEngine;
using System.Collections;

public class tetrisMain : MonoBehaviour
{
	[Header("Настройки Блоков")]
	public Transform cube;
	public int maxBlockSize = 5;
	public GameObject[] briks;

	[Header("Настройки размеров поля")]
	public int fieldWidth = 10;
	public int fieldHeight = 20;

	[Header("Настройки призрака")]
	public bool useGhost = true;
	public Transform ghostCube;
	public GameObject ghost;
	
	[Header("Настройки скорости игры")]
	public float fallSpeed = 2.0f;							//скорость падения кирпичика
	public float fallSpeedUltra = 50.0f;						//скорость во время ускорения при падении
	
	[Header("Настройки управления")]
	public bool useMobileControl = false;
	public int sensivity = 70;

	[Header("Настройки шейдера куба")]
	public float fallingCubeLight = 0.56f;					
	public float cubeLight = 1.0f;

	[Header("Расположение превью")]						
	public float previewX = 20;
	public float previewY = 16;
	
	[Header("Настройки шейдера рамки")]
	public float brightnessCentral;
	public float brightnessLampInside;
	public float brightnessLampOutside;
	public float brightnessLampGradient;
	public float brightnessReflectorGradient;
	public float colorAnimationChangeSpeed = 120;

	[Header("Ограничитель FPS")]
	[Tooltip("Только для ios, для остального выставить 0")]
	public int frameRate = 60;		

	[Header("Тестовая рамка из кубиков")]
	public bool useArea = true;
	public Transform areaCube;	

	[HideInInspector]
	public float score;
	[HideInInspector]
	public bool pause;
	[HideInInspector]
	public int _fieldWidth;
	[HideInInspector]
	public int _fieldHeight;
	[HideInInspector]
	public bool[,] _field;
	[HideInInspector]
	public Color nextBrickColor;
	[HideInInspector]
	public Color nextBrickColor2;
	[HideInInspector]
	public Color currentBrickColor;
	[HideInInspector]
	public Color currentBrickColor2;
	[HideInInspector]
	public float currentFallSpeed;
	[HideInInspector]
	public bool blockDown = false;

	private int[] _cubePositions;
	private int[] _rowsForDeleting;
	private int _firstBrick;
	private int _secondBrick;
	private int _scoreLvl;
	private GameObject _border;
	private Transform[] _cubeReferences;

	void Start () 
	{
		
		_border = GameObject.FindGameObjectWithTag("border");
		_border.GetComponent<Renderer>().material.SetFloat ("_Power1", brightnessCentral);
		_border.GetComponent<Renderer>().material.SetFloat ("_Power2", brightnessLampInside);
		_border.GetComponent<Renderer>().material.SetFloat ("_Power3", brightnessLampOutside);
		_border.GetComponent<Renderer>().material.SetFloat ("_Step", brightnessLampGradient);
		_border.GetComponent<Renderer>().material.SetFloat ("_Step2", brightnessReflectorGradient);

		if (useMobileControl)
			gameObject.AddComponent<mobileControl>();
		else
			gameObject.AddComponent<pcControl>();

		_fieldWidth = fieldWidth + maxBlockSize * 2;
		_fieldHeight = fieldHeight + maxBlockSize;
		_field = functions.createAreaMatrix (_fieldWidth, _fieldHeight, maxBlockSize);

		if (useArea)
			functions.generateArea (_field, _fieldWidth, _fieldHeight, areaCube);			//вывод поля для наглядности

		_cubeReferences = new Transform[_fieldWidth * _fieldHeight];
		_cubePositions = new int[_fieldWidth * _fieldHeight];
		spawnBrick (true);
		_scoreLvl = 0;
	}
	
	void spawnBrick(bool first)
	{
		if (first) 
		{
			_firstBrick = Random.Range(0, briks.Length);
			_secondBrick = Random.Range(0, briks.Length);
		}
		else
		{
			_firstBrick = _secondBrick;
			_secondBrick = Random.Range(0, briks.Length);
		}
		Instantiate (briks [_firstBrick]);
		functions.printNextBrick (briks [_secondBrick], cube);
	}
	
	public void setBrick(bool[,] brickMatrix, int xPosition, int yPosition, Color color, float mainColorCorrection)
	{
		if (currentFallSpeed > 30)
			colorAnimationChangeSpeed = 30;
		else
			colorAnimationChangeSpeed = 120;
		blockDown = true;
		int size = brickMatrix.GetLength (0);
		for (var y = 0; y < size; y++)
			for (var x = 0; x < size; x++)
				if (brickMatrix[x, y])
				{
					var cubeOnField = Instantiate(cube, new Vector3(xPosition + x, yPosition - y, 0), Quaternion.identity) as Transform;
					cubeOnField.GetComponent<Renderer>().material.SetColor("_Color1", color);
					cubeOnField.GetComponent<Renderer>().material.SetFloat("_Power1", cubeOnField.GetComponent<Renderer>().material.GetFloat("_Power1") * mainColorCorrection);
					cubeOnField.GetComponent<Renderer>().material.SetFloat("_Power2", cubeOnField.GetComponent<Renderer>().material.GetFloat("_Power2") * mainColorCorrection);
					cubeOnField.GetComponent<Renderer>().material.SetFloat("_Power5", cubeOnField.GetComponent<Renderer>().material.GetFloat("_Power5") + 0.2f);
					cubeOnField.tag = "Cube";
					_field[(int) xPosition + x, (int) yPosition - y] = true;		
				}
		StartCoroutine (checkRows (yPosition - size, size));
		spawnBrick (false);
	}

	IEnumerator checkRows(int yStart, int size)
	{
		if (yStart < 1)
			yStart = 1;																	//исключам пол
		for (int y = yStart; y < yStart + size; y++) 
		{
			int cubesInRow = 0;
			for (int x = maxBlockSize; x < _fieldWidth - maxBlockSize; x++) 
				if (_field[x, y] == true)
					cubesInRow++;
			if (cubesInRow == fieldWidth)
			{
				deleteRow(y);
				yield return new WaitForSeconds(0.13f);
				y--;
				_scoreLvl++;
			}
		}
		addScore (_scoreLvl);
	}

	void deleteRow(int yStart)
	{
		//Сдвиг массива на 1 строчку вниз
		for (int y = yStart; y < _fieldHeight -1 ; y++) 
			for (var x = maxBlockSize; x < _fieldWidth-maxBlockSize; x++) 
				_field[x, y] = _field[x, y+1];

		for (int x = maxBlockSize; x < _fieldWidth - maxBlockSize; x++) 
			_field[x, _fieldHeight-1] = false;
		 
		int cubesToMove = 0;
		foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) 
		{
			if (cube.transform.position.y > yStart) 
			{
				_cubePositions[cubesToMove] = (int) cube.transform.position.y;
				_cubeReferences[cubesToMove] = cube.transform as Transform;
				cubesToMove++;
			}
			else if (cube.transform.position.y == yStart) 
			{
				if (!cube.GetComponent<Rigidbody>())
					cube.AddComponent<Rigidbody>();
				cube.GetComponent<Collider>().enabled = true;
				cube.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-15, 15), Random.Range(-10, 10), Random.Range(-3, -10));
				cube.GetComponent<Rigidbody>().MoveRotation(new Quaternion(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
				Destroy(cube, 4f);
			}
		}
		StartCoroutine(fallEnd(_cubeReferences, _cubePositions, cubesToMove));
	}
	
	IEnumerator fallEnd(Transform[] cubeReferences, int[] cubePositions, int cubesToMove)
	{
		var t = 0.0f;
		while (t <= 1.0f) 
		{
			t += Time.deltaTime * 8f;
			for (var i = 0; i < cubesToMove; i++) 
				cubeReferences[i].position = new Vector3 (cubeReferences[i].position.x, (Mathf.Lerp (cubePositions[i], cubePositions[i]-1f, t)), 0);
			yield return new WaitForSeconds(0.0001f);
		}
		/*for (var t = 0.5f; t <= 1.0f; t += Time.deltaTime)
		{
			//yield return new WaitForSeconds(0.005f);
			//Debug.Log (cubesToMove);
			//yield return new WaitForSeconds(0.01f);
			for (var i = 0; i < cubesToMove; i++) 
			{
				//Debug.Log (cubeRF[i].position.y);
				cubeReferences[i].position = new Vector3 (cubeReferences[i].position.x, (Mathf.Lerp (cubePositions[i], cubePositions[i] - 1f, t)), 0);
				//cubeReferences[i].position = new Vector3 (cubeReferences[i].position.x, cubePositions[i] - t, 0);
				//Debug.Log(cubePositions[i]);
			}
		}*/
		//Debug.Log (t);
		yield return 0;
	}

	void addScore(int scoreLvl)
	{
		score += 100*Mathf.Pow (2, scoreLvl) - 100; //миленько, красиво и изящно, спасибо Коляше)
		_scoreLvl = 0;
	}

	void Update()
	{
		if (pause) 
			Time.timeScale = 0;
		else
			Time.timeScale = 1;

		for (int i= _fieldWidth/maxBlockSize + 1; i<_fieldWidth/maxBlockSize + fieldWidth + 1; i++)
		{
			if (_field[i,fieldHeight] == true)
			{
				Destroy(this);
				Application.LoadLevel(0);
			}
		}
	}
	
	void Awake() 
	{
		if (frameRate != 0)
			Application.targetFrameRate = frameRate;
	}
}