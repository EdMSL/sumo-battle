using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    public PlayerController playerController;
    public GameManager gameManager;
    private VisualElement _playerContainer;
    public VisualElement countdownContainer;
    public VisualElement menuContainer;
    public Label livesCounter;
    public Label waveCounter;
    public Label countdownText;
    public Button repeatGameBtn;
    public Button goToMenuBtn;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _playerContainer = uiDocument.rootVisualElement.Query<VisualElement>("hud-container");
        List<VisualElement> result = uiDocument.rootVisualElement.Query(className: "popup").ToList();

        result.ForEach(elem => elem.style.display = DisplayStyle.None);

        _playerContainer = uiDocument.rootVisualElement.Q("hud-container");
        countdownContainer = uiDocument.rootVisualElement.Q("countdown-container");
        menuContainer = uiDocument.rootVisualElement.Q("menu-container");

        livesCounter = _playerContainer.Q("lives-block").Q<Label>(className: "hud__counter");
        waveCounter = _playerContainer.Q("wave-block").Q<Label>(className: "hud__counter");
        countdownText = countdownContainer.Q<Label>("countdown-text");

        repeatGameBtn = menuContainer.Q<Button>("btn-yes");
        goToMenuBtn = menuContainer.Q<Button>("btn-no");

        repeatGameBtn.RegisterCallback<ClickEvent>(OnRepeatGameBtnClick);
        goToMenuBtn.RegisterCallback<ClickEvent>(OnGoToMenuBtnClick);

        livesCounter.text = playerController.lives.ToString();
        waveCounter.text = gameManager.wave.ToString();
    }

    private void OnGoToMenuBtnClick(ClickEvent evt)
    {
        gameManager.EndGame();
    }

    private void OnRepeatGameBtnClick(ClickEvent evt)
    {
        gameManager.RepeatGame();
    }
}
