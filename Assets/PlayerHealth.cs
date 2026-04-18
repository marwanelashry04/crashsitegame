using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3;

    public void TakeDamage(int damage)
    {
        lives -= damage;
        GameManager.instance.LoseLife();
        
        if (lives <= 0)
        {
            Destroy(gameObject);
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}