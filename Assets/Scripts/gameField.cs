using UnityEngine;
using System.Collections;

public class gameField : MonoBehaviour
{
	public static gameField Instance;

	[HideInInspector]
	public int width;
	[HideInInspector]
	public int height;
	public bool[,] field;
	[Header("Тестовая рамка из кубиков")]
	public bool useArea = true;	
	public Transform areaCube;
	private bool[,] _old_field;

	void Start()
	{
		width = tetrisMain.Instance.fieldWidth + tetrisMain.Instance.maxBlockSize * 2;
		height = tetrisMain.Instance.fieldHeight + tetrisMain.Instance.maxBlockSize;
		field = new bool[width, height];
		//генерация стен																	//+++++----------+++++
		for (int i = 0; i < height; i++) 													//+++++----------+++++
			for (int j = 0; j < tetrisMain.Instance.maxBlockSize; j++) 						//+++++----------+++++
				{																			//+++++----------+++++
					field[j, i] = true;						//левая стена					//+++++----------+++++
					field[width-1-j, i] = true;				//правая стена				    //+++++----------+++++
				}																			//+++++----------+++++
		//\генерация стен																    //+++++----------+++++
		//генерация пола																	//+++++----------+++++
		for (int i = 0; i < width; i++) 													//+++++----------+++++
			field[i, 0] = true;								//пол							//+++++----------+++++
		//\генерация пола																    //++++++++++++++++++++
		_old_field = field;
		if (useArea) {
			generateArea();
		}
	}

	void Update()
	{
		if (useArea && !(_old_field.Equals(field))) {
			generateArea();
		}
		_old_field = field;
	}

	void generateArea()
	{
		for (int y = 0; y<height; y++)
			for (int x = 0; x<width; x++)
				if (field[x, y] == true)
					Instantiate(areaCube, new Vector3(x, y, 0),Quaternion.identity);
	}

	public bool checkBrick(bool[,] brickMatrix, int xPosition, int yPosition)
	{
		int size = brickMatrix.GetLength (0);
		for (var y = size - 1; y >= 0; y--)
			for (var x = 0; x < size; x++)
				if ((brickMatrix[x, y]) && (field[xPosition + x, yPosition - y]))
					return true;
		return false;
	}

	public bool checkBrickSpecial(bool[,] brickMatrix, int xPosition, int yPosition)
	{
		int hole = 0;
		int size = brickMatrix.GetLength(0);
		for (var y = size - 1; y >= 0; y--)
			for (var x = 0; x < size; x++)
			{
				for (var y1 = 0; y1 < field.GetLength(1); y1++)
				{
					if (field[xPosition + x, y1] == false)
					{
						hole = y1;
						break;
					}
				}
				if ((brickMatrix[x, y]) && (field[xPosition + x, yPosition - y]) && (yPosition == hole))
					return true;
			}
		return false;
	}

	void Awake() 
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);
	}
}