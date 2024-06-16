using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameScript : MonoBehaviour
{
    // Settings button
    private Button buttonSettings;

    // Jauge progress bar
    private ProgressBar progressStar;

    // Settings panel
    private VisualElement settings;
    // Settings close button
    private Button settingsCloseButton;

    private float timer;

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Settings button
        buttonSettings = uiDocument.rootVisualElement.Q<Button>(name = "buttonSettings");
        buttonSettings.RegisterCallback<ClickEvent>(OpenSettings);

        // Progress star
        progressStar = uiDocument.rootVisualElement.Q<ProgressBar>(name = "progressStar");
        Debug.Log(Star.absorbedTags);

        // Settings panel
        settings = uiDocument.rootVisualElement.Q<VisualElement>(name = "Settings");
        settingsCloseButton = uiDocument.rootVisualElement.Q<Button>(name = "buttonSettingsClose");
        settingsCloseButton.RegisterCallback<ClickEvent>(CloseSettings);
    }

    private void OnDisable()
    {
        buttonSettings.UnregisterCallback<ClickEvent>(OpenSettings);
        settingsCloseButton.UnregisterCallback<ClickEvent>(CloseSettings);
    }

    private void OpenSettings(ClickEvent evt)
    {
        settings.style.display = DisplayStyle.Flex;
    }
    private void CloseSettings(ClickEvent evt)
    {
        settings.style.display = DisplayStyle.None;
    }

    private void LaunchGame(ClickEvent evt)
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    
    private void QuitGame(ClickEvent evt)
    {
        Application.Quit();
    }


    private void Start() {
        timer = 0;
    }
    private void Update() {
        timer += Time.deltaTime;
    }
}

// GameData.DATA.objectTags[]