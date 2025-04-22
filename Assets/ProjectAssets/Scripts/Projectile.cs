using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private int damage;
    private ObjectPoolDynamic pool;

    public void SetPool(ObjectPoolDynamic pool)
    {
        if(pool != null)
        {
            this.pool = pool;
        }
    }

    public void Initialize(int projectileDamage)
    {
        damage = projectileDamage;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.right * speed;
    }
    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        pool.SetObject(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();
            obstacle.TakeDamage(damage);
            ReturnToPool();
        }
        if(other.CompareTag("Enemy"))
        {
            ReturnToPool();
        }

        if (other.CompareTag("DeleterProjectile"))
        {
            ReturnToPool();
        }
    }
}