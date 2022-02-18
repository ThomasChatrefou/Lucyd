using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneFader sceneFader;
    public string levelToLoad = "MainLevel";

    public MenuFader menu;
    public float timeBeforeMenuFadeOut = 60f;
    
    private float countDown;


    private void Start()
    {
        Outline[] OutlineArray = Object.FindObjectsOfType<Outline>();
        foreach (Outline outline in OutlineArray)
        {
            outline.enabled = false;
        }
    }
    public void Awake()
    {
        countDown = timeBeforeMenuFadeOut;
    }

    public void Update()
    {
        if (menu.gameObject.activeSelf && countDown < 0)
        {
            menu.Hide();
        }

        if (Input.anyKey)
        {
            countDown = timeBeforeMenuFadeOut;
            menu.ShowUp();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Quit();
        }

        countDown -= Time.deltaTime;
    }

    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
