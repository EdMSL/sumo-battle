using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public PlayerController playerController;
    public float rotationSpeed = 50f;

    void Start()
    {
        switch (Save.difficultyLevel)
        {
            case "Easy":
                rotationSpeed = 100f;
                break;
            case "Normal":
                rotationSpeed = 80f;
                break;
            case "Hard":
                rotationSpeed = 60f;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        float horizontalInput = playerController.movement.x;
        transform.Rotate(Vector3.up, rotationSpeed * horizontalInput * Time.deltaTime);
    }
}
