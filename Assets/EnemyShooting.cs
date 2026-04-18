using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject enemyBulletPrefab;
    public float fireRate = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null)
        {
            Vector3 spawnPos = transform.position + Vector3.down * 0.5f;
            Instantiate(enemyBulletPrefab, spawnPos, Quaternion.identity);
        }
    }
}