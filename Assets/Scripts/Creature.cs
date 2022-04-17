using System.Collections;
using UnityEngine;
using PathCreation;

public enum Faction
{
    Enemy,
    Ally
}

public enum DamageType
{
    PhysX
}


public abstract class Creature : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    Transform fXContainer;
    [SerializeField]
    Collider2D hitbox;
    [SerializeField]
    ProgressBar healthBar;
    [SerializeField]
    float speed = 1;

    protected PathCreator currentPath;
    float distanceTravelled;
    float offset = 0;
    Vector2 faceDirection;
    Vector2 _predictedPos;
    public Vector2 PredictedPos { get=>_predictedPos; }

    protected float totalHp = 100;
    protected int hp = 100;
    const string DAMAGED = "DAMAGED";
    const string DEATH = "DEATH";

    const float SAFE_TIME = 1.5f;

    bool isDead = false;

    public bool IsDead { get => isDead; set
        {
            if (isDead == value)
                return;
            if (value)
            {
                OnDead();
            }
            else
            {
                OnRevived();
            }
            isDead = value;
        }
    }

    private void Awake()
    {
        hp = (int)totalHp;
    }
    public void OnDamaged(int damage, DamageType damageType)
    {
        hp -= damage;
        if (hp < 1)
        {
            //animator.SetTrigger(DEATH);
            IsDead = true;
            SetFocusable(false);
            CompletelyDisappeared();
        }
        else
        {
            //animator.SetTrigger(DAMAGED);
            healthBar.SetPercent(hp / totalHp);
            DamageBubble.Spawn(transform).Init(damage, damageType);
            //fXContainer.
        }
    }

    public void CompletelyDisappeared()
    {
        // do some anim
        gameObject.SetActive(false);
        Destroy(gameObject, SAFE_TIME);
    }

    public void Depath(PathCreator path, float offset)
    {
        distanceTravelled = 0;
        currentPath = path;
        this.offset = offset;
        this.transform.position = path.path.GetPoint(0);
    }

    protected void Move()
    {
        float step = speed * Time.deltaTime;
        distanceTravelled += step;
        Vector2 curentPosition = currentPath.path.GetPointAtDistance(distanceTravelled,TravellDone);
        faceDirection = currentPath.path.GetDirectionAtDistance(distanceTravelled);
        if (offset != 0)
        {
            transform.position = curentPosition + Vector2.Perpendicular(faceDirection) * offset;

        }
        else
        {
            transform.position = curentPosition;
        }
        _predictedPos = curentPosition + faceDirection * 0.2f;
        Debug.DrawLine(transform.position, _predictedPos, Color.black);
    }

    protected abstract void OnDead();
    protected virtual void OnRevived()
    {

    }

    protected void SetFocusable(bool isFocusable)
    {
        hitbox.enabled = isFocusable;
        healthBar.gameObject.SetActive(isFocusable);
    }



    void TravellDone()
    {
        OnTravellDone();

    }

    protected abstract void OnTravellDone();

    public abstract Faction Faction
    {
        get;
        //set;
    }
}
