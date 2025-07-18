using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    public PlayerController playerController;
    public GameManager gameManager;
    private VisualElement _playerContainer;
    public VisualElement countdownContainer;
    public Label livesCounter;
    public Label waveCounter;
    public Label countdownText;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _playerContainer = uiDocument.rootVisualElement.Q("hud-container");
        countdownContainer = uiDocument.rootVisualElement.Q("countdown-container");

        livesCounter = _playerContainer.Q("lives-block").Q<Label>(className: "hud__counter");
        waveCounter = _playerContainer.Q("wave-block").Q<Label>(className: "hud__counter");
        countdownText = countdownContainer.Q<Label>("countdown-text");

        livesCounter.text = playerController.lives.ToString();
        waveCounter.text = gameManager.wave.ToString();
    }
}
