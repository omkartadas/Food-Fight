using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverScreen;
    public Button restartButton;
    public FoodSpawner foodSpawner;

    [Header("Health System")]
    public List<Image> hearts;
    private int maxHealth;
    private int currentHealth;

    private int score = 50;
    private float gameTime = 120f;
    private bool isGameActive = false;

    private void Start() {
        int difficulty = PlayerPrefs.GetInt("GameDifficulty", -1);
        
        if (difficulty == -1) {
            Debug.Log(" No difficulty selected. Waiting in Main Menu...");
            return;
        }

        StartGame();
    }

    public void StartGame() {
        Debug.Log(" Game Started!");
        isGameActive = true;

        int difficulty = PlayerPrefs.GetInt("GameDifficulty", 0);
        maxHealth = (difficulty == 0) ? 3 : (difficulty == 1) ? 4 : 5;
        currentHealth = maxHealth;

        //  Hide extra hearts if not needed
        for (int i = 0; i < hearts.Count; i++) {
            hearts[i].gameObject.SetActive(i < maxHealth); // Show only required hearts
        }

        StartCoroutine(CountdownTimer());
        foodSpawner.StartSpawning();
    }


    IEnumerator CountdownTimer() {
        while (gameTime > 0 && isGameActive) {
            timerText.text = "Time: " + Mathf.Ceil(gameTime).ToString();
            yield return new WaitForSeconds(1f);
            gameTime -= 1f;
        }

        if (gameTime <= 0) {
            GameOver();
        }
    }

    public void UpdateScore(int points) {
        Debug.Log("UpdateScore called with points: " + points);
        score += points;

        if (scoreText != null) {
            scoreText.text = "Score: " + score.ToString();
        } else {
            Debug.LogError(" Score Text is not assigned in the Inspector!");
        }

        if (score <= 0) {
            GameOver();
        }
    }

    public void MissHealthyFood() {
        Debug.Log(" Missed a Healthy Food!");
        LoseHealth();
    }

    public void SliceJunkFood() {
        Debug.Log(" Sliced Junk Food!");
        LoseHealth();
    }


    private void LoseHealth() {
        if (currentHealth > 0) {
            hearts[currentHealth - 1].gameObject.SetActive(false); // Hide the last heart
            currentHealth--;

            Debug.Log(" Health Lost! Remaining: " + currentHealth);

            //  Trigger Haptic Feedback when losing a life
            HapticFeedback haptic = FindObjectOfType<HapticFeedback>();
            if (haptic != null) {
                haptic.VibrateOnLifeLoss();
            } else {
                Debug.LogError(" HapticFeedback script not found in the scene!");
            }

            if (currentHealth <= 0) {
                GameOver();
            }
        }
    }



    void GameOver() {
        isGameActive = false;
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public int GetScore() {
        return score;
    }


    public void RestartGame() {
        Debug.Log(" Restarting game with difficulty: " + PlayerPrefs.GetInt("GameDifficulty", 0));

        Time.timeScale = 1; //  Ensure the game is unpaused
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //  Reload the current scene
    }

}
