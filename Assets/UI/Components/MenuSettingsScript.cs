using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuSettingsScript : MonoBehaviour
{
    // Close button
    private Button closeButton;

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Close button
        var closeButton = uiDocument.rootVisualElement.Q<Button>(name = "buttonClose");
        closeButton.RegisterCallback<ClickEvent>(CloseSettings);
    }

    private void OnDisable()
    {
        closeButton.UnregisterCallback<ClickEvent>(CloseSettings);
    }

    private void CloseSettings(ClickEvent evt)
    {
        Debug.Log("close");
    }
}