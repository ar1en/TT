using UnityEngine;

public class borderShaderManager : MonoBehaviour 
{
	public static borderShaderManager Instance;
	private float colorChangeCounter;
    private Material borderMaterial;
    
	public void Awake()
    {
        Instance = this;
    }

	void Start () 
	{
        borderMaterial = GameObject.FindGameObjectWithTag("border").GetComponent<Renderer>().material;//gameObject.GetComponent<Renderer>().material;
		
		borderShaderManagerObserver BorderShaderManagerObserver = new borderShaderManagerObserver();
		ObserverManager.Instance.addObserver(BorderShaderManagerObserver);

		//setBorderColor(tetrisMain.Instance.currentBrickColor2, tetrisMain.Instance.nextBrickColor2);
	}
	
	void Update () 
	{
		if (colorChangeCounter < 1 - tetrisMain.Instance.colorAnimationChangeSpeed) //уcловие что бы максимальное перекрытие цветаа было <= 1
		{
			colorChangeCounter += tetrisMain.Instance.colorAnimationChangeSpeed;
			setBorderColorMixer(colorChangeCounter);
		}
	}

    public void setBorderColor(Color currentColor, Color nextColor)
    {
        colorChangeCounter = 0f;
		setBorderColorMixer(colorChangeCounter);
		borderMaterial.SetColor("_BorderCurrentColor", currentColor);
        borderMaterial.SetColor("_BorderNextColor", nextColor);

    }

	private void setBorderColorMixer(float colorMixer)
	{
		borderMaterial.SetFloat("_BorderColorCounter", colorMixer);
	}
}	

