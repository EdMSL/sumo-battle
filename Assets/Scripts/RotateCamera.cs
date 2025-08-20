using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Start()
    {
        switch (GameManager.Instance.difficultyLevel)
        {
            case GameManager.DifficultyLevel.Easy:
                rotationSpeed = 100f;
                break;
            case GameManager.DifficultyLevel.Normal:
                rotationSpeed = 80f;
                break;
            case GameManager.DifficultyLevel.Hard:
                rotationSpeed = 60f;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (GameManager.Instance.state == GameManager.State.GameProcess)
        {
            float horizontalInput = PlayerController.Instance.movement.x;
            transform.Rotate(Vector3.up, rotationSpeed * horizontalInput * Time.deltaTime);
        }
    }
}
