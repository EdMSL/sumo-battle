using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Awake() {
      Application.targetFrameRate = 60;
    }
    
    public void StartGame(string diffLevel)
    {
        Save.SetDifficultyLevel(diffLevel);
        SceneManager.LoadScene(1);
    }
}
