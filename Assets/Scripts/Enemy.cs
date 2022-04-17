using System.Collections;
using UnityEngine;


public abstract class Enemy : Creature
{
    protected bool moving = true;
    public override Faction Faction => Faction.Enemy;
    public abstract int mainDamage { get; }

    protected override void OnTravellDone()
    {
        Level.Instance.OnDamage(mainDamage);
        OnDead();
    }
    protected override void OnDead()
    {
        moving = false;
        Level.Instance.OnCreatureDisapear(this);
        CompletelyDisappeared();
    }

    void Update()
    {
        if (moving)
            Move();
    }
    // Use this for initialization

}
