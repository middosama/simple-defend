using System.Collections;
using UnityEngine;
using UnityCommon;

public class TestObjectAlly : Ally
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
    public CircleCollider2D rangeSensor;
    Creature focusingTarget;

    Coroutine attackCoroutine;

    public float bulletFrame;


    public Animator animator;
    public float animateTime;
    public float wantedAnimateTime;
    public override float SellValue => 0.8f; // 80% refund

    public override AllyName AllyName => AllyName.TestObject;

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
            // starting aiming
            wantedAnimateTime = (snapTime / attackSpeedMultiply) - 0.05f;
            animateTime = AnimationLength("SimpleAlly_Aiming");
            animator.SetTrigger("AimTrigger");
            animator.speed = (animateTime / wantedAnimateTime);

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
                    animateTime = AnimationLength("SimpleAlly_Aiming");
                    animator.SetTrigger("AimTrigger");
                    animator.speed = (animateTime / (0.1f - 0.05f));
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
        animator.SetTrigger("ShootTrigger");
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
        while (node != null && node.Value.IsDead)
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
                ChangeRange(5);
                break;
        }
    }

    void ChangeRange(float size)
    {
        rangeSensor.radius = size;
        rangeVisualized.size = new Vector2(size * 2, size * 2);
    }

    public override void OnFocus()
    {

        rangeVisualized.enabled = true;
    }

    public override void OnBlur()
    {
        rangeVisualized.enabled = false;
    }

    public float AnimationLength(string name)
    {
        float time = 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
            if (ac.animationClips[i].name == name)
                time = ac.animationClips[i].length;

        return time;
    }
}
