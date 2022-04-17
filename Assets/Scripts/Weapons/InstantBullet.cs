using System.Collections;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;

public class InstantBullet : DynamicObjectPool<InstantBullet>
{
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    TrailRenderer trailRenderer;
    Sequence x = null;
    public override void OnSpawn()
    {
        if (x != null)
            x.Kill();
    }

    public void Hit(Vector2 from, Creature to, float frame,int damage, DamageType damageType = DamageType.PhysX)
    {
        this.gameObject.SetActive(true);
        transform.position = from;
        trailRenderer.Clear();
        //rb.DOMove(to.transform.position, frame)
        //Path x = new TweenParams

        //rb.DOPath()
        
        x = rb.DOJump(to.PredictedPos, 3, 1, frame, false, true).OnComplete(() =>
            {
                Destroy(this);
                if(to != null)
                    to.OnDamaged(damage, damageType);
            });
    }

    //public void Hit2(Creature from, Creature to, float frame, int damage, DamageType damageType = DamageType.PhysX)
    //{
    //    //this.gameObject.SetActive(true);
    //    transform.position = from.transform.position;
    //    //rb.DOMove(to.transform.position, frame)
    //    //Path x = new TweenParams

    //    //rb.DOPath()
    //    rb.DOJump(to.transform.position, 3, 1, frame, false, true).OnComplete(() =>
    //    {
    //        to.OnDamaged(damage, damageType);
    //        Destroy(this.gameObject);
    //    });
    //}
    //private void Update()
    //{
    //    rb.rotation = 
    //}


}
