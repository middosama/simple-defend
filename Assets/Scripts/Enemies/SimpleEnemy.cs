using System.Collections;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    

    public override int mainDamage => 8;

    protected override void OnTravellDone()
    {
        base.OnTravellDone();
        
    }

    
}
