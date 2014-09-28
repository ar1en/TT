using UnityEngine;
using System.Collections;

public class tetrisMain : MonoBehaviour
{
	public Transform cube;
	public Transform areaCube;
	public Transform ghostCube;
	public float fallSpeed = 2.0f;							//скорость падения кирпичика
	public float fallSpeedUltra = 35.0f;						//скорость во время ускорения при падении
	public int maxBlockSize = 5;
	public GameObject[] briks;
	public GameObject ghost;
	//public bool isghost = true;
	//public Transform ghost;
	public int fieldWidth = 10;
	public int fieldHeight = 20;
	public int rowsClearedToSpeedup = 10;
	public float speedupAmount = 0.5f;

	[HideInInspector]
	public float score;
	[HideInInspector]
	public int _fieldWidth;
	[HideInInspector]
	public int _fieldHeight;
	[HideInInspector]
	public bool[,] _field;
	private Transform _ghost;
	private Transform[] _cubeReferences;
	private int[] _cubePositions;
	private int _firstBrick;
	private int _secondBrick;
	private int _scoreLvl;
	
	void Start () 
	{
		_fieldWidth = fieldWidth + maxBlockSize * 2;
		_fieldHeight = fieldHeight + maxBlockSize;
		_field = functions.createAreaMatrix (_fieldWidth, _fieldHeight, maxBlockSize);
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
		Instantiate (ghost);
		functions.printNextBrick (briks [_secondBrick], cube);
	}
	
	public void setBrick(bool[,] brickMatrix, int xPosition, int yPosition, Color color)
	{
		int size = brickMatrix.GetLength (0);
		for (var y = 0; y < size; y++)
			for (var x = 0; x < size; x++)
				if (brickMatrix[x, y])
				{
					Transform cubeOnField = Instantiate(cube, new Vector3(xPosition + x, yPosition - y, 0), Quaternion.identity) as Transform;
					cubeOnField.renderer.material.color = color;
					_field[(int) xPosition + x, (int) yPosition - y] = true;		
				}
		checkRows (yPosition - size, size);
		//Destroy(GameObject.FindGameObjectsWithTag("ghost"));	
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
				cube.AddComponent("Rigidbody");
				cube.collider.enabled = true;
				cube.rigidbody.velocity = new Vector3(Random.Range(-15, 15), Random.Range(-10, 10), Random.Range(-3, -10));
				cube.rigidbody.MoveRotation(new Quaternion(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
				Destroy(cube, 3f);
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
			t += Time.deltaTime;// * 0.2f;
			for (var i = 0; i < cubesToMove; i++) 
				_cubeReferences[i].position = new Vector3 (_cubeReferences[i].position.x, (Mathf.Lerp (_cubePositions[i], _cubePositions[i]-1f, t)), 0);
		}
		yield return 0;
	}

	void addScore(int scoreLvl)
	{
		score += 100*Mathf.Pow (2, scoreLvl) - 100; //миленько, красиво и изящно, спасибо Коляше)
		_scoreLvl = 0;
	}
}