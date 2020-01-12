using UnityEngine;
public abstract class observer
{
    public abstract void  onNotify();
}

public class blockObserver : observer
{
    public override void onNotify()
    {
        tetrisMain.Instance.checkRowsAsync();
    }
}

public class borderShaderManagerObserver : observer
{
    public override void onNotify()
    {
        borderShaderManager.Instance.setBorderColor(tetrisMain.Instance.currentBrickColor, tetrisMain.Instance.nextBrickColor);
    }
}