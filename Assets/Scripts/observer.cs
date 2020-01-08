using System.Collections;
using System.Collections.Generic;
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