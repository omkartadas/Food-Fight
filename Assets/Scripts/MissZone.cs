using UnityEngine;

public class MissZone : MonoBehaviour {
    private GameManager gameManager;

    void Start() {
        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null) {
            Debug.LogError("GameManager not found in MissZone!");
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other == null) return; // Prevents null reference issues

        if (other.CompareTag("Healthy")) {
            if (gameManager != null) {
                gameManager.MissHealthyFood(); //  Reduce health when healthy food is missed
            } else {
                Debug.LogError("GameManager is missing in MissZone!");
            }
        }

        //  Return food to pool instead of destroying
        FoodPool.Instance.ReturnToPool(other.gameObject);
    }
}
