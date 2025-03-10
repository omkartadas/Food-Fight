using UnityEngine;

public class MainMenu : MonoBehaviour {
    public GameObject mainMenuUI;
    public GameManager gameManager;

    private void Start() {
        Debug.Log(" Main Menu is waiting for input...");
        Time.timeScale = 0;
    }

    public void SetDifficulty(int difficulty) {
        Debug.Log(" SetDifficulty() called with difficulty: " + difficulty);
        PlayerPrefs.SetInt("GameDifficulty", difficulty);
        PlayerPrefs.Save();

        if (mainMenuUI != null) {
            mainMenuUI.SetActive(false);
        }

        gameManager.StartGame();
        Time.timeScale = 1;
    }


}
