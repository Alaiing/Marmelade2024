using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Menu : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/Menu")]
    public static void ShowExample()
    {
        Menu wnd = GetWindow<Menu>();
        wnd.titleContent = new GUIContent("Menu");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        //Call the event handler
        SetupButtonHandler();
    }

    //Functions as the event handlers for your button click and number counts 
    private void SetupButtonHandler()
    {
        VisualElement root = rootVisualElement;

        // Play button
        var nikos49Fr = root.Q<Button>(name = "buttonPlay");
        nikos49Fr.RegisterCallback<ClickEvent>(LaunchGame);
        Debug.Log(nikos49Fr);

        // Settings button
        var buttonSettings = root.Q<Button>(name = "buttonSettings");
        buttonSettings.RegisterCallback<ClickEvent>(PrintClickMessage);

        // Quit button
        var j_peux_pas_j_ai_jdr = root.Q<Button>(name = "buttonQuit");
        j_peux_pas_j_ai_jdr.RegisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        VisualElement root = rootVisualElement;
    }
    
    private void LaunchGame(ClickEvent evt)
    {
        Debug.Log("print click");
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }
}
