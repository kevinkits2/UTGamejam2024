using UnityEngine;

public class RotateSprite : MonoBehaviour {
    public float minSpeed = 5f;      // Minimum rotation speed
    public float maxSpeed = 10f;     // Maximum rotation speed
    public float rotationAngle = 4f; // Maximum rotation angle (4 degrees)

    private bool rotatingRight = true;  // Direction of rotation
    private float currentRotation = 0f; // Track the current rotation angle
    private float rotationSpeed;        // Current rotation speed

    void Start() {
        // Set the initial rotation speed to a random value between minSpeed and maxSpeed
        rotationSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void Update() {
        float rotationStep = rotationSpeed * Time.deltaTime; // Calculate the rotation for this frame

        // Check the direction and rotate accordingly
        if (rotatingRight) {
            currentRotation += rotationStep;
            transform.Rotate(Vector3.forward, rotationStep);

            // If we've reached the maximum angle, reverse direction and randomize the speed
            if (currentRotation >= rotationAngle) {
                rotatingRight = false;
                rotationSpeed = Random.Range(minSpeed, maxSpeed); // Randomize speed
            }
        }
        else {
            currentRotation -= rotationStep;
            transform.Rotate(Vector3.forward, -rotationStep);

            // If we've reached the negative maximum angle, reverse direction and randomize the speed
            if (currentRotation <= -rotationAngle) {
                rotatingRight = true;
                rotationSpeed = Random.Range(minSpeed, maxSpeed); // Randomize speed
            }
        }
    }
}
