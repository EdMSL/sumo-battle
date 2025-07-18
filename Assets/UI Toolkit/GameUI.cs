using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    private VisualElement _playerContainer;
    private Label _livesCounter;
    private Label _waveCounter;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _playerContainer = uiDocument.rootVisualElement.Q("hud-container");

        _livesCounter = _playerContainer.Q("lives-block").Q<Label>(className: "hud__counter");
        _waveCounter = _playerContainer.Q("wave-block").Q<Label>(className: "hud__counter");

        _livesCounter.text = "5";
        _waveCounter.text = "55";
    }
}
