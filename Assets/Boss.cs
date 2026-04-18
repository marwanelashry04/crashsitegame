using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int health = 20;
    public int maxHealth = 20;
    public GameObject bulletPrefab;
    public float fireRate = 1.5f;
    public Slider healthBar;

    private float minX = -4f;
    private float maxX = 4f;
    private int direction = 1;
    private float fireTimer;

    void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
    }

    void Update()
    {
        Move();
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void Move()
    {
        transform.Translate(Vector2.right * direction 
                                          * moveSpeed * Time.deltaTime);

        if (transform.position.x >= maxX)
            direction = -1;
        if (transform.position.x <= minX)
            direction = 1;
    }

    void Shoot()
    {
        Vector3 pos = transform.position;
        Instantiate(bulletPrefab, pos, Quaternion.identity);
        Instantiate(bulletPrefab, pos + new Vector3(0.5f, 0, 0), 
            Quaternion.identity);
        Instantiate(bulletPrefab, pos + new Vector3(-0.5f, 0, 0), 
            Quaternion.identity);
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        GameManager.instance.AddScore(5);
        UpdateHealthBar();
        if (health <= 0)
        {
            StartCoroutine(BossDead());
        }
    }

    IEnumerator BossDead()
    {
        GameManager.instance.ShowVictory();
        Destroy(gameObject);
        yield return null;
    }
}