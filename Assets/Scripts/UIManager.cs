using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadArScene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadRegularScene()
    {
        SceneManager.LoadScene(1);
    }

    public void SetSinglePlayer()
    {
        PlayerPrefs.SetInt("mode", 1);
    }

    public void SetPVP()
    {
        PlayerPrefs.SetInt("mode", 0);
    }
}