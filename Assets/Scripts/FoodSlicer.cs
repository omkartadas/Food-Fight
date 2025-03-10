using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class FoodSlicer : MonoBehaviour {
    public int pointValue;
    public ParticleSystem sliceEffect;

    private Camera mainCamera;
    private GameManager gameManager;

    void Start() {
        mainCamera = Camera.main;
        gameManager = FindFirstObjectByType<GameManager>(); // Assign once

        if (gameManager == null) {
            Debug.LogError("GameManager not found!");
        }
    }

    void Update() {
        if (mainCamera == null) return; // Prevent errors if camera is missing

        // Ensure Touchscreen is available before accessing it
        if (Touchscreen.current?.primaryTouch.press.isPressed ?? false) {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            CheckForSlice(touchPosition);
        }

#if UNITY_EDITOR
        // Ensure Mouse input is available before using it
        if (Mouse.current?.leftButton.wasPressedThisFrame ?? false) {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            CheckForSlice(mousePosition);
        }
#endif
    }

    void CheckForSlice(Vector2 screenPosition) {
        if (mainCamera == null) {
            Debug.LogError("Main Camera is missing!");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider != null && hit.collider.gameObject == gameObject) {
            Slice();
        }
    }

    void Slice() {
        Instantiate(sliceEffect, transform.position, Quaternion.identity);

        if (gameManager != null) {
            if (CompareTag("Healthy")) {
                gameManager.UpdateScore(pointValue); //  Increase score for healthy food
            } else if (CompareTag("Junk")) {
                gameManager.SliceJunkFood(); //  Reduce health when junk food is sliced
            }
        }

        FoodPool.Instance.ReturnToPool(gameObject);
    }

}
