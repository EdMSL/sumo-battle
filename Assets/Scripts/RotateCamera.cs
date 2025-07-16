using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public PlayerController playerController;
    public float rotationSpeed = 50f;

    void Start()
    {
        switch (Save.difficultyLevel)
        {
            case Save.DifficultyLevel.Easy:
                rotationSpeed = 100f;
                break;
            case Save.DifficultyLevel.Normal:
                rotationSpeed = 80f;
                break;
            case Save.DifficultyLevel.Hard:
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
