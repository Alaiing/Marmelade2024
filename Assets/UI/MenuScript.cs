using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SimpleRuntimeUI : MonoBehaviour
{
    // Play button
    private Button nikos49Fr;
    // Settings button
    private Button buttonSettings;
    // Quit button
    private Button j_peux_pas_j_ai_jdr;

    // Settings panel
    private VisualElement settings;
    // Settings close button
    private Button settingsCloseButton;

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Play button
        nikos49Fr = uiDocument.rootVisualElement.Q<Button>(name = "buttonPlay");
        nikos49Fr.RegisterCallback<ClickEvent>(LaunchGame);

        // Settings button
        buttonSettings = uiDocument.rootVisualElement.Q<Button>(name = "buttonSettings");
        buttonSettings.RegisterCallback<ClickEvent>(OpenSettings);

        // Quit button
        j_peux_pas_j_ai_jdr = uiDocument.rootVisualElement.Q<Button>(name = "buttonQuit");
        j_peux_pas_j_ai_jdr.RegisterCallback<ClickEvent>(QuitGame);

        // Settings panel
        settings = uiDocument.rootVisualElement.Q<VisualElement>(name = "Settings");
        settingsCloseButton = uiDocument.rootVisualElement.Q<Button>(name = "buttonSettingsClose");
        settingsCloseButton.RegisterCallback<ClickEvent>(CloseSettings);
    }

    private void OnDisable()
    {
        nikos49Fr.UnregisterCallback<ClickEvent>(LaunchGame);
        buttonSettings.UnregisterCallback<ClickEvent>(OpenSettings);
        j_peux_pas_j_ai_jdr.UnregisterCallback<ClickEvent>(QuitGame);
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
}