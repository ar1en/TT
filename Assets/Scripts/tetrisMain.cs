using UnityEngine;
using System.Collections;

public class tetrisMain : MonoBehaviour
{
	public Transform cube;
	public Transform areaCube;
	public Transform ghostCube;
	public bool useArea = true;
	public float fallSpeed = 2.0f;							//скорость падения кирпичика
	public float fallSpeedUltra = 50.0f;						//скорость во время ускорения при падении
	public int maxBlockSize = 5;
	public GameObject[] briks;
	public GameObject ghost;
	public bool useGhost = true;
	public int fieldWidth = 10;
	public int fieldHeight = 20;
	public bool useMobileControl = false;
	public int frameRate = 60;								//FPS
	public int sensivity = 70;								//
	public float fallingCubeLight = 0.56f;					//
	public float cubeLight = 1.0f;							//
	public float previewX = 20;
	public float previewY = 16;
	public float colorAnimationChangeSpeed = 120;
	public bool blockDown = false;
	
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
	
	private int[] _cubePositions;
	private int _firstBrick;
	private int _secondBrick;
	private int _scoreLvl;
	private GameObject _border;
	private Transform[] _cubeReferences;

	void Start () 
	{
		_border = GameObject.FindGameObjectWithTag("border");
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
		StartCoroutine (counter (yPosition));
		int size = brickMatrix.GetLength (0);
		for (var y = 0; y < size; y++)
			for (var x = 0; x < size; x++)
				if (brickMatrix[x, y])
				{
					var cubeOnField = Instantiate(cube, new Vector3(xPosition + x, yPosition - y, 0), Quaternion.identity) as Transform;
					cubeOnField.renderer.material.SetColor("_Color1", color);
					cubeOnField.renderer.material.SetFloat("_Power1", cubeOnField.renderer.material.GetFloat("_Power1") * mainColorCorrection);
					cubeOnField.renderer.material.SetFloat("_Power2", cubeOnField.renderer.material.GetFloat("_Power2") * mainColorCorrection);
					cubeOnField.renderer.material.SetFloat("_Power5", cubeOnField.renderer.material.GetFloat("_Power5") + 0.2f);
					_field[(int) xPosition + x, (int) yPosition - y] = true;		
				}
		checkRows (yPosition - size, size);
		spawnBrick (false);
	}
	
	void checkRows(int yStart, int size)
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
				deleteRows(y);
				y--;
				_scoreLvl++;
			}
		}
		addScore (_scoreLvl);
	}

	void deleteRows(int yStart)
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
				if (!cube.rigidbody)
					cube.AddComponent("Rigidbody");
				cube.collider.enabled = true;
				cube.rigidbody.velocity = new Vector3(Random.Range(-15, 15), Random.Range(-10, 10), Random.Range(-3, -10));
				cube.rigidbody.MoveRotation(new Quaternion(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
				Destroy(cube, 4f);
			}
		}
		StartCoroutine(fallEnd(_cubeReferences, _cubePositions, cubesToMove));
	}

	//метод оставлен только для возможной доработки анимации падения кубов после удаления строки, при ненадобности засунуть в deleteRows
	IEnumerator fallEnd(Transform[] _cubeReferences, int[] _cubePositions, int cubesToMove)
	{
		var t = 0.0f;
		while (t <= 1.0f) 
		{
			t += Time.deltaTime * 10f;
			for (var i = 0; i < cubesToMove; i++) 
				_cubeReferences[i].position = new Vector3 (_cubeReferences[i].position.x, (Mathf.Lerp (_cubePositions[i], _cubePositions[i]-1f, t)), 0);
		}
		yield return 0;
	}

	IEnumerator counter(int _y)
	{
		for (int i = _y; i > 0; i--) 
		{
			yield return new WaitForSeconds(0.02f);
			_border.renderer.material.SetFloat ("_coordSc", i);
		}
		_border.renderer.material.SetFloat ("_coordSc", 24);
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