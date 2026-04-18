using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int health = 3;

    void Update()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.AddScore(10);
            Destroy(gameObject);
        }
    }

	void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        other.GetComponent<PlayerHealth>().TakeDamage(1);
        Destroy(gameObject);
    }
}
}