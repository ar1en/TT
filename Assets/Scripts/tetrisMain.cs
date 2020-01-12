using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class tetrisMain : MonoBehaviour
{
	public static tetrisMain Instance;

	[Header("Настройки Блоков")]
	public Transform cube;
	public int maxBlockSize = 5;
	[HideInInspector]
	public int poolSize;
	public GameObject[] briks;
	public bool noUseRandomGeneration = false;
	public int[] blockBuffer = new int[] {8, 8, 8, 0, 0, 0, 0, 7};

	[Header("Настройки размеров поля")]
	public int fieldWidth = 10;
	public int fieldHeight = 30;

	[Header("Настройки призрака")]
	public bool useGhost = true;
	public Transform ghostCube;
	public GameObject ghost;
	
	[Header("Настройки скорости игры")]
	public float fallSpeed = 2.0f;								//скорость падения кирпичика
	public float fallSpeedUltra = 50.0f;						//скорость во время ускорения при падении
	
	[Header("Настройки управления")]
	public bool useMobileControl = false;
	public int sensivity = 70;

	[Header("Настройки превью")]
	public bool usePreview = true;
	public float previewX = 20;
	public float previewY = 16;
	
	[Header("Настройки шейдера рамки")]
	public float colorAnimationChangeSpeed = 0.02f;

	[Header("Ограничитель FPS")]
	[Tooltip("Только для ios, для остального выставить 0")]
	public int frameRate = 60;
	[HideInInspector]
	public float score;
	[HideInInspector]
	public bool pause;
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
	private int[] _rowsForDeleting;
	private int _firstBrick;
	private int _secondBrick;
	private int _scoreLvl;
	private Transform[] _cubeReferences;
	private int[] _briksRates;
	private int _bricksRateSum;
	private bool _checkingRows = false;

	void Awake() 
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);
		if (frameRate != 0)
			Application.targetFrameRate = frameRate;
	}
	void Start () 
	{
		gameObject.AddComponent<ObserverManager>();
		gameObject.AddComponent<borderShaderManager>();
		
		if (useMobileControl)
			gameObject.AddComponent<mobileControl>();
		else
			gameObject.AddComponent<pcControl>();
      
		_cubeReferences = new Transform[fieldWidth * fieldHeight];
		_cubePositions = new int[fieldWidth * fieldHeight];
		_scoreLvl = 0;

		for (int i = 0; i < briks.Length; i++)
		{
			briks[i].GetComponent<block>().cubeMatherial = createMaterial(briks[i].GetComponent<block>().color, false);
			briks[i].GetComponent<block>().cubeOnFieldMatherial = createMaterial(briks[i].GetComponent<block>().color, true);
		}

		poolSize = fieldHeight * fieldWidth;
		poolManager.Instance.createPool(poolSize, cube);
		spawnBrick(true);
	}

	void Update()
	{
		if (pause) 
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
	}
	
	public Material createMaterial(Color color, bool light)
	{
		var result = new Material(cube.GetComponent<Renderer> ().sharedMaterial);
		result.SetColor("_MainColorCube", color);
		return result;
	}

	public void spawnBrick(bool first)
	{
		if (first) 
		{
			_firstBrick = functions.randomBrick();
			_secondBrick = functions.randomBrick();
		}
		else
		{
			_firstBrick = _secondBrick;
			_secondBrick = functions.randomBrick();
		}
		Instantiate (briks [_firstBrick]);
		if (usePreview)
			functions.printNextBrick (briks [_secondBrick], cube);
	}
	
	public void checkRowsAsync()
	{
		StartCoroutine (_checkRows());
	}
	
	private IEnumerator _checkRows()
	{
		if (!_checkingRows) 
		{
			_checkingRows = true;
			for (int y = 1; y < fieldHeight; y++) 
			{
				int cubesInRow = 0;
				for (int x = maxBlockSize; x < gameField.Instance.width - maxBlockSize; x++) 
					if (gameField.Instance.field[x, y] == true)
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
			_checkingRows = false;
		}
	}

	void deleteRow(int yStart)
	{
		//Сдвиг массива на 1 строчку вниз
		for (int y = yStart; y < gameField.Instance.height -1 ; y++) 
			for (var x = maxBlockSize; x < gameField.Instance.width - maxBlockSize; x++) 
				gameField.Instance.field[x, y] = gameField.Instance.field[x, y+1];

		for (int x = maxBlockSize; x < gameField.Instance.width - maxBlockSize; x++) 
			gameField.Instance.field[x, gameField.Instance.height-1] = false;
		 
		int cubesToMove = 0;
		foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) 
		{
			if (cube.transform.position.y > yStart) 
			{
				_cubePositions[cubesToMove] = (int) cube.transform.position.y;
				_cubeReferences[cubesToMove] = cube.transform as Transform;
				cubesToMove++;
			}
			else if (yStart == cube.transform.position.y) 
			{
				if (!cube.GetComponent<Rigidbody>())
					cube.AddComponent<Rigidbody>();
				cube.GetComponent<Collider>().enabled = true;
				cube.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-15, 15), Random.Range(-10, 10), Random.Range(-3, -10));
				Quaternion rotation = new Quaternion();
				rotation.eulerAngles = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
				cube.GetComponent<Rigidbody>().MoveRotation(rotation);
				StartCoroutine(hideCubeWithPing(cube, 2f));
			}
		}
		StartCoroutine(fallEnd(_cubeReferences, _cubePositions, cubesToMove));
	}

	IEnumerator hideCubeWithPing(GameObject cube, float ping)
	{
		yield return new WaitForSeconds (ping);
		poolManager.Instance.returnCubeToPool(cube.transform);
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
		yield return 0;
	}

	void addScore(int scoreLvl)
	{
		score += 100 * Mathf.Pow (2, scoreLvl) - 100; //миленько, красиво и изящно, спасибо Коляше
		_scoreLvl = 0;
	}

	public void endGameCheck()
	{
		for (int i = gameField.Instance.width / maxBlockSize + 1; i < gameField.Instance.width / maxBlockSize + fieldWidth + 1; i++)
		{
			if (gameField.Instance.field[i, fieldHeight - maxBlockSize] == true)
				endGame();
		}
	}

	void endGame()
	{
		SceneManager.LoadScene("Main");
	}
}