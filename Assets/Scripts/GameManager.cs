using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int nbrTorchLighted = 0;

    public GameObject portal;

    [SerializeField] private float resetLevelCooldown = 60f;
    private float resetLevelTimer;
    public SceneFader sceneFader;
    [SerializeField] private string menuName;

    LevelEnding lvlEnd;


    private void Awake()
    {
        //singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //reset level
        resetLevelTimer = resetLevelCooldown;

        //level ending
        lvlEnd = GameObject.Find("LevelEndingCanvas").GetComponent<LevelEnding>();
    }

    private void Update()
    {
        // camera slider
        if (Input.GetKeyDown("c"))
            StartCoroutine(GetComponent<CameraManager>().CamSlide());

        // level reset
        if (Input.anyKey)
            resetLevelTimer = resetLevelCooldown;

        if (resetLevelTimer < 0)
            sceneFader.FadeTo(menuName);

        resetLevelTimer -= Time.deltaTime;

        // level ending test
        if (Input.GetKeyDown(KeyCode.E))
            SpawnEndingPortal();
    }

    //Light torch
    public void CountTorch()
    {
        nbrTorchLighted += 1;
        StartCoroutine(GetComponent<CameraManager>().CamSlide());

        if (nbrTorchLighted == 3)
            SpawnEndingPortal();
    }

    public void SpawnEndingPortal()
    {
        Instantiate(portal, new Vector3(0, 1, 7), new Quaternion(0, 90, 90, 90));
    }

    public void EndLevel()
    {
        if (lvlEnd)
            lvlEnd.Display();
    }
}
