using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;

    [SerializeField] private string MainMenuName = "MainMenu";

    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseUI.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
    }

    public void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        if (sceneFader != null)
        {
            sceneFader.FadeTo(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        if (sceneFader != null)
        {
            sceneFader.FadeTo(MainMenuName);
        }
        else
        {
            SceneManager.LoadScene(MainMenuName);
        }
    }
}
