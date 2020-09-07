using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public static MenuUI instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    private void Start()
    {
        AudioController.instance.PlayAudio(AudioType.ST1);
    }
    public void LoadSceneGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void LoadSceneMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Back()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            Application.Quit();
        else
            LoadSceneMenu();
    }
    public void UIClickSFX()
    {
        AudioController.instance.PlayAudio(AudioType.SFX3);
    }
}