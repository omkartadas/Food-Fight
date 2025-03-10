using UnityEngine;

public class BladeEffect : MonoBehaviour {
    public TrailRenderer trail;   // Reference to the Trail Renderer
    public AudioSource sliceSound; // Slicing sound effect
    private Camera cam;
    private bool isTouching = false;
    public ParticleSystem bladeParticles;

    void Start() {
        cam = Camera.main;
        trail.emitting = false; // Hide trail initially
    }

    void Update() {
        if (Input.GetMouseButton(0)) // Touch Hold
        {
            isTouching = true;
            trail.emitting = true;

            // Convert touch position to world space
            Vector3 touchPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

            // Smooth movement
            transform.position = Vector3.Lerp(transform.position, touchPosition, 0.5f);

            if (!sliceSound.isPlaying)
                sliceSound.Play(); // Play sound once per slice
        } else if (Input.GetMouseButtonUp(0)) // Touch Release
          {
            isTouching = false;
            trail.emitting = false;
        }


        {
            if (Input.GetMouseButton(0)) // Touch Hold
            {
                isTouching = true;
                trail.emitting = true;
                bladeParticles.Play(); // Activate particles
            } else if (Input.GetMouseButtonUp(0)) // Touch Release
              {
                isTouching = false;
                trail.emitting = false;
                bladeParticles.Stop(); // Stop particles
            }
        }
    }
}
