using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DynamicObjectPool<T> : ObjectPool<T> where T : DynamicObjectPool<T>
{

    Coroutine destroyTimer = null;

    protected T current;

    public new static T Spawn(Transform parent = null)
    {
        T obj;
        if(Pool.Count > 0)
        {
            obj = Pool.Dequeue();
        }
        else
        {
            obj = Instantiate(Prefab, Main.ObjectPool);
        }
        //obj.current = obj;
        obj.OnSpawn();

        obj.StopTimingDestroy();
        if (parent != null)
        {
            obj.transform.SetParent(parent, false);
        }
        return obj;

    }

    //public abstract void OnInstantiate();
    public void Destroy(T obj)
    {
        Pool.Enqueue(obj);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Main.ObjectPool, false);
        //transform.parent = Main.ObjectPool;
    }
    [Obsolete("Destroy() is deprecated, please use Destroy(this) instead.", true)]
    public new void Destroy()
    {

    }

    IEnumerator TimingDestroy(T obj,float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        Destroy(obj);
    }

    public void Destroy(T obj,float delayTime = 0)
    {
        StopTimingDestroy();
        if (delayTime > 0)
        {
            if (gameObject.activeInHierarchy)
            {
                destroyTimer = StartCoroutine(TimingDestroy(obj,delayTime));
            }
        }
        else
        {
            Destroy(obj);
        }
    }

}
