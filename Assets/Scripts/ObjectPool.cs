using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : ObjectPool<T>
{
    protected static Queue<T> Pool = new Queue<T>();
    protected static Vector3 originPosition;
    protected static Vector3 originScale;
    Coroutine destroyTimer = null;

    public static T Prefab;
    public static void Enqueue(int poolSize, T Prefabs)
    {
        Prefab = Prefabs;
        Pool.Clear();
        originPosition = Prefabs.transform.position;
        originScale = Prefabs.transform.localScale;
        for (int i = 0; i < poolSize; i++)
        {
            T gameObject = Instantiate(Prefabs, Main.ObjectPool);
            Pool.Enqueue(gameObject);
            gameObject.gameObject.SetActive(false);
            //poolingObj.OnInstantiate();
        }
    }

    public static T Spawn(Transform parent = null)
    {
        T obj;
        obj = Pool.Dequeue();
        Pool.Enqueue(obj);
        obj.OnSpawn();

        obj.StopTimingDestroy();
        if (parent != null)
        {
            obj.transform.SetParent(parent, false);
            //obj.transform.localPosition = originPosition;
            //obj.transform.localScale = originScale;
        }
        return obj;

    }

    public abstract void OnSpawn();
    //public abstract void OnInstantiate();
    public void Destroy()
    {
        
        gameObject.SetActive(false);
        transform.SetParent(Main.ObjectPool, false);
        //transform.parent = Main.ObjectPool;
    }

    IEnumerator TimingDestroy(float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        Destroy();
    }

    public void Destroy(float delayTime = 0)
    {
        StopTimingDestroy();
        if (delayTime > 0)
        {
            if (gameObject.activeInHierarchy)
            {
                destroyTimer = StartCoroutine(TimingDestroy(delayTime));
            }
        }
        else
        {
            Destroy();
        }
    }

    public void StopTimingDestroy()
    {
        if (destroyTimer != null)
        {
            StopCoroutine(destroyTimer);
            destroyTimer = null;
        }
    }
}
