using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartButton()
    {
        Debug.Log("Button pressed");
        // Startet Spiel-Szene
        SceneManager.LoadScene("Game");
    }

    void QuitButton()
    {
    #if UNITY_EDITOR
            // Quit funktioniert nicht im Editor, stattdessen Playmode falsch setzen
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}
