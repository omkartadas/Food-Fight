using System.Collections.Generic;
using UnityEngine;

public class FoodPool : MonoBehaviour {
    public static FoodPool Instance; // Singleton (so we can access this from anywhere)

    [Header("Food Pool Settings")]
    public GameObject[] healthyFoods;
    public GameObject[] junkFoods;
    public int poolSize = 10; // Number of food objects to create at start

    private Queue<GameObject> foodPool = new Queue<GameObject>(); // Stores reusable food objects

    void Awake() {
        Instance = this; // Assign this script as a Singleton
    }

    void Start() {
        if (healthyFoods.Length == 0 || junkFoods.Length == 0) {
            Debug.LogError("Food arrays are empty! Assign food prefabs in the Inspector.");
            return;
        }

        for (int i = 0; i < poolSize; i++) {
            AddFoodToPool(healthyFoods[Random.Range(0, healthyFoods.Length)]);
            AddFoodToPool(junkFoods[Random.Range(0, junkFoods.Length)]);
        }

        Debug.Log("FoodPool initialized with " + foodPool.Count + " food objects.");
    }


    void AddFoodToPool(GameObject prefab) {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false); // Hide initially
        foodPool.Enqueue(obj); // Add to pool
    }

    public GameObject GetFood() {
        if (foodPool.Count > 0) {
            GameObject food = foodPool.Dequeue();

            // Check if food was accidentally destroyed
            if (food == null) {
                Debug.LogWarning("Pooled object was destroyed! Creating a new one.");
                return Instantiate(healthyFoods[Random.Range(0, healthyFoods.Length)]);
            }

            food.SetActive(true);
            return food;
        }

        // If pool is empty, create new food as backup
        Debug.LogWarning("Food pool empty! Creating new food.");
        return Instantiate(healthyFoods[Random.Range(0, healthyFoods.Length)]);
    }


    public void ReturnToPool(GameObject food) {
        if (food == null) return; // Prevent errors

        food.SetActive(false);
        food.transform.position = new Vector3(0, -10, 0); // Move off-screen

        foodPool.Enqueue(food);
        Debug.Log(food.name + " returned to pool. Pool size: " + foodPool.Count);
    }

}
