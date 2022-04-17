using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Common.SystemCommon;

[RequireComponent(typeof(Collider2D))]
public class FocusRange : MonoBehaviour
{
    LinkedList<Creature> targetList = new LinkedList<Creature>();
    Condition<bool,Creature> IsTarget = (x)=>true;

   
    public Action<Creature> OnFirst;
    public Action<Creature> OnOut;

    public LinkedList<Creature> TargetList { get => targetList; }

    public void Init(Condition<bool, Creature> isTarget, Action<Creature> onFirst = null, Action<Creature> onOut = null)
    {
        IsTarget = isTarget;
        OnFirst = onFirst;
        OnOut = onOut;
    }

    //private void Start()
    //{
    //    if(IsTarget == null)
    //    {
    //        Debug.Log("IsTarget must be assign");
    //    }
    //}
    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // all Creature must be focusable
        Creature target = collision.GetComponentInParent<Creature>();
        if (IsTarget(target)){
            targetList.AddLast(target);
            if(targetList.Count == 1 && OnFirst != null)
            {
                OnFirst(target);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Creature target = collision.GetComponent<Creature>();
        if (IsTarget(target))
        {
            targetList.Remove(target); //Memory vs speed
            if (OnOut != null)
                OnOut(target);
        }
    }


    

}

