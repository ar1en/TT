using UnityEngine;
using System.Collections;

public class tetrisMain : MonoBehaviour
{
	[Header("Настройки Блоков")]
	public Transform cube;
	public int maxBlockSize = 5;
	public int poolSize = 150;
	public GameObject[] briks;
    public bool noUseRandomGeneration = false;
    public int[] blockBuffer = new int[] {8, 8, 8, 0, 0, 0, 0, 7};

	[Header("Настройки размеров поля")]
	public int fieldWidth = 10;
	public int fieldHeight = 20;

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

    /*[Header("Настройки шейдера куба")]
    //public float fallingCubeLight = 0.56f;					
    //public float cubeLight = 1.0f;
    //public Material cubeMaterial;
    /*public Shader cubeShader;
    /*public Texture cubeTexture;
    public float power1 = 1.6f;
	public float power2 = 1.5f;*/

	[Header("Настройки превью")]
	public bool usePreview = true;
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
	[HideInInspector]
	public Transform[] pool;
    [HideInInspector]
    //public Material[] cubeMaterials;

	private int[] _cubePositions;
	private int[] _rowsForDeleting;
	private int _firstBrick;
	private int _secondBrick;
	private int _scoreLvl;
	private GameObject _border;
	private Transform[] _cubeReferences;
	private int[] _briksRates;
    private int _bricksRateSum;

	void Start () 
	{
		
		/*_border = GameObject.FindGameObjectWithTag("border");
		_border.GetComponent<Renderer>().material.SetFloat ("_Power1", brightnessCentral);
		_border.GetComponent<Renderer>().material.SetFloat ("_Power2", brightnessLampInside);
		_border.GetComponent<Renderer>().material.SetFloat ("_Power3", brightnessLampOutside);
		_border.GetComponent<Renderer>().material.SetFloat ("_Step", brightnessLampGradient);
		_border.GetComponent<Renderer>().material.SetFloat ("_Step2", brightnessReflectorGradient);*/

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
		_scoreLvl = 0;

        for (int i = 0; i < briks.Length; i++)
        {
            briks[i].GetComponent<block>().cubeMatherial = createMaterial(briks[i].GetComponent<block>().color, false);
            briks[i].GetComponent<block>().cubeOnFieldMatherial = createMaterial(briks[i].GetComponent<block>().color, true);
        }
        
        //create pool
		pool = new Transform[poolSize];

		for (int i = 0; i < poolSize; i++) 
		{
			pool[i] = Instantiate(cube) as Transform;
			pool [i].gameObject.layer = 8;
            //pool[i].GetComponent<Renderer>().material = null;//cubeMaterial;
			pool[i].gameObject.SetActive(false);
		}
		//\create pool
        spawnBrick(true);
	}

    public Material createMaterial(Color color, bool light)
    {
		var result = new Material(cube.GetComponent<Renderer> ().sharedMaterial);
		result.SetColor("_Color1", color);
		//var result = new Material(cubeShader);
        /*esult.SetColor("_Color1", color);
		/*result.SetFloat("_Power1", power1);
		result.SetFloat("_Power2", power2);
        result.SetTexture("_1stTex", cubeTexture);
        
		if (light)
            result.SetFloat("_Power5", 1f);*/
        return result;
    }

	public Transform getCubeFromPool()
	{
       //int m = 12;
        for (int i = 0; i < poolSize; i++) 
		{
			if (pool[i].gameObject.activeSelf == false)// && (m == 0))
			{
                //if (m == 0)
               // {
                    pool[i].gameObject.SetActive(true);
                    return pool[i];
               // }
               // m--;
			}
		}
        Debug.Log("WARNING NULL FROM POOL!!!");
		return null;
	}

    void debugGetPoolStatus()
    {
        int active = 0;
        int passive = 0;
        for (int i = 0; i < poolSize; i++)
            if (pool[i].gameObject.activeSelf == true)
                active++;
            else
                passive++;
        Debug.Log("Пул на " + poolSize + " элементов. " + passive + " в резерве. " + active + " используется");
    }

	void spawnBrick(bool first)
	{
        //debugGetPoolStatus();

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
	
	public void setBrick(bool[,] brickMatrix, int xPosition, int yPosition, Color color, Material material)
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
					//var cubeOnField = Instantiate(cube, new Vector3(xPosition + x, yPosition - y, 0), Quaternion.identity) as Transform;
					//pool
					var cubeOnField = getCubeFromPool();
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
				//cube.GetComponent<Rigidbody>().MoveRotation(new Quaternion(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
				Quaternion rotation = new Quaternion();
				rotation.eulerAngles = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
				cube.GetComponent<Rigidbody>().MoveRotation(rotation);
				//Destroy(cube, 4f);
				StartCoroutine(hideCubeWithPing(cube, 4f));
			}
		}
		StartCoroutine(fallEnd(_cubeReferences, _cubePositions, cubesToMove));
	}

	IEnumerator hideCubeWithPing(GameObject cube, float ping)
	{
		yield return new WaitForSeconds (ping);
        functions.hideCube(cube.transform);
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
		score += 100 * Mathf.Pow (2, scoreLvl) - 100; //миленько, красиво и изящно, спасибо Коляше)
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
				//Destroy(this);
				Application.LoadLevel(0);
                //SceneManager.LoadScene;
			}
		}
	}
	
	void Awake() 
	{
		if (frameRate != 0)
			Application.targetFrameRate = frameRate;
	}
}