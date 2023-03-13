using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private VisualElement _playButton;
    private VisualElement _exitButton;

    
    // Start is called before the first frame update
    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        _playButton = root.Q<VisualElement>("PlayButton");
        _exitButton = root.Q<VisualElement>("ExitButton");
        
        _playButton.RegisterCallback<ClickEvent>(PlayGame);
        _exitButton.RegisterCallback<ClickEvent>(ExitGame);
    }

    private static void ExitGame(ClickEvent evt)
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    private static void PlayGame(ClickEvent evt)
    {
        SceneManager.LoadScene("Scene");
    }
}
