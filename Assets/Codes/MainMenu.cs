using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public String levelToLoad = "MainLevel";

    public SceneFader sceneFader;

    public void Play() {
        sceneFader.FadeTo(levelToLoad);  
    }

    public void Quit() {
        Debug.Log("Excitiong...");
        Application.Quit();
    }
}
