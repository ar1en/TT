using UnityEngine;

public class borderShaderManager : MonoBehaviour 
{
	public bool colorIsSend = true;
	public float colorChangeCounter;

    private Material borderMaterial;
    

	void Start () 
	{
        borderMaterial = gameObject.GetComponent<Renderer>().material;

	}
	
	void Update () 
	{
		if ((colorChangeCounter < tetrisMain.Instance.colorAnimationChangeSpeed) && !tetrisMain.Instance.blockDown)
		{
			colorChangeCounter++;
            //setBorderColor();//gameObject.GetComponent<Renderer>().material.SetFloat ("_BorderColorCounter", colorChangeCounter * (1/ tetrisMain.Instance.colorAnimationChangeSpeed));
		}
		else if ((colorChangeCounter >= tetrisMain.Instance.colorAnimationChangeSpeed) && !tetrisMain.Instance.blockDown)
		{
			if (!colorIsSend)
			{
                setBorderColor(tetrisMain.Instance.currentBrickColor, tetrisMain.Instance.nextBrickColor, 0);
				colorIsSend = true;
			}
		}
		else if (tetrisMain.Instance.blockDown && (colorChangeCounter < tetrisMain.Instance.colorAnimationChangeSpeed))
		{
            setBorderColor(tetrisMain.Instance.currentBrickColor2, tetrisMain.Instance.nextBrickColor2, 0);
            tetrisMain.Instance.blockDown = false;
		}
	}

    void setBorderColor(Color currentColor, Color nextColor, int colorChangeCounter)
    {
        borderMaterial.SetColor("_BorderCurrentColor", currentColor);
        borderMaterial.SetColor("_BorderNextColor", nextColor);
        borderMaterial.SetInt("_BorderColorCounter", 0);
    }
}
