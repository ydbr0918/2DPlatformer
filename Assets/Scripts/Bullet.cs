using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 1;

    private float dirX;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        dirX = transform.localScale.x > 0 ? 1f : -1f;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * dirX * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyBoss boss = collision.GetComponent<EnemyBoss>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}