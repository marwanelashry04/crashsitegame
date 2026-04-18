using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI levelCompleteText;

    private int score = 0;
    private int lives = 3;
    private bool levelComplete = false;
    private bool gameEnded = false;
    public int scoreToNextLevel = 100;
    private string[] heartStates = { "", "♥", "♥ ♥", "♥ ♥ ♥" };

    void Awake()
    {
        instance = this;
        gameOverText.gameObject.SetActive(false);
        levelCompleteText.gameObject.SetActive(false);
    }

    public void AddScore(int amount)
    {
        if (gameEnded) return;
        score += amount;
        scoreText.text = "Score: " + score;
        StopCoroutine("PunchScale");
        StartCoroutine(PunchScale(scoreText.transform));

        if (score >= scoreToNextLevel && !levelComplete)
        {
            levelComplete = true;
            StartCoroutine(ShowLevelComplete());
        }
    }

    public void LoseLife()
    {
        if (gameEnded) return;
        lives--;
        livesText.text = heartStates[Mathf.Max(0, lives)];
        StopCoroutine("ShakeText");
        StartCoroutine(ShakeText(livesText.transform));

        if (lives <= 0)
        {
            StartCoroutine(ShowGameOver());
        }
    }

    IEnumerator PunchScale(Transform target)
    {
        Vector3 original = target.localScale;
        target.localScale = original * 1.4f;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 8f;
            target.localScale = Vector3.Lerp(original * 1.4f, original, t);
            yield return null;
        }
        target.localScale = original;
    }

    IEnumerator ShakeText(Transform target)
    {
        Vector3 original = target.localPosition;
        float duration = 0.4f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = original.x + Random.Range(-6f, 6f);
            float y = original.y + Random.Range(-6f, 6f);
            target.localPosition = new Vector3(x, y, original.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localPosition = original;
    }

    IEnumerator ShowLevelComplete()
    {
        gameEnded = true;
        FirebaseAuthSimple auth = FindObjectOfType<FirebaseAuthSimple>();
        if (auth != null) auth.SaveScore(score);

        levelCompleteText.gameObject.SetActive(true);
        levelCompleteText.text = "LEVEL COMPLETE!";
        levelCompleteText.color = Color.green;

        Vector3 original = levelCompleteText.transform.localScale;
        levelCompleteText.transform.localScale = Vector3.zero;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            levelCompleteText.transform.localScale = Vector3.Lerp(
                Vector3.zero, original, t);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator ShowGameOver()
    {
        gameEnded = true;
        FirebaseAuthSimple auth = FindObjectOfType<FirebaseAuthSimple>();
        if (auth != null) auth.SaveScore(score);

        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "GAME OVER";
        gameOverText.color = Color.red;

        Vector3 original = gameOverText.transform.localScale;
        gameOverText.transform.localScale = Vector3.zero;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            gameOverText.transform.localScale = Vector3.Lerp(
                Vector3.zero, original, t);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowVictory()
    {
        if (gameEnded) return;
        StartCoroutine(VictoryScreen());
    }

    IEnumerator VictoryScreen()
    {
        gameEnded = true;
        FirebaseAuthSimple auth = FindObjectOfType<FirebaseAuthSimple>();
        if (auth != null) auth.SaveScore(score);

        levelCompleteText.gameObject.SetActive(true);
        levelCompleteText.text = "YOU WIN!";
        levelCompleteText.color = Color.yellow;

        Vector3 original = levelCompleteText.transform.localScale;
        levelCompleteText.transform.localScale = Vector3.zero;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            levelCompleteText.transform.localScale = Vector3.Lerp(
                Vector3.zero, original, t);
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("SampleScene");
    }
}