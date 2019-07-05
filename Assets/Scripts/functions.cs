using UnityEngine;
using System.Collections;

public class functions : MonoBehaviour 
{
	public static bool[,] createAreaMatrix(int width, int height, int maxBlockSize)
	{
		bool[,] field = new bool[width, height];
		
		//генерация стен																	//+++++----------+++++
		for (int i = 0; i < height; i++) 													//+++++----------+++++
			for (int j = 0; j < maxBlockSize; j++) 											//+++++----------+++++
		{																					//+++++----------+++++
			field[j, i] = true;								//левая стена					//+++++----------+++++
			field[width-1-j, i] = true;						//правая стена				    //+++++----------+++++
		}																					//+++++----------+++++
		//\генерация стен																    //+++++----------+++++
		//генерация пола																	//+++++----------+++++
		for (int i = 0; i < width; i++) 													//+++++----------+++++
			field[i, 0] = true;								//пол							//+++++----------+++++
		//\генерация пола																    //++++++++++++++++++++
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

    public static bool checkBrickSpecial(bool[,] brickMatrix, int xPosition, int yPosition, bool[,] field)
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

    public static int randomBrick()
    {
        tetrisMain _main = GameObject.Find("main").GetComponent<tetrisMain>();
        ////////////////////debug-->
        if (_main.noUseRandomGeneration)
        {
            int brickNumber = 0;
            for (int i = 0; i < _main.blockBuffer.Length; i++)
            {
                if (_main.blockBuffer[i] != 99)
                {
                    brickNumber = _main.blockBuffer[i];
                    _main.blockBuffer[i] = 99;
                    break;
                }
            }
            return brickNumber;
        }
        ////////////////////<--debug
        int sum = 0;
        int result = 0;

        int[] rates = new int[_main.briks.Length];

        for (var i = 0; i < _main.briks.Length; i++)
        {
            sum += _main.briks[i].GetComponent<block>().rate;
            rates[i] = _main.briks[i].GetComponent<block>().rate;
        }
        
        var rand = Random.Range(0, sum*10);

        for (var i = 0; i < _main.briks.Length; i++)
        {
            
            var sumJ = 0;
            var sumM = 0;
            for (var j = 0; j <= i; j++)
            {
                sumJ += rates[j];
                if (j <= i - 1)
                    sumM += rates[j];
            }

            if ((rand > sumM * 10) && (rand <= sumJ * 10))
                result = i;
        }
        return (result);
    }

	public static void printNextBrick(GameObject brick, Transform cube)
	{
//		GameObject _border = GameObject.FindGameObjectWithTag("border");
		tetrisMain _main = GameObject.Find ("main").GetComponent<tetrisMain>();
		foreach (GameObject _cube in GameObject.FindGameObjectsWithTag("preview")) 
			Destroy(_cube);
			//_cube.gameObject.SetActive (false);
		block _block = brick.GetComponent<block>();
		//_border.renderer.material.SetColor("_Color4", _block.color);
		_main.nextBrickColor2 = _main.nextBrickColor;
		_main.nextBrickColor = _block.color;
		var _size = _block.brick.Length;
		for (int y = 0; y < _size; y++)
			for (int x = 0; x < _size; x++)
				if (_block.brick[x][y] == "1"[0])				
				{
					var _brick = Instantiate(cube, new Vector3(y - _size*0.5f + _main.previewX, (_size - x) + _size * 0.5f - _size + _main.previewY, -0.3f), Quaternion.identity) as Transform;
					//var _brick = _main.getCubeFromPool();
					//_brick.transform.position = new Vector3(y - _size*0.5f + _main.previewX, (_size - x) + _size * 0.5f - _size + _main.previewY, -0.3f);
					_brick.tag = "preview";
					//задание цвета
                    _brick.GetComponent<Renderer>().material = _block.cubeMatherial;
					_brick.gameObject.layer = 8;
					//_brick.renderer.material.color = _block.color;
					//_brick.GetComponent<Renderer>().material.SetColor("_Color1", _block.color);
					//_brick.GetComponent<Renderer>().material.SetFloat("_Power1", _brick.GetComponent<Renderer>().material.GetFloat("_Power1") * _block.mainColorCorrection);
					//_brick.GetComponent<Renderer>().material.SetFloat("_Power2", _brick.GetComponent<Renderer>().material.GetFloat("_Power2") * _block.mainColorCorrection);
					//\задание цвета
					//_brick.parent = _transform;
				}
	}
}
