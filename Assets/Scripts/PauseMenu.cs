using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool IsPauded = false;
    public GameObject PauseMenuUI;


    private void Start()
    {
      IsPauded = false;
      Resume();
}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPauded)
            {
                Resume();
                
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPauded = false ;
    }
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPauded = true;
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }



}
