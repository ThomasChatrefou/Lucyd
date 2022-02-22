using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnding : MonoBehaviour
{
    [SerializeField] private GameObject endingUI;
    [SerializeField] private string MainMenuName = "MainMenu";

    private SceneFader sceneFader;

    private void Awake()
    {
        sceneFader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
    }

    public void Display()
    {
        endingUI.SetActive(true);
        Time.timeScale = 0f;
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
