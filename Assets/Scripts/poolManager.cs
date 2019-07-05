using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolManager : MonoBehaviour
{
    public static poolManager Instance { get; private set; }

    Transform[] pool;
    GameObject poolGameObject;

    public void createPool(int poolSize, Transform cube)
    {
        poolGameObject = new GameObject();
        poolGameObject.name = "cubesPool";
        pool = new Transform[poolSize];
        for (int i=0; i<poolSize; i++)
        {
            pool[i] = Instantiate(cube) as Transform;
            pool[i].gameObject.layer = 8;
            pool[i].gameObject.SetActive(false);
            pool[i].parent = poolGameObject.transform;
        }
    }

    public Transform getCubeFromPool()
    {
        for (int i=0; i<pool.Length; i++)
        {
            if (pool[i].gameObject.activeSelf == false)
            {
                pool[i].gameObject.SetActive(true);
                return pool[i];
            }
        }
        Debug.LogWarning("Pool is empty!");
        return null;
    }

    public void returnCubeToPool(Transform cube)
    {
        Destroy(cube.GetComponent<Rigidbody>());
        cube.DetachChildren();
        cube.transform.position = new Vector3(0, 0, 0);
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0, 0, 0);
        cube.transform.rotation = rotation;
        if (cube.GetComponent<Collider>())
            cube.GetComponent<Collider>().enabled = false;
        cube.tag = "Untagged";
        cube.gameObject.isStatic = false;
        cube.gameObject.SetActive(false);
        cube.parent = poolGameObject.transform;
    }

    public void debugGetPoolStatus()
    {
        int active = 0;
        int passive = 0;
        for (int i = 0; i < pool.Length; i++)
            if (pool[i].gameObject.activeSelf == true)
                active++;
            else
                passive++;
        Debug.Log("Пул на " + pool.Length + " элементов. " + passive + " в резерве. " + active + " используется");
    }

    public void Awake()
    {
        Instance = this;
    }
}
