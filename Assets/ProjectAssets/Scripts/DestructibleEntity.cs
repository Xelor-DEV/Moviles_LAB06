using UnityEngine;

public class DestructibleEntity : MonoBehaviour
{
    [SerializeField] protected AttackerStats attackerStats;
    protected ObjectPoolStatic pool;

    public AttackerStats AttackerStats
    {
        get
        {
            return attackerStats;
        }
        set
        {
            attackerStats = value;
        }
    }

    public void SetPool(ObjectPoolStatic pool)
    {
        if (pool != null)
        {
            this.pool = pool;
        }
    }

    public virtual void Init()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * attackerStats.Speed;
    }

    protected void ReturnToStaticPool()
    {
        gameObject.SetActive(false);
        pool.SetObject(gameObject);
    }
}