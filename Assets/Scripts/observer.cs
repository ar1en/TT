using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class observer : MonoBehaviour
{
    public abstract void  onNotify();
}

public class blockObserver : observer
{
    public override void onNotify()
    {
        //Debug.Log("on Notify!");
        tetrisMain.Instance.checkRows2();
    }
}