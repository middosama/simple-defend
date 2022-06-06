using System.Collections;
using UnityEngine;
using UnityCommon;

public class SimpleAlly : Ally
{


    [SerializeField]
    private readonly float reloadTime = 0.5f, snapTime = 0.5f;
    float doneReloadTime = 0, doneSnapTime = 0;
    //private float aps;
    [SerializeField]
    private float attackSpeedMultiply = 1f;

    int burstCharge = int.MaxValue;
    int burstChargeCount = 0;
    int burstCount = 3;

    [SerializeField]
    FocusRange attackRange;
    [SerializeField]
    SpriteRenderer rangeVisualized;
    Creature focusingTarget;

    Coroutine attackCoroutine;

    public float bulletFrame;

    

    public override float SellValue => 0.8f; // 80% refund

    public override AllyName AllyName => AllyName.Simple;

    IEnumerator Attack()
    {
        while (true)
        {
            if (focusingTarget.IsDead)
            {
                NextEnemy();
                yield break;
                //if(focusingTarget == null)
            }

            while (doneReloadTime > Time.time)
            {
                yield return null;
            }
            // snap
            doneSnapTime = Time.time + (snapTime / attackSpeedMultiply);
            while (doneSnapTime > Time.time)
            {
                yield return null;
            }
            doneReloadTime = Time.time + (reloadTime / attackSpeedMultiply);

            if (burstChargeCount > burstCharge)
            {
                burstChargeCount = 0;
                for (int i = 0; i < burstCount; i++)
                {
                    Shoot();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                Shoot();
                burstChargeCount++;
            }
            
            //yield return reloadBlocker;
        }
    }


    void Shoot()
    {
        //Instantiate(InstantBullet.Prefab).Hit2(this, focusingTarget, bulletFrame, 1);

        //Debug.Log("fuck");
        InstantBullet.Spawn().Hit(transform.position, focusingTarget, bulletFrame, 10);
        // shoot

    }



    void StartAttack(Creature creature)
    {
        Debug.Log("StartAttack");
        focusingTarget = creature;
        attackCoroutine = StartCoroutine(Attack());
    }

    void OnEnemyEscape(Creature creature)
    {
        if (creature == focusingTarget)
        {
            StopAttack();
            NextEnemy();
        }
    }

    void StopAttack()
    {
        focusingTarget = null;
        StopCoroutine(attackCoroutine);
    }

    void NextEnemy()
    {
        var node = attackRange.TargetList.First;
        while(node != null && node.Value.IsDead )
        {
            attackRange.TargetList.RemoveFirst();
            node = attackRange.TargetList.First;
        }


        if (node != null)
        {
            StartAttack(node.Value); // get next enemy in list, current enemy was dead
        }
        else
        {
            StopAttack();
        }
    }

    private bool IsEnemy(Creature target)
    {
        return target.Faction == Faction.Enemy;
    }

    protected override void OnAssign()
    {
        // anim
        attackRange.Init(IsEnemy, StartAttack, OnEnemyEscape);
    }
     

    

    public override void OnApplyAbility(AllyAbilityName ability)
    {
        switch (ability)
        {
            case AllyAbilityName.Burst1:
                burstCharge = 5;
                break;
            case AllyAbilityName.Burst2:
                burstCharge = 2;
                break;
        }
    }

    public override void OnFocus()
    {
        rangeVisualized.enabled = true;
    }

    public override void OnBlur()
    {
        rangeVisualized.enabled = false;
    }
}
