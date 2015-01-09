using UnityEngine;
using System.Collections;

public class functions : MonoBehaviour 
{
	public static bool[,] createAreaMatrix(int width, int height, int maxBlockSize)
	{
		bool[,] field = new bool[width, height];
		
		//генерация стен																	//+++++----------+++++
		for (int i = 0; i < height; i++) 														//+++++----------+++++
			for (int j = 0; j < maxBlockSize; j++) 											//+++++----------+++++
		{																					//+++++----------+++++
			field[j, i] = true;								//левая стена					//+++++----------+++++
			field[width-1-j, i] = true;						//правая стена				//+++++----------+++++
		}																					//+++++----------+++++
		//\генерация стен																//+++++----------+++++
		//генерация пола																	//+++++----------+++++
		for (int i = 0; i < width; i++) 														//+++++----------+++++
			field[i, 0] = true;								//пол							//+++++----------+++++
		//\генерация пола																//++++++++++++++++++++
		return field;
	}

	public static void generateArea(bool[,] matrix, int width, int height, Transform areaCube)
	{
		for (int y = 0; y<height; y++)
			for (int x = 0; x<width; x++)
				if (matrix[x, y] == true)
					Instantiate(areaCube, new Vector3(x, y, 0),Quaternion.identity);
	}

	public static bool checkBrick(bool[,] brickMatrix, int xPosition, int yPosition, bool[,] field)
	{
		int size = brickMatrix.GetLength (0);
		for (var y = size - 1; y >= 0; y--)
			for (var x = 0; x < size; x++)
				if ((brickMatrix[x, y]) && (field[xPosition + x, yPosition - y]))
					return true;
		return false;
	}

	public static void printNextBrick(GameObject brick, Transform cube)
	{
		tetrisMain _main = GameObject.Find ("main").GetComponent<tetrisMain>();
		foreach (GameObject _cube in GameObject.FindGameObjectsWithTag("preview")) 
			Destroy(_cube);
		block _block = brick.GetComponent<block>();
		var _size = _block.brick.Length;
		for (int y = 0; y < _size; y++)
			for (int x = 0; x < _size; x++)
				if (_block.brick[x][y] == "1"[0])				
				{
					var _brick = Instantiate(cube, new Vector3(y - _size*0.5f + _main.previewX, (_size - x) + _size * 0.5f - _size + _main.previewY, -0.3f), Quaternion.identity) as Transform;
					_brick.tag = "preview";
					//задание цвета
					//_brick.renderer.material.color = _block.color;
					_brick.renderer.material.SetColor("_Color1", _block.color);
					//\задание цвета
					//_brick.parent = _transform;
				}
//		_transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
	}
}
