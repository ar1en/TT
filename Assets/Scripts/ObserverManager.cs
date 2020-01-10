using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObserverManager : MonoBehaviour
{
    public static ObserverManager Instance;
    private List<observer> observers;

    public void createObserverManager()
    {
        observers = new List<observer>();
    }

    public void notify()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].onNotify();
        }
    }

    public void addObserver(observer observer)
    {
        observers.Add(observer);
    }

    public void Awake()
    {
        Instance = this;
    }
}
