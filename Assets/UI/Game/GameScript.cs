using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameScript : MonoBehaviour
{
    // Settings button
    private Button buttonSettings;

    // Jauge progress bar
    private ProgressBar belgiquemeilleurequefrance;
    private VisualElement progressBarInside;
    private int total;
    private int? currentTag;

    // Settings panel
    private VisualElement settings;
    // Settings close button
    private Button settingsCloseButton;
    private Button settingsGoToMenuButton;
    private Button settingsQuitButton;

    // gameover panel
    private VisualElement basilicVegeta;
    private Button gameOverReplay;
    private Label finalTimerLabel;

    // Timer
    private Label timerLabel;
    private float timer;

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Settings button
        buttonSettings = uiDocument.rootVisualElement.Q<Button>(name = "buttonMenu");
        buttonSettings.RegisterCallback<ClickEvent>(OpenSettings);

        // Progress star
        belgiquemeilleurequefrance = uiDocument.rootVisualElement.Q<ProgressBar>(name = "progressStar");
        belgiquemeilleurequefrance.value = 0;
        progressBarInside = uiDocument.rootVisualElement.Q<VisualElement>(className: "unity-progress-bar__progress");

        // Settings panel
        settings = uiDocument.rootVisualElement.Q<VisualElement>(name = "Menu");
        settingsCloseButton = uiDocument.rootVisualElement.Q<Button>(name = "buttonMenuClose");
        settingsCloseButton.RegisterCallback<ClickEvent>(CloseSettings);
        settingsGoToMenuButton = uiDocument.rootVisualElement.Q<Button>(name = "buttonGoMain");
        settingsGoToMenuButton.RegisterCallback<ClickEvent>(GoToMain);
        settingsQuitButton = uiDocument.rootVisualElement.Q<Button>(name = "buttonQuit");
        settingsQuitButton.RegisterCallback<ClickEvent>(QuitGame);

        // Game Over
        basilicVegeta = uiDocument.rootVisualElement.Q<VisualElement>(name = "GameOver");
        gameOverReplay = uiDocument.rootVisualElement.Q<Button>(name = "buttonReplay");
        gameOverReplay.RegisterCallback<ClickEvent>(RelaunchGame);
        finalTimerLabel = uiDocument.rootVisualElement.Q<Label>(name = "finalTimerLabel");
        Star.OnPlayerEaten += DisplayGameOver;

        // Timer
        timerLabel = uiDocument.rootVisualElement.Q<Label>(name = "Timer");
    }

    private void OnDisable()
    {
        buttonSettings.UnregisterCallback<ClickEvent>(OpenSettings);
        settingsCloseButton.UnregisterCallback<ClickEvent>(CloseSettings);
        gameOverReplay.UnregisterCallback<ClickEvent>(RelaunchGame);
        settingsGoToMenuButton.UnregisterCallback<ClickEvent>(GoToMain);
        settingsQuitButton.UnregisterCallback<ClickEvent>(QuitGame);

        Star.OnPlayerEaten -= DisplayGameOver;
    }

    private void DisplayGameOver() {
        // Pause game
        Time.timeScale = 0;

        finalTimerLabel.text = ((int)timer).ToString() + "s";
        basilicVegeta.style.display = DisplayStyle.Flex;
    }

    private void RelaunchGame(ClickEvent evt) {
        // Play game
        Time.timeScale = 1;

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void OpenSettings(ClickEvent evt)
    {
        Debug.Log("settings");
        // Pause game
        Time.timeScale = 0;

        settings.style.display = DisplayStyle.Flex;
    }
    private void CloseSettings(ClickEvent evt)
    {
        // Pause game
        Time.timeScale = 1;

        settings.style.display = DisplayStyle.None;
    }

    private void LaunchGame(ClickEvent evt)
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void GoToMain(ClickEvent evt) {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    
    private void QuitGame(ClickEvent evt)
    {
        Application.Quit();
    }


    private void Start() {
        timer = 0;
        total = 0;
        currentTag = null;
    }
    private void Update() {
        timer += Time.deltaTime;

        timerLabel.text = ((int)timer).ToString() + "s";
    
        belgiquemeilleurequefrance.value = (Star.absorbedTags[0] + Star.absorbedTags[1] + Star.absorbedTags[2] + Star.absorbedTags[3]) / 0.6f;
        int biggest = Array.IndexOf(Star.absorbedTags, Mathf.Max(Star.absorbedTags[0], Star.absorbedTags[1], Star.absorbedTags[2], Star.absorbedTags[3]));
        if(currentTag != biggest) {
            currentTag = biggest;
            progressBarInside.style.backgroundColor = GameData.DATA.objectTags[biggest].color;
        }

    } 
}