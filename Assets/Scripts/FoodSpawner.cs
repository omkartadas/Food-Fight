using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour {
    [Header("Food Types")]
    public GameObject[] healthyFoods;
    public GameObject[] junkFoods;

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public float spawnRate = 1.5f;
    public float minLaunchForce = 5f;
    public float maxLaunchForce = 10f;
    public float spawnRangeX = 2f; // Allows dynamic range changes

    private bool isSpawning = false; //  Prevents food from spawning before difficulty is selected

    public void StartSpawning() {
        if (!isSpawning) {
            isSpawning = true;

            int difficulty = PlayerPrefs.GetInt("GameDifficulty", 0); // Default to Easy

            switch (difficulty) {
                case 0: // Easy
                    spawnRate = 1.5f;
                    break;
                case 1: // Medium
                    spawnRate = 0.1f;
                    break;
                case 2: // Hard
                    spawnRate = 0.5f;
                    break;
            }

            StartCoroutine(SpawnFood());
        }
    }

    IEnumerator SpawnFood() {
        while (true) {
            yield return new WaitForSeconds(spawnRate);

            //  Spawn multiple foods at once (2 in Easy, 3 in Medium, 4 in Hard)
            int foodCount = (PlayerPrefs.GetInt("GameDifficulty", 0) == 0) ? 1 :
                            (PlayerPrefs.GetInt("GameDifficulty", 0) == 1) ? 2 : 3;

            for (int i = 0; i < foodCount; i++) {
                SpawnRandomFood();
            }
        }
    }


    void SpawnRandomFood() {
        if (FoodPool.Instance == null) {
            Debug.LogError("FoodPool Instance is null! Make sure FoodPool exists in the scene.");
            return;
        }

        GameObject food = FoodPool.Instance.GetFood(); // Get food from pool

        if (food == null) {
            Debug.LogError("GetFood() returned null! Check if the pool has food objects.");
            return;
        }

        Vector3 spawnPos = new Vector3(Random.Range(-2f, 2f), spawnPoint.position.y, 0f);
        food.transform.position = spawnPos; // Move food to spawn position
        food.SetActive(true); // Activate food

        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb != null) {
            Vector3 launchForce = new Vector3(Random.Range(-1f, 1f), 1f, 0f).normalized * Random.Range(minLaunchForce, maxLaunchForce);
            rb.linearVelocity = launchForce; // Apply velocity
            rb.angularVelocity = Random.insideUnitSphere * 3;
        }
    }
}
